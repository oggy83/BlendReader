using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Blender
{
	public class BlendAddress
	{
		#region properties 

		/// <summary>
		/// get a pointer value which represents an old memory address
		/// </summary>
		private ulong m_address;
		public ulong Address
		{
			get
			{
				return m_address;
			}
		}

		/// <summary>
		/// get a address as string
		/// </summary>
		public string AddressString
		{
			get
			{
				return IsNull()? "null" : "0x" + m_address.ToString("x8");
			}
		}

		#endregion // properties

		public BlendAddress(ulong address, BlendAddressMapper mapper)
		{
			m_address = address;
			m_mapper = mapper;
		}

		/// <summary>
		/// get whether a given type can dereference this address
		/// </summary>
		/// <param name="baseType">base type (QualifiedBlendType.BaseType)</param>
		/// <returns></returns>
		public bool CanDereference(IBlendType baseType)
		{
			if (!IsNull())
			{
				if (baseType.GetType() != typeof(UnknownBlendType))
				{
					return true;
				}
			}

			return false;
		}

		public BlendValue Dereference(IBlendType type)
		{
			BlendValue result = null;
			try
			{
				int offset = (int)m_address;
				using (var reader = new BinaryReader(m_mapper.GetStreamFromAddress(m_address)))
				{
					var context = new ReadValueContext() { reader = reader, mapper = m_mapper };
					result = type.ReadValue(context);
				}
			}
			catch (Exception e)
			{
				throw new BlenderException("Failed to dereference {0} as {1}", AddressString, type.Name);
			}

			return result;
		}

		public BlendValue Dereference()
		{
			return Dereference(m_mapper.GetHintType(m_address));
		}

		public bool IsNull()
		{
			return Address == 0;
		}

		#region private members

		private BlendAddressMapper m_mapper;

		#endregion // private members
	}
}
