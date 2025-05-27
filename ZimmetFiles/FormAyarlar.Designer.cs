namespace ZWebApp
{
    partial class FormAyarlar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAyarlar));
            labelAdminler = new Label();
            label2 = new Label();
            label3 = new Label();
            txtKullaniciAdi = new TextBox();
            txtSifre = new TextBox();
            btnEkle = new Button();
            btnSil = new Button();
            dgvAdminler = new DataGridView();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            label4 = new Label();
            label5 = new Label();
            btnTutanaklariSifirla = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAdminler).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // labelAdminler
            // 
            labelAdminler.AutoSize = true;
            labelAdminler.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            labelAdminler.Location = new Point(831, 284);
            labelAdminler.Name = "labelAdminler";
            labelAdminler.Size = new Size(146, 28);
            labelAdminler.TabIndex = 0;
            labelAdminler.Text = "Tüm Adminler";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(181, 79);
            label2.Name = "label2";
            label2.Size = new Size(95, 20);
            label2.TabIndex = 1;
            label2.Text = "Kullanıcı Adı;";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(234, 124);
            label3.Name = "label3";
            label3.Size = new Size(42, 20);
            label3.TabIndex = 2;
            label3.Text = "Şifre;";
            // 
            // txtKullaniciAdi
            // 
            txtKullaniciAdi.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            txtKullaniciAdi.Location = new Point(341, 76);
            txtKullaniciAdi.Name = "txtKullaniciAdi";
            txtKullaniciAdi.Size = new Size(125, 27);
            txtKullaniciAdi.TabIndex = 3;
            // 
            // txtSifre
            // 
            txtSifre.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            txtSifre.Location = new Point(341, 121);
            txtSifre.Name = "txtSifre";
            txtSifre.Size = new Size(125, 27);
            txtSifre.TabIndex = 4;
            txtSifre.UseSystemPasswordChar = true;
            // 
            // btnEkle
            // 
            btnEkle.Location = new Point(168, 202);
            btnEkle.Name = "btnEkle";
            btnEkle.Size = new Size(108, 40);
            btnEkle.TabIndex = 5;
            btnEkle.Text = "Admin Ekle";
            btnEkle.UseVisualStyleBackColor = true;
            btnEkle.Click += btnEkle_Click_1;
            // 
            // btnSil
            // 
            btnSil.Location = new Point(308, 202);
            btnSil.Name = "btnSil";
            btnSil.Size = new Size(171, 40);
            btnSil.TabIndex = 6;
            btnSil.Text = "Seçili Admini Sil";
            btnSil.UseVisualStyleBackColor = true;
            btnSil.Click += btnSil_Click;
            // 
            // dgvAdminler
            // 
            dgvAdminler.BackgroundColor = SystemColors.ButtonHighlight;
            dgvAdminler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAdminler.Location = new Point(831, 324);
            dgvAdminler.Name = "dgvAdminler";
            dgvAdminler.RowHeadersWidth = 51;
            dgvAdminler.Size = new Size(177, 191);
            dgvAdminler.TabIndex = 7;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Resource1.logo;
            pictureBox1.Location = new Point(168, 284);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(331, 174);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(279, 9);
            label1.Name = "label1";
            label1.Size = new Size(245, 31);
            label1.TabIndex = 9;
            label1.Text = "Admin Kontrol Paneli";
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(610, 76);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(398, 191);
            dataGridView1.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label4.Location = new Point(609, 45);
            label4.Name = "label4";
            label4.Size = new Size(203, 28);
            label4.TabIndex = 11;
            label4.Text = "Son yapılan işlemler";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = SystemColors.ButtonFace;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            label5.ForeColor = SystemColors.AppWorkspace;
            label5.Location = new Point(234, 461);
            label5.Name = "label5";
            label5.Size = new Size(209, 20);
            label5.TabIndex = 12;
            label5.Text = "Bilgi İşlem Daire Başkanlığı";
            // 
            // btnTutanaklariSifirla
            // 
            btnTutanaklariSifirla.Location = new Point(609, 400);
            btnTutanaklariSifirla.Name = "btnTutanaklariSifirla";
            btnTutanaklariSifirla.Size = new Size(189, 38);
            btnTutanaklariSifirla.TabIndex = 13;
            btnTutanaklariSifirla.Text = "Tutanak Sayacını Sıfırla";
            btnTutanaklariSifirla.UseVisualStyleBackColor = true;
            btnTutanaklariSifirla.Click += btnTutanaklariSifirla_Click;
            // 
            // FormAyarlar
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1020, 527);
            Controls.Add(btnTutanaklariSifirla);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(dgvAdminler);
            Controls.Add(btnSil);
            Controls.Add(btnEkle);
            Controls.Add(txtSifre);
            Controls.Add(txtKullaniciAdi);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(labelAdminler);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormAyarlar";
            Text = "FormAyarlar";
            ((System.ComponentModel.ISupportInitialize)dgvAdminler).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelAdminler;
        private Label label2;
        private Label label3;
        private TextBox txtKullaniciAdi;
        private TextBox txtSifre;
        private Button btnEkle;
        private Button btnSil;
        private DataGridView dgvAdminler;
        private PictureBox pictureBox1;
        private Label label1;
        private DataGridView dataGridView1;
        private Label label4;
        private Label label5;
        private Button btnTutanaklariSifirla;
    }
}