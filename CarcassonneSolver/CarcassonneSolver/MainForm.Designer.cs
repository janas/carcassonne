namespace CarcassonneSolver
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
            this.XMLbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // XMLbutton
            // 
            this.XMLbutton.Location = new System.Drawing.Point(12, 12);
            this.XMLbutton.Name = "XMLbutton";
            this.XMLbutton.Size = new System.Drawing.Size(75, 23);
            this.XMLbutton.TabIndex = 0;
            this.XMLbutton.Text = "Load XML";
            this.XMLbutton.UseVisualStyleBackColor = true;
            this.XMLbutton.Click += new System.EventHandler(this.XMLbutton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.XMLbutton);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button XMLbutton;
    }
}

