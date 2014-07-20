using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace Blender
{
	public static class Dialog
	{
		public static void ShowError(BlenderException e)
		{
#if DEBUG
			Debugger.Break();
#endif
			MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
