using System;
using System.Data.SqlClient;

namespace OgrenciDersSecimSistemi.DataAccess
{
    public class baglantilar
    {
        public SqlConnection baglanti()
        {
            // Veritabanı bağlantı dizesi
            SqlConnection conn = new SqlConnection("Data Source=CAVA;Initial Catalog=ogrenci_sistemi;Integrated Security=True;TrustServerCertificate=True;");
            try
            {
                conn.Open(); // Veritabanına bağlantıyı açıyoruz
            }
            catch (Exception ex)
            {
                // Eğer bağlantı hatası olursa, hata mesajı gösteriyoruz
                Console.WriteLine("Bağlantı hatası: " + ex.Message);
            }
            return conn;
        }
    }
}
