using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blender
{
	public class BlenderException : Exception
	{
		public BlenderException(string message)
		: base(message)
		{
			// nothing
		}

		public BlenderException(string format, params object[] args)
			: base(String.Format(format, args))
		{
			// nothing
		}

	}
}
