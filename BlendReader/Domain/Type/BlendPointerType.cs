using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Blender
{
	/// <summary>
	/// This class represents pointer-type.
	/// </summary>
	public class BlendPointerType : IBlendType
	{
		#region properties 

		/// <summary>
		/// get a name of type
		/// </summary>
		public string Name
		{
			get
			{
				return m_baseType.ToString() + "*";
			}
		}

		/// <summary>
		/// get a pre-qualified type
		/// </summary>
		private IBlendType m_baseType;
		public virtual IBlendType BaseType
		{
			get
			{
				return m_baseType;
			}
		}

		#endregion // properties

		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="type">base type</param>
		public BlendPointerType(IBlendType baseType)
		{
			Debug.Assert(baseType != null, "type must not be null");

			m_baseType = baseType;
		}

		/// <summary>
		/// get size of this type
		/// </summary>
		/// <returns>size [byte]</returns>
		public int SizeOf()
		{
			return GetPointerSizeOf();
		}

		public String ToString()
		{
			return Name;
		}

		public bool Equals(IBlendType type)
		{
			var thisType = type as BlendPointerType;
			if (thisType == null)
			{
				// type unmatched
				return false;
			}

			return m_baseType.Equals(thisType.m_baseType);
		}

		public static int GetPointerSizeOf()
		{
			// assumes 64bit environment
			return 8;
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
			if (GetPointerSizeOf() == 4)
			{
				// 32bit
				obj = new BlendAddress(context.reader.ReadUInt32(), context.mapper);
			}
			else
			{
				// 64bit
				obj = new BlendAddress(context.reader.ReadUInt64(), context.mapper);
			}

			return new BlendValue(this, obj);
		}
		
	}
}
