using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blender
{
	public interface IBlenderReaderListener
	{
		/// <summary>
		/// on create entities from file
		/// </summary>
		/// <param name="entities">entities which represents a file document</param>
		void OnLoadDocument(BlockHeaderEntity[] entities);
	}
}
