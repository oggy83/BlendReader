using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Blender
{
	/// <summary>
	/// Utility for binary operation
	/// </summary>
	public static class BinaryUtil
	{
		public static string ReadAsciiString(BinaryReader reader)
		{
			var sb = new StringBuilder();
			while (true)
			{
				byte b = reader.ReadByte();
				if (b == 0)
				{
					break;
				}

				sb.Append((char)b);
			}

			return sb.ToString();
		}

		public static byte[] ReadBytesFromStream(Stream stream)
		{
			long position = stream.Position;

			var reader = new BinaryReader(stream);
			var result = reader.ReadBytes((int)(stream.Length - position));
			stream.Position = position;

			return result;
		}

	}
}
