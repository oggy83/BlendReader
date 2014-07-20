using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Blender
{
	/// <summary>
	/// arguments of IBlendType.ReadValue()
	/// </summary>
	public struct ReadValueContext
	{
		/// <summary>
		/// reader object which contains a binary stream
		/// </summary>
		public BinaryReader reader;

		/// <summary>
		/// address mapper for making a BlendAddress
		/// </summary>
		public BlendAddressMapper mapper;
	}

	/// <summary>
	/// Interface of variable type in .blend file
	/// </summary>
	public interface IBlendType : IEquatable<IBlendType>
	{
		/// <summary>
		/// get size of this type
		/// </summary>
		/// <returns>size [byte]</returns>
		int SizeOf();

		/// <summary>
		/// Read value corresponded this type from binary
		/// </summary>
		/// <param name="context">variable for making a value</param>
		/// <returns>value</returns>
		/// <remarks>
		/// this method read SizeOf() bytes from the given binary stream
		/// </remarks>
		BlendValueCapsule ReadValue(ReadValueContext context);

		/// <summary>
		/// get a name of type
		/// </summary>
		string Name { get;  }

	}
}
