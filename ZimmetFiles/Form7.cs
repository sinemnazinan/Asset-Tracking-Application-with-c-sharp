using Microsoft.Data.SqlClient;
using System.Data;

namespace ZWebApp
{

    public partial class Form7 : BaseForm
    {
        private string adSoyad;
        private DataGridView dataGridView1;
        private PictureBox logo; // Logo için sınıf değişkeni
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;
        public Form7(string adSoyad)
        {
            InitializeComponent();
            this.adSoyad = adSoyad;
            this.Load += Form7_Load;
            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Kişinin Tüm Zimmetlerini Gör";
            this.Size = new System.Drawing.Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeDataGrid();

        }
        private void InitializeDataGrid()
        {
            dataGridView1 = new DataGridView();
            this.WindowState = FormWindowState.Maximized;

            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ScrollBars = ScrollBars.Both;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 12);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 13, FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 40; // veya 50 deneyebilirsin, yazıya göre
            this.Controls.Add(dataGridView1); // Form'a ekle

            // 1. Alt panel (butonların geleceği yer)
            Panel panelAlt = new Panel();
            panelAlt.Dock = DockStyle.Bottom;
            panelAlt.Height = 80;
            panelAlt.BackColor = Color.White;

            // 2. Belge Oluştur butonu
            Button btnBelgeOlustur = new Button();
            btnBelgeOlustur.Text = "Belge Oluştur";
            btnBelgeOlustur.Font = new Font("Arial", 10, FontStyle.Bold);
            btnBelgeOlustur.Width = 150;
            btnBelgeOlustur.Height = 40;
            btnBelgeOlustur.Location = new Point((this.ClientSize.Width / 2) - 170, 20); // sola doğru
            btnBelgeOlustur.Anchor = AnchorStyles.Bottom;

            // 3. Tutanak Oluştur butonu
            Button btnTutanakOlustur = new Button();
            btnTutanakOlustur.Text = "Tutanak Oluştur";
            btnTutanakOlustur.Font = new Font("Arial", 10, FontStyle.Bold);
            btnTutanakOlustur.Width = 150;
            btnTutanakOlustur.Height = 40;
            btnTutanakOlustur.Location = new Point((this.ClientSize.Width / 2) + 20, 20); // sağa doğru
            btnTutanakOlustur.Anchor = AnchorStyles.Bottom;

            // 4. Butonları panele ekle
            panelAlt.Controls.Add(btnBelgeOlustur);
            panelAlt.Controls.Add(btnTutanakOlustur);

            //Panele eklenen butonların konumunu ayarla
            btnBelgeOlustur.Location = new Point((panelAlt.Width - btnBelgeOlustur.Width) / 2 - 100, (panelAlt.Height - btnBelgeOlustur.Height) / 2);
            btnTutanakOlustur.Location = new Point((panelAlt.Width - btnTutanakOlustur.Width) / 2 + 100, (panelAlt.Height - btnTutanakOlustur.Height) / 2);
            // Butonların konumunu ayarla
            btnBelgeOlustur.Anchor = AnchorStyles.Bottom;
            btnTutanakOlustur.Anchor = AnchorStyles.Bottom;
            // Butonların boyutunu ayarla
            btnBelgeOlustur.Size = new Size(150, 40);
            btnTutanakOlustur.Size = new Size(150, 40);
            // Butonların fontunu ayarla
            btnBelgeOlustur.Font = new Font("Arial", 10, FontStyle.Bold);
            btnTutanakOlustur.Font = new Font("Arial", 10, FontStyle.Bold);
            // Butonların arka plan rengini ayarla
            btnBelgeOlustur.BackColor = Color.LightBlue;
            btnTutanakOlustur.BackColor = Color.LightGreen;
            // Butonların metin rengini ayarla
            btnBelgeOlustur.ForeColor = Color.Black;
            btnTutanakOlustur.ForeColor = Color.Black;
            // Butonların kenarlarını yuvarla
            btnBelgeOlustur.FlatStyle = FlatStyle.Flat;
            btnTutanakOlustur.FlatStyle = FlatStyle.Flat;
            btnBelgeOlustur.FlatAppearance.BorderSize = 0;
            btnTutanakOlustur.FlatAppearance.BorderSize = 0;
            // Butonların kenarlarını yuvarla
            btnBelgeOlustur.FlatAppearance.BorderColor = Color.LightBlue;
            btnTutanakOlustur.FlatAppearance.BorderColor = Color.LightGreen;

            //Panele resim ekle
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Resource1.logo;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Size = new Size(120, 120);
            pictureBox.Location = new Point(10, (panelAlt.Height - pictureBox.Height) / 2);
            pictureBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            panelAlt.Controls.Add(pictureBox);

