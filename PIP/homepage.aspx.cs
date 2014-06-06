using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text.RegularExpressions;
using ASPSnippets.FaceBookAPI;
using System.Web.Script.Serialization;

public partial class homepage : System.Web.UI.Page
{
    SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Domanitos\Downloads\PIPv36\PIP\App_Data\PIP.mdf';Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");

    bool ok = false;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["nume"] == null)
        {
           
            ButtonCreareEveniment.Visible = false;
            ButtonEvenimenteCreate.Visible = false;
           // ButtonEvenimenteCreateAscunde.Visible = false;
            Button_EvenimenteOrganizate.Visible = false;
           // Button_EvenimenteOrganizateAscunde.Visible = false;
            btnLoginFacebook.Visible = true;
        }
        else
        {
            invitatiiCaOrganizator();
            ButtonCreareEveniment.Visible = true;
            ButtonEvenimenteCreate.Visible = true;
          //  ButtonEvenimenteCreateAscunde.Visible = true;
            Button_EvenimenteOrganizate.Visible = true;
            btnLoginFacebook.Visible = false;
          }

        featurednelogat(sender, e);

        if (ButtonSearch.ToolTip == "1")
            ButtonSearch_Click(sender, e);

        if (ButtonEvenimenteCreate.ToolTip == "1")
        {
            ButtonCreate_Click(sender, e);
            //LabelMesajEroare.Text += "DA";
        }

        if (Button_EvenimenteOrganizate.ToolTip == "1")
        {
            Button_EvenimenteOrganizate_Click(sender, e);
          
        }
        //if(GridView_listaEvenimenteOrganizate.ToolTip == "0")
        //GridViewFeatured.Visible = false;

        //if (GridView_listaEvenimenteOrganizate.ToolTip == "0")
        
        //LabelMesajEroare.Text += okk;

        if (Session["nume"] != null)
        {
            evenimente_participa();
        }

        FaceBookConnect.API_Key = "523932864394077";
        FaceBookConnect.API_Secret = "e53bfe7b968d894da5f91fb562551ae5";
        if (!IsPostBack)
        {
            if (Request.QueryString["error"] == "access_denied")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('User has denied access.')", true);
                return;
            }

            string code = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                string data = FaceBookConnect.Fetch(code, "me");
                FaceBookUser faceBookUser = new JavaScriptSerializer().Deserialize<FaceBookUser>(data);
                faceBookUser.PictureUrl = string.Format("https://graph.facebook.com/{0}/picture", faceBookUser.Id);
                //lblId.Text = faceBookUser.Id;
                //Session["nume"] = faceBookUser.UserName;

                //lblEmail.Text = faceBookUser.Email;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT id, acont, email, parola FROM utilizator;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                sqlConnection1.Open();
                using (SqlDataReader SRD = cmd.ExecuteReader())
                {
                    while (SRD.Read())
                    {
                        if ((faceBookUser.Email == SRD.GetString(SRD.GetOrdinal("email"))) && (faceBookUser.Id == SRD.GetString(SRD.GetOrdinal("parola"))))
                        {
                            Session["nume"] = SRD.GetString(SRD.GetOrdinal("acont"));
                            Session["id"] = SRD.GetInt32(SRD.GetOrdinal("id"));
                            ok = true;
                        }
                    }
                }
                sqlConnection1.Close();
                if (!ok)
                {
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandText = "INSERT INTO utilizator(acont, email, parola, nume, prenume) VALUES('" + faceBookUser.Name + "','" + faceBookUser.Email + "','" + faceBookUser.Id + "', '', '')";
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = sqlConnection1;
                    sqlConnection1.Open();
                    cmd2.ExecuteNonQuery();
                    sqlConnection1.Close();
                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.CommandText = "SELECT id, acont, email, parola FROM utilizator;";
                    cmd3.CommandType = CommandType.Text;
                    cmd3.Connection = sqlConnection1;
                    sqlConnection1.Open();
                    using (SqlDataReader SRD = cmd3.ExecuteReader())
                    {
                        while (SRD.Read())
                        {
                            if ((faceBookUser.Email == SRD.GetString(SRD.GetOrdinal("email"))) && (faceBookUser.Id == SRD.GetString(SRD.GetOrdinal("parola"))))
                            {
                                Session["nume"] = SRD.GetString(SRD.GetOrdinal("acont"));
                                Session["id"] = SRD.GetInt32(SRD.GetOrdinal("id"));
                                ok = true;
                            }
                        }
                    }
                    sqlConnection1.Close();
                }
                btnLoginFacebook.Visible = false;
                Response.Redirect("homepage.aspx");
            }
        }

    }

    protected void LoginFacebook(object sender, EventArgs e)
    {
        FaceBookConnect.Authorize("user_photos,email", Request.Url.AbsoluteUri.Split('?')[0]);
    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {
        ButtonEvenimenteCreate.ToolTip = "0";
       // Button_EvenimenteOrganizate.ToolTip = "0";
        ButtonSearch.ToolTip = "1";
        GridView_listaEvenimenteOrganizate.ToolTip = "0";

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        // cmd.CommandText = "select * from evenimente where nume = '" + SearchBox.Text + "';";
        cmd.CommandType = CommandType.Text;

        cmd.CommandText = "select * from evenimente ";

        if (SearchBox.Text != "")
        {
            String[] cuvinteSearchBox = SearchBox.Text.Split(' ');
            ArrayList cuvinteCautate = new ArrayList();
            foreach (String cuvant in cuvinteSearchBox)
            { // elimin elementele de tipul ""
                if (cuvant != "")
                {
                    cuvinteCautate.Add(cuvant);
                }
            }

            if (cuvinteCautate.Count == 0)
            { // caz special cand se introduce in search box doar spatiu
                return;
            }

            for (int pasCuvant = 0; pasCuvant < cuvinteCautate.Count; pasCuvant++)
            {

                if (pasCuvant == cuvinteCautate.Count - 1)
                {
                    if (pasCuvant == 0)
                        cmd.CommandText += " where ( etichete like '%" + cuvinteCautate[pasCuvant] + "%' )" + " or nume = '" + SearchBox.Text + "';";
                    else
                        cmd.CommandText += " ( etichete like '%" + cuvinteCautate[pasCuvant] + "%' )" + " or nume = '"+SearchBox.Text+"';";
                }
                else
                {
                    if (pasCuvant == 0)
                    {
                        cmd.CommandText += " where ( etichete like '%" + cuvinteCautate[pasCuvant] + "%' )" + " or ";
                    }
                    else
                    {
                        cmd.CommandText += " ( etichete like '%" + cuvinteCautate[pasCuvant] + "%' )" + " or ";
                    }

                }
            }
            //LabelMesajEroare.Text = cmd.CommandText;
            //-----------------------------------

            try
            {
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
                //using (SqlDataReader evenimetfeatured = cmd.ExecuteReader())
                //{
                //    while (evenimetfeatured.Read())
                //    {

                //        //contor++;
                //    }
                //    evenimetfeatured.Close();
                //}
                //LabelMesajEroare.Text = " contor = "+contor +" ";

                //foreach (int idEvent in idevenimetfeatured)
                //{

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
              //  LabelMesajEroare.Text += " index eveniment " + index_eveniment + " ";
            }
            catch (Exception ex)
            {
                LabelMesajEroare.Text += "Eroare in functia ButtonCreate_CLick " + ex.Message;
            }
       

            //sqlConnection1.Open();
            //if (((Label)Panelsearch.FindControl("k1")) == null)
            //{
            //    Label k = new Label();
            //    k.ID = "k1";
            //    k.Text = "<h2>Rezultatele cautarii:</h2>";
            //    Panelsearch.Controls.Add(k);
            //}
            //using (SqlDataReader SRD = cmd.ExecuteReader())
            //{
            //    while (SRD.Read())
            //    {
            //        LinkButton btn = new LinkButton();
            //        btn.ID = "LinkEveniment" + SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
            //        if (((LinkButton)Panelsearch.FindControl(btn.ID)) == null)
            //        {
            //            ButtonSearch.ToolTip = "1";
            //            btn.ID = "LinkEveniment" + SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
            //            btn.Text = SRD.GetString(SRD.GetOrdinal("nume")).ToString();
            //            btn.Click += new EventHandler(butoneveniment);
            //            Panelsearch.Controls.Add(btn);
            //            //Panelsearch.Controls.Add(new LiteralControl("<br />"));

                     
            //            //Label l2 = new Label();
            //            //l2.Text = "Nume: " + SRD.GetString(SRD.GetOrdinal("nume")).ToString();
            //            //Panelsearch.Controls.Add(l2);
            //            //Panelsearch.Controls.Add(new LiteralControl("</br>"));
                     
            //            Label l3 = new Label();

            //            if (Convert.IsDBNull(SRD["descriere"])) l3.Text = "";
            //            else l3.Text = "<br />Descriere :" + SRD.GetString(SRD.GetOrdinal("descriere")).ToString();
            //            l3.Visible = false;
            //            Panelsearch.Controls.Add(l3);

            //            Label l4 = new Label();

            //            if (Convert.IsDBNull(SRD["data_inceperii"])) l4.Text = "";
            //            else l4.Text =  "<br />"+"Data :" + SRD.GetDateTime(SRD.GetOrdinal("data_inceperii")).ToString();

            //            Panelsearch.Controls.Add(l4);

            //            Label l5 = new Label();
            //            l5.Text = "<br />";
            //            if (Convert.IsDBNull(SRD["oras"])) l5.Text += "";
            //            else l5.Text += " "+SRD.GetString(SRD.GetOrdinal("oras")).ToString();
            //            if (Convert.IsDBNull(SRD["judet"])) l5.Text += "";
            //            else l5.Text += " "+SRD.GetString(SRD.GetOrdinal("judet")).ToString();
            //            if (Convert.IsDBNull(SRD["tara"])) l5.Text += "";
            //            else l5.Text += " "+SRD.GetString(SRD.GetOrdinal("tara")).ToString();

            //            Panelsearch.Controls.Add(l5);

            //            Label l = new Label();
            //            l.Text = "<p>ID: " + SRD.GetInt32(SRD.GetOrdinal("Id")).ToString() + "</p>";
            //            l.Visible = false;
            //            Panelsearch.Controls.Add(l);
            //            //Panelsearch.Controls.Add(new LiteralControl("<br />"));
            //            Panelsearch.Controls.Add(new LiteralControl("<p></p>"));
            //        }
            //    }
            //}
            sqlConnection1.Close();
        }
    }
    
    protected void ButtonCreate_Click(object sender, EventArgs e)
    {
        GridView_listaEvenimenteOrganizate.ToolTip = "0";

        int id = Convert.ToInt32(Session["id"]);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "select * from evenimente where id_proprietar=" + id + ";";
        cmd.CommandType = CommandType.Text;
        
        ButtonEvenimenteCreate.ToolTip = "1";
        ButtonSearch.ToolTip = "0";
        //Button_EvenimenteOrganizate.ToolTip = "0";

        try
        {
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
            //using (SqlDataReader evenimetfeatured = cmd.ExecuteReader())
            //{
            //    while (evenimetfeatured.Read())
            //    {
                    
            //        //contor++;
            //    }
            //    evenimetfeatured.Close();
            //}
            //LabelMesajEroare.Text = " contor = "+contor +" ";

            //foreach (int idEvent in idevenimetfeatured)
            //{

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
                ButtonEvenimenteCreate.ToolTip = "1";
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
            LabelMesajEroare.Text += "Eroare in functia ButtonCreate_CLick "+ ex.Message;
        }
       
        //if (((Label)Panelcreate.FindControl("k2")) == null)
        //{
        //    Label k = new Label();
        //    k.ID = "k2";
        //    k.Text = "Rezultatele cautarii:";
        //    Panelcreate.Controls.Add(k);
        //    Panelcreate.Controls.Add(new LiteralControl("</br>"));
        //}
        //using (SqlDataReader SRD = cmd.ExecuteReader())
        //{
        //    while (SRD.Read())
        //    {
        //        LinkButton btn = new LinkButton();
        //        btn.ID = "LinkEvenimentCreat" + SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
        //        if (((LinkButton)Panelcreate.FindControl(btn.ID)) == null)
        //        {
        //            ButtonEvenimenteCreate.ToolTip = "1";
        //            btn.ID = "LinkEvenimentCreat" + SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
        //            btn.Text = SRD.GetString(SRD.GetOrdinal("nume")).ToString();
        //            btn.Click += new EventHandler(butonevenimentcreat);
        //            Panelcreate.Controls.Add(btn);
        //            Panelcreate.Controls.Add(new LiteralControl("</br>"));
                 
        //            Label l3 = new Label();

        //            if (Convert.IsDBNull(SRD["descriere"])) l3.Text = "";
        //            else l3.Text = "Descriere :" + SRD.GetString(SRD.GetOrdinal("descriere")).ToString();

        //            Panelcreate.Controls.Add(l3);

        //            Label l = new Label();
        //            l.Text = "ID: " + SRD.GetInt32(SRD.GetOrdinal("Id")).ToString();
        //            l.Visible = false;
        //            Panelcreate.Controls.Add(l);
        //            Panelcreate.Controls.Add(new LiteralControl("</br>"));
        //            Panelcreate.Controls.Add(new LiteralControl("<p></p>"));
        //        }
        //    }
        //}
        sqlConnection1.Close();
    }

    protected void Button_CreazaEveniment(object sender, EventArgs e)
    {
        Response.Redirect("CreazaEveniment.aspx");
    }

    protected void ButtonCreateAscunde_Click(object sender, EventArgs e)
    {
        ButtonEvenimenteCreate.ToolTip = "0";
        Panelcreate.Controls.Clear();

    }

    protected void butoneveniment(object sender, EventArgs e)
    {
        LinkButton x = (LinkButton)sender;
        Session["IdEvenimentSelectat"] = Convert.ToInt32(x.ID.Substring(13));
        SearchBox.Text = Session["IdEvenimentSelectat"].ToString();
        Response.Redirect("Eveniment.aspx");
    }
   // int okk = 0;
    protected void toPaginaEvenimentAccesat(object sender, EventArgs e)
    {
        LinkButton x = (LinkButton)sender;
        if(x.ToolTip == "ev_creat"){
        Session["IdEvenimentSelectat"] = Convert.ToInt32(x.ID.Substring(13));
        //SearchBox.Text = Session["IdEvenimentSelectat"].ToString();
       // okk++;
       // LabelMesajEroare.Text += " in catrepag " + Session["IdEvenimentSelectat"] + " ";
        Response.Redirect("Eveniment.aspx");
       }
        
    }

    protected void butonevenimentcreat(object sender, EventArgs e)
    {
        LinkButton x = (LinkButton)sender;
        Session["IdEvenimentSelectat"] = Convert.ToInt32(x.ID.Substring(18));
        SearchBox.Text = Session["IdEvenimentSelectat"].ToString();
        Response.Redirect("Eveniment.aspx");
    }

    protected void invitatiiCaOrganizator()
    {
        SqlCommand cmd = new SqlCommand();
        int invitatiiExista = 0;

        //LabelMesajEroare.Text += Session["id"] + "s#";

        try
        {
            String query = "SELECT count(id_eveniment) from organizeaza where id_organizator = " + Session["id"] + " and aprobat=0;";
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
                String query = "SELECT id_eveniment from organizeaza where id_organizator = " + Session["id"] + " and aprobat=0;";
                sqlConnection1.Open();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
             
                DataTable Table_Evenimente = new DataTable();

                if (Table_Evenimente.Columns.Count == 0)
                {
                    Table_Evenimente.Columns.Add("ID", typeof(int));
                   // Table_Evenimente.Columns.Add("Nume", typeof(string));
                    Table_Evenimente.Columns.Add("Data", typeof(string));
                    Table_Evenimente.Columns.Add("Oras", typeof(string));
                            
                }

                List<int> idFiecareEveniment = new List<int>();
                List<string> numeFiecareEveniment = new List<string>();

                using (SqlDataReader fiecareEvenimentLaCareEsteInvitat = cmd.ExecuteReader())
                {
                    while (fiecareEvenimentLaCareEsteInvitat.Read())
                    {
                        idFiecareEveniment.Add(fiecareEvenimentLaCareEsteInvitat.GetInt32(fiecareEvenimentLaCareEsteInvitat.GetOrdinal("id_eveniment")));
                    }
                    fiecareEvenimentLaCareEsteInvitat.Close();
                }

                foreach (int idEvent in idFiecareEveniment)
                {
                    //LabelMesajEroare.Text += idEvent + "s#";
                    query = "SELECT id,nume,data_inceperii,oras from evenimente where id = " + idEvent;
                    cmd.CommandText = query;
                    SqlDataReader InfoEvenimente = cmd.ExecuteReader();

                    while (InfoEvenimente.Read())
                    {

                        DataRow NewRow = Table_Evenimente.NewRow(); //un nou rand!!!

                        NewRow[0] = InfoEvenimente.GetInt32(0);
                      //  NewRow[1] = InfoEvenimente.GetString(1);
                        numeFiecareEveniment.Add(InfoEvenimente.GetString(1));

                        NewRow[1] = InfoEvenimente.GetDateTime(2).ToString("dd MM yyyy");
                        //LabelMesajEroare.Text += " Opa ";
                        NewRow[2] = InfoEvenimente.GetString(3);
                        //LabelMesajEroare.Text += " Opa ";
                        Table_Evenimente.Rows.Add(NewRow);

                    }
                    InfoEvenimente.Close();
                }
                listaInvitatii.DataSource = Table_Evenimente;
                listaInvitatii.DataBind();

                int index_eveniment = 0; //pentru fiecare id de eveniment
                foreach (GridViewRow row in listaInvitatii.Rows)
                {
                    int i = 0;
                    LinkButton accepta = new LinkButton();
                    accepta.ID = "a" + idFiecareEveniment[index_eveniment].ToString(); //fiecare buton are id-ul evenimentului pe care il refera
                     accepta.Text = "Yes";
                    accepta.Click += new EventHandler(acceptaCaOrganizator);
                    row.Cells[i].Controls.Add(accepta);
                    i++;
                    LinkButton refuza = new LinkButton();
                    refuza.ID = "r" + idFiecareEveniment[index_eveniment].ToString();
                    refuza.Text = "No";
                    refuza.Click += new EventHandler(refuzaCaOrganizator);
                    row.Cells[i].Controls.Add(refuza);
                    i++;
                    LinkButton nume = new LinkButton();
                    nume.ID = "n" + idFiecareEveniment[index_eveniment].ToString();
                    nume.Text = numeFiecareEveniment[index_eveniment];
                    nume.ToolTip = "toEvent";
                    nume.Click += new EventHandler(catrePaginaEveniment);
                    row.Cells[i].Controls.Add(nume);
                    index_eveniment++;
                }
            }
            catch (Exception ex)
            {
                LabelMesajEroare.Text += "Nu merrrrge11 " + ex.Message;
            }
            sqlConnection1.Close();

            listaInvitatii.Visible = true;
        }
    }

    protected void featurednelogat(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        String query = "select TOP 5 id_eveniment,count(id_utilizator) from participa group by id_eveniment order by id_eveniment desc";
        sqlConnection1.Open();
        cmd.CommandText = query;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;

        DataTable Table_Evenimente = new DataTable();

        if (Table_Evenimente.Columns.Count == 0)
        {
            Table_Evenimente.Columns.Add("Titlu", typeof(string));
            Table_Evenimente.Columns.Add("Locatie", typeof(string));
            Table_Evenimente.Columns.Add("Data", typeof(string));
         
        }

        List<int> idevenimetfeatured = new List<int>();
        List<string> numeFiecareEveniment = new List<string>();

        using (SqlDataReader evenimetfeatured = cmd.ExecuteReader())
        {
            while (evenimetfeatured.Read())
            {
                idevenimetfeatured.Add(evenimetfeatured.GetInt32(evenimetfeatured.GetOrdinal("id_eveniment")));
            }
            evenimetfeatured.Close();
        }

        foreach (int idEvent in idevenimetfeatured)
        {
            //LabelMesajEroare.Text += idEvent + "s#";
            query = "SELECT id,nume,data_inceperii,oras from evenimente where id = " + idEvent;
            cmd.CommandText = query;
            SqlDataReader InfoEvenimente = cmd.ExecuteReader();

            while (InfoEvenimente.Read())
            {

                DataRow NewRow = Table_Evenimente.NewRow(); //un nou rand!!!

                NewRow[0] = InfoEvenimente.GetString(1);
                //  NewRow[1] = InfoEvenimente.GetString(1);
                numeFiecareEveniment.Add(InfoEvenimente.GetString(1));

                NewRow[2] = InfoEvenimente.GetDateTime(2).ToString("dd MM yyyy");
                //LabelMesajEroare.Text += " Opa ";
                NewRow[1] = InfoEvenimente.GetString(3);
                //LabelMesajEroare.Text += " Opa ";
                Table_Evenimente.Rows.Add(NewRow);

            }
            InfoEvenimente.Close();
        }
        GridViewFeatured.DataSource = Table_Evenimente;
        GridViewFeatured.DataBind();

        int index_eveniment = 0; //pentru fiecare id de eveniment
        foreach (GridViewRow row in GridViewFeatured.Rows)
        {
            int i = 0;
            LinkButton spre = new LinkButton();
            spre.ID = "LinkEveniment" + idevenimetfeatured[index_eveniment].ToString(); //fiecare buton are id-ul evenimentului pe care il refera
            spre.Text = numeFiecareEveniment[index_eveniment].ToString();
            spre.Click += new EventHandler(butoneveniment);
            row.Cells[i].Controls.Add(spre);
            //i++;

            //Label refuza = new Label();
            //refuza.ID = "r" + idevenimetfeatured[index_eveniment].ToString();
            //refuza.Text = row.Cells[2].Text.ToString();
            //row.Cells[i].Controls.Add(refuza);
            //i++;
            //Label nume = new Label();
            //nume.ID = "n" + idevenimetfeatured[index_eveniment].ToString();
            //nume.Text = row.Cells[1].Text.ToString();
            //nume.ToolTip = "toEvent";
            //row.Cells[i].Controls.Add(nume);
            index_eveniment++;
        }
        sqlConnection1.Close();
           
    }

    protected void acceptaCaOrganizator(object sender,EventArgs e)
    {
        LinkButton eveniment = (LinkButton)sender;
        int idEveniment = 0;
        if(eveniment.Text == "Yes"){
        idEveniment = Convert.ToInt32(eveniment.ID.Substring(1));
        }
        SqlCommand cmd = new SqlCommand();
       

        try
        {
            String query = "Update organizeaza set aprobat = 1 where id_eveniment = " + idEveniment;
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
        Response.Redirect("homepage.aspx");

    }

    protected void refuzaCaOrganizator(object sender, EventArgs e)
    {
        LinkButton eveniment = (LinkButton)sender;
        int idEveniment = 0;
        if (eveniment.Text == "No")
        {
            idEveniment = Convert.ToInt32(eveniment.ID.Substring(1));
        }
        SqlCommand cmd = new SqlCommand();


        try
        {
            String query = "Delete from organizeaza where id_eveniment = " + idEveniment + " and id_organizator = "+Session["id"];
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
        Response.Redirect("homepage.aspx");
    }

    protected void retragereCaOrganizator(object sender, EventArgs e)
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
        Response.Redirect("homepage.aspx");
    }

    protected void catrePaginaEveniment(object sender, EventArgs e)
    {
        //LabelMesajEroare.Text = "*";
        LinkButton eveniment = (LinkButton)sender;
        int idEveniment = 0;
        if (eveniment.Text == "Click")
        {
            //  LabelMesajEroare.Text = "&";
            idEveniment = Convert.ToInt32(eveniment.ID.Substring(1));
        }
        
        
            if(eveniment.ToolTip == "toEvent"){
            idEveniment = Convert.ToInt32(eveniment.ID.Substring(1));
            }
        

        Session["IdEvenimentSelectat"] = idEveniment;
        // SearchBox.Text = Session["IdEvenimentSelectat"].ToString();
       // LabelMesajEroare.Text += "%" + idEveniment;
         Response.Redirect("Eveniment.aspx");
    }


    protected void Button_EvenimenteOrganizate_Click(object sender, EventArgs e)
    {
        GridView_listaEvenimenteOrganizate.Visible = true;
        GridView_listaEvenimenteOrganizate.ToolTip = "1";

        SqlCommand cmd = new SqlCommand();
        int invitatiiExista = 0;

        //LabelMesajEroare.Text = "*";
        //if (Button_EvenimenteOrganizate.ToolTip == "0")
            Button_EvenimenteOrganizate.ToolTip = "1";
       //else
       //    Button_EvenimenteOrganizate.ToolTip = "0";

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
            LabelMesajEroare.Text = "Nu merge buton evenimente organizate " + ex.Message;
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
                    //Table_Evenimente.Columns.Add("ID", typeof(int));
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

                       // NewRow[0] = InfoEvenimente.GetInt32(0);
                        NewRow[0] = InfoEvenimente.GetString(1);
                        NewRow[1] = InfoEvenimente.GetDateTime(2).ToString("dd MM yyyy");
                        //LabelMesajEroare.Text += " Opa ";
                        NewRow[2] = InfoEvenimente.GetString(3);
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
                    Button_EvenimenteOrganizate.ToolTip = "1";
                    int i = 0;
                    LinkButton accepta = new LinkButton();
                    accepta.ID = "a" + idFiecareEveniment[index_eveniment].ToString(); //fiecare buton are id-ul evenimentului pe care il refera
                    accepta.Text = "Renunta";
                    accepta.Click += new EventHandler(retragereCaOrganizator);
                    row.Cells[i].Controls.Add(accepta);
                    i++;
                    LinkButton linkCatreEveniment = new LinkButton();
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



    protected void ButtonEvenimenteOrganizateAscunde_Click(object sender, EventArgs e)
    {
        Button_EvenimenteOrganizate.ToolTip = "0";
        GridView_listaEvenimenteOrganizate.Visible = false;
    }

    protected void evenimente_participa()
    {
        SqlCommand cmd = new SqlCommand();
        String query = "select * from participa,evenimente where id_utilizator=" + Convert.ToInt32(Session["id"])+" and id_eveniment=id";
        sqlConnection1.Open();
        cmd.CommandText = query;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;

        DataTable Table_Evenimente = new DataTable();

        if (Table_Evenimente.Columns.Count == 0)
        {
            Table_Evenimente.Columns.Add("Titlu", typeof(string));
            Table_Evenimente.Columns.Add("Locatie", typeof(string));
            Table_Evenimente.Columns.Add("Data", typeof(string));

        }

        List<int> idevenimetfeatured = new List<int>();
        List<string> numeFiecareEveniment = new List<string>();

        using (SqlDataReader evenimetfeatured = cmd.ExecuteReader())
        {
            while (evenimetfeatured.Read())
            {
                idevenimetfeatured.Add(evenimetfeatured.GetInt32(evenimetfeatured.GetOrdinal("id_eveniment")));
            }
            evenimetfeatured.Close();
        }

        foreach (int idEvent in idevenimetfeatured)
        {
            //LabelMesajEroare.Text += idEvent + "s#";
            query = "SELECT id,nume,data_inceperii,oras from evenimente where id = " + idEvent;
            cmd.CommandText = query;
            SqlDataReader InfoEvenimente = cmd.ExecuteReader();

            while (InfoEvenimente.Read())
            {

                DataRow NewRow = Table_Evenimente.NewRow(); //un nou rand!!!

                NewRow[0] = InfoEvenimente.GetString(1);
                //  NewRow[1] = InfoEvenimente.GetString(1);
                numeFiecareEveniment.Add(InfoEvenimente.GetString(1));

                NewRow[2] = InfoEvenimente.GetDateTime(2).ToString("dd MM yyyy");
                //LabelMesajEroare.Text += " Opa ";
                NewRow[1] = InfoEvenimente.GetString(3);
                //LabelMesajEroare.Text += " Opa ";
                Table_Evenimente.Rows.Add(NewRow);

            }
            InfoEvenimente.Close();
        }
        GridViewParticipa.DataSource = Table_Evenimente;
        GridViewParticipa.DataBind();

        int index_eveniment = 0; //pentru fiecare id de eveniment
        foreach (GridViewRow row in GridViewParticipa.Rows)
        {
            int i = 0;
            LinkButton spre = new LinkButton();
            spre.ID = "LinkEveniment" + idevenimetfeatured[index_eveniment].ToString(); //fiecare buton are id-ul evenimentului pe care il refera
            spre.Text = numeFiecareEveniment[index_eveniment].ToString();
            spre.Click += new EventHandler(butoneveniment);
            row.Cells[i].Controls.Add(spre);
            //i++;

            //Label refuza = new Label();
            //refuza.ID = "r" + idevenimetfeatured[index_eveniment].ToString();
            //refuza.Text = row.Cells[2].Text.ToString();
            //row.Cells[i].Controls.Add(refuza);
            //i++;
            //Label nume = new Label();
            //nume.ID = "n" + idevenimetfeatured[index_eveniment].ToString();
            //nume.Text = row.Cells[1].Text.ToString();
            //nume.ToolTip = "toEvent";
            //row.Cells[i].Controls.Add(nume);
            index_eveniment++;
        }
        sqlConnection1.Close();

    }
   
}