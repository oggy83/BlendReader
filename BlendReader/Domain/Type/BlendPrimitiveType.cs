using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Blender
{
	public class BlendPrimitiveType : IBlendType
	{
		#region public type

		public enum BaseTypes
		{
			Char = 0,
			Short,
			Int,
			Float,
			Void,
		}

		private static String[] m_toStringTable = {
			"char",
			"short",
			"int",
			"float",
			"void",
		};

		private static int[] m_sizeOfTable = {
			1,
			2,
			4,
			4,
			0,
		};

		public BaseTypes Base
		{
			get
			{
				return m_type;
			}
		}

		#endregion // public types

		public BlendPrimitiveType(BaseTypes type)
		{
			m_type = type;
		}

		/// <summary>
		/// get size of this type
		/// </summary>
		/// <returns>size [byte]</returns>
		public int SizeOf()
		{
			return m_sizeOfTable[(int)m_type];
		}

		/// <summary>
		/// Read value corresponded this type from binary
		/// </summary>
		/// <param name="context">variable for making a value</param>
		/// <returns>value</returns>
		/// <seealso cref="IBlendType.ReadValue"/>
		public BlendValue ReadValue(ReadValueContext context)
		{
			object obj = null;
			switch (m_type)
			{
				case BaseTypes.Char:
					obj = (char)context.reader.ReadByte();
					break;

				case BaseTypes.Short:
					obj = (short)context.reader.ReadInt16();
					break;

				case BaseTypes.Int:
					obj = (int)context.reader.ReadInt32();
					break;

				case BaseTypes.Float:
					obj = (float)context.reader.ReadSingle();
					break;

				default:
					Debug.Assert(false, "unsupported type " + m_toStringTable[(int)m_type]);
					break;
			}

			return new BlendValue(this, obj);
		}

		/// <summary>
		/// get a name of type
		/// </summary>
		public string Name
		{
			get
			{
				return m_toStringTable[(int)m_type];
			}
		}


		public override String ToString()
		{
			return m_toStringTable[(int)m_type];
		}

		public bool Equals(IBlendType type)
		{
			var primitive = type as BlendPrimitiveType;
			if (primitive == null)
			{
				// type unmatched
				return false;	
			}

			return m_type == primitive.m_type;
		}

		public static BlendPrimitiveType Char()
		{
			return new BlendPrimitiveType(BaseTypes.Char);
		}

		public static BlendPrimitiveType Short()
		{
			return new BlendPrimitiveType(BaseTypes.Short);
		}

		public static BlendPrimitiveType Int()
		{
			return new BlendPrimitiveType(BaseTypes.Int);
		}

		public static BlendPrimitiveType Float()
		{
			return new BlendPrimitiveType(BaseTypes.Float);
		}

		public static BlendPrimitiveType Void()
		{
			return new BlendPrimitiveType(BaseTypes.Void);
		}

		public static BlendPrimitiveType[] AllTypes()
		{
			return new BlendPrimitiveType[] 
			{
				Char(),
				Short(),
				Int(),
				Float(),
				Void()
			};
		}

		#region private members

		BaseTypes m_type;

		#endregion // private members
	}
}
