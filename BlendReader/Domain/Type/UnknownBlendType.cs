using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Blender
{
	public class UnknownBlendType : IBlendType
	{
		public UnknownBlendType(string name, int size)
		{
			m_name = name;
			m_size = size;
		}

		/// <summary>
		/// get size of this type
		/// </summary>
		/// <returns>size [byte]</returns>
		public int SizeOf()
		{
			return m_size;
		}

		/// <summary>
		/// Read value corresponded this type from binary
		/// </summary>
		/// <param name="context">variable for making a value</param>
		/// <returns>value</returns>
		/// <seealso cref="IBlendType.ReadValue"/>
		public BlendValueCapsule ReadValue(ReadValueContext context)
		{
			var sb = new StringBuilder("0x");
			for (int byteIndex = 0; byteIndex < m_size; ++byteIndex)
			{
				byte b = context.reader.ReadByte();
				sb.Append(b.ToString("x2"));
			}

			return new BlendValueCapsule(this, sb.ToString());
		}

		/// <summary>
		/// get a name of type
		/// </summary>
		public string Name 
		{
			get
			{
				return m_name;
			}
		}

		public override String ToString()
		{
			return "???";
		}

		public bool Equals(IBlendType type)
		{
			return this == type;
		}

		#region private members

		public string m_name;
		public int m_size;

		#endregion // private members
	}
}
