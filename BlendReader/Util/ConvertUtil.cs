using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Blender
{
	public static class ConvertUtil
	{
		public static String CharArray2String(object obj)
		{
			var sb = new StringBuilder();
			object[] array = (object[])obj;
			foreach (object o in array)
			{
				char? c = (char?)o;
				if (c == '\0')
				{
					break;
				}

				sb.Append(c);
			}

			return sb.ToString();
		}

		

	}
}
