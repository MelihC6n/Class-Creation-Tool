using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;

namespace WebApplication1
{
    public class Uret
    {

        public MemoryStream ClassDondur(string tabloAdi, string projeAdi, string veritabaniAdi, string kullaniciadi, string sifre, string baglantiAdres)
        {
            string classAdi = tabloAdi;
            SqlConnection con = new SqlConnection();
            if (baglantiAdres != "")
            {
                con.ConnectionString = "Server = " + baglantiAdres + "; Database = " + veritabaniAdi + "; User Id = " + kullaniciadi + "; password = " + sifre + "";
            }
            else
            {
                con.ConnectionString = "Server = localhost; Database=" + veritabaniAdi + ";Trusted_Connection=True;";
            }
            MemoryStream memoryStream = new MemoryStream();
            TextWriter classFile = new StreamWriter(memoryStream);
            classFile.WriteLine("using System;" + Environment.NewLine + "using System.Collections.Generic;" + Environment.NewLine + "using System.Linq;" + Environment.NewLine + "using System.Web;");
            classFile.WriteLine("");
            classFile.WriteLine("namespace " + BuyukBasHarf(projeAdi) + Environment.NewLine + "{" + Environment.NewLine + "public class " + BuyukBasHarf(classAdi) + Environment.NewLine + "{");
            SqlCommand cmd = new SqlCommand("select DATA_TYPE, COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where table_name ='" + tabloAdi + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string kolonTuru = dr[0].ToString();
                string kolonAdi = dr[1].ToString();
                if (kolonTuru == "int")
                { kolonTuru = "int"; }
                else if (kolonTuru == "tinyint")
                { kolonTuru = "int"; }
                else if (kolonTuru == "bigint")
                { kolonTuru = "UInt64"; }
                else if (kolonTuru == "varchar")
                { kolonTuru = "string"; }
                else if (kolonTuru == "nchar")
                { kolonTuru = "string"; }
                else if (kolonTuru == "money")
                { kolonTuru = "double"; }
                else if (kolonTuru == "date")
                { kolonTuru = "DateTime"; }
                else if (kolonTuru == "bit")
                { kolonTuru = "Boolean"; }
                else
                { kolonTuru = "string"; }
                classFile.WriteLine("public " + kolonTuru + " " + KucukBasHarf(kolonAdi) + " { get; set; }");
            }
            con.Close();
            classFile.WriteLine("}" + Environment.NewLine + "}");
            classFile.Flush();

            return memoryStream;
        }

