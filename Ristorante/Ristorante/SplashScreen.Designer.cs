namespace Ristorante
{
    partial class SplashScreen
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
            this.components = new System.ComponentModel.Container();
            this.nameLbl = new System.Windows.Forms.Label();
            this.verLbl = new System.Windows.Forms.Label();
            this.taimer = new System.Windows.Forms.Timer(this.components);
            this.ownersLbl = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.valLbl = new System.Windows.Forms.Label();
            this.progressBar1 = new Ristorante.ProgressBarEx();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLbl
            // 
            this.nameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLbl.ForeColor = System.Drawing.Color.White;
            this.nameLbl.Location = new System.Drawing.Point(86, 7);
            this.nameLbl.Name = "nameLbl";
            this.nameLbl.Size = new System.Drawing.Size(279, 55);
            this.nameLbl.TabIndex = 0;
            this.nameLbl.Text = "Ristorante";
            this.nameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // verLbl
            // 
            this.verLbl.AutoSize = true;
            this.verLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verLbl.ForeColor = System.Drawing.Color.White;
            this.verLbl.Location = new System.Drawing.Point(352, 31);
            this.verLbl.Name = "verLbl";
            this.verLbl.Size = new System.Drawing.Size(88, 25);
            this.verLbl.TabIndex = 1;
            this.verLbl.Text = "Ver 2.0";
            // 
            // taimer
            // 
            this.taimer.Enabled = true;
            this.taimer.Interval = 20;
            this.taimer.Tick += new System.EventHandler(this.taimer_Tick);
            // 
            // ownersLbl
            // 
            this.ownersLbl.ForeColor = System.Drawing.Color.White;
            this.ownersLbl.Location = new System.Drawing.Point(12, 243);
            this.ownersLbl.Name = "ownersLbl";
            this.ownersLbl.Size = new System.Drawing.Size(428, 17);
            this.ownersLbl.TabIndex = 3;
            this.ownersLbl.Text = "Developed by Giovanni Lorenzini";
            this.ownersLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Ristorante.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(86, 70);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(279, 127);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // valLbl
            // 
            this.valLbl.AutoSize = true;
            this.valLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valLbl.ForeColor = System.Drawing.Color.White;
            this.valLbl.Location = new System.Drawing.Point(210, 227);
            this.valLbl.Name = "valLbl";
            this.valLbl.Size = new System.Drawing.Size(29, 16);
            this.valLbl.TabIndex = 5;
            this.valLbl.Text = "0%";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.DimGray;
            this.progressBar1.ForeColor = System.Drawing.Color.DimGray;
            this.progressBar1.Location = new System.Drawing.Point(86, 209);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(279, 10);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 2;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(452, 269);
            this.Controls.Add(this.valLbl);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ownersLbl);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.verLbl);
            this.Controls.Add(this.nameLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLbl;
        private System.Windows.Forms.Label verLbl;
        private System.Windows.Forms.Timer taimer;
        private Ristorante.ProgressBarEx progressBar1;
        private System.Windows.Forms.Label ownersLbl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label valLbl;
    }
}