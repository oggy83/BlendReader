using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blender
{
	/// <summary>
	/// This class contains a pair of value and its type, and access methods to value.
	/// </summary>
	/// <remarks>
	/// The implementation of each access method is provided by sub class which is defined by IBlendType.
	/// </remarks>
	public class BlendValueCapsule
	{
		#region properties

		private IBlendType m_type;
		public IBlendType Type
		{
			get
			{
				return m_type;
			}

		}

		#endregion // properties

		public BlendValueCapsule(IBlendType type, object value)
		{
			m_type = type;
			m_rawValue = value;
		}

		virtual public BlendValueCapsule GetAt(int index)
		{
			throw new BlenderException("BlendValue.GetAt() is not implemented");
		}

		virtual public BlendValueCapsule GetAt(int index1, int index2)
		{
			throw new BlenderException("BlendValue.GetAt() is not implemented");
		}

		virtual public BlendValueCapsule GetMember(string name)
		{
			throw new BlenderException("BlendValue.GetMember() is not implemented");
		}

		virtual public IEnumerable<object> GetAllValue()
		{
			yield return m_rawValue;
		}

		public string GetAllValueAsString()
		{
			return (string)GetAllValue().First();
		}

		public Type GetRawValue<Type>()
		{
			return (Type)m_rawValue;
		}

		public object GetRawValue()
		{
			return m_rawValue;
		}

		#region properties

		private object m_rawValue;

		#endregion // prorperties
	}
}
