using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blender
{
	public class BlendValue
	{
		public IBlendType Type;
		public object RawValue;

		public BlendValue(IBlendType type, object value)
		{
			Type = type;
			RawValue = value;
		}

		virtual public BlendValue GetAt(int index)
		{
			var objs = RawValue as object[];
			var type = (QualifiedBlendType)Type;
			return new BlendValue(type.BaseType, objs[index]);
		}

		virtual public BlendValue GetAt(int index1, int index2)
		{
			var objs = RawValue as object[];
			var tmp = objs[index1] as object[];
			var type = (QualifiedBlendType)Type;
			return new BlendValue(type.BaseType, tmp[index2]);
		}

		virtual public BlendValue GetMember(string name)
		{
			return BlendStructureType.GetMemberValue(this, name);
		}

		public IEnumerable<object> GetAllValue()
		{
			if (RawValue is Dictionary<string, BlendValue>)
			{
				return BlendStructureType.GetAllRawValue(this);
			}

			var values = _GetAllValue(Type, RawValue);
			return values;
		}

		#region private methods

		private static IEnumerable<object> _GetAllValue(IBlendType arg1, object arg2)
		{
			if (arg2 is object[])
			{
				var objs = (object[])arg2;
				if (objs.Length > 0 && objs[0] is char)
				{
					// Parse as string
					yield return ConvertUtil.CharArray2String(arg2);
				}
				else if (objs.Length > 0 && objs[0] is object[])
				{
					// Parse as 2 dimension array
					foreach (var tmp in objs)
					{
						var tmpArray = (object[])tmp;
						var type = (QualifiedBlendType)arg1;
						foreach (var obj in tmpArray.SelectMany(o => _GetAllValue(type.BaseType, o)))
						{
							yield return obj;
						}
					}
				}
				else
				{
					// Parse as 1 dimension array
					var type = (QualifiedBlendType)arg1;
					foreach (var obj in objs.SelectMany(o => _GetAllValue(type.BaseType, o)))
					{
						yield return obj;
					}
				}
			}
			else
			{
				yield return arg2;
			}
		}

		/*
		virtual private static IEnumerable<object> _GetAllValue(object arg)
		{
			if (arg is object[])
			{
				var objs = (object[])arg;
				if (objs.Length > 0 && objs[0] is char)
				{
					// Parse as string
					yield return ConvertUtil.CharArray2String(arg);
				}
				else if (objs.Length > 0 && objs[0] is object[])
				{
					// Parse as 2 dimension array
					foreach (var tmp in objs)
					{
						var tmpArray = (object[])tmp;
						foreach (var obj in tmpArray.SelectMany(o => _GetAllValue(o)))
						{
							yield return obj;
						}
					}
				}
				else
				{
					foreach (var obj in objs.SelectMany(o => _GetAllValue(o)))
					{
						yield return obj;
					}
				}
			}
			else if (arg is Dictionary<string, object>)
			{
				var objs = (Dictionary<string, object>)arg;
				foreach (var obj in objs.Values.SelectMany(o => _GetAllValue(o)))
				{
					yield return obj;
				}
			}
			else
			{
				yield return arg;
			}
		}
		 * */

		#endregion // private methods
	}
}
