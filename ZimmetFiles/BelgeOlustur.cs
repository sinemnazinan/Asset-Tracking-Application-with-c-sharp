using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace ZWebApp
{

    public class BelgeOlustur
    {
        private static string fontPath = @"C:\Windows\Fonts\arial.ttf"; // Windows için Arial
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;        // **Arial Bold Fontunu Yükle**

        PdfFont arialFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
        PdfFont arialBoldFont = PdfFontFactory.CreateFont("C:/Windows/Fonts/arialbd.ttf", PdfEncodings.IDENTITY_H);


        public async void ZimmetBelgesiOlustur(string ad, string soyad, string teslimEden)
        {
            DataTable zimmetData = GetZimmetData(ad, soyad);
            DataTable kisiData = GetKullaniciBilgileri(ad, soyad);
            DataTable urunData = GetZimmetliUrunler(ad, soyad);

            if (zimmetData.Rows.Count == 0)
            {
                MessageBox.Show("Veri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (kisiData.Rows.Count == 0)
            {
                MessageBox.Show("Teslim Alan bilgisi bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool telefonSimKayitVarMi = !string.IsNullOrEmpty(zimmetData.Rows[0]["TelefonNumarası"].ToString())
                                      || !string.IsNullOrEmpty(zimmetData.Rows[0]["IMEINumarası"].ToString());

            if (!telefonSimKayitVarMi && urunData.Rows.Count == 0)
            {
                MessageBox.Show("Telefon-SIM kaydı ve zimmetli ürün kaydı bulunamadı. Belge oluşturulmayacak.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Dosyası|*.pdf";
            saveFileDialog.Title = "Zimmet Belgesi Kaydet";
            saveFileDialog.FileName = $"{ad}_{soyad}_Zimmet.pdf"; // otomatik isim

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            if (telefonSimKayitVarMi)
            {
                using (PdfWriter writer = new PdfWriter(saveFileDialog.FileName))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf))
                {
                    document.Add(new Paragraph("CEP TELEFONU & GSM SIM KART VE HAT ZİMMET TESLİM TUTANAĞI")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetFont(arialBoldFont)); // Helvetica yerine Arial kullan

                    Table table = new Table(2).UseAllAvailableWidth(); // 2 sütunlu tablo
                    table.AddCell(new Cell().Add(new Paragraph("TARİH: " + DateTime.Now.ToString("dd/MM/yyyy")).SetFont(arialFont))
                                            .SetBorder(Border.NO_BORDER)
                                            .SetTextAlignment(TextAlignment.LEFT));

                    table.AddCell(new Cell().Add(new Paragraph("TUTANAK NUMARASI: " + GetNextTutanakNumarasi()).SetFont(arialFont))
                                            .SetBorder(Border.NO_BORDER)
                                            .SetTextAlignment(TextAlignment.RIGHT));

                    document.Add(table);

                    document.Add(new Paragraph("\nZİMMETLİ OLARAK TESLİM EDİLEN CEP TELEFONU VE SIM KART")
                        .SetFont(arialBoldFont)  // ✅ Arial Bold fontunu kullan
                        .SetUnderline()
                        .SetMarginBottom(5)
                        .SetMultipliedLeading(0.8f));

                    // 2 sütunlu yeni bir tablo oluştur (Başlık | Değer)
                    Table bilgiTablosu = new Table(2).UseAllAvailableWidth().SetPaddingRight(180f);

                    // **Başlık ve Değerleri Ekleyelim**
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Marka:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["Üretici"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Model:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["Model"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Seri Numarası:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["SeriNumarası"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("IMEI Numarası:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["IMEINumarası"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Cihaz Durumu:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["CihazDurumu"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Telekominikasyon Şirketi:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["TelekomŞirketi"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Telefon Numarası:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["TelefonNumarası"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Tarife Tanımı:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["TarifeTanımı"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Kurum İçi Dakika Paketi:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["KurumİçiDakikaPaketi"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("Kurum Dışı Dakika Paketi:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["KurumDışıDakikaPaketi"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph("SMS Paketi:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    bilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["SmsPaketi"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    // **Tabloyu PDF'e ekleyelim**
                    document.Add(bilgiTablosu);

                    document.Add(new Paragraph("\nÇALIŞAN KİMLİK BİLGİLERİ")
                        .SetFont(arialBoldFont)  // ✅ Arial Bold fontunu kullan
                        .SetUnderline()
                        .SetMarginBottom(5)
                        .SetMultipliedLeading(0.8f));
                    // **ÇALIŞAN BİLGİLERİ TABLOSU**
                    Table calisanBilgiTablosu = new Table(2).UseAllAvailableWidth();

                    calisanBilgiTablosu.AddCell(new Cell().Add(new Paragraph("Ad Soyad:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER)).SetPaddingRight(50f);
                    calisanBilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["Ad"].ToString() + " " + zimmetData.Rows[0]["Soyad"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    calisanBilgiTablosu.AddCell(new Cell().Add(new Paragraph("TC Kimlik No:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    calisanBilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["TCkimlik"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    calisanBilgiTablosu.AddCell(new Cell().Add(new Paragraph("Görev:").SetFont(arialBoldFont)).SetBorder(Border.NO_BORDER));
                    calisanBilgiTablosu.AddCell(new Cell().Add(new Paragraph(zimmetData.Rows[0]["Görev"].ToString()).SetFont(arialFont)).SetBorder(Border.NO_BORDER));

                    // **Tabloyu PDF'e ekleyelim**
                    document.Add(calisanBilgiTablosu);

                    // Sözleşme metni
                    document.Add(new Paragraph("•Yukarıda bilgileri belirtilen cep telefonu ve belirtilmiş ise cep telefonu hattı yukarıda " +
                    "kimlik bilgileri belirtilen çalışana şirket telefon görüşmeleri ve iletişimi için teslim " +
                    "edilmiş ve zimmetlenmiştir. Cihaz ve hat bilgilerini kabul, beyan ve taahhüt ederim.")
                        .SetFont(arialFont)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMarginBottom(0)
                        .SetMultipliedLeading(1f));

                    document.Add(new Paragraph("•Çalışan emanet olarak aldığı cep telefonunu ile iş konulu görüşmeler yapacaktır ve iş bu teslim " +
                    "tutanağında belirtilen şirket cep telefonu kullanımı ve cep telefonu hattı kullanım politikalarını " +
                    "eksiksiz biçimde okumuştur, anlamıştır, kabul etmiştir ve kurallara uyacağını beyan ve taahhüt " +
                    "etmiştir.")
                        .SetFont(arialFont)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMarginBottom(0)
                        .SetMultipliedLeading(1f));

                    document.Add(new Paragraph("•Telefon kusursuz çalışır durumda çalışana teslim edilmiştir. Teslim alınırken çalışan tarafından " +
                    "tüm fonksiyonları kontrol edilmiştir, cihaz fabrika ayarlarında teslim edilmiştir, kurum " +
                    "kullanımındaki yazılımlar eksiksiz olarak yüklenmiştir.")
                        .SetFont(arialFont)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMarginBottom(0)
                        .SetMultipliedLeading(1f));

                    document.Add(new Paragraph("•Çalışan cihazı usulüne göre kullanacağını, yalnızca iş görüşmeleri ve iletişimi için kullanacağını, " +
                    "paket görüşme süresi, SMS adeti ve veri kullanım miktarlarına dikkat ederek tanımlanan miktarların " +
                    "dışında kullanım yapmayacağını, mesai saatleri dışında acil durumlar dışında kullanım yapmayacağını " +
                    "ve cihazı kapalı durumda tutacağını, şirket için gerekli programları dışında hiçbir program " +
                    "yüklemeyeceğini ve iş dışı konularda hiçbir cep telefonu ve hattının sağladığı hiçbir iletişim " +
                    "ve veri erişim yöntemini kullanmayacağını kabul, beyan ve taahhüt ederim.")
                        .SetFont(arialFont)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMarginBottom(0)
                        .SetMultipliedLeading(1f));

                    document.Add(new Paragraph("•Çalışan olarak cep telefonu ve cep telefonu hattı ile ilgili olabilecek ve konu olabilecek emniyet " +
                    "ve adli her tür işlemde tüm sorumluluğun bana ait olduğunu kabul, beyan ve taahhüt ederim.")
                        .SetFont(arialFont)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMarginBottom(0)
                        .SetMultipliedLeading(1f));

                    document.Add(new Paragraph("\nTeslim Eden : " + teslimEden + "\t\t\t\t İmza\n")
                        .SetMarginBottom(0)
                        .SetMultipliedLeading(1f)
                        .SetFont(arialBoldFont));

                    document.Add(new Paragraph($"Teslim Alan : {zimmetData.Rows[0]["Ad"]} {zimmetData.Rows[0]["Soyad"]} \t\t\t\t İmza\n")
                        .SetMarginBottom(0)
                        .SetMultipliedLeading(0.8f)
                        .SetFont(arialBoldFont));
                    ZimmetBelgesiIkinciSayfaEkle(pdf, document, ad, soyad, teslimEden);
                    document.Close(); // PDF'i kapat
                }
            }
            else
            {
                using (PdfWriter writer = new PdfWriter(saveFileDialog.FileName))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf))
                {
                    // Direk ikinci sayfa (başka sayfa yok zaten)
                    ZimmetBelgesiIkinciSayfaEkle(pdf, document, ad, soyad, teslimEden);
                    document.Close(); // PDF'i kapat
                }
            }


            // PDF dosyasını aç
            Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
            byte[] pdfBytes = File.ReadAllBytes(saveFileDialog.FileName);
            VeritabaniyaKaydet(ad, soyad, Path.GetFileName(saveFileDialog.FileName), "Zimmet", pdfBytes, false);


            // 1 saniye bekle PDF açılması için
            await Task.Delay(1000);



            Logger.Kaydet("Zimmet belgesi oluşturuldu", ad + " " + soyad);
        }

        public static DataTable GetZimmetData(string ad, string soyad)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
               SELECT 
                    k.Ad, k.Soyad, k.Görev, k.TeslimTarihi, k.Tc AS TcKimlik,
                    k.İşTelefonu, k.EvTelefonu, k.FaksNumarası, k.Email,
                    u.ÜrünID, u.ÜrünAdı, u.Üretici, u.Model, u.AlışTarihi, u.Durum,
                    t.IMEINumarası, t.SeriNumarası, t.CihazDurumu, t.TelekomŞirketi, t.TelefonNumarası, t.TarifeTanımı, t.KurumİçiDakikaPaketi, 
                    t.KurumDışıDakikaPaketi, t.SmsPaketi, t.İnternetPaketi, t.PaketÜcretlendirme
                FROM Kullanıcılar k
                LEFT JOIN Ürünler u ON k.Ad = u.Ad AND k.Soyad = u.Soyad
                LEFT JOIN TelefonVeSIM t ON k.Ad = t.Ad AND k.Soyad = t.Soyad
                WHERE k.Ad = @Ad AND k.Soyad = @Soyad";


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ad", ad);
                    cmd.Parameters.AddWithValue("@Soyad", soyad);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    conn.Open();
                    da.Fill(dt);
                }
            }
            return dt;
        }


        private string GetNextTutanakNumarasi()
        {
            int yil = DateTime.Now.Year;
            int yeniSiraNo = 1;
            string tutanakNo = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 🔹 Tüm yıllardaki en büyük SiraNo'yu al
                string query = "SELECT MAX(SiraNo) FROM TutanakNumaralari";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        yeniSiraNo = Convert.ToInt32(result) + 1;  // 🔹 Global olarak artır
                    }
                }

                // Yeni tutanak numarasını oluştur
                tutanakNo = $"{yil}/{yeniSiraNo}";

                // Yeni numarayı veritabanına kaydet
                string insertQuery = "INSERT INTO TutanakNumaralari (Yil, SiraNo) VALUES (@Yil, @SiraNo)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Yil", yil);
                    cmd.Parameters.AddWithValue("@SiraNo", yeniSiraNo);
                    cmd.ExecuteNonQuery();
                }
            }

            return tutanakNo;
        }



        public static DataTable GetZimmetliUrunler(string ad, string soyad)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT
                        u.ÜrünID AS No,
                        u.ÜrünAdı AS Ürün,
                        u.Üretici,
                        u.Model,
                        u.AlışTarihi,
                        u.Durum,
                        u.Açıklama
                    FROM Ürünler u
                    WHERE u.Ad = @Ad AND u.Soyad = @Soyad;
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ad", ad);
                    cmd.Parameters.AddWithValue("@Soyad", soyad);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    conn.Open();
                    da.Fill(dt);
                }
                return dt;
            }
        }


        public static DataTable GetKullaniciBilgileri(string ad, string soyad)

        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                        SELECT Ad, Soyad, Tc AS TcKimlik, Şirket, Departman, Görev, 
                               CepTelefonu, İşTelefonu, EvTelefonu, FaksNumarası, Email
                        FROM Kullanıcılar
                        WHERE Ad = @Ad AND Soyad = @Soyad";


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ad", ad);
                    cmd.Parameters.AddWithValue("@Soyad", soyad);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    conn.Open();
                    da.Fill(dt);
                }
            }
            return dt;
        }

        private void ZimmetBelgesiIkinciSayfaEkle(PdfDocument pdf, Document document, string ad, string soyad, string teslimEden)

        {
            DataTable urunData = GetZimmetliUrunler(ad, soyad);
            DataTable kisiData = GetKullaniciBilgileri(ad, soyad);


            if (urunData.Rows.Count == 0)
            {
                document.Add(new Paragraph("\nKullanıcıya ait zimmetli ürün bulunmamaktadır.\n").SetFontSize(12));
                return;
            }
            // Eğer zaten ilk sayfa yoksa ve bu metot ilk sayfa olacaksa next page ekleme!
            if (pdf.GetNumberOfPages() > 0)
            {
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE)); // Sadece ilk sayfa varsa yeni sayfa aç
            }
            Table table = new Table(2).UseAllAvailableWidth(); // 2 sütunlu tablo
            table.AddCell(new Cell().Add(new Paragraph("TARİH: " + DateTime.Now.ToString("dd/MM/yyyy")).SetFont(arialFont))
                                    .SetBorder(Border.NO_BORDER)
                                    .SetTextAlignment(TextAlignment.LEFT));

            table.AddCell(new Cell().Add(new Paragraph("TUTANAK NUMARASI: " + GetNextTutanakNumarasi()).SetFont(arialFont))
                                    .SetBorder(Border.NO_BORDER)
                                    .SetTextAlignment(TextAlignment.RIGHT));

            document.Add(table);
            document.Add(new Paragraph("ZİMMET FORMU")
                .SetFont(arialBoldFont)
                .SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER));

            // 📌 **Önce Kullanıcı Bilgileri Tablosunu Ekleyelim**
            document.Add(new Paragraph("KULLANICI BİLGİLERİ")
                .SetFont(arialBoldFont)
                .SetFontSize(12));

            float[] columnWidths = { 100, 150, 100, 150 }; // İlk ve üçüncü sütun sabit genişlikte olacak
            Table kisiTable = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

            string GetValue(object obj) => obj == DBNull.Value || obj == null || string.IsNullOrEmpty(obj.ToString()) ? " - " : obj.ToString();

            kisiTable.AddCell(new Cell().Add(new Paragraph("Ad:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["Ad"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("Soyad:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["Soyad"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("TC Kimlik:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["TcKimlik"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("Şirket:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["Şirket"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("Departman:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["Departman"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("Görev:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["Görev"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("Cep Telefonu:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["CepTelefonu"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("İş Telefonu:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["İşTelefonu"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("Ev Telefonu:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["EvTelefonu"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("Faks Numarası:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["FaksNumarası"])).SetFont(arialFont)));

            kisiTable.AddCell(new Cell().Add(new Paragraph("Email:").SetFont(arialBoldFont)));
            kisiTable.AddCell(new Cell().Add(new Paragraph(GetValue(kisiData.Rows[0]["Email"])).SetFont(arialFont)));

            document.Add(kisiTable);
            int sayac = 1;
            foreach (DataRow row in urunData.Rows)
            {
                Table urunTable = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

                // **Ürün Başlığı**
                document.Add(new Paragraph("\nÜrün Bilgileri")
                    .SetFont(arialBoldFont)
                    .SetFontSize(12));


                urunTable.AddCell(new Cell().Add(new Paragraph("No:").SetFont(arialBoldFont)));
                urunTable.AddCell(new Cell().Add(new Paragraph(sayac.ToString()).SetFont(arialFont))); // ✔ Artık sıralı

                urunTable.AddCell(new Cell().Add(new Paragraph("Ürün:").SetFont(arialBoldFont)));
                urunTable.AddCell(new Cell().Add(new Paragraph(GetValue(row["Ürün"])).SetFont(arialFont)));

                urunTable.AddCell(new Cell().Add(new Paragraph("Üretici:").SetFont(arialBoldFont)));
                urunTable.AddCell(new Cell().Add(new Paragraph(GetValue(row["Üretici"])).SetFont(arialFont)));

                urunTable.AddCell(new Cell().Add(new Paragraph("Model:").SetFont(arialBoldFont)));
                urunTable.AddCell(new Cell().Add(new Paragraph(GetValue(row["Model"])).SetFont(arialFont)));

                urunTable.AddCell(new Cell().Add(new Paragraph("Alış Tarihi:").SetFont(arialBoldFont)));
                urunTable.AddCell(new Cell().Add(new Paragraph(GetValue(row["AlışTarihi"])).SetFont(arialFont)));

                urunTable.AddCell(new Cell().Add(new Paragraph("Durum:").SetFont(arialBoldFont)));
                urunTable.AddCell(new Cell().Add(new Paragraph(GetValue(row["Durum"])).SetFont(arialFont)));

                urunTable.AddCell(new Cell().Add(new Paragraph("Açıklama:").SetFont(arialBoldFont)));
                urunTable.AddCell(new Cell(2, 4).Add(new Paragraph(GetValue(row["Açıklama"])).SetFont(arialFont)));

                document.Add(urunTable);
                sayac++;
            }

            document.Add(new Paragraph("\nSÖZLEŞME METNİ")
                .SetFont(arialBoldFont)
                .SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("• Ekte listede yer alan ürünler işyerinde ve kullanıcının görevine " +
                "ait işlerde kullanılmak üzere, formda bilgileri yer alan personele iş bu tutanak ile teslim edilmiştir.")
                .SetFont(arialFont)
                .SetFontSize(12)
                .SetMarginBottom(0)
                .SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph("• Zimmetlenen bu ürünler Bera Holding Bilgi Teknolojileri departmanı " +
                "görevlileri tarafından kontrolden geçirilmiş olup üzerinde hiçbir lisanssız yazılım bulunmamaktadır.")
                .SetFont(arialFont)
                .SetFontSize(12)
                .SetMarginBottom(0)
                .SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph("• Ayrıca kullanıcı, kullanıma sunulan; donanım, yazılım, internet, " +
                "mail, kurumsal telefon ve faks hizmetlerini yönetmeliğe uygun şekilde, işyerinde " +
                "ve görevine ait işlerde kullanacağını, lisanssız hiçbir yazılım yüklemeyeceğini, Aksi halde doğabilecek " +
                "hukuki ve cezai sorumluluğun tarafına ait olduğunu taahhüt eder")
                .SetFont(arialFont)
                .SetFontSize(12)
                .SetMarginBottom(0)
                .SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph("\nTeslim Eden : " + teslimEden + " \t\t\t\t İmza\n")
                .SetMarginBottom(10)
                .SetMultipliedLeading(1f)
                .SetFont(arialBoldFont));

            document.Add(new Paragraph($"Teslim Alan : {kisiData.Rows[0]["Ad"]} {kisiData.Rows[0]["Soyad"]} \t\t\t\t İmza\n")
                .SetMarginBottom(10)
                .SetMultipliedLeading(1f)
                .SetFont(arialBoldFont));
        }
        public static void VeritabaniyaKaydet(string ad, string soyad, string belgeAdi, string belgeTuru, byte[] icerik, bool imzaliMi)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Belgeler (KullanıcıAd, KullanıcıSoyad, BelgeAdı, BelgeTuru, BelgeIcerik, Imzali, OlusturmaTarihi)
                         VALUES (@Ad, @Soyad, @BelgeAdı, @BelgeTuru, @Icerik, @Imzali, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ad", ad);
                    cmd.Parameters.AddWithValue("@Soyad", soyad);
                    cmd.Parameters.AddWithValue("@BelgeAdı", belgeAdi);
                    cmd.Parameters.AddWithValue("@BelgeTuru", belgeTuru);
                    cmd.Parameters.AddWithValue("@Icerik", icerik);
                    cmd.Parameters.AddWithValue("@Imzali", imzaliMi);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}








