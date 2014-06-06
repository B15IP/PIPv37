using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class profil : System.Web.UI.Page
{
    SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Domanitos\Downloads\PIPv36\PIP\App_Data\PIP.mdf';Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");    
 
    protected void Page_Load(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;

        cmd.CommandText = "select acont,nume,prenume,email,parola from utilizator where acont='" + Session["nume"].ToString() + "';";
        cmd.CommandType = CommandType.Text;

        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                Labelacont.Text = SRD.GetString(SRD.GetOrdinal("acont")).ToString();
                LabelNume.Text = SRD.GetString(SRD.GetOrdinal("nume")).ToString();
                LabelPrenume.Text = SRD.GetString(SRD.GetOrdinal("prenume")).ToString();
                LabelEmail.Text = SRD.GetString(SRD.GetOrdinal("email")).ToString();
            }
        }


        sqlConnection1.Close();

      //  afisareEvenimenteOrganizate();
    }

    protected void ButtonNume_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        if (TextBoxNume.Visible == true)
        {
            TextBoxNume.Visible = false;
            cmd.Connection = sqlConnection1;
            cmd.CommandText = "update utilizator set nume='" + TextBoxNume.Text + "' where acont='" + Session["nume"].ToString() + "';";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            LabelNume.Text = TextBoxNume.Text;
        }
        else
        {
            TextBoxNume.Visible = true;
            TextBoxNume.Text = LabelNume.Text;

        }
        sqlConnection1.Close();
    }
    protected void ButtonPrenume_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        if (TextBoxPrenume.Visible == true)
        {
            TextBoxPrenume.Visible = false;
            cmd.Connection = sqlConnection1;
            cmd.CommandText = "update utilizator set prenume='" + TextBoxPrenume.Text + "' where acont='" + Session["nume"].ToString() + "';";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            LabelPrenume.Text = TextBoxPrenume.Text;
        }
        else
        {
            TextBoxPrenume.Visible = true;
            TextBoxPrenume.Text = LabelPrenume.Text;

        }
        sqlConnection1.Close();
    }
    protected void Buttonschimbaparola(object sender, EventArgs e)
    {
        if (Panelschimbaparola.Visible == false)
            Panelschimbaparola.Visible = true;
        else Panelschimbaparola.Visible = false;
    }
    protected void Buttonschimba_Click(object sender, EventArgs e)
    {
        Labelparolaeroare.Text = "";
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;

        cmd.CommandText = "select parola from utilizator where acont='" + Session["nume"].ToString() + "';";
        cmd.CommandType = CommandType.Text;

        if (TextBoxparola.Text != cmd.ExecuteScalar().ToString())
        {
            Labelparolaeroare.Text = "Parola veche incorecta!";
            return;
        }
        if (TextBoxparolan.Text != TextBoxparolan2.Text)
        {
            Labelparolaeroare.Text = "Eroare confirmare parola noua!";
            return;
        }

        cmd.CommandText = "update utilizator set parola='" + TextBoxparolan.Text + "' where acont='" + Session["nume"].ToString() + "';";
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        Labelparolaeroare.Text = "Parola modificat cu succes!";

        sqlConnection1.Close();

    }

    protected void afisareEvenimenteOrganizate()
    {
        SqlCommand cmd = new SqlCommand();
        int invitatiiExista = 0;

        try
        {
            String query = "SELECT count(id_eveniment) from organizeaza where id_organizator = " + Session["id"] + " and aprobat=1;";
            sqlConnection1.Open();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            invitatiiExista = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            LabelMesajEroare.Text = "Nu merge " + ex.Message;
        }
        sqlConnection1.Close();


        if (invitatiiExista != 0)
        {

            try
            {
                String query = "SELECT id_eveniment from organizeaza where id_organizator = " + Session["id"] + " and aprobat=1;";
                sqlConnection1.Open();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;

                DataTable Table_Evenimente = new DataTable();

                if (Table_Evenimente.Columns.Count == 0)
                {
                    Table_Evenimente.Columns.Add("ID", typeof(int));
                    Table_Evenimente.Columns.Add("Nume", typeof(string));
                    Table_Evenimente.Columns.Add("Data", typeof(string));
                    Table_Evenimente.Columns.Add("Oras", typeof(string));

                }

                List<int> idFiecareEveniment = new List<int>();

                using (SqlDataReader fiecareEvenimentLaCareEsteOrganizator = cmd.ExecuteReader())
                {
                    while (fiecareEvenimentLaCareEsteOrganizator.Read())
                    {
                        idFiecareEveniment.Add(fiecareEvenimentLaCareEsteOrganizator.GetInt32(fiecareEvenimentLaCareEsteOrganizator.GetOrdinal("id_eveniment")));
                    }
                    fiecareEvenimentLaCareEsteOrganizator.Close();
                }

                foreach (int idEvent in idFiecareEveniment)
                {

                    query = "SELECT id,nume,data_inceperii,oras from evenimente where id = " + idEvent;
                    cmd.CommandText = query;
                    SqlDataReader InfoEvenimente = cmd.ExecuteReader();

                    while (InfoEvenimente.Read())
                    {

                        DataRow NewRow = Table_Evenimente.NewRow(); //un nou rand!!!

                        NewRow[0] = InfoEvenimente.GetInt32(0);
                        NewRow[1] = InfoEvenimente.GetString(1);
                        NewRow[2] = InfoEvenimente.GetDateTime(2).ToString("dd MM yyyy");
                        //LabelMesajEroare.Text += " Opa ";
                        NewRow[3] = InfoEvenimente.GetString(3);
                        //LabelMesajEroare.Text += " Opa ";
                        Table_Evenimente.Rows.Add(NewRow);

                    }
                    InfoEvenimente.Close();
                }
                GridView_listaEvenimenteOrganizate.DataSource = Table_Evenimente;
                GridView_listaEvenimenteOrganizate.DataBind();

             

                int index_eveniment = 0; //pentru fiecare id de eveniment
                foreach (GridViewRow row in GridView_listaEvenimenteOrganizate.Rows)
                {
                    int i = 0;
                    LinkButton accepta = new LinkButton();
                    accepta.ID = "a" + idFiecareEveniment[index_eveniment].ToString(); //fiecare buton are id-ul evenimentului pe care il refera
                    accepta.Text = "Renunta";
                    accepta.Click += new EventHandler(refuzaCaOrganizator);
                    row.Cells[i].Controls.Add(accepta);
                    i++;
                    LinkButton linkCatreEveniment= new LinkButton();
                    linkCatreEveniment.ID = "l" + idFiecareEveniment[index_eveniment].ToString();
                    linkCatreEveniment.Text = "Click";
                   linkCatreEveniment.Click += new EventHandler(catrePaginaEveniment);
                    row.Cells[i].Controls.Add(linkCatreEveniment);
                    index_eveniment++;
                }
            }
            catch (Exception ex)
            {
                LabelMesajEroare.Text += "Nu merrrrge " + ex.Message;
            }
            sqlConnection1.Close();

            GridView_listaEvenimenteOrganizate.Visible = true;
        }
    }

    protected void refuzaCaOrganizator(object sender, EventArgs e)
    {
        LinkButton eveniment = (LinkButton)sender;
        int idEveniment = 0;
        if (eveniment.Text == "Renunta")
        {
            idEveniment = Convert.ToInt32(eveniment.ID.Substring(1));
        }
        SqlCommand cmd = new SqlCommand();


        try
        {
            String query = "Delete from organizeaza where id_eveniment = " + idEveniment + " and id_organizator = " + Session["id"];
            sqlConnection1.Open();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            LabelMesajEroare.Text = "Nu merge " + ex.Message;
        }
        sqlConnection1.Close();
        Response.Redirect("profil.aspx");
    }

    protected void catrePaginaEveniment(object sender, EventArgs e)
    {
        LinkButton eveniment = (LinkButton)sender;
        int idEveniment = 0;
        if (eveniment.Text == "Click")
        {
            idEveniment = Convert.ToInt32(eveniment.ID.Substring(1));
        }

        Session["IdEvenimentSelectat"] = idEveniment;
       // SearchBox.Text = Session["IdEvenimentSelectat"].ToString();
        Response.Redirect("Eveniment.aspx");
    }
}