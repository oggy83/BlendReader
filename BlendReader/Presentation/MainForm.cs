using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blender
{
	public partial class MainForm : Form, IBlenderReaderListener

	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void _OnLoad(object sender, EventArgs e)
		{
			BlendReader.Initialize();
			var app = BlendReader.GetInstance();
			app.SetListener(this);
			//app.OpenFile("C:/blender/test.blend");// test
		}

		#region from IBlendReaderListener

		public void OnLoadDocument(BlendEntityBase[] entities)
		{
			m_sceneTree.Nodes.Clear();// clear old entities

			foreach (var entity in entities)
			{
				var node = new TreeNode(entity.Name);
				if (entity.Children.Count() != 0)
				{
					int index = 0;
					foreach (var child in entity.Children)
					{
						var childNode = new TreeNode(child.Name + " [" + index + "]");
						childNode.Tag = child;
						node.Nodes.Add(childNode);
						
						index++;
					}

					node.Text = entity.Name + " (" + entity.Children[0].Name + " x " + entity.Children.Count() + ")";
				}

				m_sceneTree.Nodes.Add(node);

			}
		}

		#endregion // from IBlendReaderListener

		#region private members

		#endregion // private members

		private void _OnAfterSelectTreeView(object sender, TreeViewEventArgs e)
		{
			var userdata = e.Node.Tag;
			if (userdata != null && userdata is BlendEntityBase)
			{
				var entity = (BlendEntityBase)userdata;
				m_propertyPanel.Attach(entity.Value);
			}
			else
			{
				m_propertyPanel.Attach(null);
			}
		}

		private void _OnDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void _OnDragDrop(object sender, DragEventArgs e)
		{
			var fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);

			var app = BlendReader.GetInstance();
			if (app.OpenFile(fileNames[0]))
			{
				m_toolStripStatusLabel.Text = fileNames[0];
			}
			else
			{
				Dialog.ShowError(new BlenderException("Failed to open file"));
			}
		}

		
	}
}
