using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class OrganizatoriEveniment : System.Web.UI.Page
{
    SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Domanitos\Downloads\PIPv36\PIP\App_Data\PIP.mdf';Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");    
  
    bool Proprietar = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        VerificareProprietar(sender, e);
        if (Proprietar)
        {
            ButtonInvitaOrganizatori.Visible = true;
            OrganizatoriInvitati();
        }
    }
    private void VerificareProprietar(object sender, EventArgs e)
    {
        if (Session["nume"] != null)
        {
            int Id_curent = Convert.ToInt32(Session["Id"]);
            int Id_prop = 0;
            SqlCommand cmd = new SqlCommand();
            sqlConnection1.Open();
            cmd.Connection = sqlConnection1;

            cmd.CommandText = "select id_proprietar from evenimente where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader SRD = cmd.ExecuteReader())
            {
                while (SRD.Read())
                {
                    Id_prop = SRD.GetInt32(SRD.GetOrdinal("id_proprietar"));
                }
            }
            sqlConnection1.Close();
            if (Id_prop == Id_curent)
                Proprietar = true;
        }
    }
    protected void ButtonInvitaOrganizatori_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        int utilizatorExista = 0, idUtilizatorInvitat = -1;
        if (UserNameOrganizatorInvitat.Text == "")
        {
            MesajEroare.Text = "Va rugam sa completati casuta text cu un username";
            return;
        }

        try // verificare ca exista acest username
        {
            sqlConnection1.Open();
            cmd.CommandText = "Select count(id) from utilizator where acont COLLATE SQL_Latin1_General_CP1_CS_AS ='" + UserNameOrganizatorInvitat.Text + "' COLLATE SQL_Latin1_General_CP1_CS_AS ;";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            utilizatorExista = Convert.ToInt32(cmd.ExecuteScalar());

        }
        catch (Exception ex)
        {

            MesajEroare.Text = ex.Message;
        }
        sqlConnection1.Close();

        if (utilizatorExista == 0)
        {
            MesajEroare.Text = "Utilizator nu exista";
            return;
        }


        try // selectarea id-ului celui invitat
        {
            sqlConnection1.Open();
            cmd.CommandText = "Select id from utilizator where acont COLLATE SQL_Latin1_General_CP1_CS_AS ='" + UserNameOrganizatorInvitat.Text + "' COLLATE SQL_Latin1_General_CP1_CS_AS;";
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader SRD = cmd.ExecuteReader())
            {
                while (SRD.Read())
                {
                    idUtilizatorInvitat = SRD.GetInt32(SRD.GetOrdinal("id"));
                }
            }
        }

        catch (Exception ex)
        {

            MesajEroare.Text = ex.Message;
        }
        sqlConnection1.Close();

        utilizatorExista = 0;
        try // verificare daca exista deja aceasta invitatie - desi e verificata de cheia primara
        {
            sqlConnection1.Open();
            cmd.CommandText = "Select count(id_organizator) from organizeaza where id_eveniment =" + Session["IdEvenimentSelectat"] + " and id_organizator = " + idUtilizatorInvitat;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            // am facut economie de variabile si am folosit tot utilizatorExista
            utilizatorExista = Convert.ToInt32(cmd.ExecuteScalar());

        }
        catch (Exception ex)
        {

            MesajEroare.Text = ex.Message;
        }
        sqlConnection1.Close();
        // verific si ca nu cumva sa-si trimita lui(proprietarul) invitatie
        if (utilizatorExista != 0 || idUtilizatorInvitat == (int)Session["id"])
        {
            MesajEroare.Text = "Mai introduceti inca o data username-ul";
            return;
        }

        try // inserarea in organizeaza cu aprobat luat default false
        {
            sqlConnection1.Open();
            cmd.CommandText = "Insert into organizeaza(id_eveniment,id_organizator) values( " + Session["IdEvenimentSelectat"] + ", " + idUtilizatorInvitat + ")";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            MesajEroare.Text = ex.Message;
        }
        sqlConnection1.Close();
        LabelInvitatieTrimisa.Text = "Invitatie trimisa cu succes ";
       // Response.Redirect("OrganizatoriEveniment.aspx");
        Response.Redirect("OrganizatoriEveniment.aspx");

    }

   protected void OrganizatoriInvitati(){
       SqlCommand cmd = new SqlCommand();
        int organizatoriExista = 0;

        try
        {
            String query = "SELECT count(id_eveniment) from organizeaza where id_eveniment = " + Session["idEvenimentSelectat"] ;
            sqlConnection1.Open();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            organizatoriExista = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            MesajEroare.Text = "Nu merge " + ex.Message;
        }
        sqlConnection1.Close();

        if (organizatoriExista != 0)
        {

            try
            {
                String query = "SELECT id_organizator,aprobat from organizeaza where id_eveniment = " + Session["idEvenimentSelectat"];
                sqlConnection1.Open();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                List<int> idFiecareOrganizator = new List<int>();
                List<Boolean> Aprobat = new List<Boolean>();
                DataTable Table_ListaOrganizatori = new DataTable();
                if (Table_ListaOrganizatori.Columns.Count == 0)
                {
                    Table_ListaOrganizatori.Columns.Add("Account", typeof(string));
                    Table_ListaOrganizatori.Columns.Add("Status", typeof(string));
                }
                using (SqlDataReader fiecareOrganizator = cmd.ExecuteReader())
                {
                    while (fiecareOrganizator.Read())
                    {
                        idFiecareOrganizator.Add(fiecareOrganizator.GetInt32(fiecareOrganizator.GetOrdinal("id_organizator")));
                        Aprobat.Add(fiecareOrganizator.GetBoolean(fiecareOrganizator.GetOrdinal("aprobat")));
                    }
                    fiecareOrganizator.Close();
                }
              // pentru fiecare id luam username-ul si il afisam alaturi de statusul curent
                for(int id = 0; id < idFiecareOrganizator.Count; id ++){
                    query = "SELECT acont from utilizator where id = " + idFiecareOrganizator[id];
                    cmd.CommandText = query;
                    SqlDataReader fiecareAcont = cmd.ExecuteReader(); // execut comanda SQL
                    fiecareAcont.Read();
                    
                    DataRow NewRow = Table_ListaOrganizatori.NewRow(); //un nou rand!!!

                    NewRow[0] = fiecareAcont.GetString(fiecareAcont.GetOrdinal("acont"));
                    if(Aprobat[id] == true){
                    NewRow[1] = "acceptat";
                    }
                    else
                        if (Aprobat[id] == false)
                        {
                        NewRow[1] = "pending";
                    }
                    Table_ListaOrganizatori.Rows.Add(NewRow); // adaug rand nou
                    fiecareAcont.Close();
                }
                listaOrganizatoriEvent.DataSource = Table_ListaOrganizatori;
                listaOrganizatoriEvent.DataBind();


                int index_organizator = 0; //pentru fiecare id de eveniment
                foreach (GridViewRow row in listaOrganizatoriEvent.Rows)
                {
                    int i = 0;
                    LinkButton elimina = new LinkButton();
                    elimina.ID = "a" + idFiecareOrganizator[index_organizator].ToString(); //fiecare buton are id-ul evenimentului pe care il refera
                    elimina.Text = "Elimina";
                    elimina.Click += new EventHandler(eliminaOrganizator);
                    row.Cells[i].Controls.Add(elimina);
                    //i++;
                    //LinkButton linkCatreEveniment = new LinkButton();
                    //linkCatreEveniment.ID = "l" + idFiecareEveniment[index_eveniment].ToString();
                    //linkCatreEveniment.Text = "Click";
                    //linkCatreEveniment.Click += new EventHandler(catrePaginaEveniment);
                    //row.Cells[i].Controls.Add(linkCatreEveniment);
                    index_organizator++;
                }

            }
            catch (Exception ex)
            {
                MesajEroare.Text += " Nu merge " + ex.Message;
            }
            sqlConnection1.Close();


           

        }
    }

   protected void eliminaOrganizator(object sender, EventArgs e)
   {
       LinkButton eveniment = (LinkButton)sender;
       int idOrg = 0;
       if (eveniment.Text == "Elimina")
       {
           idOrg = Convert.ToInt32(eveniment.ID.Substring(1));
       }
       SqlCommand cmd = new SqlCommand();


       try
       {
           String query = "Delete from organizeaza where id_eveniment = " + Session["idEvenimentSelectat"] + " and id_organizator = " + idOrg;
           sqlConnection1.Open();
           cmd.CommandText = query;
           cmd.CommandType = CommandType.Text;
           cmd.Connection = sqlConnection1;
           cmd.ExecuteNonQuery();
       }
       catch (Exception ex)
       {
           MesajEroare.Text = "Nu merge " + ex.Message;
       }
       sqlConnection1.Close();
       Response.Redirect("OrganizatoriEveniment.aspx");
   }
   protected void inapoi_Click(object sender, EventArgs e)
   {
       Response.Redirect("Eveniment.aspx");
   }

}