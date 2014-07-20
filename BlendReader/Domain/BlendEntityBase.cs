using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blender
{
	public class BlendEntityBase
	{
		#region properties

		private String m_name;
		public String Name
		{
			get
			{
				return m_name;
			}
		}

		private BlendValueCapsule m_value;
		public BlendValueCapsule Value
		{
			get
			{
				return m_value;
			}
		}

		private List<BlendEntityBase> m_children;
		public List<BlendEntityBase> Children
		{
			get
			{
				return m_children;
			}
		}

		#endregion // properties

		public BlendEntityBase(String name, BlendValueCapsule value)
		{
			m_name = name;
			m_value = value;
			m_children = new List<BlendEntityBase>();
		}


	}
}