        public MemoryStream DbProcessDondur(string tabloAdi, string projeAdi, string veritabaniAdi, string kullaniciadi, string sifre, string baglantiAdres)
        {
            #region insert
            string classAdi = "DbProcess";
            SqlConnection con = new SqlConnection();
            if (baglantiAdres != "")
            {
                con.ConnectionString = "Server = " + baglantiAdres + "; Database = " + veritabaniAdi + "; User Id = " + kullaniciadi + "; password = " + sifre + "";
            }
            else
            {
                con.ConnectionString = "Server = localhost; Database=" + veritabaniAdi + ";Trusted_Connection=True;";
            }
            MemoryStream memoryStream = new MemoryStream();
            TextWriter classFile = new StreamWriter(memoryStream);
            classFile.WriteLine("using System;" + Environment.NewLine + "using System.Collections.Generic;" + Environment.NewLine + "using System.Linq;" + Environment.NewLine + "using System.Web;" + Environment.NewLine + "using System.Data.SqlClient;");
            classFile.WriteLine("");
            classFile.WriteLine("namespace " + projeAdi + Environment.NewLine + "{" + Environment.NewLine + "public class " + classAdi + Environment.NewLine + "{");
            classFile.WriteLine("");
            classFile.WriteLine("SqlConnection con = new SqlConnection(\""+con.ConnectionString+"\");");
            classFile.WriteLine("public void " + BuyukBasHarf(tabloAdi) + "Insert" + "(" + BuyukBasHarf(tabloAdi) + " " + KucukBasHarf(tabloAdi) + "Degisken)" + Environment.NewLine + "{");
            SqlCommand cmd = new SqlCommand("select column_name as 'kolon isimleri' from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + tabloAdi + "'", con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string insertValue = "";
            string parameters = "";
            while (dr.Read())
            {
                if (dr[0].ToString() != "id" && dr[0].ToString() != "ID")
                {
                    insertValue = insertValue + dr[0].ToString() + ",";
                    parameters = parameters + "@" + dr[0].ToString() + ",";
                }

            }
            dr.Close();
            insertValue = insertValue.Substring(0, insertValue.Length - 1);
            parameters = parameters.Substring(0, parameters.Length - 1);

            classFile.WriteLine("SqlCommand insert = new SqlCommand(\"insert into " + tabloAdi + "(" + insertValue + ") values(" + parameters + ")\",con);");


            SqlDataReader dr2 = cmd.ExecuteReader();
            while (dr2.Read())
            {
                if (dr2[0].ToString() != "id" && dr2[0].ToString() != "ID")
                {
                    classFile.WriteLine("insert.Parameters.AddWithValue(\"@" + dr2[0].ToString() + "\"," + KucukBasHarf(tabloAdi) + "Degisken." + KucukBasHarf(dr2[0].ToString()) + ");");
                }
            }
            dr2.Close();
            classFile.WriteLine("KomutCalistir(insert);" + Environment.NewLine + "}" + Environment.NewLine);

            #endregion

            #region update                                

            classFile.WriteLine("public void " + BuyukBasHarf(tabloAdi) + "Update" + "(" + BuyukBasHarf(tabloAdi) + " " + KucukBasHarf(tabloAdi) + "Degisken)" + Environment.NewLine + "{");

            SqlDataReader dr3 = cmd.ExecuteReader();
            string updateValue = "";
            while (dr3.Read())
            {
                if (dr3[0].ToString() != "id" && dr3[0].ToString() != "ID")
                {
                    updateValue = updateValue + dr3[0].ToString() + "=@" + dr3[0].ToString() + ",";
                }

            }
            dr3.Close();
            updateValue = updateValue.Substring(0, updateValue.Length - 1);
            classFile.WriteLine("SqlCommand update = new SqlCommand(\"update " + tabloAdi + " set " + updateValue + " where ID=@ID\", con);");


            SqlDataReader dr4 = cmd.ExecuteReader();
            while (dr4.Read())
            {
                if (dr4[0].ToString() != "id" && dr4[0].ToString() != "ID")
                {
                    classFile.WriteLine("update.Parameters.AddWithValue(\"@" + dr4[0].ToString() + "\"," + KucukBasHarf(tabloAdi) + "Degisken." + KucukBasHarf(dr4[0].ToString()) + ");");
                }
            }
            dr4.Close();
            classFile.WriteLine("update.Parameters.AddWithValue(\"@ID\"," + KucukBasHarf(tabloAdi) + "Degisken.ID);");
            classFile.WriteLine("KomutCalistir(update);" + Environment.NewLine + "}" + Environment.NewLine);

            #endregion

            #region delete
            classFile.WriteLine("public void " + BuyukBasHarf(tabloAdi) + "Delete" + "(" + BuyukBasHarf(tabloAdi) + " " + KucukBasHarf(tabloAdi) + "Degisken)" + Environment.NewLine + "{");
            classFile.WriteLine("SqlCommand delete= new SqlCommand(\"delete from " + tabloAdi + " where ID=@ID\",con);");
            classFile.WriteLine("delete.Parameters.AddWithValue(\"@ID\"," + KucukBasHarf(tabloAdi) + "Degisken.ID);");
            classFile.WriteLine("KomutCalistir(delete);" + Environment.NewLine + "}" + Environment.NewLine);


            #endregion

            #region select

            classFile.WriteLine("public List<" + BuyukBasHarf(tabloAdi) + "> " + BuyukBasHarf(tabloAdi) + "Select()" + Environment.NewLine + "{");
            classFile.WriteLine("List<" + BuyukBasHarf(tabloAdi) + "> result = new List<" + BuyukBasHarf(tabloAdi) + ">();");
            classFile.WriteLine("con.Open();" + Environment.NewLine + "SqlCommand komut = new SqlCommand(\"select * from " + tabloAdi + "\",con);");
            classFile.WriteLine("SqlDataReader dr = komut.ExecuteReader();");
            classFile.WriteLine("while (dr.Read())" + Environment.NewLine + "{");
            classFile.WriteLine(BuyukBasHarf(tabloAdi) + " " + KucukBasHarf(tabloAdi) + " = new " + BuyukBasHarf(tabloAdi) + "()" + Environment.NewLine + "{");
            SqlCommand cmd2 = new SqlCommand("select DATA_TYPE, COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where table_name ='" + tabloAdi + "'", con);
            SqlDataReader dr5 = cmd2.ExecuteReader();
            int drSayisi = 0;
            string classAttirubutes = "";
            while (dr5.Read())
            {
                string kolonTuru = dr5[0].ToString();
                string kolonAdi = dr5[1].ToString();
                if (kolonTuru == "int")
                { kolonTuru = "Convert.ToInt32(";
                }
                else if (kolonTuru == "tinyint")
                { kolonTuru = "Convert.ToInt32(";
                }
                else if (kolonTuru == "bigint")
                { kolonTuru = "Convert.ToUInt64(";
                }
                else if (kolonTuru == "varchar")
                { kolonTuru = "Convert.ToString(";
                }
                else if (kolonTuru == "nchar")
                { kolonTuru = "Convert.ToString(";
                }
                else if (kolonTuru == "money")
                { kolonTuru = "Convert.ToDouble(";
                }
                else if (kolonTuru == "date")
                { kolonTuru = "Convert.ToDateTime(";
                }
                else if (kolonTuru == "bit")
                { kolonTuru = "Convert.ToBoolean(";
                }
                else
                { kolonTuru = "Convert.ToString(";
                }
                classAttirubutes=classAttirubutes+Environment.NewLine+KucukBasHarf(kolonAdi) + " = " + kolonTuru + "dr[" + drSayisi + "]),";
                drSayisi++;
            }
            classFile.WriteLine(classAttirubutes.Substring(0, classAttirubutes.Length - 1));
            classFile.WriteLine("};"+Environment.NewLine+"result.Add(" + KucukBasHarf(tabloAdi) + ");"+Environment.NewLine+"}");
            classFile.WriteLine("con.Close();" + Environment.NewLine + "return result;" + Environment.NewLine + "}"+Environment.NewLine);

            #endregion

            classFile.WriteLine("public Boolean KomutCalistir(SqlCommand komut)" + Environment.NewLine + "{" + Environment.NewLine + "con.Open();" + Environment.NewLine + "komut.ExecuteNonQuery();" + Environment.NewLine + "komut.Dispose();" + Environment.NewLine + "con.Close();" + Environment.NewLine + "return true;" + Environment.NewLine + "}");
            classFile.WriteLine(Environment.NewLine + "}" + Environment.NewLine + "}");
            classFile.Flush();
            con.Close();
            return memoryStream;
        }

        public MemoryStream WebFormDondur(string webFormad, string projeAd, string tabloAd, string veritabaniAdi, string kullaniciadi, string sifre, string baglantiAdres)
        {
            SqlConnection con = new SqlConnection();
            if (baglantiAdres != "")
            {
                con.ConnectionString = "Server = " + baglantiAdres + "; Database = " + veritabaniAdi + "; User Id = " + kullaniciadi + "; password = " + sifre + "";
            }
            else
            {
                con.ConnectionString = "Server = localhost; Database=" + veritabaniAdi + ";Trusted_Connection=True;";
            }
            MemoryStream memoryStream = new MemoryStream();
            TextWriter webFormFile = new StreamWriter(memoryStream);
            webFormFile.WriteLine("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"" + webFormad + ".aspx.cs\" Inherits=\"" + projeAd + "." + webFormad + "\" %>");
            webFormFile.WriteLine("<!DOCTYPE html>");
            webFormFile.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            webFormFile.WriteLine("<head runat=\"server\">");
            webFormFile.WriteLine("<meta http-equiv=\"Content-Type\"content=\"text/html; charset = utf-8\"/>");
            webFormFile.WriteLine("<title></title>" + Environment.NewLine + "</head>");
            webFormFile.WriteLine("<body>");
            webFormFile.WriteLine("<form id=\"form1\" runat=\"server\">");
            webFormFile.WriteLine("<asp:Repeater ID=\"rpt"+ tabloAd + "\" runat=\"server\">");
            webFormFile.WriteLine("<HeaderTemplate>");
            webFormFile.WriteLine("<table border=\"1\">");
            webFormFile.WriteLine("<tr style=\"background-color:darkgreen;color:black\">");

            SqlCommand cmd = new SqlCommand("select column_name as 'kolon isimleri' from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + tabloAd + "'", con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                if (dr[0].ToString() != "id" || dr[0].ToString() != "ID")
                {
                    webFormFile.WriteLine("<td>" + dr[0].ToString() + "</td>");

                }

            }
            dr.Close();
            webFormFile.WriteLine("</tr>");
            webFormFile.WriteLine("</HeaderTemplate>");
            webFormFile.WriteLine("<ItemTemplate>");
            webFormFile.WriteLine("<tr style = \"background-color:gainsboro\" >");


            SqlDataReader dr2 = cmd.ExecuteReader();
            while (dr2.Read())
            {
                if (dr2[0].ToString() != "id" || dr2[0].ToString() != "ID")
                {
                    webFormFile.WriteLine("<td><%#Eval(\"" + dr2[0].ToString() + "\")%>\"</td>");

                }

            }
            dr2.Close();
            con.Close();
            webFormFile.WriteLine("</tr>");
            webFormFile.WriteLine("</ItemTemplate>");
            webFormFile.WriteLine("<FooterTemplate>");
            webFormFile.WriteLine("</table>" + Environment.NewLine + "</FooterTemplate>");

            webFormFile.WriteLine("</asp:Repeater>" + Environment.NewLine + "</form>" + Environment.NewLine + "</body>");


            webFormFile.WriteLine("</html>");

            webFormFile.Flush();
            return memoryStream;
        }

        public MemoryStream AspxCsDondur(string tabloAdi, string projeAdi, string veritabaniAdi, string webFormAd, string kullaniciadi, string sifre, string baglantiAdres)
        {
            string classAdi = tabloAdi;
            SqlConnection con = new SqlConnection();
            if (baglantiAdres != "")
            {
                con.ConnectionString = "Server = " + baglantiAdres + "; Database = " + veritabaniAdi + "; User Id = " + kullaniciadi + "; password = " + sifre + "";
            }
            else
            {
                con.ConnectionString = "Server = localhost; Database=" + veritabaniAdi + ";Trusted_Connection=True;";
            }
            MemoryStream memoryStream = new MemoryStream();
            TextWriter classFile = new StreamWriter(memoryStream);
            classFile.WriteLine("using System;" + Environment.NewLine + "using System.Collections.Generic;" + Environment.NewLine + "using System.Linq;" + Environment.NewLine + "using System.Web;" + Environment.NewLine + "using System.Web.UI;" + Environment.NewLine + "using System.Web.UI.WebControls;" + Environment.NewLine);
            classFile.WriteLine("namespace " + BuyukBasHarf(projeAdi) + Environment.NewLine + "{" + Environment.NewLine + "public partial class " + BuyukBasHarf(webFormAd) + " : System.Web.UI.Page" + Environment.NewLine + "{");
            classFile.WriteLine("protected void Page_Load(object sender, EventArgs e)" + Environment.NewLine + "{");
            classFile.WriteLine("DbProcess db = new DbProcess();");
            classFile.WriteLine("List<"+BuyukBasHarf(tabloAdi)+ "> " +KucukBasHarf(tabloAdi)+ "Listele = new List<" + BuyukBasHarf(tabloAdi) + ">();");
            classFile.WriteLine(KucukBasHarf(tabloAdi) + "Listele = db."+ BuyukBasHarf(tabloAdi) + "Select();");
            classFile.WriteLine("rpt" + tabloAdi+".DataSource = " + KucukBasHarf(tabloAdi) + "Listele;");
            classFile.WriteLine("rpt" + tabloAdi + ".DataBind();");

            classFile.WriteLine("}" + Environment.NewLine + "}" + Environment.NewLine + "}");
            classFile.Flush();
            return memoryStream;
        }

        public MemoryStream AspxDesignerCsDondur(string tabloAdi, string projeAdi, string webFormAd , string kullaniciadi, string sifre, string baglantiAdres)
        {
            MemoryStream memoryStream = new MemoryStream();
            TextWriter classFile = new StreamWriter(memoryStream);
            classFile.WriteLine("namespace " + projeAdi + Environment.NewLine + "{");
            classFile.WriteLine("public partial class " + webFormAd + Environment.NewLine + "{");
            classFile.WriteLine("protected global::System.Web.UI.HtmlControls.HtmlForm form1;");
            classFile.WriteLine("protected global::System.Web.UI.WebControls.Repeater rpt"+tabloAdi+";" + Environment.NewLine + "}" + Environment.NewLine + "}");

            classFile.Flush();
            return memoryStream;
        }

    public string BuyukBasHarf(string classAdi)
        {
            string result="";
            result = classAdi.Substring(0, 1).ToUpper() + classAdi.Substring(1, classAdi.Length - 1);
            return result;
        }

        public string KucukBasHarf(string attributeAdi)
        {
            string result = "";
            result = attributeAdi.Substring(0, 1).ToLower() + attributeAdi.Substring(1, attributeAdi.Length - 1);
            return result;
        }

    }
}
