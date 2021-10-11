
namespace NEA
{
    partial class NEA_Proj
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NEA_Proj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 761);
            this.DoubleBuffered = true;
            this.Name = "NEA_Proj";
            this.Text = "NEA";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseProj);
            this.Load += new System.EventHandler(this.LoadProj);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintProj);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NEA_ProjMouseClick);
            this.ResumeLayout(false);

        }

        #endregion
    }
}