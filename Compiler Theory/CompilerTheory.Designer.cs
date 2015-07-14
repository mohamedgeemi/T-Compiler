namespace Compiler_Theory
{
    partial class CompilerTheory
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
            this.Phase1 = new System.Windows.Forms.Button();
            this.Phase3 = new System.Windows.Forms.Button();
            this.Phase2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Phase1
            // 
            this.Phase1.BackColor = System.Drawing.Color.Gray;
            this.Phase1.Font = new System.Drawing.Font("Tempus Sans ITC", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Phase1.Location = new System.Drawing.Point(101, 36);
            this.Phase1.Name = "Phase1";
            this.Phase1.Size = new System.Drawing.Size(447, 51);
            this.Phase1.TabIndex = 0;
            this.Phase1.Text = "Phase 1: Scanner Implementation.";
            this.Phase1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Phase1.UseVisualStyleBackColor = false;
            this.Phase1.Click += new System.EventHandler(this.Phase1_Click);
            // 
            // Phase3
            // 
            this.Phase3.BackColor = System.Drawing.Color.Gray;
            this.Phase3.Font = new System.Drawing.Font("Tempus Sans ITC", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Phase3.Location = new System.Drawing.Point(101, 150);
            this.Phase3.Name = "Phase3";
            this.Phase3.Size = new System.Drawing.Size(447, 51);
            this.Phase3.TabIndex = 1;
            this.Phase3.Text = "Phase 3: Semantic Analysis Implementation.";
            this.Phase3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Phase3.UseVisualStyleBackColor = false;
            this.Phase3.Click += new System.EventHandler(this.Phase3_Click);
            // 
            // Phase2
            // 
            this.Phase2.BackColor = System.Drawing.Color.Gray;
            this.Phase2.Font = new System.Drawing.Font("Tempus Sans ITC", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Phase2.Location = new System.Drawing.Point(101, 93);
            this.Phase2.Name = "Phase2";
            this.Phase2.Size = new System.Drawing.Size(447, 51);
            this.Phase2.TabIndex = 2;
            this.Phase2.Text = "Phase 2: Parser Implementation.";
            this.Phase2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Phase2.UseVisualStyleBackColor = false;
            this.Phase2.Click += new System.EventHandler(this.Phase2_Click);
            // 
            // CompilerTheory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Compiler_Theory.Properties.Resources.Compiler_Theory;
            this.ClientSize = new System.Drawing.Size(644, 267);
            this.Controls.Add(this.Phase2);
            this.Controls.Add(this.Phase3);
            this.Controls.Add(this.Phase1);
            this.MaximumSize = new System.Drawing.Size(660, 306);
            this.MinimumSize = new System.Drawing.Size(660, 306);
            this.Name = "CompilerTheory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CompilerTheory";
            this.Load += new System.EventHandler(this.CompilerTheory_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Phase1;
        private System.Windows.Forms.Button Phase3;
        private System.Windows.Forms.Button Phase2;
    }
}