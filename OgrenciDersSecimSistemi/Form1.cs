using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OgrenciDersSecimSistemi.DataAccess;
namespace OgrenciDersSecimSistemi
{
    public partial class Form1 : Form
    {
        private void OgrenciListele()
        {
            try
            {
                // Veritabanı bağlantısı
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                // SQL sorgusu
                string query = "SELECT * FROM Ogrenciler"; // Tablonuzun adı "Ogrenciler" ise

                // DataAdapter ve DataTable ile verileri çekiyoruz
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // DataGridView'e verileri bağlama
                dgvOgrenciler.DataSource = dt;

                conn.Close(); // Bağlantıyı kapatıyoruz
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void dgvOgrenciler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtOgrenciAdSoyad.Text = dgvOgrenciler.CurrentRow.Cells["OgrenciAd"].Value.ToString();
                txtOgrenciNumara.Text = dgvOgrenciler.CurrentRow.Cells["OgrenciSoyad"].Value.ToString();
                txtGANO.Text = dgvOgrenciler.CurrentRow.Cells["GANO"].Value.ToString();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'ogrenci_sistemiDataSet2.OgrenciDersler' table. You can move, or remove it, as needed.
            this.ogrenciDerslerTableAdapter.Fill(this.ogrenci_sistemiDataSet2.OgrenciDersler);
            // TODO: This line of code loads data into the 'ogrenci_sistemiDataSet1.Dersler' table. You can move, or remove it, as needed.
            this.derslerTableAdapter.Fill(this.ogrenci_sistemiDataSet1.Dersler);
            OgrenciListele(); // Form yüklendiğinde öğrenci listesini getir
            OgrencileriGetir(); // Öğrenci combobox'ını doldur
               DersleriGetir();    // Ders combobox'ını doldur
            
        }

        private void btnOgrenciEkle_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısı
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                // SQL sorgusu
                string query = "INSERT INTO Ogrenciler (AdSoyad, Numara, Gano) VALUES (@adSoyad, @numara, @gano)";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Parametreleri ekle
                cmd.Parameters.AddWithValue("@adSoyad", txtOgrenciAdSoyad.Text); // TextBox Adı
                cmd.Parameters.AddWithValue("@numara", txtOgrenciNumara.Text);   // TextBox Numara
                cmd.Parameters.AddWithValue("@gano", Convert.ToDouble(txtGANO.Text)); // TextBox GANO

                // Sorguyu çalıştır
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Öğrenci başarıyla eklendi.");
                    OgrenciListele(); // Listeyi güncelle
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnOgrenciGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                // Öğrenci ID'sini manuel alıyoruz
                if (string.IsNullOrWhiteSpace(txtOgrenciId.Text))
                {
                    MessageBox.Show("Lütfen geçerli bir Öğrenci ID giriniz.");
                    return;
                }

                int ogrenciId = Convert.ToInt32(txtOgrenciId.Text);

                // Veritabanı bağlantısı
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                // SQL sorgusu
                string query = "UPDATE Ogrenciler SET AdSoyad = @adSoyad, Numara = @numara, Gano = @gano WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Parametreleri ekle
                cmd.Parameters.AddWithValue("@adSoyad", txtOgrenciAdSoyad.Text);
                cmd.Parameters.AddWithValue("@numara", txtOgrenciNumara.Text);
                cmd.Parameters.AddWithValue("@gano", Convert.ToDouble(txtGANO.Text));
                cmd.Parameters.AddWithValue("@id", ogrenciId);

                // Sorguyu çalıştır
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Öğrenci bilgileri güncellendi.");
                    OgrenciListele(); // Listeyi güncelle
                }
                else
                {
                    MessageBox.Show("Öğrenci bulunamadı. Lütfen ID'yi kontrol edin.");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnOgrenciSil_Click(object sender, EventArgs e)
        {
            try
            {
                // Öğrenci ID'sini manuel alıyoruz
                if (string.IsNullOrWhiteSpace(txtOgrenciId.Text))
                {
                    MessageBox.Show("Lütfen geçerli bir Öğrenci ID giriniz.");
                    return;
                }

                int ogrenciId = Convert.ToInt32(txtOgrenciId.Text);

                // Veritabanı bağlantısı
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                // SQL sorgusu
                string query = "DELETE FROM Ogrenciler WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Parametreyi ekle
                cmd.Parameters.AddWithValue("@id", ogrenciId);

                // Sorguyu çalıştır
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Öğrenci silindi.");
                    OgrenciListele(); // Listeyi güncelle
                }
                else
                {
                    MessageBox.Show("Öğrenci bulunamadı. Lütfen ID'yi kontrol edin.");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

        }
        private void DersListele()
        {
            try
            {
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                string query = "SELECT * FROM Dersler";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvDersler.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnDersEkle_Click(object sender, EventArgs e)
        {
            try
            {
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                string query = "INSERT INTO Dersler (DersAdi, DersKodu, Kredi, Kontenjan) VALUES (@dersAdi, @dersKodu, @kredi, @kontenjan)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@dersAdi", txtDersAdi.Text);
                cmd.Parameters.AddWithValue("@dersKodu", txtDersKodu.Text);
                cmd.Parameters.AddWithValue("@kredi", Convert.ToInt32(numKredi.Value));
                cmd.Parameters.AddWithValue("@kontenjan", Convert.ToInt32(numKontenjan.Value));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Ders başarıyla eklendi.");
                DersListele();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnDersGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDersId.Text))
                {
                    MessageBox.Show("Lütfen güncellenecek dersin ID'sini giriniz.");
                    return;
                }

                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                string query = "UPDATE Dersler SET DersAdi = @dersAdi, DersKodu = @dersKodu, Kredi = @kredi, Kontenjan = @kontenjan WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@dersAdi", txtDersAdi.Text);
                cmd.Parameters.AddWithValue("@dersKodu", txtDersKodu.Text);
                cmd.Parameters.AddWithValue("@kredi", Convert.ToInt32(numKredi.Value));
                cmd.Parameters.AddWithValue("@kontenjan", Convert.ToInt32(numKontenjan.Value));
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtDersId.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Ders başarıyla güncellendi.");
                DersListele();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnDersSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDersId.Text))
                {
                    MessageBox.Show("Lütfen silinecek dersin ID'sini giriniz.");
                    return;
                }

                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                string query = "DELETE FROM Dersler WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtDersId.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Ders başarıyla silindi.");
                DersListele();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        private void dgvDersler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtDersId.Text = dgvDersler.CurrentRow.Cells["Id"].Value.ToString();
                txtDersAdi.Text = dgvDersler.CurrentRow.Cells["DersAdi"].Value.ToString();
                txtDersKodu.Text = dgvDersler.CurrentRow.Cells["DersKodu"].Value.ToString();
                numKredi.Value = Convert.ToInt32(dgvDersler.CurrentRow.Cells["Kredi"].Value);
                numKontenjan.Value = Convert.ToInt32(dgvDersler.CurrentRow.Cells["Kontenjan"].Value);
            }
        }
        private void OgrenciDersListele(int ogrenciId)
        {
            try
            {
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();

                string query = @"SELECT od.Id, d.DersAdi, d.DersKodu, d.Kredi, od.OnayDurumu
                         FROM OgrenciDersler od
                         INNER JOIN Dersler d ON od.DersId = d.Id
                         WHERE od.OgrenciId = @ogrenciId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ogrenciId", ogrenciId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvOgrenciDersler.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
       
        private void OgrencileriGetir()
        {
            try
            {
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();
                {
                    SqlCommand cmd = new SqlCommand("SELECT Id, AdSoyad FROM Ogrenciler", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbOgrenci.DisplayMember = "AdSoyad"; // Görünecek alan
                    cmbOgrenci.ValueMember = "Id";        // Seçilecek alan
                    cmbOgrenci.DataSource = dt;           // Combobox'a verileri bağla
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Öğrenciler yüklenirken hata oluştu: " + ex.Message);
            }
        }
        
        private void DersleriGetir()
        {
            try
            {
                baglantilar db = new baglantilar();
                SqlConnection conn = db.baglanti();
                {
                    SqlCommand cmd = new SqlCommand("SELECT Id, DersAdi FROM Dersler WHERE Kontenjan > 0", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbDers.DisplayMember = "DersAdi"; // Görünecek alan
                    cmbDers.ValueMember = "Id";       // Seçilecek alan
                    cmbDers.DataSource = dt;          // Combobox'a verileri bağla
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dersler yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void btnOgrenciDersEkle_Click(object sender, EventArgs e)
        {
            int ogrenciId = Convert.ToInt32(cmbOgrenci.SelectedValue);
            int dersId = Convert.ToInt32(cmbDers.SelectedValue);

            try
            {
                using (SqlConnection con = new SqlConnection("Server=.;Database=ogrenci_sistemi;Trusted_Connection=True;"))
                {
                    con.Open();

                    // Öğrencinin mevcut GANO'sunu al
                    string ganoQuery = "SELECT Gano FROM Ogrenciler WHERE Id = @OgrenciId";
                    double gano;
                    using (SqlCommand ganoCmd = new SqlCommand(ganoQuery, con))
                    {
                        ganoCmd.Parameters.AddWithValue("@OgrenciId", ogrenciId);
                        gano = Convert.ToDouble(ganoCmd.ExecuteScalar());
                    }

                    // GANO'ya göre kredi limiti belirle
                    int krediLimiti = gano < 3 ? 21 : 30;

                    // Öğrencinin mevcut toplam kredilerini al
                    string toplamKrediQuery = @"
                SELECT ISNULL(SUM(d.Kredi), 0) 
                FROM OgrenciDersler od 
                INNER JOIN Dersler d ON od.DersId = d.Id 
                WHERE od.OgrenciId = @OgrenciId";
                    int mevcutToplamKredi;
                    using (SqlCommand toplamKrediCmd = new SqlCommand(toplamKrediQuery, con))
                    {
                        toplamKrediCmd.Parameters.AddWithValue("@OgrenciId", ogrenciId);
                        mevcutToplamKredi = Convert.ToInt32(toplamKrediCmd.ExecuteScalar());
                    }

                    // Eklenmek istenen dersin kredisi alınır
                    string dersKrediQuery = "SELECT Kredi FROM Dersler WHERE Id = @DersId";
                    int dersKredisi;
                    using (SqlCommand dersKrediCmd = new SqlCommand(dersKrediQuery, con))
                    {
                        dersKrediCmd.Parameters.AddWithValue("@DersId", dersId);
                        dersKredisi = Convert.ToInt32(dersKrediCmd.ExecuteScalar());
                    }

                    // Toplam kredinin kredi limitini aşması kontrol edilir
                    if (mevcutToplamKredi + dersKredisi > krediLimiti)
                    {
                        MessageBox.Show($"Ders eklenemiyor. Toplam kredi limiti ({krediLimiti}) aşıldı.");
                        return;
                    }

                    // Kontenjan kontrolü yapılır
                    string kontenjanQuery = @"
                SELECT 
                    (SELECT COUNT(*) FROM OgrenciDersler WHERE DersId = @DersId) AS KayitliOgrenciSayisi,
                    Kontenjan 
                FROM Dersler 
                WHERE Id = @DersId";
                    int kayitliOgrenciSayisi, kontenjan;
                    using (SqlCommand kontenjanCmd = new SqlCommand(kontenjanQuery, con))
                    {
                        kontenjanCmd.Parameters.AddWithValue("@DersId", dersId);
                        using (SqlDataReader reader = kontenjanCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                kayitliOgrenciSayisi = reader.GetInt32(0);
                                kontenjan = reader.GetInt32(1);
                            }
                            else
                            {
                                MessageBox.Show("Ders bilgileri alınamadı.");
                                return;
                            }
                        }
                    }

                    if (kayitliOgrenciSayisi >= kontenjan)
                    {
                        MessageBox.Show("Ders kontenjanı dolmuş.");
                        return;
                    }

                    // Aynı öğrenci-ders kaydının eklenmesini engelle
                    string checkQuery = "SELECT COUNT(*) FROM OgrenciDersler WHERE OgrenciId = @OgrenciId AND DersId = @DersId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@OgrenciId", ogrenciId);
                        checkCmd.Parameters.AddWithValue("@DersId", dersId);
                        int exists = (int)checkCmd.ExecuteScalar();

                        if (exists > 0)
                        {
                            MessageBox.Show("Bu öğrenci zaten bu dersi seçmiş!");
                            return;
                        }
                    }

                    // Kayıt ekleme işlemi
                    string query = "INSERT INTO OgrenciDersler (OgrenciId, DersId) VALUES (@OgrenciId, @DersId)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OgrenciId", ogrenciId);
                        cmd.Parameters.AddWithValue("@DersId", dersId);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ders başarıyla eklendi!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }










        private void btnOgrenciDersSil_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                if (int.TryParse(txtDersId1.Text, out id))
                {
                    using (SqlConnection con = new SqlConnection("Server=.;Database=ogrenci_sistemi;Trusted_Connection=True;"))
                    {
                        con.Open();

                        string query = "DELETE FROM OgrenciDersler WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Kayıt başarıyla silindi!");
                                LoadOgrenciDerslerTablosu(); // Tabloları güncelle
                            }
                            else
                            {
                                MessageBox.Show("Silinecek kayıt bulunamadı!");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Geçerli bir ID giriniz!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        
    }

       
            private void btnDanismanOnayla_Click(object sender, EventArgs e)
            {
            try
            {
                int id;
                if (int.TryParse(txtDersId1.Text, out id))
                {
                    using (SqlConnection con = new SqlConnection("Server=.;Database=ogrenci_sistemi;Trusted_Connection=True;"))
                    {
                        con.Open();

                        string query = "UPDATE OgrenciDersler SET OnayDurumu = 1 WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Onay işlemi başarıyla tamamlandı!");
                                LoadOgrenciDerslerTablosu(); // Tabloları güncelle
                            }
                            else
                            {
                                MessageBox.Show("Onaylanacak kayıt bulunamadı!");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Geçerli bir ID giriniz!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        private void LoadOgrenciDerslerTablosu()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Server=.;Database=ogrenci_sistemi;Trusted_Connection=True;"))
                {
                    con.Open();

                    string query = "SELECT * FROM OgrenciDersler";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvOgrenciDersler.DataSource = dt; // ÖğrenciDersler için DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnTabloGuncelle_Click(object sender, EventArgs e)
        {
            {
                
                LoadOgrenciDerslerTablosu();
                MessageBox.Show("Tablolar güncellendi!");
            }
        }
    }
}




