namespace Compiler_Theory
{
    partial class Semantic
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.FileDataBox = new System.Windows.Forms.TextBox();
            this.SemanticTreeView = new System.Windows.Forms.TreeView();
            this.AddFileButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ExpandAllCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ExpandAllCheckBox);
            this.groupBox1.Controls.Add(this.RunButton);
            this.groupBox1.Controls.Add(this.FileDataBox);
            this.groupBox1.Controls.Add(this.SemanticTreeView);
            this.groupBox1.Controls.Add(this.AddFileButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1190, 655);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Semantic";
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(213, 26);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(201, 23);
            this.RunButton.TabIndex = 6;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // FileDataBox
            // 
            this.FileDataBox.Location = new System.Drawing.Point(6, 55);
            this.FileDataBox.Multiline = true;
            this.FileDataBox.Name = "FileDataBox";
            this.FileDataBox.Size = new System.Drawing.Size(408, 591);
            this.FileDataBox.TabIndex = 4;
            // 
            // SemanticTreeView
            // 
            this.SemanticTreeView.Location = new System.Drawing.Point(420, 41);
            this.SemanticTreeView.Name = "SemanticTreeView";
            this.SemanticTreeView.Size = new System.Drawing.Size(749, 608);
            this.SemanticTreeView.TabIndex = 3;
            // 
            // AddFileButton
            // 
            this.AddFileButton.Location = new System.Drawing.Point(6, 26);
            this.AddFileButton.Name = "AddFileButton";
            this.AddFileButton.Size = new System.Drawing.Size(201, 23);
            this.AddFileButton.TabIndex = 0;
            this.AddFileButton.Text = "Add File";
            this.AddFileButton.UseVisualStyleBackColor = true;
            this.AddFileButton.Click += new System.EventHandler(this.AddFileButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ExpandAllCheckBox
            // 
            this.ExpandAllCheckBox.AutoSize = true;
            this.ExpandAllCheckBox.Location = new System.Drawing.Point(420, 18);
            this.ExpandAllCheckBox.Name = "ExpandAllCheckBox";
            this.ExpandAllCheckBox.Size = new System.Drawing.Size(134, 17);
            this.ExpandAllCheckBox.TabIndex = 7;
            this.ExpandAllCheckBox.Text = "Expand All Tree Nodes";
            this.ExpandAllCheckBox.UseVisualStyleBackColor = true;
            this.ExpandAllCheckBox.CheckedChanged += new System.EventHandler(this.ExpandAllCheckBox_CheckedChanged);
            // 
            // Semantic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 681);
            this.Controls.Add(this.groupBox1);
            this.Name = "Semantic";
            this.Text = "Semantic";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TreeView SemanticTreeView;
        private System.Windows.Forms.TextBox FileDataBox;
        private System.Windows.Forms.Button AddFileButton;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.CheckBox ExpandAllCheckBox;
    }
}