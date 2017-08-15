namespace Ristorante
{
    partial class Impostazioni
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Impostazioni));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.info = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.generalSettings = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cashRegistersNumber = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cursorChk = new System.Windows.Forms.CheckBox();
            this.fullscreenChk = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectDB = new System.Windows.Forms.Button();
            this.selectDB = new System.Windows.Forms.Button();
            this.resetDB = new System.Windows.Forms.Button();
            this.pathLbl = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.advancedSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.printerFooter = new System.Windows.Forms.TextBox();
            this.printerHeading = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.printerComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.info.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.generalSettings.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cashRegistersNumber)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.advancedSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.okBtn);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(490, 72);
            this.panel1.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 33.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(93, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(277, 52);
            this.label5.TabIndex = 1;
            this.label5.Text = "Impostazioni";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Ristorante.Properties.Resources.settings_icon;
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(91, 68);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(423, 9);
            this.okBtn.Margin = new System.Windows.Forms.Padding(2);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(57, 55);
            this.okBtn.TabIndex = 8;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // openFile
            // 
            this.openFile.Filter = "Database (*.db)|*.db";
            // 
            // info
            // 
            this.info.Controls.Add(this.label6);
            this.info.Controls.Add(this.pictureBox2);
            this.info.Controls.Add(this.label1);
            this.info.Controls.Add(this.label2);
            this.info.Location = new System.Drawing.Point(4, 44);
            this.info.Name = "info";
            this.info.Padding = new System.Windows.Forms.Padding(3);
            this.info.Size = new System.Drawing.Size(481, 171);
            this.info.TabIndex = 3;
            this.info.Text = "Informazioni";
            this.info.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 54);
            this.label6.MaximumSize = new System.Drawing.Size(300, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 112);
            this.label6.TabIndex = 13;
            this.label6.Text = "Modificare il database con:\r\nDB Browser for SQLite\r\n\r\nPer informazioni contattare" +
    ":\r\nGiovanni Lorenzini\r\ngioam.lorenzini@gmail.com\r\n334 7648426";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Ristorante.Properties.Resources.logo;
            this.pictureBox2.Location = new System.Drawing.Point(374, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(99, 90);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(-5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(253, 55);
            this.label1.TabIndex = 9;
            this.label1.Text = "Ristorante";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(244, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Ver 2.0";
            // 
            // generalSettings
            // 
            this.generalSettings.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.generalSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.generalSettings.Controls.Add(this.groupBox5);
            this.generalSettings.Controls.Add(this.groupBox2);
            this.generalSettings.Controls.Add(this.groupBox1);
            this.generalSettings.Location = new System.Drawing.Point(4, 44);
            this.generalSettings.Name = "generalSettings";
            this.generalSettings.Size = new System.Drawing.Size(481, 171);
            this.generalSettings.TabIndex = 2;
            this.generalSettings.Text = "Generali";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cashRegistersNumber);
            this.groupBox5.Location = new System.Drawing.Point(224, 110);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(249, 53);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Numero Registratori di Cassa";
            // 
            // cashRegistersNumber
            // 
            this.cashRegistersNumber.Location = new System.Drawing.Point(6, 23);
            this.cashRegistersNumber.Maximum = new decimal(new int[] {
            55,
            0,
            0,
            0});
            this.cashRegistersNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cashRegistersNumber.Name = "cashRegistersNumber";
            this.cashRegistersNumber.Size = new System.Drawing.Size(237, 20);
            this.cashRegistersNumber.TabIndex = 7;
            this.cashRegistersNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cashRegistersNumber.ValueChanged += new System.EventHandler(this.cashRegistersNumber_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cursorChk);
            this.groupBox2.Controls.Add(this.fullscreenChk);
            this.groupBox2.Location = new System.Drawing.Point(9, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 53);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Interfaccia Grafica";
            // 
            // cursorChk
            // 
            this.cursorChk.AutoSize = true;
            this.cursorChk.Location = new System.Drawing.Point(86, 24);
            this.cursorChk.Name = "cursorChk";
            this.cursorChk.Size = new System.Drawing.Size(120, 17);
            this.cursorChk.TabIndex = 6;
            this.cursorChk.Text = "Nascondi Puntatore";
            this.cursorChk.UseVisualStyleBackColor = true;
            this.cursorChk.CheckedChanged += new System.EventHandler(this.curChk_CheckedChanged);
            // 
            // fullscreenChk
            // 
            this.fullscreenChk.AutoSize = true;
            this.fullscreenChk.Location = new System.Drawing.Point(6, 24);
            this.fullscreenChk.Name = "fullscreenChk";
            this.fullscreenChk.Size = new System.Drawing.Size(74, 17);
            this.fullscreenChk.TabIndex = 5;
            this.fullscreenChk.Text = "Fullscreen";
            this.fullscreenChk.UseVisualStyleBackColor = true;
            this.fullscreenChk.CheckedChanged += new System.EventHandler(this.fullscreenChk_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connectDB);
            this.groupBox1.Controls.Add(this.selectDB);
            this.groupBox1.Controls.Add(this.resetDB);
            this.groupBox1.Controls.Add(this.pathLbl);
            this.groupBox1.Location = new System.Drawing.Point(9, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 97);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DataBase";
            // 
            // connectDB
            // 
            this.connectDB.Location = new System.Drawing.Point(157, 25);
            this.connectDB.Name = "connectDB";
            this.connectDB.Size = new System.Drawing.Size(145, 23);
            this.connectDB.TabIndex = 3;
            this.connectDB.Text = "Connect DB";
            this.connectDB.UseVisualStyleBackColor = true;
            this.connectDB.Click += new System.EventHandler(this.connectDB_Click);
            // 
            // selectDB
            // 
            this.selectDB.Location = new System.Drawing.Point(6, 25);
            this.selectDB.Name = "selectDB";
            this.selectDB.Size = new System.Drawing.Size(145, 23);
            this.selectDB.TabIndex = 2;
            this.selectDB.Text = "Select DB";
            this.selectDB.UseVisualStyleBackColor = true;
            this.selectDB.Click += new System.EventHandler(this.selectDB_Click);
            // 
            // resetDB
            // 
            this.resetDB.Enabled = false;
            this.resetDB.Location = new System.Drawing.Point(308, 25);
            this.resetDB.Name = "resetDB";
            this.resetDB.Size = new System.Drawing.Size(145, 23);
            this.resetDB.TabIndex = 4;
            this.resetDB.Text = "Reset DB";
            this.resetDB.UseVisualStyleBackColor = true;
            this.resetDB.Click += new System.EventHandler(this.resetDB_Click);
            // 
            // pathLbl
            // 
            this.pathLbl.AutoSize = true;
            this.pathLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathLbl.Location = new System.Drawing.Point(6, 67);
            this.pathLbl.MaximumSize = new System.Drawing.Size(447, 0);
            this.pathLbl.Name = "pathLbl";
            this.pathLbl.Size = new System.Drawing.Size(22, 13);
            this.pathLbl.TabIndex = 19;
            this.pathLbl.Text = "C:\\";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.generalSettings);
            this.tabControl1.Controls.Add(this.advancedSettings);
            this.tabControl1.Controls.Add(this.info);
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 40);
            this.tabControl1.Location = new System.Drawing.Point(0, 75);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(489, 219);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 1;
            // 
            // advancedSettings
            // 
            this.advancedSettings.Controls.Add(this.groupBox3);
            this.advancedSettings.Location = new System.Drawing.Point(4, 44);
            this.advancedSettings.Name = "advancedSettings";
            this.advancedSettings.Size = new System.Drawing.Size(481, 171);
            this.advancedSettings.TabIndex = 4;
            this.advancedSettings.Text = "Avanzate";
            this.advancedSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.printerFooter);
            this.groupBox3.Controls.Add(this.printerHeading);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.printerComboBox);
            this.groupBox3.Location = new System.Drawing.Point(9, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(464, 157);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stampante";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Nome Stampante";
            // 
            // printerFooter
            // 
            this.printerFooter.Location = new System.Drawing.Point(116, 121);
            this.printerFooter.Name = "printerFooter";
            this.printerFooter.Size = new System.Drawing.Size(342, 20);
            this.printerFooter.TabIndex = 4;
            this.printerFooter.TextChanged += new System.EventHandler(this.printerFooter_TextChanged);
            // 
            // printerHeading
            // 
            this.printerHeading.Location = new System.Drawing.Point(116, 95);
            this.printerHeading.Name = "printerHeading";
            this.printerHeading.Size = new System.Drawing.Size(342, 20);
            this.printerHeading.TabIndex = 3;
            this.printerHeading.TextChanged += new System.EventHandler(this.printerHeading_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Piè di Pagina";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Intestazione";
            // 
            // printerComboBox
            // 
            this.printerComboBox.FormattingEnabled = true;
            this.printerComboBox.Location = new System.Drawing.Point(116, 49);
            this.printerComboBox.Name = "printerComboBox";
            this.printerComboBox.Size = new System.Drawing.Size(342, 21);
            this.printerComboBox.TabIndex = 2;
            this.printerComboBox.SelectedIndexChanged += new System.EventHandler(this.printerComboBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(113, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(345, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Lasciare il campo vuoto per disattivare la stampa";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Impostazioni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 294);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Impostazioni";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ristorante";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Impostazioni_FormClosing);
            this.Load += new System.EventHandler(this.Impostazioni_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.info.ResumeLayout(false);
            this.info.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.generalSettings.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cashRegistersNumber)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.advancedSettings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.TabPage info;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage generalSettings;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cursorChk;
        private System.Windows.Forms.CheckBox fullscreenChk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button connectDB;
        private System.Windows.Forms.Button selectDB;
        private System.Windows.Forms.Button resetDB;
        private System.Windows.Forms.Label pathLbl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown cashRegistersNumber;
        private System.Windows.Forms.TabPage advancedSettings;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox printerComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox printerFooter;
        private System.Windows.Forms.TextBox printerHeading;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
    }
}