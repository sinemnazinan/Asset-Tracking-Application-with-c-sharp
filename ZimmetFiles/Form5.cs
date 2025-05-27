using Microsoft.Data.SqlClient;
using System.Data;

namespace ZWebApp
{
    public partial class Form5 : BaseForm
    {
        private string adSoyad;
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;
        private DataGridView dataGridView1;

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
            dataGridView1.CellDoubleClick += DgvZimmetler_CellDoubleClick;
            // 1. Alt panel (butonların geleceği yer)
            Panel panelAlt = new Panel();
            panelAlt.Dock = DockStyle.Bottom;
            panelAlt.Height = 80;
            panelAlt.BackColor = Color.White;

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


            this.Controls.Add(panelAlt);

        }
        public Form5(string girilenAdSoyad)
        {
            InitializeComponent();
            adSoyad = girilenAdSoyad;
            this.Load += new System.EventHandler(this.Form5_Load);
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Kişinin Zimmetini Düş";
            this.Size = new System.Drawing.Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeDataGrid(); // DataGrid'i oluştur
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            string[] isimParcalari = adSoyad.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (isimParcalari.Length < 2)
            {
                MessageBox.Show("Ad ve soyad bilgisi eksik veya hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
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
                        this.Width = totalWidth + 40;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DgvZimmetler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dgv = sender as DataGridView;
                string urunID = dgv.Rows[e.RowIndex].Cells["ÜrünID"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    "Bu ürünü zimmetten düşmek istediğinize emin misiniz?",
                    "Zimmet Düşür",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = @"
                    -- Eğer ürün bir telefona bağlıysa önce TelefonVeSIM tablosundan sil
                    DELETE FROM TelefonVeSIM WHERE ÜrünID = @ÜrünID;

                    -- Ardından ürünü Ürünler tablosundan sil
                    DELETE FROM Ürünler WHERE ÜrünID = @ÜrünID;
                ";

                        SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                        deleteCmd.Parameters.AddWithValue("@ÜrünID", urunID);
                        deleteCmd.ExecuteNonQuery();

                        MessageBox.Show("Zimmet başarıyla düşürüldü!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            Logger.Kaydet("Zimmet düşürüldü", adSoyad);
        }


    }
}




