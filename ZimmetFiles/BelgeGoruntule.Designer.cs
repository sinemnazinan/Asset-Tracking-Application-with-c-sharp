namespace ZWebApp
{
    partial class Form8
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form8));
            btnAc = new Button();
            btnSil = new Button();
            dgvBelgeler = new DataGridView();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvBelgeler).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnAc
            // 
            btnAc.Location = new Point(625, 154);
            btnAc.Name = "btnAc";
            btnAc.Size = new Size(146, 29);
            btnAc.TabIndex = 1;
            btnAc.Text = "Seçili Belgeyi Aç";
            btnAc.UseVisualStyleBackColor = true;
            btnAc.Click += btnAc_Click;
            // 
            // btnSil
            // 
            btnSil.Location = new Point(625, 205);
            btnSil.Name = "btnSil";
            btnSil.Size = new Size(146, 29);
            btnSil.TabIndex = 2;
            btnSil.Text = "Seçili Belgeyi Sil";
            btnSil.UseVisualStyleBackColor = true;
            btnSil.Click += btnSil_Click;
            // 
            // dgvBelgeler
            // 
            dgvBelgeler.AllowUserToAddRows = false;
            dgvBelgeler.AllowUserToDeleteRows = false;
            dgvBelgeler.AllowUserToResizeRows = false;
            dgvBelgeler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBelgeler.BackgroundColor = SystemColors.ButtonHighlight;
            dgvBelgeler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBelgeler.Location = new Point(12, 23);
            dgvBelgeler.Name = "dgvBelgeler";
            dgvBelgeler.ReadOnly = true;
            dgvBelgeler.RowHeadersWidth = 51;
            dgvBelgeler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBelgeler.Size = new Size(607, 421);
            dgvBelgeler.TabIndex = 7;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Resource1.logo;
            pictureBox1.InitialImage = null;
            pictureBox1.Location = new Point(30, 450);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(204, 108);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            label1.ForeColor = SystemColors.ActiveBorder;
            label1.Location = new Point(30, 569);
            label1.Name = "label1";
            label1.Size = new Size(539, 21);
            label1.TabIndex = 9;
            label1.Text = "Bera Holding Bilgi İşlem Daire Başkanlığı Tarafından Hazırlanmıştır.";
            // 
            // Form8
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 599);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(dgvBelgeler);
            Controls.Add(btnSil);
            Controls.Add(btnAc);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form8";
            Text = "Belgeleri Görüntüle";
            ((System.ComponentModel.ISupportInitialize)dgvBelgeler).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnAc;
        private Button btnSil;
        private DataGridView dgvBelgeler;
        private PictureBox pictureBox1;
        private Label label1;
    }
}