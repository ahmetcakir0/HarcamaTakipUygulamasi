using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;

namespace HarcamaTakipUygulaması
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=MBB-01-BIL065-N\\SQLEXPRESS;Initial Catalog=HDB;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    conn.Open(); admin, 1234
        //    MessageBox.Show("Bağlantı başarılı bir şekilde oluşturuldu!");
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    conn.Close();
        //    MessageBox.Show("Bağlantı kesildi!");
        //}

        private void Form1_Load(object sender, EventArgs e)
        {

            combobox_doldur();
            griddoldur();


            
        }

        private void combobox_doldur()
        {
            comboBox1.Items.Clear();

            string query = "SELECT [ID], [KategoriAdi] FROM [HDB].[dbo].[Kategoriler]";

            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "KategoriAdi"; 
            comboBox1.ValueMember = "ID";            
            comboBox1.Text = "Seçiniz...";          
            conn.Close();

        }

        private void griddoldur()
        {
            conn.Open();



            string query = "SELECT H.[ID]\r\n      ,H.[Tarih]\r\n      ,H.[Miktar]\r\n      ,H.[Aciklama]\r\n\t  ,K.KategoriAdi\r\n  FROM [dbo].[Harcamalar] as H\r\n  INNER JOIN Kategoriler as K ON K.ID = H.Kategori\r\n";

            SqlDataAdapter veri = new SqlDataAdapter(query, conn);
            DataTable table = new DataTable();
            veri.Fill(table);
            dataGridView1.DataSource = table;


            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand veriKaydet = new SqlCommand("INSERT INTO Harcamalar (Tarih, Miktar, Aciklama, Kategori) VALUES (@tarih, @miktar, @aciklama, @kategori)", conn);
            veriKaydet.Parameters.AddWithValue("@tarih", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            veriKaydet.Parameters.AddWithValue("@miktar", textBox1.Text);
            veriKaydet.Parameters.AddWithValue("@aciklama", textBox2.Text);
            veriKaydet.Parameters.AddWithValue("@kategori", comboBox1.SelectedValue);
            veriKaydet.ExecuteNonQuery();
            conn.Close();
            griddoldur();
            //conn.Open();
            //SqlCommand veriKaydet = new SqlCommand("INSERT INTO Harcamalar (Tarih, Miktar, Aciklama, Kategori) VALUES (@tarih, @miktar, @aciklama, @kategori)", conn);
            //veriKaydet.Parameters.AddWithValue("@tarih", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            //veriKaydet.Parameters.AddWithValue("@miktar", textBox1.Text);
            //veriKaydet.Parameters.AddWithValue("@aciklama", textBox2.Text);
            //veriKaydet.Parameters.AddWithValue("@kategori", comboBox1.SelectedValue);
            //veriKaydet.ExecuteNonQuery();
            //conn.Close();
        }
        private void btn_Sil_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                    string deleteQuery = "DELETE FROM Harcamalar WHERE ID = @ID";

                    conn.Open();

                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, conn);
                    deleteCommand.Parameters.AddWithValue("@ID", selectedID);

                    deleteCommand.ExecuteNonQuery();

                    

                    MessageBox.Show("Kayıt başarıyla silindi!");
                }
                else
                {
                    MessageBox.Show("Lütfen silmek için bir satır seçin.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme hatası: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            griddoldur();
        }
    }
}
