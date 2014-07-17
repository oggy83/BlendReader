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
				return new QualifiedBlendType(type, dimCountArray);
			}
		}

		/// <summary>
		/// factory method 
		/// </summary>
		/// <param name="type">base type</param>
		/// <param name="isPointer">is pointer type</param>
		/// <param name="dimCountArray">array of size</param>
		/// <remarks>
		/// if any params do not qualify a base type, this methods just returns it.
		/// see also constructors.
		/// </remarks>
		public static IBlendType From(IBlendType type, bool isPointer, int a, int b)
		{
			if (isPointer)
			{
				type = new BlendPointerType(type);
			}

			if ((a == 0 && b == 0))
			{
				return type;
			}
			else
			{
				return new QualifiedBlendType(type, a, b);
			}
		}
		
	}
}
