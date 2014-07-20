using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blender
{
	public static class BlendTypeFactory
	{
		/// <summary>
		/// factory method 
		/// </summary>
		/// <param name="type">base type</param>
		/// <param name="isPointer">is pointer type</param>
		/// <param name="dimCountArray">array of size</param>
		/// <remarks>
		/// if any params do not qualify a base type, this methods just returns it.
		/// see also constructors.
		/// If you do not want to an array type, set dim1Count and dim2Count to 0.
		/// Array and pointer type meanns an array of pointer type.
		/// </remarks>
		public static IBlendType From(IBlendType type, bool isPointer, int[] dimCountArray)
		{
			if (isPointer)
			{
				type = new BlendPointerType(type);
			}

			if ((dimCountArray == null || dimCountArray.Length == 0))
			{
				return type;
			}
			else
			{
				return new BlendArrayType(type, dimCountArray);
			}
		}

		
	}
}
