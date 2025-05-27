using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ZWebApp
{
    internal class Logger
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;
        public static void Kaydet(string islem, string hedef)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 🔹 Önce varsa fazla kayıtları sil
                    string kontrolQuery = "SELECT COUNT(*) FROM IslemLog";
                    SqlCommand kontrolCmd = new SqlCommand(kontrolQuery, conn);
                    int kayitSayisi = (int)kontrolCmd.ExecuteScalar();

                    if (kayitSayisi >= 5)
                    {
                        // En eski kayıtları sil (en düşük Id'ye göre)
                        string silQuery = @"
                    DELETE FROM IslemLog 
                    WHERE Id IN (
                        SELECT Id FROM IslemLog 
                        ORDER BY Tarih ASC
                        OFFSET 0 ROWS FETCH NEXT @adet ROWS ONLY
                    )";
                        SqlCommand silCmd = new SqlCommand(silQuery, conn);
                        silCmd.Parameters.AddWithValue("@adet", kayitSayisi - 4); // Yalnızca son 5 kalsın
                        silCmd.ExecuteNonQuery();
                    }

                    // 🔹 Yeni log kaydı ekle
                    string ekleQuery = "INSERT INTO IslemLog (KullaniciAdi, Islem, Hedef) VALUES (@kullanici, @islem, @hedef)";
                    SqlCommand ekleCmd = new SqlCommand(ekleQuery, conn);
                    ekleCmd.Parameters.AddWithValue("@kullanici", Session.KullaniciAdi);
                    ekleCmd.Parameters.AddWithValue("@islem", islem);
                    ekleCmd.Parameters.AddWithValue("@hedef", hedef);
                    ekleCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Log kaydı başarısız: " + ex.Message);
            }
        }
    }
}
