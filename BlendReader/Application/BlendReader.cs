using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Blender
{
	public class BlendReader
	{
		#region properties

		#endregion // properties

		private  BlendReader()
		{
			// nothing
		}

		public void SetListener(IBlenderReaderListener listener)
		{
			m_listener = listener;
		}

		/// <summary>
		/// open a blend file and load entities
		/// </summary>
		/// <param name="filePath">input .blend file path</param>
		/// <returns>success/failed</returns>
		/// <remarks>
		/// in case of success, this methods calls IBlendReaderListener.OnLoadDocument()
		/// </remarks>
		public bool OpenFile(string filePath)
		{
			OpenFileService.Request req;
			req.FilePath = filePath;
			OpenFileService.Result result;
			if (OpenFileService.Execute(req, out result))
			{
				m_listener.OnLoadDocument(result.EntityList.ToArray());
				return true;
			}

			// failed
			return false;
		}

		public static void Initialize()
		{
			Debug.Assert(s_app == null);
			s_app = new BlendReader();
		}

		public static BlendReader GetInstance()
		{
			return s_app;
		}

		#region private members

		private IBlenderReaderListener m_listener = null;

		private static BlendReader s_app = null;

		#endregion // private members
	}
}
