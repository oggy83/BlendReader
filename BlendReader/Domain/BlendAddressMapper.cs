using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Blender
{
	/// <summary>
	/// This class maps old memory address to range of .blend binary
	/// </summary>
	public class BlendAddressMapper
	{
		public BlendAddressMapper(byte[] binary)
		{
			m_binary = binary;
			m_map = new Dictionary<ulong, Tuple<int, int, IBlendType>>();
		}

		/// <summary>
		/// add entry of mapping
		/// </summary>
		/// <param name="address">old memory address</param>
		/// <param name="offset">offset from the top of .blend binary</param>
		/// <param name="size">entry binary size [byte]</param>
		/// <param name="type">hint type for dereference</param>
		public void AddEntry(ulong address, int offset, int size, IBlendType type)
		{
			Debug.Assert(m_binary.Length >= (offset + size), "size is too big");
			m_map.Add(address, Tuple.Create(offset, size, type));
		}

		/// <summary>
		/// get a binary stream beginning from a given address
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public Stream GetStreamFromAddress(ulong address)
		{
			Tuple<int, int, IBlendType> tmp;
			if (!m_map.TryGetValue(address, out tmp))
			{
				return null;
			}

			return new MemoryStream(m_binary, tmp.Item1, tmp.Item2); 
		}

		/// <summary>
		/// get hint type for dereference
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public IBlendType GetHintType(ulong address)
		{
			Tuple<int, int, IBlendType> tmp;
			if (!m_map.TryGetValue(address, out tmp))
			{
				return null;
			}

			return tmp.Item3;
		}

		#region private members

		private byte[] m_binary;

		private Dictionary<ulong, Tuple<int, int, IBlendType>> m_map;

		#endregion // private members
	}
}
