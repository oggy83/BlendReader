using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Blender
{
	public class BlendTypeRepository
	{
		public BlendTypeRepository()
		{
			int capacity = 1024;
			m_tables = new Dictionary<string, IBlendType>(capacity);
			m_sdnaList = new List<BlendStructureType>(capacity);

			// add all primitive type
			foreach (var type in BlendPrimitiveType.AllTypes())
			{
				Add(type);
			}
		}

		public void Add(IBlendType type)
		{
			if (m_tables.ContainsKey(type.Name))
			{
				Debug.Assert(false, type.Name + "is already added");
				return;
			}

			m_tables[type.Name] = type;

			if (type is BlendStructureType)
			{
				var sType = (BlendStructureType)type;
				_InsertSdnaType(sType);
			}

		}

		public IBlendType Find(string name)
		{
			IBlendType type = null;
			m_tables.TryGetValue(name, out type);
			return type;
		}

		public IBlendType Find(int sdnaIndex)
		{
			return m_sdnaList[sdnaIndex];
		}


		#region private members

		private Dictionary<string, IBlendType> m_tables;
		private List<BlendStructureType> m_sdnaList;

		#endregion // private members

		#region private methods

		private void _InsertSdnaType(BlendStructureType sType)
		{
			int sdnaIndex = sType.SdnaIndex;
			if (m_sdnaList.Count() <= sdnaIndex)
			{
				ListUtil.AppendN(m_sdnaList, null, sdnaIndex - m_sdnaList.Count() + 1);
			}

			m_sdnaList[sdnaIndex] = sType;
		}

		#endregion // private methods
	}
}
