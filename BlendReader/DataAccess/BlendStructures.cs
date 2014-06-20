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
				new BlendStructureType.MemberDecl("identifier", new QualifiedBlendType(BlendPrimitiveType.Char(), false, 7, 0)),
				new BlendStructureType.MemberDecl("pointer_size", new QualifiedBlendType(BlendPrimitiveType.Char(), false, 0, 0)),
				new BlendStructureType.MemberDecl("endianness", new QualifiedBlendType(BlendPrimitiveType.Char(), false, 0, 0)),
				new BlendStructureType.MemberDecl("version_numbers", new QualifiedBlendType(BlendPrimitiveType.Char(), false, 3, 0))
			});


		public static BlendStructureType FileBlockHeader = 
			new BlendStructureType("FileBlockHeader", new BlendStructureType.MemberDecl[]
			{
				new BlendStructureType.MemberDecl("code", new QualifiedBlendType(BlendPrimitiveType.Char(), false, 4, 0)),
				new BlendStructureType.MemberDecl("size", new QualifiedBlendType(BlendPrimitiveType.Int(), false, 0, 0)),
				new BlendStructureType.MemberDecl("old_memory_address", new QualifiedBlendType(BlendPrimitiveType.Void(), true, 0, 0)),
				new BlendStructureType.MemberDecl("sdna_index", new QualifiedBlendType(BlendPrimitiveType.Int(), false, 0, 0)),
				new BlendStructureType.MemberDecl("count", new QualifiedBlendType(BlendPrimitiveType.Int(), false, 0, 0)),
			});

		
	}
}
