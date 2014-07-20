using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Blender
{
	public class BlenderException : Exception
	{
		public BlenderException(string message)
		: base(message)
		{
#if DEBUG
			Debugger.Break();
#endif
		}

		public BlenderException(string format, params object[] args)
			: base(String.Format(format, args))
		{
#if DEBUG
			Debugger.Break();
#endif
		}

	}
}
