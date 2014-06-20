using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Blender
{
	public static class ListUtil
	{

		public static void AppendN<Type>(List<Type> list, Type v, int n)
		{
			if (n <= 0)
			{
				return;
			}

			for (int i = 0; i < n; ++i)
			{
				list.Add(v);
			}
		}

		public static ReadOnlyCollection<T> CreateReadOnlyCollection<T>()
		{
			return new ReadOnlyCollection<T>(new T[0]);
		}
	}
}
