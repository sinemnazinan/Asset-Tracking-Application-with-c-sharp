using Microsoft.Data.SqlClient;
using System.Data;

namespace ZWebApp
{
    public partial class FormAyarlar : Form
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;

        public FormAyarlar()
        {

            InitializeComponent();

            dgvAdminler.ReadOnly = true; // Değiştirilemesin
            dgvAdminler.AllowUserToAddRows = false; // Alttaki boş satırı gizle
            dgvAdminler.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Sadece tüm satır seçilsin
            dgvAdminler.MultiSelect = false; // Tek satır seçilebilsin
            dgvAdminler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // Kolon genişliği otomatik

            dataGridView1.ReadOnly = true; // Değiştirilemesin
            dataGridView1.AllowUserToAddRows = false; // Alttaki boş satırı gizle
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Sadece tüm satır seçilsin
            dataGridView1.MultiSelect = false; // Tek satır seçilebilsin
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // Kolon genişliği otomatik
            this.Controls.Add(this.btnTutanaklariSifirla);
            FormAyarlar_Load(this, EventArgs.Empty); // Form yüklendiğinde adminleri listele

        }

        private void FormAyarlar_Load(object sender, EventArgs e)
        {

            AdminleriListele();
            LoglariYukle();

            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void AdminleriListele()
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT KullaniciAdi FROM Adminler";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAdminler.DataSource = dt;
                if (dt.Rows.Count == 0)
                {
                    dt.Columns.Add("KullaniciAdi");
                }

            }
            // Satır sayısına göre DataGridView yüksekliğini ayarla
            int satirSayisi = dgvAdminler.Rows.Count;
            int satirYuksekligi = dgvAdminler.RowTemplate.Height;
            int baslikYuksekligi = dgvAdminler.ColumnHeadersHeight;

            int toplamYukseklik = (satirSayisi * satirYuksekligi) + baslikYuksekligi + 10;

            int maxYukseklik = 200; // en fazla bu kadar uzayabilir
            dgvAdminler.Height = Math.Min(toplamYukseklik, maxYukseklik);
            dgvAdminler.ScrollBars = ScrollBars.Vertical;
            dgvAdminler.AllowUserToResizeRows = false;
            dgvAdminler.AllowUserToResizeColumns = false;



        }
        private void btnEkle_Click_1(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string kontrolQuery = "SELECT COUNT(*) FROM Adminler WHERE KullaniciAdi = @kadi";
                SqlCommand kontrolCmd = new SqlCommand(kontrolQuery, conn);
                kontrolCmd.Parameters.AddWithValue("@kadi", kullaniciAdi);
                int sayi = (int)kontrolCmd.ExecuteScalar();

                if (sayi > 0)
                {
                    MessageBox.Show("Bu kullanıcı adı zaten var.");
                    return;
                }

                string ekleQuery = "INSERT INTO Adminler (KullaniciAdi, Sifre) VALUES (@kadi, @sifre)";
                SqlCommand ekleCmd = new SqlCommand(ekleQuery, conn);
                ekleCmd.Parameters.AddWithValue("@kadi", kullaniciAdi);
                ekleCmd.Parameters.AddWithValue("@sifre", sifre);
                ekleCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Yeni admin eklendi!");
            AdminleriListele();
            txtKullaniciAdi.Clear();
            txtSifre.Clear();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvAdminler.CurrentRow == null || dgvAdminler.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Lütfen silmek için geçerli bir satır seçiniz.");
                return;
            }

            string seciliKullanici = dgvAdminler.CurrentRow.Cells["KullaniciAdi"].Value.ToString();

            DialogResult result = MessageBox.Show($"'{seciliKullanici}' adlı admini silmek istiyor musunuz?",
                "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string silQuery = "DELETE FROM Adminler WHERE KullaniciAdi = @kadi";
                SqlCommand silCmd = new SqlCommand(silQuery, conn);
                silCmd.Parameters.AddWithValue("@kadi", seciliKullanici);
                silCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Admin başarıyla silindi!");
            AdminleriListele();
        }
        private void LoglariYukle()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM IslemLog ORDER BY Tarih DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }

            // Dinamik boyutlandırma
            int satirSayisi = dataGridView1.Rows.Count;
            int satirYuksekligi = dataGridView1.RowTemplate.Height;
            int baslikYuksekligi = dataGridView1.ColumnHeadersHeight;

            int toplamYukseklik = (satirSayisi * satirYuksekligi) + baslikYuksekligi + 20;
            int maxYukseklik = 200;
            dataGridView1.Height = Math.Min(toplamYukseklik, maxYukseklik);
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.ScrollBars = ScrollBars.Horizontal;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
        }

        private void btnTutanaklariSifirla_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Tüm tutanak numarası kayıtlarını silmek istediğinize emin misiniz?\nBu işlem geri alınamaz!\n(!BU İŞLEMİN SENE BAŞINDA YANLIZCA 1 KEZ YAPILMASI ÖNERİLİR!)",
                "Tutanakları Sıfırla",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM TutanakNumaralari";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Tüm tutanak numaraları başarıyla silindi.");
            }


        }


    }
}

