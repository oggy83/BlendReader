using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Blender
{
	/// <summary>
	/// This class maps old memory address to offset from the top of .blend binary
	/// </summary>
	public class BlendAddressMapper
	{
		public BlendAddressMapper(byte[] binary)
		{
			m_binry = binary;
			m_map = new Dictionary<ulong, Tuple<int, IBlendType>>();
		}

		/// <summary>
		/// add entry of mapping
		/// </summary>
		/// <param name="address">old memory address</param>
		/// <param name="offset">offset from the top of .blend binary</param>
		/// <param name="type">hint type for dereference</param>
		public void AddEntry(ulong address, int offset, IBlendType type)
		{
			m_map.Add(address, Tuple.Create(offset, type));
		}

		/// <summary>
		/// get a binary stream beginning from a given address
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public Stream GetStreamFromAddress(ulong address)
		{
			Tuple<int, IBlendType> tmp;
			if (!m_map.TryGetValue(address, out tmp))
			{
				return null;
			}

			return new MemoryStream(m_binry, tmp.Item1, m_binry.Length - tmp.Item1); 
		}

		/// <summary>
		/// get hint type for dereference
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public IBlendType GetHintType(ulong address)
		{
			Tuple<int, IBlendType> tmp;
			if (!m_map.TryGetValue(address, out tmp))
			{
				return null;
			}

			return tmp.Item2;
		}

		#region private members

		private byte[] m_binry;

		private Dictionary<ulong, Tuple<int, IBlendType>> m_map;

		#endregion // private members
	}
}
