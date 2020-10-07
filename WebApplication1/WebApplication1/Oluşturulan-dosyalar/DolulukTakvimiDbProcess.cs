using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebApplication1
{
public class DbProcess
{

SqlConnection con = new SqlConnection("Server = localhost; Database=OtelRezervasyon;Trusted_Connection=True;");
public void DolulukTakvimiInsert(DolulukTakvimi dolulukTakvimiDegisken)
{
SqlCommand insert = new SqlCommand("insert into DolulukTakvimi(OdaID,DoluTarih) values(@OdaID,@DoluTarih)",con);
insert.Parameters.AddWithValue("@OdaID",dolulukTakvimiDegisken.odaID);
insert.Parameters.AddWithValue("@DoluTarih",dolulukTakvimiDegisken.doluTarih);
KomutCalistir(insert);
}

public void DolulukTakvimiUpdate(DolulukTakvimi dolulukTakvimiDegisken)
{
SqlCommand update = new SqlCommand("update DolulukTakvimi set OdaID=@OdaID,DoluTarih=@DoluTarih where ID=@ID", con);
update.Parameters.AddWithValue("@OdaID",dolulukTakvimiDegisken.odaID);
update.Parameters.AddWithValue("@DoluTarih",dolulukTakvimiDegisken.doluTarih);
update.Parameters.AddWithValue("@ID",dolulukTakvimiDegisken.ID);
KomutCalistir(update);
}

public void DolulukTakvimiDelete(DolulukTakvimi dolulukTakvimiDegisken)
{
SqlCommand delete= new SqlCommand("delete from DolulukTakvimi where ID=@ID",con);
delete.Parameters.AddWithValue("@ID",dolulukTakvimiDegisken.ID);
KomutCalistir(delete);
}

public List<DolulukTakvimi> DolulukTakvimiSelect()
{
List<DolulukTakvimi> result = new List<DolulukTakvimi>();
con.Open();
SqlCommand komut = new SqlCommand("select * from DolulukTakvimi",con);
SqlDataReader dr = komut.ExecuteReader();
while (dr.Read())
{
DolulukTakvimi dolulukTakvimi = new DolulukTakvimi()
{

odaID = Convert.ToInt32(dr[0]),
doluTarih = Convert.ToDateTime(dr[1])
};
result.Add(dolulukTakvimi);
}
con.Close();
return result;
}

public Boolean KomutCalistir(SqlCommand komut)
{
con.Open();
komut.ExecuteNonQuery();
komut.Dispose();
con.Close();
return true;
}

}
}
