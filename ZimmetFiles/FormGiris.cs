using Microsoft.Data.SqlClient;
namespace ZWebApp
{
    public partial class FormGiris : Form
    {


        public FormGiris()
        {

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AcceptButton = btnGiris;
            float scaleFactor = 1.30f; // %15 büyütme
            ScaleUI(scaleFactor);

        }



        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.");
                return;
            }

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZimmetDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Adminler WHERE KullaniciAdi = @kadi AND Sifre = @sifre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kadi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@sifre", sifre);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Session.KullaniciAdi = kullaniciAdi;
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Hatalı kullanıcı adı veya şifre!");
                }
            }
        }
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
    }
}
