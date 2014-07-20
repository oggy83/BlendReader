using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Blender
{
	public class BlockHeaderEntity : BlendEntityBase
	{
		#region properties

		private BlendAddress m_address;
		public BlendAddress OldAddress
		{
			get
			{
				return m_address;
			}
		}

		public String Code
		{
			get
			{
				return Name;
			}
		}

		/// <summary>
		/// get file block size
		/// </summary>
		/// <remarks>block size is the sum of DNA structures size</remarks>
		private int m_size;
		public int Size
		{
			get
			{
				return m_size;
			}
		}

		private int m_sdnaIndex;
		public int SdnaIndex
		{
			get
			{
				return m_sdnaIndex;
			}
		}

		private int m_count;
		public int Count
		{
			get
			{
				return m_count;
			}
		}

		#endregion // properties

		public BlockHeaderEntity(BlendValueCapsule value)
			: base(value.GetMember("code").GetAllValueAsString(), value)
		{
			m_address = (BlendAddress)Value.GetMember("old_memory_address").RawValue;
			m_size = (int)value.GetMember("size").RawValue;
			m_sdnaIndex = (int)value.GetMember("sdna_index").RawValue;
			m_count = (int)value.GetMember("count").RawValue;
		}

		public static BlockHeaderEntity ReadValue(ReadValueContext context)
		{
			var value = BlendStructures.FileBlockHeader.ReadValue(context);
			return new BlockHeaderEntity(value);
		}

	}
}
