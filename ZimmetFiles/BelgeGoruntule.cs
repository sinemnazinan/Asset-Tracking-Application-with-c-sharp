using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace ZWebApp
{
    public partial class Form8 : Form
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;
        private string ad = "";
        private string soyad = "";

        public Form8(string girilenAdSoyad)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            string[] isimParcalari = girilenAdSoyad.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (isimParcalari.Length < 2)
            {
                MessageBox.Show("Ad ve soyad bilgisi eksik veya hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string soyad = isimParcalari[^1];
            string ad = string.Join(" ", isimParcalari[..^1]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT Id, BelgeAdı, BelgeTuru,
                        CASE WHEN Imzali = 1 THEN 'Evet' ELSE 'Hayır' END AS Imzali,
                        OlusturmaTarihi 
                         FROM Belgeler 
                         WHERE KullanıcıAd = @ad AND KullanıcıSoyad = @soyad";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@ad", ad);
                da.SelectCommand.Parameters.AddWithValue("@soyad", soyad);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvBelgeler.DataSource = dt;
                dgvBelgeler.Columns["Id"].Visible = false;
            }
        }
        private void btnAc_Click(object sender, EventArgs e)
        {
            if (dgvBelgeler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir belge seçin.");
                return;
            }

            int belgeId = Convert.ToInt32(dgvBelgeler.SelectedRows[0].Cells["Id"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT BelgeAdı, BelgeIcerik FROM Belgeler WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", belgeId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string belgeAdi = reader.GetString(0);
                            byte[] data = (byte[])reader["BelgeIcerik"];
                            string path = Path.Combine(Path.GetTempPath(), belgeAdi);
                            File.WriteAllBytes(path, data);
                            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
                        }
                    }
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvBelgeler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir belge seçin.");
                return;
            }

            string girilenSifre = Microsoft.VisualBasic.Interaction.InputBox(
                "Bu belgeyi silmek için admin şifresini giriniz:",
                "Şifre Girişi", "");

            var aktifAdmin = Session.KullaniciAdi; // Admin objesi

            bool sifreDogruMu = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Adminler WHERE KullaniciAdi = @kadi AND Sifre = @sifre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kadi", aktifAdmin);
                cmd.Parameters.AddWithValue("@sifre", girilenSifre);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sifreDogruMu = true;
                    }
                }
            }

            if (sifreDogruMu)
            {
                int belgeId = Convert.ToInt32(dgvBelgeler.SelectedRows[0].Cells["Id"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Belgeler WHERE Id = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", belgeId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Belge başarıyla silindi.");
                BelgeleriYenidenYukle();
            }
            else
            {
                MessageBox.Show("Şifre hatalı! İşlem iptal edildi.");
            }
        }
        private void BelgeleriYenidenYukle()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT Id, BelgeAdı, BelgeTuru,
                        CASE WHEN Imzali = 1 THEN 'Evet' ELSE 'Hayır' END AS Imzali,
                        OlusturmaTarihi 
                         FROM Belgeler 
                         WHERE KullanıcıAd = @ad AND KullanıcıSoyad = @soyad";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@ad", ad);
                da.SelectCommand.Parameters.AddWithValue("@soyad", soyad);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvBelgeler.DataSource = dt;
                dgvBelgeler.Columns["Id"].Visible = false;
            }
        }


    }
}
