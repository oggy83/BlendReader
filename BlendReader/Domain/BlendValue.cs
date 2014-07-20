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
			return BlendArrayType.GetAt(this, index);
		}

		virtual public BlendValue GetAt(int index1, int index2)
		{
			return BlendArrayType.GetAt(this, index1,index2);
		}

		virtual public BlendValue GetMember(string name)
		{
			return BlendStructureType.GetMemberValue(this, name);
		}

		virtual public IEnumerable<object> GetAllValue()
		{
			if (RawValue is Dictionary<string, BlendValue>)
			{
				return BlendStructureType.GetAllRawValue(this);
			}
			else if (RawValue is object[])
			{
				return BlendArrayType.GetAllRawValue(this);
			}
			else
			{
				var l = new List<object>();
				l.Add(RawValue);
				return l;
			}
		}

		virtual public string GetAllValueAsString()
		{
			return (string)GetAllValue().First();
		}

	}
}
