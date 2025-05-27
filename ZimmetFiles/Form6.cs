using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using System.Globalization;
using System.Reflection;




namespace ZWebApp
{

    public partial class Form6 : BaseForm
    {
        private string pdfPath = "";
        private string yeniPdfPath = "";

        private string teslimAlan;
        private string teslimEden;

        private Button btnSec;
        private Button btnGuncelle;

        public Form6(string teslimAlan, string teslimEden)
        {
            InitializeComponent();
            this.teslimAlan = teslimAlan;
            this.teslimEden = teslimEden;

            FormuOlustur();
        }

        private string GetDefaultPdfPath()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "tutanak_temp.pdf");

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "ZWebApp.tutanak.pdf"; // Bunu kesin manifest'ten aldın dimi aşkım

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        string[] kaynaklar = assembly.GetManifestResourceNames();
                        MessageBox.Show("Gömülü PDF bulunamadı!\n\nVar olanlar:\n" + string.Join("\n", kaynaklar));
                        return null;
                    }

                    using (FileStream fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                return tempPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF dışarı çıkarılamadı: " + ex.Message);
                return null;
            }
        }




        public void FormuOlustur()
        {
            this.Text = "Tutanak Güncelle";
            this.Size = new System.Drawing.Size(390, 180);
            this.StartPosition = FormStartPosition.CenterScreen;

            btnSec = new Button
            {
                Text = "PDF Seç",
                Location = new System.Drawing.Point(20, 20),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(100, 80),
            };
            btnSec.Click += BtnSec_Click;
            this.Controls.Add(btnSec);

            btnGuncelle = new Button
            {
                Text = "Tutanak Güncelle",
                Location = new System.Drawing.Point(140, 20),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(200, 80),

            };
            btnGuncelle.Click += BtnGuncelle_Click;
            this.Controls.Add(btnGuncelle);
        }

        private void BtnSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Dosyaları (*.pdf)|*.pdf",
                Title = "Tutanak Şablonunu Seç"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pdfPath = openFileDialog.FileName;
                MessageBox.Show($"Seçilen Dosya: {pdfPath}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pdfPath))
            {
                pdfPath = GetDefaultPdfPath();
            }

            if (string.IsNullOrEmpty(pdfPath) || !File.Exists(pdfPath))
            {
                MessageBox.Show("PDF dosyası yüklenemedi.");
                return;
            }

            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Tutanak kaydedilecek klasörü seçin";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string secilenKlasor = folderDialog.SelectedPath;

                    // Dosya adı oluştur
                    string dosyaAdi = $"{teslimAlan.Replace(" ", "_")}_Tutanak.pdf";
                    yeniPdfPath = Path.Combine(secilenKlasor, dosyaAdi);
                }
                else
                {
                    MessageBox.Show("Klasör seçilmedi, işlem iptal edildi.");
                    return;
                }
            }





            TutanakGuncelle();
            Logger.Kaydet("Tutanak oluşturuldu", teslimAlan);
        }

        private void TutanakGuncelle()
        {
            try
            {
                using (PdfReader reader = new PdfReader(pdfPath))
                using (PdfWriter writer = new PdfWriter(yeniPdfPath))
                using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                {
                    PdfPage page = pdfDoc.GetFirstPage();
                    PdfCanvas canvas = new PdfCanvas(page);

                    PdfFont font = PdfFontFactory.CreateFont("C:/Windows/Fonts/arial.ttf", PdfEncodings.IDENTITY_H);
                    float pageWidth = page.GetPageSize().GetWidth();
                    float pageHeight = page.GetPageSize().GetHeight();


                    float tarihYKoordinati = pageHeight - 150; // Başlığın biraz altına
                    string tarih = DateTime.Now.ToString("dd.MM.yyyy");

                    canvas.BeginText()
                          .SetFontAndSize(font, 12)
                          .MoveText(pageWidth - 150, tarihYKoordinati)
                          .ShowText("Tarih: " + tarih)
                          .EndText();


                    float yAltKoordinat = 280;
                    canvas.BeginText()
                          .SetFontAndSize(font, 12)
                          .MoveText(100, yAltKoordinat) // Teslim Eden başlığının hemen altı
                          .ShowText($"{teslimEden}")
                          .EndText();

                    string soyadBuyuk = teslimAlan.Split(' ').Last().ToUpper(new CultureInfo("tr-TR"));
                    string isim = string.Join(" ", teslimAlan.Split(' ').SkipLast(1)); // İlk kelimeleri al
                    string tamIsim = isim + " " + soyadBuyuk;

                    canvas.BeginText()
                          .SetFontAndSize(font, 12)
                          .MoveText(380, yAltKoordinat) // Teslim Alan başlığının hemen altı
                          .ShowText($"{tamIsim}")
                          .EndText();

                }

                MessageBox.Show($"Tutanak oluşturuldu: {yeniPdfPath}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(yeniPdfPath) { UseShellExecute = true });
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // teslimAlan bilgisinden doğru ad ve soyad ayır
            string[] parcalar = teslimAlan.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string soyad = parcalar[^1];  // son parça soyad
            string ad = string.Join(" ", parcalar[..^1]);  // geri kalanlar ad

            byte[] pdfBytes = File.ReadAllBytes(yeniPdfPath);
            BelgeOlustur.VeritabaniyaKaydet(
                ad,
                soyad,
                Path.GetFileName(yeniPdfPath),
                "Tutanak",
                pdfBytes,
                false


            );


        }

    }
}

