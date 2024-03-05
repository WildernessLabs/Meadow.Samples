
namespace RaiseTheFlagWinForms
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button = new System.Windows.Forms.Button();
            this.flag = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.flag)).BeginInit();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(32, 260);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(127, 99);
            this.button.TabIndex = 0;
            this.button.Text = "CLICK ME";
            this.button.UseVisualStyleBackColor = true;
            // 
            // flag
            // 
            this.flag.Image = ((System.Drawing.Image)(resources.GetObject("flag.Image")));
            this.flag.InitialImage = null;
            this.flag.Location = new System.Drawing.Point(12, 15);
            this.flag.Name = "flag";
            this.flag.Size = new System.Drawing.Size(223, 239);
            this.flag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.flag.TabIndex = 1;
            this.flag.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 387);
            this.Controls.Add(this.flag);
            this.Controls.Add(this.button);
            this.Name = "Form1";
            this.Text = "Flag";
            ((System.ComponentModel.ISupportInitialize)(this.flag)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button;
        private System.Windows.Forms.PictureBox flag;
    }
}

