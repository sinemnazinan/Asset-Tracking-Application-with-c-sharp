using Microsoft.Data.SqlClient;

namespace ZWebApp
{
    public partial class Form3 : BaseForm
    {
        private string AdSoyad;
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;
        public int YeniEklenenUrunID { get; private set; }
        private ComboBox cmbDurum;


        public Form3(string adSoyad)
        {
            InitializeComponent();
            this.AdSoyad = adSoyad;

            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Yeni Ürün Ekle";
            this.Size = new System.Drawing.Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Başlık Label
            Label lblBaslik = new Label();
            lblBaslik.Text = "YENİ ÜRÜN EKLE";
            lblBaslik.Font = new Font("Arial", 14, FontStyle.Bold);
            lblBaslik.ForeColor = Color.DarkBlue;
            lblBaslik.AutoSize = true;
            lblBaslik.Location = new Point(150, 20);
            this.Controls.Add(lblBaslik);

            string[] labels = { "Ürün Adı", "Alış Tarihi", "Model", "Üretici", "Açıklama" };

            TextBox[] textBoxes = new TextBox[labels.Length];
            int startY = 60;

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i];
                lbl.Location = new Point(50, startY);
                lbl.AutoSize = true;
                this.Controls.Add(lbl);

                textBoxes[i] = new TextBox();
                textBoxes[i].Location = new Point(200, startY);
                textBoxes[i].Size = new System.Drawing.Size(200, 20);
                this.Controls.Add(textBoxes[i]);

                startY += 40;
            }

            // "Durum" için Label
            Label lblDurum = new Label();
            lblDurum.Text = "Durum";
            lblDurum.Location = new Point(50, startY);
            lblDurum.AutoSize = true;
            this.Controls.Add(lblDurum);

            // "Durum" için ComboBox
            cmbDurum = new ComboBox(); // Global değişken olarak tanımlandı
            cmbDurum.Location = new Point(200, startY);
            cmbDurum.Size = new System.Drawing.Size(200, 20);
            cmbDurum.Items.AddRange(new string[] { "Yeni", "2. El" }); // Sadece 2 seçenek olacak
            cmbDurum.DropDownStyle = ComboBoxStyle.DropDownList; // Kullanıcı sadece listeden seçim yapabilir
            cmbDurum.SelectedIndex = 0; // Varsayılan olarak "Yeni" seçili gelsin
            this.Controls.Add(cmbDurum);

            startY += 40; // Diğer bileşenler için boşluk bırak

            // TextBox isimlendirme
            TextBox txtUrunAdi = textBoxes[0];
            TextBox txtAlisTarihi = textBoxes[1];
            TextBox txtModel = textBoxes[2];
            TextBox txtUretici = textBoxes[3];
            TextBox txtAciklama = textBoxes[4];

            // Kaydet Butonu
            Button btnKaydet = new Button();
            btnKaydet.Text = "✔ Kaydet";
            btnKaydet.Font = new Font("Arial", 10, FontStyle.Bold);
            btnKaydet.BackColor = Color.LightGreen;
            btnKaydet.ForeColor = Color.DarkGreen;
            btnKaydet.Size = new System.Drawing.Size(120, 40);
            btnKaydet.Location = new System.Drawing.Point(190, startY + 10);
            btnKaydet.Click += (sender, e) => BtnKaydet_Click(
                sender, e,
                txtUrunAdi, txtAlisTarihi,
                cmbDurum,
                txtModel, txtUretici, txtAciklama
            );



            this.Controls.Add(btnKaydet);
            AdjustFormSize();
        }

        private void AdjustFormSize()
        {
            int maxWidth = 0;
            int maxHeight = 0;

            foreach (Control ctrl in this.Controls)
            {
                int right = ctrl.Right + 20;  // Sağdan 20px boşluk bırak
                int bottom = ctrl.Bottom + 50; // Aşağıdan 50px boşluk bırak

                if (right > maxWidth)
                    maxWidth = right;

                if (bottom > maxHeight)
                    maxHeight = bottom;
            }

            this.ClientSize = new Size(maxWidth, maxHeight);
        }

        private void BtnKaydet_Click(object sender, EventArgs e, TextBox txtUrunAdi, TextBox txtAlisTarihi, ComboBox cmbDurum, TextBox txtModel, TextBox txtUretici, TextBox txtAciklama)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Ürünler (Ad, Soyad, ÜrünAdı, Üretici, Model, AlışTarihi, Durum, Açıklama) " +
                     "OUTPUT INSERTED.ÜrünID " +
                     "VALUES (@Ad, @Soyad, @UrunAdi, @Uretici, @Model, @AlisTarihi, @Durum, @Aciklama)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        string[] parcalar = AdSoyad.Trim().Split(' ');

                        string ad, soyad;
                        if (parcalar.Length == 1)
                        {
                            ad = parcalar[0];
                            soyad = "";
                        }
                        else
                        {
                            soyad = parcalar[parcalar.Length - 1];
                            ad = string.Join(" ", parcalar, 0, parcalar.Length - 1);
                        }

                        cmd.Parameters.AddWithValue("@Ad", ad);
                        cmd.Parameters.AddWithValue("@Soyad", soyad);

                        cmd.Parameters.AddWithValue("@UrunAdi", txtUrunAdi.Text);
                        cmd.Parameters.AddWithValue("@AlisTarihi", string.IsNullOrEmpty(txtAlisTarihi.Text) ? (object)DBNull.Value : DateTime.Parse(txtAlisTarihi.Text));
                        cmd.Parameters.AddWithValue("@Durum", cmbDurum.SelectedItem != null ? cmbDurum.SelectedItem.ToString() : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Model", txtModel.Text);
                        cmd.Parameters.AddWithValue("@Uretici", txtUretici.Text);
                        cmd.Parameters.AddWithValue("@Aciklama", string.IsNullOrEmpty(txtAciklama.Text) ? (object)DBNull.Value : txtAciklama.Text);

                        int yeniUrunID = (int)cmd.ExecuteScalar(); // Yeni eklenen ÜrünID'yi al

                        if (yeniUrunID > 0)
                        {
                            YeniEklenenUrunID = yeniUrunID;
                            MessageBox.Show("Ürün başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            DialogResult result = MessageBox.Show("Bu ürüne SIM veya Telefon tanımlamak istiyor musunuz?",
                                                                  "Telefon ve SIM Ekleyelim mi?",
                                                                  MessageBoxButtons.YesNo,
                                                                  MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                Form4 form4 = new Form4(AdSoyad, YeniEklenenUrunID);


                                form4.ShowDialog();
                            }

                            DialogResult yeniUrunSor = MessageBox.Show("Aynı kişiye başka ürün eklemek ister misiniz?",
                                           "Yeni Ürün Eklemek İster misin?",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

                            if (yeniUrunSor == DialogResult.Yes)
                            {
                                Form3 yeniForm3 = new Form3(AdSoyad);
                                yeniForm3.ShowDialog();
                            }
                            else
                            {
                                this.DialogResult = DialogResult.OK; // Form1'e dönüş

                            }
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ürün eklenirken hata oluştu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Logger.Kaydet("Ürün eklendi", AdSoyad);
        }


    }
}

