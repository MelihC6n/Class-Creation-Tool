using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        string kullaniciadi = "";
        string sifre = "";
        string baglan = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                local.Checked = true;
                TextBox3.Enabled = false;
                TextBox4.Enabled = false;
                dbAd.Enabled = false;
                tabloAd.Enabled = false;
                txtBaglanti.Enabled = false;
                Label7.Text = "";
                Button1.Enabled = false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
                if (uzak.Checked == true)
                {
                    kullaniciadi = TextBox3.Text;
                    sifre = TextBox4.Text;
                    baglan = txtBaglanti.Text;
                }
                else
                {
                    kullaniciadi = "";
                    sifre = "";
                    baglan = "";
                }
                string veritabaniAdi = dbAd.Text;
                string tabloAdi = tabloAd.Text;
                string projeAdi = "WebApplication1";
                string webFormAd = tabloAd.Text + "WebForm";

                Directory.CreateDirectory(Server.MapPath("\\Oluşturulan-dosyalar"));
                Uret uret = new Uret();
                MemoryStream al = uret.ClassDondur(tabloAdi, projeAdi, veritabaniAdi, kullaniciadi, sifre, baglan);
                using (FileStream file = new FileStream(HttpContext.Current.Request.PhysicalApplicationPath + "\\Oluşturulan-dosyalar\\" + tabloAdi + ".cs", FileMode.OpenOrCreate))
                {
                    al.Position = 0;
                    al.CopyTo(file);
                    file.Flush();
                }

                MemoryStream al2 = uret.DbProcessDondur(tabloAdi, projeAdi, veritabaniAdi, kullaniciadi, sifre, baglan);
                using (FileStream file2 = new FileStream(HttpContext.Current.Request.PhysicalApplicationPath + "\\Oluşturulan-dosyalar\\" + tabloAdi + "DbProcess.cs", FileMode.OpenOrCreate))
                {
                    al2.Position = 0;
                    al2.CopyTo(file2);
                    file2.Flush();
                }

                if (web.Checked == true)
                {
                    MemoryStream al3 = uret.WebFormDondur(webFormAd, projeAdi, tabloAdi, veritabaniAdi, kullaniciadi, sifre, baglan);
                    using (FileStream file3 = new FileStream(HttpContext.Current.Request.PhysicalApplicationPath + "\\Oluşturulan-dosyalar\\" + webFormAd + ".aspx", FileMode.OpenOrCreate))
                    {
                        al3.Position = 0;
                        al3.CopyTo(file3);
                        file3.Flush();

                    }

                    MemoryStream al4 = uret.AspxCsDondur(tabloAdi, projeAdi, veritabaniAdi, webFormAd, kullaniciadi, sifre, baglan);
                    using (FileStream file4 = new FileStream(HttpContext.Current.Request.PhysicalApplicationPath + "\\Oluşturulan-dosyalar\\" + webFormAd + ".aspx.cs", FileMode.OpenOrCreate))
                    {
                        al4.Position = 0;
                        al4.CopyTo(file4);
                        file4.Flush();
                    }

                    MemoryStream al5 = uret.AspxDesignerCsDondur(tabloAdi, projeAdi, webFormAd, kullaniciadi, sifre, baglan);
                    using (FileStream file5 = new FileStream(HttpContext.Current.Request.PhysicalApplicationPath + "\\Oluşturulan-dosyalar\\" + webFormAd + ".aspx.designer.cs", FileMode.OpenOrCreate))
                    {
                        al5.Position = 0;
                        al5.CopyTo(file5);
                        file5.Flush();
                    }
                }
                Label7.Text = tabloAdi + " dosyaları başarıyla oluşturuldu.";
        }

        protected void uzak_CheckedChanged(object sender, EventArgs e)
        {
            if (uzak.Checked == true)
            {
                TextBox3.Enabled = true;
                TextBox4.Enabled = true;
                txtBaglanti.Enabled = true;
                Button1.Enabled = false;
                dbAd.Items.Clear();
                tabloAd.Items.Clear();
                Label5.Text = "";
            }
        }

        protected void local_CheckedChanged(object sender, EventArgs e)
        {
            if (local.Checked == true)
            {
                TextBox3.Enabled = false;
                TextBox4.Enabled = false;
                txtBaglanti.Enabled = false;
                Button1.Enabled = false;
                dbAd.Items.Clear();
                tabloAd.Items.Clear();

                Label5.Text = "";
                TextBox3.Text = "";
                TextBox4.Text = "";
                txtBaglanti.Text = "";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                if (uzak.Checked == true)
                {
                    kullaniciadi = TextBox3.Text;
                    sifre = TextBox4.Text;
                    baglan = txtBaglanti.Text;
                    con.ConnectionString = "Server = " + baglan + "; Database = " + dbAd.Text + "; User Id = " + kullaniciadi + "; password = " + sifre + "";
                }
                else
                {
                    kullaniciadi = "";
                    sifre = "";
                    con.ConnectionString = "Server = localhost; Database=" + dbAd.Text + ";Trusted_Connection=True;";
                }
                SqlCommand komut = new SqlCommand("select name from sys.databases", con);
                con.Open();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    dbAd.Items.Add(dr[0].ToString());
                }


                con.Close();
                dbAd.Enabled = true;
                tabloAd.Enabled = true;
                Label5.Text = "Bağlanma başarılı veritabanı adı ve ardından tablo adı seçerek işlemi tamamlayın.";
                Button1.Enabled = true;
                

            }
            catch (Exception)
            {

                Label5.Text = "Bağlanma başarısız kullanıcı adı ve şifrenizin doğruluğunu kontrol ediniz.";
                dbAd.Items.Clear();
                dbAd.Enabled = false;
                tabloAd.Items.Clear();
                tabloAd.Enabled = false;
                Button1.Enabled = false;
            }
            Label7.Text = "";
        }

        protected void dbAd_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabloAd.Items.Clear();
            SqlConnection con = new SqlConnection();
            if (uzak.Checked == true)
            {
                kullaniciadi = TextBox3.Text;
                sifre = TextBox4.Text;
                baglan = txtBaglanti.Text;
                con.ConnectionString = "Server = " + baglan + "; Database = " + dbAd.Text + "; User Id = " + kullaniciadi + "; password = " + sifre + "";
            }
            else
            {
                kullaniciadi = "";
                sifre = "";
                con.ConnectionString = "Server = localhost; Database=" + dbAd.Text + ";Trusted_Connection=True;";
            }
            SqlCommand komut = new SqlCommand("use " + dbAd.Text + " select name from sys.tables", con);
            con.Open();
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                tabloAd.Items.Add(dr[0].ToString());
            }
            con.Close();

        }
    }
}