            // Sağ köşe logo
            PictureBox logoSag = new PictureBox();
            logoSag.Image = Resource1.logo;
            logoSag.SizeMode = PictureBoxSizeMode.Zoom;
            logoSag.Size = new Size(120, 120);
            logoSag.Location = new Point(panelAlt.Width - logoSag.Width - 10, (panelAlt.Height - logoSag.Height) / 2);
            logoSag.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            panelAlt.Controls.Add(logoSag);
            // 5. Paneli forma ekle
            this.Controls.Add(panelAlt);





            btnBelgeOlustur.Click += BtnBelgeOlustur_Click;
            btnTutanakOlustur.Click += BtnTutanakOlustur_Click;

        }

        private void Form7_Load(object sender, EventArgs e)
        {
            logo = new PictureBox();
            logo.Name = "Bera Holding_logo"; // Control dizininde erişilebilir olması için
            logo.Image = Resource1.logo;
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Size = new Size(400, 200);

            // En alta konumlandır
            logo.Location = new Point((this.ClientSize.Width - logo.Width) / 2, this.ClientSize.Height - 300);
            logo.Anchor = AnchorStyles.Bottom;

            this.Controls.Add(logo);
            // Formun tam ekranın ortasında açılmasını sağla (hem yatay hem dikey olarak)
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,  // Yatay ortalama
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 3 // Dikey ortalama
            );

            try
            {
                string[] isimParcalari = adSoyad.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (isimParcalari.Length < 2)
                {
                    MessageBox.Show("Ad ve soyad ayrıştırılamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string soyad = isimParcalari[isimParcalari.Length - 1];
                string ad = string.Join(" ", isimParcalari, 0, isimParcalari.Length - 1);


                string query = @"SELECT
                            Ürünler.*,
                            TelefonVeSIM.TelefonNumarası,
                            TelefonVeSIM.IMEINumarası,
                            TelefonVeSIM.SeriNumarası,
                            TelefonVeSIM.CihazDurumu,
                            TelefonVeSIM.TelekomŞirketi,
                            TelefonVeSIM.TarifeTanımı,
                            TelefonVeSIM.KurumİçiDakikaPaketi,
                            TelefonVeSIM.KurumDışıDakikaPaketi,
                            TelefonVeSIM.SmsPaketi,
                            TelefonVeSIM.İnternetPaketi,
                            TelefonVeSIM.PaketÜcretlendirme
                        FROM Ürünler
                        LEFT JOIN TelefonVeSIM ON Ürünler.ÜrünID = TelefonVeSIM.ÜrünID
                        WHERE Ürünler.Ad = @Ad AND Ürünler.Soyad = @Soyad;";


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@Ad", ad);
                        da.SelectCommand.Parameters.AddWithValue("@Soyad", soyad);

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // DataGrid'e veri yükle
                        dataGridView1.DataSource = dt;

                        if (dataGridView1.Columns.Contains("ÜrünID"))
                        {
                            dataGridView1.Columns["ÜrünID"].Visible = false;
                        }

                        // DataGrid'in satır sayısına göre formun yüksekliğini ayarla
                        int rowCount = dataGridView1.RowCount;
                        int rowHeight = dataGridView1.RowTemplate.Height;

                        // Form yüksekliğini, satır sayısı ve biraz padding ile ayarla
                        this.Height = 150 + (rowCount * rowHeight); // 150 formun başlangıç yüksekliği, padding ekledik

                        dataGridView1.Columns["Ad"].DisplayIndex = 0;
                        dataGridView1.Columns["Soyad"].DisplayIndex = 1;

                        // Tablonun genişliğine göre formun genişliğini ayarla
                        int totalWidth = 0;
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            totalWidth += column.Width;
                        }

                        // Form genişliğini ayarla
                        this.Width = totalWidth + 40; // Genişliği dinamik olarak ayarla, biraz padding ekleyelim

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnBelgeOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                string[] isimParcalari = adSoyad.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (isimParcalari.Length < 2)
                {
                    MessageBox.Show("Ad ve soyad ayrıştırılamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string soyad = isimParcalari[isimParcalari.Length - 1];
                string ad = string.Join(" ", isimParcalari, 0, isimParcalari.Length - 1);

                string teslimEden = Session.KullaniciAdi;

                BelgeOlustur belge = new BelgeOlustur();
                belge.ZimmetBelgesiOlustur(ad, soyad, teslimEden);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Belge oluşturulurken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnTutanakOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                string[] isimParcalari = adSoyad.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (isimParcalari.Length < 2)
                {
                    MessageBox.Show("Ad ve soyad ayrıştırılamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string soyad = isimParcalari[isimParcalari.Length - 1];
                string ad = string.Join(" ", isimParcalari, 0, isimParcalari.Length - 1);
                string teslimEden = Session.KullaniciAdi;

                string teslimAlan = $"{ad} {soyad}";
                Form6 tutanak = new Form6(teslimAlan, teslimEden);
                tutanak.FormuOlustur();
                tutanak.ShowDialog(); // Formu göster
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tutanak oluşturulurken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    }
}
