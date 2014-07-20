using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace Blender
{
	public static class BlendStructures
	{
		public static BlendStructureType GlobalHeader = 
			new BlendStructureType("GlobalHeader", new BlendStructureType.MemberDecl[] 
			{
				new BlendStructureType.MemberDecl("identifier", new BlendArrayType(BlendPrimitiveType.Char(), 7, 0)),
				new BlendStructureType.MemberDecl("pointer_size", BlendPrimitiveType.Char()),
				new BlendStructureType.MemberDecl("endianness", BlendPrimitiveType.Char()),
				new BlendStructureType.MemberDecl("version_numbers", new BlendArrayType(BlendPrimitiveType.Char(), 3, 0))
			});


		public static BlendStructureType FileBlockHeader = 
			new BlendStructureType("FileBlockHeader", new BlendStructureType.MemberDecl[]
			{
				new BlendStructureType.MemberDecl("code", new BlendArrayType(BlendPrimitiveType.Char(), 4, 0)),
				new BlendStructureType.MemberDecl("size", BlendPrimitiveType.Int()),
				new BlendStructureType.MemberDecl("old_memory_address", new BlendPointerType(BlendPrimitiveType.Void())),
				new BlendStructureType.MemberDecl("sdna_index", BlendPrimitiveType.Int()),
				new BlendStructureType.MemberDecl("count", BlendPrimitiveType.Int()),
			});

		
	}
}
