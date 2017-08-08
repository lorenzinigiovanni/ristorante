namespace Ristorante
{
    partial class Tile
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.descLbl = new System.Windows.Forms.Label();
            this.numberLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // descLbl
            // 
            this.descLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.descLbl.Location = new System.Drawing.Point(3, 3);
            this.descLbl.Margin = new System.Windows.Forms.Padding(3);
            this.descLbl.Name = "descLbl";
            this.descLbl.Size = new System.Drawing.Size(214, 60);
            this.descLbl.TabIndex = 0;
            this.descLbl.Text = "Descrizione Piatto";
            this.descLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numberLbl
            // 
            this.numberLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numberLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.numberLbl.Location = new System.Drawing.Point(3, 69);
            this.numberLbl.Margin = new System.Windows.Forms.Padding(3);
            this.numberLbl.Name = "numberLbl";
            this.numberLbl.Size = new System.Drawing.Size(214, 48);
            this.numberLbl.TabIndex = 1;
            this.numberLbl.Text = "123";
            this.numberLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.descLbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.numberLbl, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(220, 120);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // Tile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Red;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximumSize = new System.Drawing.Size(880, 480);
            this.MinimumSize = new System.Drawing.Size(110, 60);
            this.Name = "Tile";
            this.Size = new System.Drawing.Size(220, 120);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label descLbl;
        private System.Windows.Forms.Label numberLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
