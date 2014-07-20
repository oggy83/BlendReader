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
		public IBlendType Type;
		public object RawValue;

		public BlendValueCapsule(IBlendType type, object value)
		{
			Type = type;
			RawValue = value;
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
			yield return RawValue;
		}

		virtual public string GetAllValueAsString()
		{
			return (string)GetAllValue().First();
		}

	}
}
