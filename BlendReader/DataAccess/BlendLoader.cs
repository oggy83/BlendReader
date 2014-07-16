using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Blender
{
	public class BlendLoader
	{
		public BlendLoader(BlendTypeRepository rep)
		{
			m_repository = rep;
		}

		public List<BlockHeaderEntity> FromFile(string filePath)
		{
			using (var stream = new FileStream(filePath, FileMode.Open))
			{
				return FromMemory(stream);
			}
		}

		public List<BlockHeaderEntity> FromMemory(Stream stream)
		{
			var reader = new BinaryReader(stream);
			var mapper = new BlendAddressMapper(BinaryUtil.ReadBytesFromStream(stream));

			var context = new ReadValueContext() { reader = reader, mapper = mapper };
			var declList = _ReadDna1Block(context);
			//var sortedDeclList = _SortByTypeDependency(declList);

			// before create member type, register a parent type 
			foreach (var decl in declList)
			{
				var parentType = BlendStructureType.CreateImperfect(decl.name, decl.sdnaIndex);
				m_repository.Add(parentType);
			}

			// build member decls and set to parent 
			foreach (var decl in declList)
			{
				var parentType = (BlendStructureType) m_repository.Find(decl.name);

				int fieldCount = decl.fieldNames.Length;
				var memberDecls = new BlendStructureType.MemberDecl[fieldCount];
				for (int fieldIndex = 0; fieldIndex < fieldCount; ++fieldIndex)
				{
					string type = decl.fieldTypes[fieldIndex];
					string name = decl.fieldNames[fieldIndex];
					int tLen = decl.fieldLengths[fieldIndex];

					var typeObj = m_repository.Find(type);
					if (typeObj == null)
					{
						typeObj = new UnknownBlendType(type + " " + "name", tLen);
					}

					memberDecls[fieldIndex] = _ParseType(typeObj, name, tLen);
				}

				// set member decls 
				parentType.SetMemberDecls(memberDecls);
			}

			stream.Seek(0, SeekOrigin.Begin);
			return  _CreateEntityList(context, m_repository);
		}

		#region private types

		private class _StructureDecl
		{
			public string name;
			public int sdnaIndex;
			public int size;
			//public _BlendTypeFactory[] fieldTypeFactories;
			public string[] fieldTypes;
			public int[] fieldLengths;
			public string[] fieldNames;
		}

		private class _StructureDeclBoolTuple
		{
			public _StructureDecl Item1;
			public bool Item2;

			public _StructureDeclBoolTuple(_StructureDecl item1, bool item2)
			{
				Item1 = item1;
				Item2 = item2;
			}
		}

		#endregion // private types

		#region private members

		private BlendTypeRepository m_repository;

		#endregion // private members

		#region private methods

		private static List<_StructureDecl> _ReadDna1Block(ReadValueContext context)
		{
			var declList = new List<_StructureDecl>();
			var header = BlendStructures.GlobalHeader.ReadValue(context);
			if ((char)header.GetMember("pointer_size").RawValue == '_')
			{
				throw new BlenderException("32bit pointer-size is unsupported");
			}

			if ((char)header.GetMember("endianness").RawValue == 'V')
			{
				throw new BlenderException("big endian is unsupported");
			}

			while (true)
			{
				var fileBlock = BlendStructures.FileBlockHeader.ReadValue(context);
				var code = ConvertUtil.CharArray2String(fileBlock.GetMember("code").RawValue);
				int size = (int)fileBlock.GetMember("size").RawValue;

				if (code == "DNA1")
				{
					context.reader.ReadBytes(4);// SDNA
					context.reader.ReadBytes(4);// NAME

					int nameCount = context.reader.ReadInt32();
					var names = new string[nameCount];
					int readByteCount = 0;
					for (int index = 0; index < nameCount; ++index)
					{
						string s = BinaryUtil.ReadAsciiString(context.reader);
						names[index] = s;
						readByteCount += (s.Length + 1);
					}

					context.reader.ReadBytes(readByteCount * 3 % 4);// align
					context.reader.ReadBytes(4);// TYPE

					int typeCount = context.reader.ReadInt32();
					readByteCount = 0;
					var types = new string[typeCount];
					for (int index = 0; index < typeCount; ++index)
					{
						string s = BinaryUtil.ReadAsciiString(context.reader);
						types[index] = s;
						readByteCount += (s.Length + 1);
					}

					context.reader.ReadBytes(readByteCount * 3 % 4);// align
					context.reader.ReadBytes(4);// TLEN

					readByteCount = 0;
					var tLens = new int[typeCount];
					for (int index = 0; index < typeCount; ++index)
					{
						int len = context.reader.ReadUInt16();
						tLens[index] = len;
						readByteCount += 2;
					}

					context.reader.ReadBytes(readByteCount * 3 % 4);// align
					context.reader.ReadBytes(4);// STRC

					int structCount = context.reader.ReadInt32();
					readByteCount = 0;
					for (int index = 0; index < structCount; ++index)
					{
						var decl = new _StructureDecl();

						int typeIndex = context.reader.ReadInt16();
						decl.name = types[typeIndex];
						decl.sdnaIndex = index;
						decl.size = tLens[typeIndex];

						int fieldCount = context.reader.ReadInt16();
						decl.fieldTypes = new string[fieldCount];
						decl.fieldLengths = new int[fieldCount];
						decl.fieldNames = new string[fieldCount];
						for (int fieldIndex = 0; fieldIndex < fieldCount; ++fieldIndex)
						{
							int fieldTypeIndex = context.reader.ReadInt16();
							int fieldNameIndex = context.reader.ReadInt16();

							decl.fieldTypes[fieldIndex] = types[fieldTypeIndex];
							decl.fieldLengths[fieldIndex] = tLens[fieldTypeIndex];
							decl.fieldNames[fieldIndex] = names[fieldNameIndex];
						}

						declList.Add(decl);
					}
				}
				else if (code == "ENDB")
				{
					// END
					break;
				}
				else
				{
					// Skip
					context.reader.ReadBytes(size);
				}
			}

			return declList;
		}

		/*
		private List<_StructureDecl> _SortByTypeDependency(List<_StructureDecl> declList)
		{
			int capacity = declList.Count();
			var result = new List<_StructureDecl>(capacity);
			var tmpTable = new Dictionary<String, _StructureDeclBoolTuple>(capacity);
			foreach (var decl in declList)
			{
				tmpTable.Add(decl.name, new _StructureDeclBoolTuple(decl, false));
			}

			foreach (var v in tmpTable.Values)
			{
				_RegisterTypeRecursively(v, tmpTable, result);
			}

			return result;
		}

		private void _RegisterTypeRecursively(_StructureDeclBoolTuple target, Dictionary<String, _StructureDeclBoolTuple> tmpTable, List<_StructureDecl> result)
		{
			if (target.Item2)
			{
				// already registered
				return;
			}

			// call recursively for each member 
			foreach (var type in target.Item1.fieldTypes)
			{
				if (type == target.Item1.name)
				{
					continue;
				}

				if (m_repository.Find(type) != null)
				{
					// primitive type
					continue;
				}

				_StructureDeclBoolTuple tmpValue = null;
				if (!tmpTable.TryGetValue(type, out tmpValue))
				{
					// Error?
					continue;
				}

				_RegisterTypeRecursively(tmpValue, tmpTable, result);
			}

			target.Item2 = true;
			result.Add(target.Item1);
		}
		*/

		/// <summary>
		/// Create a .blend type from SDNA member name with type qualifiers
		/// </summary>
		/// <param name="type">.blend type</param>
		/// <param name="name">member name</param>
		/// <param name="tLen">size of type [byte]</param>
		/// <remarks>
		/// qualifier of array and pointer is contained in SDNA member name.
		/// this is inconvenient to make domain classes.
		/// this function moves these qualifiers from name to type.
		/// </remarks>
		private static BlendStructureType.MemberDecl _ParseType(IBlendType type, string name, int tLen)
		{
			string description = type.Name + " " + name;
			Match match = null;

			match = FunctionPointerRegex.Match(name, 0);
			if (match.Success)
			{
				// function pointer is unsupported 
				int ptSize = QualifiedBlendType.GetPointerSizeOf();
				return new BlendStructureType.MemberDecl(match.Groups[1].Value, new UnknownBlendType(description, ptSize));
			}

			match = PointerAndNameRegex.Match(name, 0);
			if (match.Success)
			{
				string pointers = match.Groups[1].Value;
				string baseName = match.Groups[2].Value;

				// parse pointer qualifiers
				bool isPointer = (pointers.Length >= 1);
				if (pointers.Length >= 2)
				{
					// double pointer is unsupported
					int ptSize = QualifiedBlendType.GetPointerSizeOf();
					return new BlendStructureType.MemberDecl(baseName, new UnknownBlendType(description, ptSize));
				}
				
				// parse array qualifiers
				List<int> dimCountArray = new List<int>();
				var matches = ArraySizeRegex.Matches(name, match.Length);
				foreach (Match m in matches)
				{
					string size = m.Groups[1].Value;
					dimCountArray.Add(int.Parse(size));
				}

				var resultType = QualifiedBlendType.From(type, isPointer, dimCountArray.ToArray());
				return new BlendStructureType.MemberDecl(baseName, resultType);
			}
			else
			{
				// unknown
				return new BlendStructureType.MemberDecl(name, new UnknownBlendType(description, tLen));
			}

		}

		
		private static Regex FunctionPointerRegex = new Regex(@"^\(\*(\w+)\)\(\)$");
		private static Regex ArraySizeRegex = new Regex(@"\[(\d+)\]");
		private static Regex PointerAndNameRegex = new Regex(@"^(\**)(\w+)");

		private static List<BlockHeaderEntity> _CreateEntityList(ReadValueContext context, BlendTypeRepository repository)
		{
			var result = new List<BlockHeaderEntity>();

			BlendStructures.GlobalHeader.ReadValue(context);
			while (true)
			{
				var blockEntity = BlockHeaderEntity.ReadValue(context);
				switch (blockEntity.Code)
				{
					case "DNA1":
						// skip
						result.Add(blockEntity);
						context.reader.ReadBytes(blockEntity.Size);
						break;

					case "ENDB":
						// end of file
						result.Add(blockEntity);
						return result;

					case "REND": // RenderInfo
					case "TEST": // Preview Image
						// skip
						result.Add(blockEntity);
						context.reader.ReadBytes(blockEntity.Size);
						break;

					default:
						{
							var type = repository.Find(blockEntity.SdnaIndex);

							// register address mapping
							int sdnaSize = blockEntity.Count * type.SizeOf();
							context.mapper.AddEntry(blockEntity.OldAddress.Address, (int)context.reader.BaseStream.Position, blockEntity.Size, type);
							
							if (blockEntity.Count == 1 && blockEntity.SdnaIndex == 0 && blockEntity.Size != type.SizeOf())
							{
								// Error? skip
								result.Add(blockEntity);
								context.reader.ReadBytes(blockEntity.Size);
							}
							else
							{
								Debug.Assert(type.SizeOf() * blockEntity.Count == blockEntity.Size, "structure size unmatched");
								for (int i = 0; i < blockEntity.Count; ++i)
								{
									var value = type.ReadValue(context);
									blockEntity.Children.Add(new BlendEntityBase(value.Type.Name, value));
								}

								result.Add(blockEntity);

							}

						}
						break;
				
				}
			}

		}

		#endregion // private methods

	}
}
