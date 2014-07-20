using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blender
{
	public class PropertyPanel : Panel
	{
		public PropertyPanel()
		{
			InitializeComponent();

			this.treeViewAdv1.LoadOnDemand = true;
			//m_structureViewModel = new StructureViewModel();
			//this.treeViewAdv1.Model = m_structureViewModel;
		}

		public void Attach(BlendValueCapsule value)
		{
			if (value == null)
			{
				this.treeViewAdv1.Model = null;
			}
			else
			{
				m_structureViewModel = new StructureViewModel(value);
				this.treeViewAdv1.Model = m_structureViewModel;
				//m_structureViewModel.Attach(value);
			}
		}

		#region private members

		private Aga.Controls.Tree.TreeViewAdv treeViewAdv1;
		private Aga.Controls.Tree.TreeColumn treeColumn1;
		private Aga.Controls.Tree.TreeColumn treeColumn2;
		private Aga.Controls.Tree.TreeColumn treeColumn3;
		private Aga.Controls.Tree.TreeColumn treeColumn4;

		Aga.Controls.Tree.NodeControls.NodeTextBox _name;
		Aga.Controls.Tree.NodeControls.NodeTextBox _type;
		Aga.Controls.Tree.NodeControls.NodeTextBox _value;
		Aga.Controls.Tree.NodeControls.NodeTextBox _size;

		private StructureViewModel m_structureViewModel = null;

		#endregion // private members

		#region private methods

		private void InitializeComponent()
		{
			this.treeViewAdv1 = new Aga.Controls.Tree.TreeViewAdv();
			this.treeColumn1 = new Aga.Controls.Tree.TreeColumn();
			this.treeColumn2 = new Aga.Controls.Tree.TreeColumn();
			this.treeColumn3 = new Aga.Controls.Tree.TreeColumn();
			this.treeColumn4 = new Aga.Controls.Tree.TreeColumn();
			this._name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this._type = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this._value = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this._size = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.SuspendLayout();
			// 
			// treeViewAdv1
			// 
			this.treeViewAdv1.AllowColumnReorder = true;
			this.treeViewAdv1.BackColor = System.Drawing.SystemColors.Window;
			this.treeViewAdv1.Columns.Add(this.treeColumn1);
			this.treeViewAdv1.Columns.Add(this.treeColumn2);
			this.treeViewAdv1.Columns.Add(this.treeColumn3);
			this.treeViewAdv1.Columns.Add(this.treeColumn4);
			this.treeViewAdv1.Cursor = System.Windows.Forms.Cursors.Default;
			this.treeViewAdv1.DefaultToolTipProvider = null;
			this.treeViewAdv1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewAdv1.DragDropMarkColor = System.Drawing.Color.Black;
			this.treeViewAdv1.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
			this.treeViewAdv1.LineColor = System.Drawing.SystemColors.ControlDark;
			this.treeViewAdv1.Location = new System.Drawing.Point(0, 0);
			this.treeViewAdv1.Model = null;
			this.treeViewAdv1.Name = "treeViewAdv1";
			this.treeViewAdv1.NodeControls.Add(this._name);
			this.treeViewAdv1.NodeControls.Add(this._type);
			this.treeViewAdv1.NodeControls.Add(this._value);
			this.treeViewAdv1.NodeControls.Add(this._size);
			this.treeViewAdv1.SelectedNode = null;
			this.treeViewAdv1.Size = new System.Drawing.Size(626, 406);
			this.treeViewAdv1.TabIndex = 0;
			this.treeViewAdv1.Text = "treeViewAdv1";
			this.treeViewAdv1.UseColumns = true;
			// 
			// treeColumn1
			// 
			this.treeColumn1.Header = "Name";
			this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumn1.Width = 100;
			// 
			// treeColumn2
			// 
			this.treeColumn2.Header = "Type";
			this.treeColumn2.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumn2.Width = 100;
			// 
			// treeColumn3
			// 
			this.treeColumn3.Header = "Value";
			this.treeColumn3.MinColumnWidth = 10;
			this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumn3.Width = 150;
			// 
			// treeColumn4
			// 
			this.treeColumn4.Header = "Size[byte]";
			this.treeColumn4.MaxColumnWidth = 100;
			this.treeColumn4.MinColumnWidth = 100;
			this.treeColumn4.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumn4.Width = 100;

			this._name.DataPropertyName = "Name";
			this._name.IncrementalSearchEnabled = true;
			this._name.LeftMargin = 3;
			this._name.ParentColumn = this.treeColumn1;
			this._name.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
			this._name.UseCompatibleTextRendering = true;

			this._type.DataPropertyName = "Type";
			this._type.IncrementalSearchEnabled = true;
			this._type.LeftMargin = 3;
			this._type.ParentColumn = this.treeColumn2;
			this._type.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
			this._type.UseCompatibleTextRendering = true;

			this._value.DataPropertyName = "Value";
			this._value.IncrementalSearchEnabled = true;
			this._value.LeftMargin = 3;
			this._value.ParentColumn = this.treeColumn3;
			this._value.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
			this._value.UseCompatibleTextRendering = true;

			this._size.DataPropertyName = "Size";
			this._size.IncrementalSearchEnabled = true;
			this._size.LeftMargin = 3;
			this._size.ParentColumn = this.treeColumn4;
			this._size.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
			this._size.UseCompatibleTextRendering = true;

			// 
			// ColumnHandling
			// 
			this.Controls.Add(this.treeViewAdv1);
			this.Name = "ColumnHandling";
			this.Size = new System.Drawing.Size(626, 406);
			this.ResumeLayout(false);
		}

		#endregion // private methods
	}
}
