using Microsoft.Data.SqlClient;
using System.Data;

namespace ZWebApp
{
    public partial class Form1 : BaseForm
    {
        private string seciliAdSoyad = ""; // Seçilen kullanıcının cep telefonu numarası saklanacak
        private int seciliUrunID = 0; // Form4 için gerekli olan ÜrünID'yi sakla
        private PictureBox logo; // Logo için sınıf değişkeni
        public string secilenTeslimEden = "";
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;


        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load); // Form Load eventini bağla
            FormuOlustur(); // Butonları ve arayüzü kod ile oluştur
            AdjustFormSize();
            ButonlariOrtala();
            this.FormClosed += Form1_FormClosed;


        }
        private void Form1_Load(object sender, EventArgs e)
        {

            //float scaleFactor = 1.00f; // %15 büyütme
            //ScaleUI(scaleFactor);
            // Formun tam ekranın ortasında açılmasını sağla (hem yatay hem dikey olarak)
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,  // Yatay ortalama
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 3 // Dikey ortalama
            );
            logo = new PictureBox();
            logo.Name = "logo"; // Control dizininde erişilebilir olması için
            logo.Image = Resource1.logo;
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Size = new Size(200, 100);

            // En alta konumlandır
            logo.Location = new Point((this.ClientSize.Width - logo.Width) / 2, this.ClientSize.Height - 130);
            logo.Anchor = AnchorStyles.Bottom;

            this.Controls.Add(logo);
            Label lblHazirlayan = new Label();
            lblHazirlayan.Text = "Bera Holding Bilgi İşlem Daire Başkanlığı Tarafından Hazırlanmıştır";
            lblHazirlayan.Font = new Font("Arial", 9, FontStyle.Italic);
            lblHazirlayan.AutoSize = true;
            lblHazirlayan.ForeColor = Color.Gray;
            lblHazirlayan.Location = new Point((this.ClientSize.Width - lblHazirlayan.PreferredWidth) / 2, this.ClientSize.Height - 15);
            lblHazirlayan.Anchor = AnchorStyles.Bottom;
            this.Controls.Add(lblHazirlayan);

            //Button btnAyarlar = new Button();
            //btnAyarlar.Name = "btnAyarlar";
            //btnAyarlar.Text = "⚙️";
            //btnAyarlar.Font = new Font("Arial", 10, FontStyle.Bold);
            //btnAyarlar.Size = new Size(40, 40);
            //btnAyarlar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            //btnAyarlar.Location = new Point(this.ClientSize.Width - btnAyarlar.Width - 5, this.ClientSize.Height - btnAyarlar.Height - 20);
            //btnAyarlar.Click += BtnAyarlar_Click;
            //this.Controls.Add(btnAyarlar);

            // Formun yüksekliğini tekrar hesapla
            AdjustFormSize();
        }

        private void FormuOlustur()
        {
            // Form özellikleri
            this.Text = "Ana Kontrol Paneli";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Başlık
            Label lblBaslik = new Label();
            lblBaslik.Text = "KONTROL PANELİ";
            lblBaslik.Font = new Font("Arial", 14, FontStyle.Bold);
            lblBaslik.TextAlign = ContentAlignment.MiddleCenter;
            lblBaslik.Size = new Size(350, 30);
            lblBaslik.Location = new Point(25, 10);
            this.Controls.Add(lblBaslik);
            // Yeni X konumu (Form genişliğinin ortası - başlığın genişliğinin yarısı)
            lblBaslik.Location = new Point((this.ClientSize.Width - lblBaslik.Width) / 2, lblBaslik.Location.Y);

            // Butonlar
            int butonX = 100;
            int baslangicY = 150;
            int dikeyBosluk = 50;

            // Yeni Kullanıcı Ekle Butonu
            Button btnKullaniciEkle = ButonOlustur("Yeni Kullanıcı Ekle", butonX, baslangicY);
            btnKullaniciEkle.Click += BtnKullaniciEkle_Click;
            this.Controls.Add(btnKullaniciEkle);

            // Zimmet Kaydım Var Butonu (YENİ EKLENDİ)
            Button btnZimmetVar = ButonOlustur("Önceden Zimmet Kaydım Var", butonX, baslangicY + dikeyBosluk);
            btnZimmetVar.Click += BtnZimmetliKullanici_Click;
            this.Controls.Add(btnZimmetVar);

            //// Yeni Ürün Ekle Butonu
            //Button btnUrunEkle = ButonOlustur("Yeni Ürün Ekle", butonX, baslangicY + (dikeyBosluk * 2));
            //btnUrunEkle.Click += BtnUrunEkle_Click;
            //this.Controls.Add(btnUrunEkle);

            //// Telefon ve SIM Ekle Butonu
            //Button btnTelefonSimEkle = ButonOlustur("Telefon ve SIM Ekle", butonX, baslangicY + (dikeyBosluk * 3));
            //btnTelefonSimEkle.Click += BtnTelefonSimEkle_Click;
            //this.Controls.Add(btnTelefonSimEkle);

            // Zimmet Belgesi Oluştur Butonu
            Button btnZimmetBelgesi = ButonOlustur("Zimmet Belgesi Oluştur", butonX, baslangicY + (dikeyBosluk * 2));
            btnZimmetBelgesi.Click += BtnZimmetBelgesiOlustur_Click;
            this.Controls.Add(btnZimmetBelgesi);

            // Tutanak Oluştur Butonu
            Button btnTutanakOlustur = ButonOlustur("Tutanak Oluştur", butonX, baslangicY + (dikeyBosluk * 3));
            btnTutanakOlustur.Click += BtnTutanakOlustur_Click;
            this.Controls.Add(btnTutanakOlustur);

            // Zimmet Sil Butonu
            Button btnZimmetSil = ButonOlustur("Kişinin Tüm Zimmetini Sil", butonX, baslangicY + (dikeyBosluk * 4));
            btnZimmetSil.Click += BtnZimmetSil_Click;
            this.Controls.Add(btnZimmetSil);

            // Zimmet Düşür Butonu
            Button btnZimmetDusur = ButonOlustur("Zimmet Düşür", butonX, baslangicY + (dikeyBosluk * 5));
            btnZimmetDusur.Click += BtnZimmetDusur_Click;
            this.Controls.Add(btnZimmetDusur);

            // Zimmetleri Gör Butonu
            Button btnZimmetleriGor = ButonOlustur("Zimmetleri Gör", butonX, baslangicY + (dikeyBosluk * 6));
            btnZimmetleriGor.Click += btnZimmetleriGor_Click;
            this.Controls.Add(btnZimmetleriGor);

            // Belgeleri Gör Butonu (örnek olarak Ayarlar'dan önce)
            Button btnBelgeleriGor = ButonOlustur("Belgeleri Gör", butonX, baslangicY + (dikeyBosluk * 7));
            btnBelgeleriGor.Click += BtnBelgeleriGor_Click;
            this.Controls.Add(btnBelgeleriGor);

            // İmzalanan Belgeyi İçeri Aktar Butonu
            Button btnBelgeIceriAktar = ButonOlustur("İmzalanan Belgeyi İçeri Aktar", butonX, baslangicY + (dikeyBosluk * 8));
            btnBelgeIceriAktar.Click += BtnBelgeIceriAktar_Click;
            this.Controls.Add(btnBelgeIceriAktar);

            //Ayarlar Butonu
            Button btnAyarlar = ButonOlustur("Ayarlar", butonX, baslangicY + (dikeyBosluk * 7));
            btnAyarlar.Click += BtnAyarlar_Click;
            this.Controls.Add(btnAyarlar);

            // Çıkış Butonu
            Button btnCikis = ButonOlustur("Çıkış", butonX, baslangicY + (dikeyBosluk * 7));
            btnCikis.Click += BtnCikis_Click;
            this.Controls.Add(btnCikis);

        }
        private void BtnAyarlar_Click(object sender, EventArgs e)
        {
            FormAyarlar ayarlarForm = new FormAyarlar();
            ayarlarForm.ShowDialog();
        }
        private async void BtnBelgeIceriAktar_Click(object sender, EventArgs e)
        {
            string girilenDeger = Microsoft.VisualBasic.Interaction.InputBox(
                "Belgeyi eklemek istediğiniz kişinin adını veya soyadını girin:", "Kullanıcı Seçimi", "");

            if (string.IsNullOrWhiteSpace(girilenDeger))
            {
                MessageBox.Show("Lütfen geçerli bir ad veya soyad girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıları getir
            DataTable kullanicilar = await KullaniciAraAsync(girilenDeger);

            if (kullanicilar.Rows.Count == 0)
            {
                MessageBox.Show("Girilen bilgiye uygun kullanıcı bulunamadı!", "Kullanıcı Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Seçim listesi göster
            List<string> kullaniciListesi = new List<string>();
            foreach (DataRow row in kullanicilar.Rows)
            {
                kullaniciListesi.Add(row["Ad"].ToString() + " " + row["Soyad"].ToString());
            }

            string secilenAdSoyad = KullaniciSec(kullaniciListesi);

            if (string.IsNullOrEmpty(secilenAdSoyad))
                return;

            string belgeTuru = BelgeTuruSec(); // türü kullanıcıya sor
            if (string.IsNullOrEmpty(belgeTuru))
            {
                MessageBox.Show("Belge türü seçilmedi!", "İptal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dosya seçimi
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "İmzalı PDF Belgesi Seç";
                openFileDialog.Filter = "PDF Dosyası|*.pdf";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] isimParcalari = secilenAdSoyad.Trim().Split(' ');
                    string ad = string.Join(" ", isimParcalari.Take(isimParcalari.Length - 1));
                    string soyad = isimParcalari.Last();

                    byte[] belgeBytes = File.ReadAllBytes(openFileDialog.FileName);
                    string belgeAdi = Path.GetFileName(openFileDialog.FileName);

                    BelgeOlustur.VeritabaniyaKaydet(ad, soyad, belgeAdi, belgeTuru, belgeBytes, true);

                    MessageBox.Show("Belge başarıyla veritabanına kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }



        private async void btnZimmetleriGor_Click(object? sender, EventArgs e)
        {
            string girilenDeger = Microsoft.VisualBasic.Interaction.InputBox(
                "Zimmetlerini görmek istediğiniz kişinin adını veya soyadını girin:",
                "Zimmetleri Gör", "");

            if (string.IsNullOrWhiteSpace(girilenDeger))
            {
                MessageBox.Show("Lütfen geçerli bir ad veya soyad girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıları bul
            DataTable kullanicilar = await KullaniciAraAsync(girilenDeger);

            if (kullanicilar.Rows.Count == 0)
            {
                MessageBox.Show("Girilen bilgiye uygun kullanıcı bulunamadı!", "Kullanıcı Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> kullaniciListesi = new List<string>();
            foreach (DataRow row in kullanicilar.Rows)
            {
                kullaniciListesi.Add(row["Ad"].ToString() + " " + row["Soyad"].ToString());
            }

            string secilenAdSoyad = KullaniciSec(kullaniciListesi);

            if (!string.IsNullOrEmpty(secilenAdSoyad))
            {
                // Form7'yi aç ve zimmetleri göster
                Form7 zimmetGorForm = new Form7(secilenAdSoyad);
                zimmetGorForm.ShowDialog();
            }
        }


        private Button ButonOlustur(string text, int x, int y)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(300, 40);
            btn.Location = new Point(x, y);
            btn.Font = new Font("Arial", 10, FontStyle.Bold);
            return btn;
        }

        // Buton Click Eventleri
        private void BtnKullaniciEkle_Click(object sender, EventArgs e)
        {
            Form2 yeniKullaniciForm = new Form2();
            if (yeniKullaniciForm.ShowDialog() == DialogResult.OK)
            {
                seciliAdSoyad = yeniKullaniciForm.YeniEklenenAdSoyad;
                Console.WriteLine("Seçilen Kullanıcı: " + seciliAdSoyad);
            }

        }

        private async void BtnZimmetliKullanici_Click(object sender, EventArgs e)
        {
            string girilenDeger = Microsoft.VisualBasic.Interaction.InputBox(
                "Ad veya Soyad Giriniz:", "Önceden Zimmet Kaydım Var", "", -1, -1);

            if (string.IsNullOrWhiteSpace(girilenDeger))
            {
                MessageBox.Show("Lütfen bir isim veya soyisim girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable kullanicilar = await KullaniciAraAsync(girilenDeger);

            if (kullanicilar.Rows.Count == 0)
            {
                MessageBox.Show("Girilen bilgiye uygun kullanıcı bulunamadı!", "Kullanıcı Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> kullaniciListesi = new List<string>();
            foreach (DataRow row in kullanicilar.Rows)
            {
                kullaniciListesi.Add(row["Ad"].ToString() + " " + row["Soyad"].ToString());
            }

            string secilenAdSoyad = KullaniciSec(kullaniciListesi);

            if (!string.IsNullOrEmpty(secilenAdSoyad))
            {
                Form3 form3 = new Form3(secilenAdSoyad);
                form3.Show();
            }
        }

        private async Task<DataTable> KullaniciAraAsync(string girilenDeger)
        {
            DataTable dt = new DataTable();
            string query = "SELECT Ad, Soyad FROM Kullanıcılar WHERE Ad LIKE @Deger OR Soyad LIKE @Deger";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Deger", "%" + girilenDeger.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
        private string KullaniciSec(List<string> kullanicilar)
        {
            using (Form secimForm = new Form())
            {
                secimForm.Text = "Kullanıcı Seç";
                secimForm.Size = new Size(300, 200);
                secimForm.StartPosition = FormStartPosition.CenterScreen;

                ListBox lstKullanicilar = new ListBox();
                lstKullanicilar.Size = new Size(250, 100);
                lstKullanicilar.Location = new Point(25, 20);

                foreach (var kullanici in kullanicilar)
                {
                    lstKullanicilar.Items.Add(kullanici);
                }

                Button btnTamam = new Button();
                btnTamam.Text = "Seç";
                btnTamam.Location = new Point(100, 130);
                btnTamam.DialogResult = DialogResult.OK;

                secimForm.Controls.Add(lstKullanicilar);
                secimForm.Controls.Add(btnTamam);
                secimForm.AcceptButton = btnTamam;

                if (secimForm.ShowDialog() == DialogResult.OK && lstKullanicilar.SelectedItem != null)
                {
                    return lstKullanicilar.SelectedItem.ToString();
                }
                return "";
            }
        }
        private void BtnUrunEkle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(seciliAdSoyad)) // Eğer bir kullanıcı seçilmişse
            {
                Form3 yeniUrunForm = new Form3(seciliAdSoyad);
                if (yeniUrunForm.ShowDialog() == DialogResult.OK)
                {
                    int yeniUrunID = yeniUrunForm.YeniEklenenUrunID;
                    if (yeniUrunID > 0)
                    {
                        MessageBox.Show("Ürün başarıyla eklendi! Şimdi telefon ve SIM ekleyebilirsiniz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        seciliUrunID = yeniUrunID; // Form4 için gerekli olan ÜrünID'yi sakla
                    }
                    else
                    {
                        MessageBox.Show("Ürün eklenmedi, lütfen tekrar deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce bir kullanıcı ekleyin veya seçin.");
            }

        }
        private void BtnTelefonSimEkle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(seciliAdSoyad) && seciliUrunID > 0) // Kullanıcı ve ürün kontrolü
            {
                Form4 telefonSimForm = new Form4(seciliAdSoyad, seciliUrunID);
                telefonSimForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Lütfen önce bir kullanıcı seçin ve en az bir ürün ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void BtnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private async void BtnZimmetBelgesiOlustur_Click(object sender, EventArgs e)
        {
            string girilenDeger = Microsoft.VisualBasic.Interaction.InputBox(
            "Zimmet belgesi oluşturmak için ad veya soyad giriniz:",
            "Zimmet Belgesi Oluştur",
            ""
            );

            if (string.IsNullOrWhiteSpace(girilenDeger))
            {
                MessageBox.Show("Lütfen en az bir ad veya soyad girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıyı Ad veya Soyad ile ara
            DataTable kullanicilar = await KullaniciAraAsync(girilenDeger);

            if (kullanicilar.Rows.Count == 0)
            {
                MessageBox.Show("Girilen bilgiye uygun kullanıcı bulunamadı!", "Kullanıcı Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> kullaniciListesi = new List<string>();
            foreach (DataRow row in kullanicilar.Rows)
            {
                kullaniciListesi.Add(row["Ad"].ToString() + " " + row["Soyad"].ToString());
            }

            string secilenAdSoyad = KullaniciSec(kullaniciListesi);

            if (!string.IsNullOrEmpty(secilenAdSoyad))
            {
                string[] isimler = secilenAdSoyad.Split(' ');
                string ad = string.Join(" ", isimler, 0, isimler.Length - 1);
                string soyad = isimler[isimler.Length - 1];
                DataTable zimmetData = BelgeOlustur.GetZimmetData(ad, soyad);

                if (zimmetData.Rows.Count == 0)
                {
                    MessageBox.Show("Bu kişiyle kayıtlı bir zimmet bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Teslim Eden Seç
                string teslimEden = Session.KullaniciAdi;
                if (string.IsNullOrEmpty(teslimEden))
                {
                    MessageBox.Show("Teslim eden kişi seçilmedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Zimmet belgesini oluştur
                BelgeOlustur belge = new BelgeOlustur();

                belge.ZimmetBelgesiOlustur(ad, soyad, teslimEden);


            }

        }

        private async void BtnBelgeleriGor_Click(object sender, EventArgs e)
        {
            string girilenDeger = Microsoft.VisualBasic.Interaction.InputBox(
    "Belgeyi görmek istediğiniz kişinin adını veya soyadını girin:", "Kullanıcı Seçimi", "");

            if (!string.IsNullOrWhiteSpace(girilenDeger))
            {
                DataTable kullanicilar = await KullaniciAraAsync(girilenDeger);

                if (kullanicilar.Rows.Count == 0)
                {
                    MessageBox.Show("Kullanıcı bulunamadı.");
                    return;
                }

                List<string> kullaniciListesi = new();
                foreach (DataRow row in kullanicilar.Rows)
                    kullaniciListesi.Add(row["Ad"] + " " + row["Soyad"]);

                string secilenAdSoyad = KullaniciSec(kullaniciListesi);
                if (!string.IsNullOrEmpty(secilenAdSoyad))
                {
                    Form8 belgeFormu = new Form8(secilenAdSoyad);
                    belgeFormu.ShowDialog();
                }
            }

        }

        private async void BtnZimmetSil_Click(object sender, EventArgs e)
        {
            string girilenDeger = Microsoft.VisualBasic.Interaction.InputBox(
                "Zimmet kayıtlarını silmek için ad veya soyad giriniz:",
                "Zimmet Sil", ""
            );

            if (string.IsNullOrWhiteSpace(girilenDeger))
            {
                MessageBox.Show("Lütfen en az bir ad veya soyad girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıları bul
            DataTable kullanicilar = await KullaniciAraAsync(girilenDeger);

            if (kullanicilar.Rows.Count == 0)
            {
                MessageBox.Show("Girilen bilgiye uygun kullanıcı bulunamadı!", "Kullanıcı Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> kullaniciListesi = new List<string>();
            foreach (DataRow row in kullanicilar.Rows)
            {
                kullaniciListesi.Add(row["Ad"].ToString() + " " + row["Soyad"].ToString());
            }

            string secilenAdSoyad = KullaniciSec(kullaniciListesi);

            if (!string.IsNullOrEmpty(secilenAdSoyad))
            {
                // Adminin şifresini veritabanından al
                string adminPassword = GetAdminPasswordFromDb(Session.KullaniciAdi);

                // Kullanıcıdan şifreyi iste
                string girilenSifre = Microsoft.VisualBasic.Interaction.InputBox(
                    "Bu işlemi gerçekleştirmek için yetkili şifresini girin:",
                    "Şifre Girişi", ""
                );

                // Şifreyi kontrol et
                if (girilenSifre != adminPassword)
                {
                    MessageBox.Show("Şifre hatalı! İşlem iptal edildi.", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult onay = MessageBox.Show(
                    $"{secilenAdSoyad} adlı kişinin tüm zimmet kayıtlarını silmek istediğinize emin misiniz?",
                    "Onay",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (onay == DialogResult.No) return;

                // Zimmet silme işlemi
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string[] isimler = secilenAdSoyad.Split(' ');
                    string ad = string.Join(" ", isimler, 0, isimler.Length - 1);
                    string soyad = isimler[isimler.Length - 1];

                    // Zimmetle ilgili tüm verileri sil
                    string deleteTelefonVeSIM = "DELETE FROM TelefonVeSIM WHERE Ad = @Ad AND Soyad = @Soyad";
                    using (SqlCommand cmd = new SqlCommand(deleteTelefonVeSIM, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ad", ad);
                        cmd.Parameters.AddWithValue("@Soyad", soyad);
                        cmd.ExecuteNonQuery();
                    }

                    string deleteUrunler = "DELETE FROM Ürünler WHERE Ad = @Ad AND Soyad = @Soyad";
                    using (SqlCommand cmd = new SqlCommand(deleteUrunler, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ad", ad);
                        cmd.Parameters.AddWithValue("@Soyad", soyad);
                        cmd.ExecuteNonQuery();
                    }

                    string deleteKullanici = "DELETE FROM Kullanıcılar WHERE Ad = @Ad AND Soyad = @Soyad";
                    using (SqlCommand cmd = new SqlCommand(deleteKullanici, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ad", ad);
                        cmd.Parameters.AddWithValue("@Soyad", soyad);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Zimmet başarıyla silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.Kaydet("Zimmet silindi", secilenAdSoyad);
            }
        }

        private string GetAdminPasswordFromDb(string kullaniciAdi)
        {
            string password = string.Empty;

            // Veritabanına bağlanıyoruz
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Adminin şifresini almak için SQL sorgusu
                string query = "SELECT Sifre FROM Adminler WHERE KullaniciAdi = @kullaniciAdi";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                // Şifreyi alıyoruz
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    password = result.ToString();
                }
            }

            return password; // Şifreyi geri döndürüyoruz
        }

        private async void BtnZimmetDusur_Click(object sender, EventArgs e)
        {
            string girilenDeger = Microsoft.VisualBasic.Interaction.InputBox(
                "Zimmetten düşülecek kişinin ad veya soyadını girin:",
                "Zimmet Düşür", "");

            if (string.IsNullOrWhiteSpace(girilenDeger))
            {
                MessageBox.Show("Lütfen geçerli bir ad veya soyad girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıları bul
            DataTable kullanicilar = await KullaniciAraAsync(girilenDeger);

            if (kullanicilar.Rows.Count == 0)
            {
                MessageBox.Show("Girilen bilgiye uygun kullanıcı bulunamadı!", "Kullanıcı Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> kullaniciListesi = new List<string>();
            foreach (DataRow row in kullanicilar.Rows)
            {
                kullaniciListesi.Add(row["Ad"].ToString() + " " + row["Soyad"].ToString());
            }

            string secilenAdSoyad = KullaniciSec(kullaniciListesi);

            if (!string.IsNullOrEmpty(secilenAdSoyad))
            {
                // Seçilen kullanıcıyı Form5'e gönder
                Form5 zimmetSilForm = new Form5(secilenAdSoyad);
                zimmetSilForm.ShowDialog();
            }

        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // Tüm uygulamayı kapat
        }

        private async void BtnTutanakOlustur_Click(object sender, EventArgs e)
        {
            string girilenDeger = Microsoft.VisualBasic.Interaction.InputBox(
                "Tutanak oluşturmak için ad veya soyad giriniz:",
                "Tutanak Oluştur", ""
            );

            if (string.IsNullOrWhiteSpace(girilenDeger))
            {
                MessageBox.Show("Lütfen geçerli bir ad veya soyad girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıları bul
            DataTable kullanicilar = await KullaniciAraAsync(girilenDeger);

            if (kullanicilar.Rows.Count == 0)
            {
                MessageBox.Show("Girilen bilgiye uygun kullanıcı bulunamadı!", "Kullanıcı Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> kullaniciListesi = new List<string>();
            foreach (DataRow row in kullanicilar.Rows)
            {
                kullaniciListesi.Add(row["Ad"].ToString() + " " + row["Soyad"].ToString());
            }

            string secilenAdSoyad = KullaniciSec(kullaniciListesi);

            if (!string.IsNullOrEmpty(secilenAdSoyad))
            {
                string teslimEden = Session.KullaniciAdi;
                if (string.IsNullOrEmpty(teslimEden))
                {
                    MessageBox.Show("Teslim eden kişi seçilmedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Form6 tutanakForm = new Form6(secilenAdSoyad, teslimEden);
                tutanakForm.ShowDialog();
            }


        }

        //private async Task<(string, bool)> KullaniciBilgisiGetir(string girilenDeger)
        //{
        //    string adSoyad = "";
        //    bool kayitVar = false;

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        await conn.OpenAsync();
        //        string query = "SELECT Ad, Soyad FROM Kullanıcılar WHERE Ad LIKE @Deger OR Soyad LIKE @Deger";

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@Deger", "%" + girilenDeger.Trim() + "%");
        //            SqlDataReader reader = await cmd.ExecuteReaderAsync();

        //            List<string> bulunanKullanicilar = new List<string>();

        //            while (reader.Read())
        //            {
        //                bulunanKullanicilar.Add(reader["Ad"].ToString() + " " + reader["Soyad"].ToString());
        //                kayitVar = true;
        //            }

        //            if (bulunanKullanicilar.Count == 0)
        //            {
        //                return ("", false);
        //            }
        //            else if (bulunanKullanicilar.Count == 1)
        //            {
        //                adSoyad = bulunanKullanicilar[0];
        //            }
        //            else
        //            {
        //                adSoyad = KullaniciSec(bulunanKullanicilar);
        //            }
        //        }
        //    }

        //    return (adSoyad, kayitVar);
        //}


        //public static string TeslimEdenSec()
        //{
        //    using (Form teslimEdenForm = new Form())
        //    {
        //        teslimEdenForm.Text = "Teslim Eden Seç";
        //        teslimEdenForm.Size = new Size(300, 150);
        //        teslimEdenForm.StartPosition = FormStartPosition.CenterScreen;

        //        ComboBox cmbTeslimEden = new ComboBox();
        //        cmbTeslimEden.Items.AddRange(new string[] { "Yasin BAYRAKCI", "Ercan KAPÇI", "Burçin GÜNER" });
        //        cmbTeslimEden.DropDownStyle = ComboBoxStyle.DropDownList;
        //        cmbTeslimEden.Location = new Point(50, 20);
        //        cmbTeslimEden.Width = 180;

        //        Button btnTamam = new Button();
        //        btnTamam.Text = "Tamam";
        //        btnTamam.Location = new Point(100, 60);
        //        btnTamam.DialogResult = DialogResult.OK;

        //        teslimEdenForm.Controls.Add(cmbTeslimEden);
        //        teslimEdenForm.Controls.Add(btnTamam);

        //        teslimEdenForm.AcceptButton = btnTamam;

        //        if (teslimEdenForm.ShowDialog() == DialogResult.OK)
        //        {
        //            return cmbTeslimEden.SelectedItem?.ToString() ?? "";
        //        }
        //        return "";
        //    }

        //}
        //private void cmbTeslimEden_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ComboBox cmbTeslimEden = sender as ComboBox;
        //    if (cmbTeslimEden != null)
        //    {
        //        secilenTeslimEden = cmbTeslimEden.SelectedItem.ToString();
        //    }
        //}



        private void AdjustFormSize()
        {
            int maxWidth = 0;
            int maxHeight = 0;
            this.Width = 200; // Başlangıç genişliği

            foreach (Control ctrl in this.Controls)
            {
                int right = ctrl.Right + 20;
                int bottom = ctrl.Bottom + 50;

                if (right > maxWidth)
                    maxWidth = right;

                if (bottom > maxHeight)
                    maxHeight = bottom;
            }

            // Eğer logo ekliyse, formun yüksekliğine logoyu da ekle
            if (this.Controls.ContainsKey("logo"))
            {
                Control logo = this.Controls["logo"];
                maxHeight += logo.Height + 20; // Logoyu da hesaba kat
            }


            this.Size = new Size(maxWidth, maxHeight + 20);

        }

        private void ButonlariOrtala()
        {
            int formGenislik = this.ClientSize.Width;  // Formun genişliği
            int butonGenislik = 250;  // Butonların sabit genişliği
            int butonYukseklik = 40;  // Butonların yüksekliği
            int aralik = 10;  // Butonlar arasındaki boşluk
            int toplamButonYukseklik = 0; // Tüm butonların toplam yüksekliği hesaplanacak

            // Formdaki butonları bul ve yükseklik hesapla
            foreach (Control control in this.Controls)
            {
                if (control is Button btn)
                {
                    toplamButonYukseklik += butonYukseklik + aralik;
                }
            }

            // Eğer logo oluşturulmadıysa, butonları konumlandırırken hata vermesin diye kontrol ekleyelim.
            if (logo != null)
            {
                toplamButonYukseklik += logo.Height + aralik;
            }


            // Butonların başlangıç noktası (butonlar ve logoyu ortalayarak başlat)
            int baslangicY = ((this.ClientSize.Height + 50) - toplamButonYukseklik) / 2;

            // Butonları tekrar konumlandır
            foreach (Control control in this.Controls)
            {
                if (control is Button btn)
                {
                    btn.Size = new Size(butonGenislik, butonYukseklik);
                    int kaydirmaMiktari = 15; // Butonları sola kaydırma miktarı
                    btn.Location = new Point((formGenislik - butonGenislik) / 2 - kaydirmaMiktari, baslangicY);

                    baslangicY += butonYukseklik + aralik; // Bir sonraki butonu aşağı kaydır
                }
            }

            // Formun yüksekliğini otomatik artır
            this.Height = toplamButonYukseklik + 50;
        }

        // UI'yi ölçeklendir
        private void ScaleUI(float scaleFactor)
        {
            foreach (Control control in this.Controls)
            {
                control.Location = new Point(
                    (int)(control.Location.X * scaleFactor),
                    (int)(control.Location.Y * scaleFactor)
                );

                control.Size = new Size(
                    (int)(control.Size.Width * scaleFactor),
                    (int)(control.Size.Height * scaleFactor)
                );

                // Eğer buton ya da label ise yazı fontunu da büyüt
                if (control is Button || control is Label)
                {
                    control.Font = new Font(control.Font.FontFamily, control.Font.Size * scaleFactor, control.Font.Style);
                }
            }

            // Formun kendisini büyüt
            this.Size = new Size((int)(this.Width * scaleFactor), (int)(this.Height * scaleFactor));
        }

        private string BelgeTuruSec()
        {
            using (Form turSecForm = new Form())
            {
                turSecForm.Text = "Belge Türü Seç";
                turSecForm.Size = new Size(300, 150);
                turSecForm.StartPosition = FormStartPosition.CenterScreen;

                ComboBox cmbBelgeTurleri = new ComboBox();
                cmbBelgeTurleri.Items.AddRange(new string[] { "Zimmet", "Tutanak" });
                cmbBelgeTurleri.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbBelgeTurleri.Location = new Point(50, 20);
                cmbBelgeTurleri.Width = 180;

                Button btnTamam = new Button();
                btnTamam.Text = "Tamam";
                btnTamam.Location = new Point(100, 60);
                btnTamam.DialogResult = DialogResult.OK;

                turSecForm.Controls.Add(cmbBelgeTurleri);
                turSecForm.Controls.Add(btnTamam);

                turSecForm.AcceptButton = btnTamam;

                if (turSecForm.ShowDialog() == DialogResult.OK && cmbBelgeTurleri.SelectedItem != null)
                {
                    return cmbBelgeTurleri.SelectedItem.ToString();
                }
                return null;
            }
        }

    }
}

