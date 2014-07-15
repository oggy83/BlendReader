using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Blender
{
	class OpenFileService
	{
		public struct Request
		{
			/// <summary>
			/// target blend file path
			/// </summary>
			public String FilePath;
		}

		public struct Result
		{
			/// <summary>
			/// entities which created from file
			/// </summary>
			public List<BlockHeaderEntity> EntityList;

			public string ErrorMessage;
		}

		public static bool Execute(Request req, out Result result)
		{
			result = new Result();

			try
			{
				var rep = new BlendTypeRepository();
				var loader = new BlendLoader(rep);

				result.EntityList = loader.FromFile(req.FilePath);
			}
			catch (Exception e)
			{
				return false;
			}

			
			return true;
		}
	}
}
