namespace Blender
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_statusStrip = new System.Windows.Forms.StatusStrip();
			this.m_toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.m_sceneTree = new Blender.EntityTreeView();
			this.m_propertyPanel = new Blender.PropertyPanel();
			this.m_statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_statusStrip
			// 
			this.m_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_toolStripStatusLabel});
			this.m_statusStrip.Location = new System.Drawing.Point(0, 699);
			this.m_statusStrip.Name = "m_statusStrip";
			this.m_statusStrip.Size = new System.Drawing.Size(949, 22);
			this.m_statusStrip.TabIndex = 0;
			// 
			// m_toolStripStatusLabel
			// 
			this.m_toolStripStatusLabel.Name = "m_toolStripStatusLabel";
			this.m_toolStripStatusLabel.Size = new System.Drawing.Size(150, 17);
			this.m_toolStripStatusLabel.Text = "Drag and drop .blend file";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.m_propertyPanel);
			this.splitContainer1.Size = new System.Drawing.Size(949, 699);
			this.splitContainer1.SplitterDistance = 312;
			this.splitContainer1.TabIndex = 1;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.m_sceneTree);
			this.splitContainer2.Size = new System.Drawing.Size(312, 699);
			this.splitContainer2.SplitterDistance = 25;
			this.splitContainer2.TabIndex = 0;
			// 
			// m_sceneTree
			// 
			this.m_sceneTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_sceneTree.HideSelection = false;
			this.m_sceneTree.Location = new System.Drawing.Point(0, 0);
			this.m_sceneTree.Name = "m_sceneTree";
			this.m_sceneTree.Size = new System.Drawing.Size(312, 670);
			this.m_sceneTree.TabIndex = 0;
			this.m_sceneTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._OnAfterSelectTreeView);
			// 
			// m_propertyPanel
			// 
			this.m_propertyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_propertyPanel.Location = new System.Drawing.Point(0, 0);
			this.m_propertyPanel.Name = "m_propertyPanel";
			this.m_propertyPanel.Size = new System.Drawing.Size(633, 699);
			this.m_propertyPanel.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(949, 721);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.m_statusStrip);
			this.Name = "MainForm";
			this.Text = "BlendReader";
			this.Load += new System.EventHandler(this._OnLoad);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this._OnDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this._OnDragEnter);
			this.m_statusStrip.ResumeLayout(false);
			this.m_statusStrip.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip m_statusStrip;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private EntityTreeView m_sceneTree;
		private PropertyPanel m_propertyPanel;
		private System.Windows.Forms.ToolStripStatusLabel m_toolStripStatusLabel;
	}
}