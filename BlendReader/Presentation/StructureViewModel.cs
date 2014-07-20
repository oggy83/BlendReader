using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blender
{
	/// <summary>
	/// this class represents a view model for PropertyPanel
	/// </summary>
	public class StructureViewModel : Aga.Controls.Tree.ITreeModel
	{
		#region events

		public event EventHandler<Aga.Controls.Tree.TreeModelEventArgs> NodesChanged = (e, sender) => { };

		public event EventHandler<Aga.Controls.Tree.TreeModelEventArgs> NodesInserted = (e, sender) => { };

		public event EventHandler<Aga.Controls.Tree.TreeModelEventArgs> NodesRemoved = (e, sender) => { };

		public event EventHandler<Aga.Controls.Tree.TreePathEventArgs> StructureChanged = (e, sender) => { };

		#endregion // events

		public StructureViewModel(BlendValueCapsule value)
		{
			_Attach(value);
		}

		public System.Collections.IEnumerable GetChildren(Aga.Controls.Tree.TreePath treePath)
		{
			_NodeModel node = null;
			if (treePath.IsEmpty())
			{
				if (m_rootNode != null)
				{
					node = m_rootNode;
				}
			}
			else
			{
				node = (_NodeModel)treePath.LastNode;
			}

			if (node != null)
			{
				if (node.IsStructure)
				{
					// scan members
					var type = (BlendStructureType)node.m_value.Type;
					foreach (var decl in type.MemberDecls)
					{
						var value = node.m_value.GetMember(decl.Name);
						var childNode = new _NodeModel(decl.Name, value);
						yield return childNode;
					}
				}
				else if (node.IsPointer)
				{
					// dereference
					var address = (BlendAddress)node.m_value.RawValue;
					var pointerType = (BlendPointerType)node.m_value.Type;
					var baseType = pointerType.BaseType;
					if (address.CanDereference(baseType))
					{
						List<BlendValueCapsule> result = null;
						try
						{
							if (baseType.Equals(BlendPrimitiveType.Void()))
							{
								// void pointer
								result = address.DereferenceAll();
							}
							else
							{
								result = address.DereferenceAll(baseType);
							}
						}
						catch (BlenderException e)
						{
							Dialog.ShowError(e);
							yield break;
						}

						if (result.Count == 1)
						{
							var childNode = new _NodeModel("*" + node.Name, result[0]);
							yield return childNode;
						}
						else 
						{
							// If the number of dereferenced objects is over 1, we show it as an array.
							for (int i = 0; i < result.Count; ++i)
							{
								var value = result[i];
								var childNode = new _NodeModel(node.Name + "[" + i + "]", value);
								yield return childNode;
							}
						}
					}
				}
				else if (node.IsArray)
				{
					var arrayType = (BlendArrayType)node.m_value.Type;
					int arrayCount = arrayType.ArrayDimension;
					switch (arrayCount)
					{
						case 1 :
							for (int i = 0; i < arrayType.GetLength(0); ++i)
							{
								var value = node.m_value.GetAt(i);
								var childNode = new _NodeModel(node.Name + "[" + i + "]", value);
								yield return childNode;
							}
							break;


						case 2 :
							for (int i = 0; i < arrayType.GetLength(0); ++i)
							{
								for (int j = 0; j < arrayType.GetLength(1); ++j)
								{
									var value = node.m_value.GetAt(i, j);
									var childNode = new _NodeModel(node.Name + "[" + i + "]" + "[" + j + "]", value);
									yield return childNode;
								}
							}
							break;
					}
					
				}
			}

			yield break;
		}

		public bool IsLeaf(Aga.Controls.Tree.TreePath treePath)
		{
			var node = (_NodeModel) treePath.LastNode;
			return !(node.IsStructure || node.IsPointer || node.IsArray);
		}

		
		#region private types

		public class _NodeModel
		{
			#region properties

			public string Name
			{
				get
				{
					return m_name;
				}
			}

			public string Type
			{
				get
				{
					return m_value.Type.Name;
				}
			}

			public string Value
			{
				get
				{
					return m_cacheValue;
				}
			}

			public string Size
			{
				get
				{
					return m_value.Type.SizeOf().ToString();
				}
			}

			public bool IsPointer
			{
				get
				{
					return m_value.Type is BlendPointerType;
				}
			}

			public bool IsArray
			{
				get
				{
					return m_value.Type is BlendArrayType;
				}
			}

			public bool IsStructure
			{
				get
				{
					if (m_value.Type is BlendStructureType)
					{
						return true;
					}

					return false;
				}
			}

			#endregion // properties

			public _NodeModel(String name, BlendValueCapsule value)
			{
				m_name = name;
				m_value = value;

				var sb = new StringBuilder();
				var values = m_value.GetAllValue();
				m_cacheValue = string.Join(", ", values.Select(val => _GetString(val)));
			}


			#region private members

			private String m_name;
			public BlendValueCapsule m_value;// temp
			private string m_cacheValue;

			#endregion // private members

			#region private methods

			private string _GetString(object o)
			{
				if (o is string)
				{
					return "\"" + o.ToString() + "\"";
				}
				else if (o is char)
				{
					return Convert.ToByte((char)o).ToString();
				}
				else if (o is BlendAddress)
				{
					var address = (BlendAddress)o;
					return address.AddressString;
				}
				else
				{
					return o.ToString();
				}
			}

			#endregion // private methods
		}

		#endregion // private types

		#region private members

		private _NodeModel m_rootNode = null;

		#endregion // private members

		#region private methods

		public void _Attach(BlendValueCapsule value)
		{
			m_rootNode = new _NodeModel("ROOT", value);
		}


		#endregion // private methods
	}
}
