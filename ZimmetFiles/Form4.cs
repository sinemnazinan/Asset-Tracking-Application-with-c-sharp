using Microsoft.Data.SqlClient;

namespace ZWebApp
{
    public partial class Form4 : BaseForm
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;        // **Arial Bold Fontunu Yükle**

        private string AdSoyad;
        private int urunID;

        public Form4(string adSoyad, int urunID)
        {
            this.AdSoyad = adSoyad;
            this.urunID = urunID;

            InitializeComponent();
            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            AdjustFormSize();
            //GetZimmetData();
        }
        private void InitializeComponent()
        {
            this.Text = "Telefon ve SIM Ekle";
            this.Size = new System.Drawing.Size(600, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblBaslik = new Label();
            lblBaslik.Text = "YENİ TELEFON VE SIM EKLE";
            lblBaslik.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            lblBaslik.AutoSize = true;
            lblBaslik.Location = new System.Drawing.Point(180, 20);
            this.Controls.Add(lblBaslik);

            int startY = 70;
            int labelX = 50;
            int textX = 250;
            int yGap = 40;

            // Label ve TextBox Tanımlamaları
            TextBox txtImei = AddLabeledTextbox("IMEI Numarası", startY, labelX, textX);
            TextBox txtMarka = AddLabeledTextbox("Marka", startY += yGap, labelX, textX);
            TextBox txtSeriNumarasi = AddLabeledTextbox("Seri Numarası", startY += yGap, labelX, textX);
            startY += yGap;
            Label lblCihazDurumu = new Label() { Text = "Cihaz Durumu:", Location = new Point(labelX, startY), AutoSize = true };
            ComboBox cmbCihazDurumu = new ComboBox() { Location = new Point(textX, startY), Width = 200 };
            cmbCihazDurumu.Items.AddRange(new string[] { "Yeni ve kutulu", "Daha önce kullanıldı" });

            this.Controls.Add(lblCihazDurumu);
            this.Controls.Add(cmbCihazDurumu);

            TextBox txtCihazEkipman = AddLabeledTextbox("Cihaz Teslim Ekipman", startY += yGap, labelX, textX);
            TextBox txtTelekomSirk = AddLabeledTextbox("Telekom Şirketi", startY += yGap, labelX, textX);
            TextBox txtTelefonNo = AddLabeledTextbox("Telefon Numarası", startY += yGap, labelX, textX);
            startY += yGap;
            Label lblPinPuk = new Label() { Text = "PinPuk Bildirimi:", Location = new Point(labelX, startY), AutoSize = true };
            ComboBox cmbPinPuk = new ComboBox() { Location = new Point(textX, startY), Width = 200 };
            cmbPinPuk.Items.AddRange(new string[] { "Yapıldı", "Yapılmadı" });

            this.Controls.Add(lblPinPuk);
            this.Controls.Add(cmbPinPuk);

            TextBox txtTarife = AddLabeledTextbox("Tarife Tanımı", startY += yGap, labelX, textX);
            TextBox txtKurumiciDak = AddLabeledTextbox("Kurum İçi Dakika Paketi", startY += yGap, labelX, textX);
            TextBox txtKurumDisiDak = AddLabeledTextbox("Kurum Dışı Dakika Paketi", startY += yGap, labelX, textX);
            TextBox txtSmsPaket = AddLabeledTextbox("SMS Paketi", startY += yGap, labelX, textX);
            TextBox txtInternetPaket = AddLabeledTextbox("İnternet Paketi", startY += yGap, labelX, textX);
            startY += yGap;
            Label lblPaketUcret = new Label() { Text = "Paket Ücretlendirme:", Location = new Point(labelX, startY), AutoSize = true };
            ComboBox cmbPaketUcret = new ComboBox() { Location = new Point(textX, startY), Width = 200 };
            cmbPaketUcret.Items.AddRange(new string[] { "Faturalı", "Kontörlü" });

            this.Controls.Add(lblPaketUcret);
            this.Controls.Add(cmbPaketUcret);
            startY += yGap;

            // Kaydet Butonu
            Button btnKaydet = new Button()
            {
                Text = "✔ Kaydet",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightGreen,
                ForeColor = Color.DarkGreen,
                Cursor = Cursors.Hand,
                Size = new Size(120, 35),
                Location = new System.Drawing.Point(250, startY + yGap),
                Width = 100
            };
            btnKaydet.Click += (sender, e) => BtnKaydet_Click(
                txtImei.Text, txtMarka.Text, txtSeriNumarasi.Text,
                cmbCihazDurumu.SelectedItem?.ToString() ?? "",
                txtCihazEkipman.Text,
                txtTelekomSirk.Text, txtTelefonNo.Text,
                cmbPinPuk.SelectedItem?.ToString() ?? "",  // PinPuk seçimi
                txtTarife.Text, txtKurumiciDak.Text, txtKurumDisiDak.Text,
                txtSmsPaket.Text, txtInternetPaket.Text,
                cmbPaketUcret.SelectedItem?.ToString() ?? ""  // Paket Ücretlendirme seçimi
            );
            this.Controls.Add(btnKaydet);
        }
        private TextBox AddLabeledTextbox(string label, int y, int labelX, int textX)
        {
            Label lbl = new Label() { Text = label, Location = new System.Drawing.Point(labelX, y), AutoSize = true };
            TextBox txt = new TextBox() { Location = new System.Drawing.Point(textX, y), Width = 200 };
            this.Controls.Add(lbl);
            this.Controls.Add(txt);
            return txt;
        }

        private void BtnKaydet_Click(
     string imei, string marka, string seriNumarasi, string cihazDurumu, string cihazEkipman,
     string telekomSirketi, string telefonNo, string pinPukDurumu, string tarife, string kurumiciDak,
     string kurumDisiDak, string smsPaket, string internetPaket, string paketUcret)
        {


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO TelefonVeSIM 
            ( Ad, Soyad,IMEINumarası, ÜrünID, Marka, SeriNumarası, CihazDurumu, CihazTeslimEkipman, TelekomŞirketi, 
            TelefonNumarası, PinPukBildirimi, TarifeTanımı, KurumİçiDakikaPaketi, KurumDışıDakikaPaketi, 
            SmsPaketi, İnternetPaketi, PaketÜcretlendirme) 
            VALUES 
            ( @Ad, @Soyad,@IMEI, @UrunID, @Marka, @SeriNumarasi, @CihazDurumu, @CihazEkipman, @TelekomSirketi, 
            @TelefonNo, @PinPuk, @Tarife, @KurumiciDak, @KurumDisiDak, @SmsPaket, @InternetPaket, @PaketUcret)";

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

                    cmd.Parameters.AddWithValue("@IMEI", imei);
                    cmd.Parameters.AddWithValue("@UrunID", this.urunID); // 📌 Bunu ekledik!
                    cmd.Parameters.AddWithValue("@Marka", marka);

                    cmd.Parameters.AddWithValue("@SeriNumarasi", seriNumarasi);
                    cmd.Parameters.AddWithValue("@CihazDurumu", cihazDurumu);
                    cmd.Parameters.AddWithValue("@CihazEkipman", cihazEkipman);
                    cmd.Parameters.AddWithValue("@TelekomSirketi", telekomSirketi);
                    cmd.Parameters.AddWithValue("@TelefonNo", telefonNo);
                    cmd.Parameters.AddWithValue("@PinPuk", pinPukDurumu);
                    cmd.Parameters.AddWithValue("@Tarife", tarife);
                    cmd.Parameters.AddWithValue("@KurumiciDak", kurumiciDak);
                    cmd.Parameters.AddWithValue("@KurumDisiDak", kurumDisiDak);
                    cmd.Parameters.AddWithValue("@SmsPaket", smsPaket);
                    cmd.Parameters.AddWithValue("@InternetPaket", internetPaket);
                    cmd.Parameters.AddWithValue("@PaketUcret", paketUcret);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Telefon ve SIM başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Kayıt eklenirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (imei is null || imei == "")
                    {
                        MessageBox.Show("Lütfen IMEI Numarasını giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            Logger.Kaydet("Telefon ve SIM eklendi", AdSoyad);
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
    }
}