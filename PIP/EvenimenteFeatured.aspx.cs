using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.OleDb;

public partial class EvenimenteFeatured : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        afisareEvenimenteAsemanatoare();
    }

    SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Domanitos\Downloads\PIPv36\PIP\App_Data\PIP.mdf';Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
    protected void afisareEvenimenteAsemanatoare()
    {
        int id = Convert.ToInt32(Session["IdEvenimentSelectat"]);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "select * from evenimente where id=" + id + ";";
        cmd.CommandType = CommandType.Text;

        //ButtonEvenimenteCreate.ToolTip = "1";
        //ButtonSearch.ToolTip = "0";

        // iau etichetele, data inceperii si orasul penru a vedea daca mai gasesc si alte evenimente cu aceleasi caracteristici

        string etichete = "%", oras = "%" ; //in etichete este si orasul
        DateTime data=DateTime.Now;
        
        try
        {
            sqlConnection1.Open();
            SqlDataReader InfoEvent = cmd.ExecuteReader();
            while (InfoEvent.Read())
            {
               
                etichete = InfoEvent.GetString(InfoEvent.GetOrdinal("etichete"));
                oras = InfoEvent.GetString(InfoEvent.GetOrdinal("oras"));
                data = InfoEvent.GetDateTime(InfoEvent.GetOrdinal("data_inceperii"));
            }
            sqlConnection1.Close();
        }
        catch (Exception ex)
        {

            LabelMesajEroare.Text += "Eroare in functia afisareInfoFeatured " + ex.Message;
        }


        try
        {
            cmd.CommandText = "select * from evenimente where (etichete like '" + etichete + "' or ( data_inceperii = '" + data + "' and oras= '" + oras + "') or data_inceperii = '" + data + "' or oras= '" + oras + "') and id !=" + id + " ;";
            cmd.CommandType = CommandType.Text;

            sqlConnection1.Open();

            DataTable Table_Evenimente = new DataTable();

            if (Table_Evenimente.Columns.Count == 0)
            {
                Table_Evenimente.Columns.Add("Titlu", typeof(string));
                Table_Evenimente.Columns.Add("Data", typeof(string));
                Table_Evenimente.Columns.Add("Locatie", typeof(string));

            }

            List<int> idevenimetfeatured = new List<int>();
            List<string> numeFiecareEveniment = new List<string>();
            int contor = 0;
      
            SqlDataReader InfoEvenimente = cmd.ExecuteReader();

            while (InfoEvenimente.Read())
            {

                DataRow NewRow = Table_Evenimente.NewRow(); //un nou rand!!!

                NewRow[0] = InfoEvenimente.GetString(1);
                //  NewRow[1] = InfoEvenimente.GetString(1);

                numeFiecareEveniment.Add(InfoEvenimente.GetString(InfoEvenimente.GetOrdinal("nume")));
                idevenimetfeatured.Add(InfoEvenimente.GetInt32(InfoEvenimente.GetOrdinal("id")));

                NewRow[1] = InfoEvenimente.GetDateTime(InfoEvenimente.GetOrdinal("data_inceperii")).ToString("dd MM yyyy");
                //LabelMesajEroare.Text += " Opa ";
                NewRow[2] = InfoEvenimente.GetString(InfoEvenimente.GetOrdinal("oras"));
                //LabelMesajEroare.Text += " Opa ";
                Table_Evenimente.Rows.Add(NewRow);
                // contor++;
            }
            InfoEvenimente.Close();
            // LabelMesajEroare.Text = " contor = " + contor + " ";
            //}
            GridViewFeatured.DataSource = Table_Evenimente;
            GridViewFeatured.DataBind();

            int index_eveniment = 0; //pentru fiecare id de eveniment
            foreach (GridViewRow row in GridViewFeatured.Rows)
            {
               
                int i = 0;
                LinkButton spre = new LinkButton();
                spre.ID = "LinkEveniment" + idevenimetfeatured[index_eveniment].ToString(); //fiecare buton are id-ul evenimentului pe care il refera
                spre.Text = numeFiecareEveniment[index_eveniment].ToString();
                spre.Click += new EventHandler(toPaginaEvenimentAccesat);
                spre.ToolTip = "ev_creat";
                row.Cells[i].Controls.Add(spre);
                // LabelMesajEroare.Text += spre.ID + "  i = " + i + " ";
                index_eveniment++;
            
            }
            // LabelMesajEroare.Text += " index eveniment "+index_eveniment + " " ;
        }
        catch (Exception ex)
        {
            LabelMesajEroare.Text += "Eroare in functia afisareInfoFeatured " + ex.Message;
        }

       
       
    
        sqlConnection1.Close();
    }

    protected void toPaginaEvenimentAccesat(object sender, EventArgs e)
    {
        LinkButton x = (LinkButton)sender;
        if (x.ToolTip == "ev_creat")
        {
            Session["IdEvenimentSelectat"] = Convert.ToInt32(x.ID.Substring(13));
            //SearchBox.Text = Session["IdEvenimentSelectat"].ToString();
            // okk++;
            // LabelMesajEroare.Text += " in catrepag " + Session["IdEvenimentSelectat"] + " ";
            Response.Redirect("Eveniment.aspx");
        }

    }
    protected void inapoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("Eveniment.aspx");
    }

}