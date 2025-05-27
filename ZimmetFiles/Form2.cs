using Microsoft.Data.SqlClient;

namespace ZWebApp
{
    public partial class Form2 : BaseForm
    {
        private TextBox txtTCKimlikNo, txtAd, txtSoyad, txtSirket, txtDepartman, txtGorev, txtEmail, txtCepTelefonu, txtIsTelefonu, txtFaksNumarasi, txtEvTelefonu;
        private Button btnKaydet;
        private Label lblBaslik;
        private TextBox[] textBoxes;

        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;
        public string YeniEklenenAdSoyad { get; private set; }

        public Form2()
        {
            InitializeComponent();


            this.Load += new System.EventHandler(this.Form2_Load);

            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            this.Text = "Yeni Kullanıcı Ekle";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            // Başlık Label
            lblBaslik = new Label()
            {
                Text = "YENİ KULLANICI EKLE",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(150, 20)
            };
            this.Controls.Add(lblBaslik);

            // Label ve TextBox'ları oluştur
            string[] labels = { "TC", "Ad", "Soyad", "Şirket", "Departman", "Görev", "Email", "CepTelefonu", "İşTelefonu", "FaksNumarası", "EvTelefonu" };
            textBoxes = new TextBox[labels.Length];

            int startY = 60;

            for (int i = 0; i < labels.Length; i++)
            {

                Label lbl = new Label()
                {
                    Text = labels[i],
                    Location = new Point(30, startY),
                    AutoSize = true,
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                this.Controls.Add(lbl);


                textBoxes[i] = new TextBox()
                {
                    Location = new Point(150, startY),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(textBoxes[i]);


                startY += 40; // Her bileşeni alta kaydır
            }

            // TextBox değişkenlerine atama
            txtTCKimlikNo = textBoxes[0];
            txtAd = textBoxes[1];
            txtSoyad = textBoxes[2];
            txtSirket = textBoxes[3];
            txtDepartman = textBoxes[4];
            txtGorev = textBoxes[5];
            txtEmail = textBoxes[6];
            txtCepTelefonu = textBoxes[7];
            txtIsTelefonu = textBoxes[8];
            txtFaksNumarasi = textBoxes[9];
            txtEvTelefonu = textBoxes[10];



            // Kaydet Butonu
            btnKaydet = new Button()
            {
                Text = "✔ Kaydet",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightGreen,
                ForeColor = Color.DarkGreen,
                Cursor = Cursors.Hand,
                Size = new Size(120, 35),
                Location = new Point(180, startY)
            };
            btnKaydet.Click += BtnKaydet_Click;
            this.Controls.Add(btnKaydet);

            AdjustFormSize();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text))
            {
                MessageBox.Show("Lütfen hem ad hem soyad giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM Kullanıcılar WHERE Ad = @Ad AND Soyad = @Soyad";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Ad", txtAd.Text.Trim());
                    checkCmd.Parameters.AddWithValue("@Soyad", txtSoyad.Text.Trim());

                    int existingCount = (int)checkCmd.ExecuteScalar();
                    if (existingCount > 0)
                    {
                        MessageBox.Show("Bu isim ve soyada sahip bir kullanıcı zaten var.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                string query = "INSERT INTO Kullanıcılar (TC, Ad, Soyad, Şirket, Departman, Görev, Email, CepTelefonu, İşTelefonu, FaksNumarası, EvTelefonu) " +
                               "VALUES (@TC, @Ad, @Soyad, @Sirket, @Departman, @Gorev, @Email, @CepTelefonu, @IsTelefonu, @FaksNumarasi, @EvTelefonu); ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TC", txtTCKimlikNo.Text);
                    cmd.Parameters.AddWithValue("@Ad", txtAd.Text);
                    cmd.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
                    cmd.Parameters.AddWithValue("@Sirket", txtSirket.Text);
                    cmd.Parameters.AddWithValue("@Departman", txtDepartman.Text);
                    cmd.Parameters.AddWithValue("@Gorev", txtGorev.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@CepTelefonu", txtCepTelefonu.Text.Trim());
                    cmd.Parameters.AddWithValue("@IsTelefonu", txtIsTelefonu.Text);
                    cmd.Parameters.AddWithValue("@FaksNumarasi", txtFaksNumarasi.Text);
                    cmd.Parameters.AddWithValue("@EvTelefonu", txtEvTelefonu.Text);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        YeniEklenenAdSoyad = txtAd.Text.Trim() + " " + txtSoyad.Text.Trim();
                        Console.WriteLine("Kaydedilen Kullanıcı: " + YeniEklenenAdSoyad);

                        MessageBox.Show("Kullanıcı başarıyla eklendi!");
                        // Burada Form3'e ad ve soyadı gönderiyoruz
                        Form3 form3 = new Form3(YeniEklenenAdSoyad);
                        form3.ShowDialog();

                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }

                    else
                    {
                        MessageBox.Show("Kullanıcı eklenirken hata oluştu.");
                    }
                }

            }
            Logger.Kaydet("Kullanıcı eklendi.", YeniEklenenAdSoyad);
        }

        private void AdjustFormSize()
        {
            int maxWidth = 0;
            int maxHeight = 0;

            foreach (Control ctrl in this.Controls)
            {
                int right = ctrl.Right + 20;
                int bottom = ctrl.Bottom + 50;

                if (right > maxWidth)
                    maxWidth = right;

                if (bottom > maxHeight)
                    maxHeight = bottom;
            }

            this.ClientSize = new Size(maxWidth, maxHeight);
        }
    }
}
