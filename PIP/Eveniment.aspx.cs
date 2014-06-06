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
//----------------------------------------------------------------------------------------
using Subgurim.Controles;
using Subgurim.Controles.GoogleChartIconMaker;
using System.Drawing;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

public partial class Eveniment : System.Web.UI.Page
{
    SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Domanitos\Downloads\PIPv36\PIP\App_Data\PIP.mdf';Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
    SqlConnection sqlConnection2 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Domanitos\Downloads\PIPv36\PIP\App_Data\PIP.mdf';Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");    
 
    bool Proprietar = false;
    bool Organizator = false;

    override protected void OnInit(EventArgs e)
    {
        InitializeComponent();
        base.OnInit(e);
    }

    private void InitializeComponent()
    {
        this.Load += new System.EventHandler(this.Page_Load);
        this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ButtonStergeEveniment.Visible = false;
        Label11sterge.Visible = false;
        ButtonStergeEvenimentOK.Visible = false;
        ButtonStergeEvenimentNO.Visible = false;
        PanelParticipanti.Visible = false;

        MesajRaspuns.Text = "";
        LabelEroareRol.Text = "";
        lblInfo.Text = "";
        this.ButtonAdaugaRol2.Click += new System.EventHandler(this.ButtonAdaugaRol2_Click);
        this.ButtonAfisareParticipanti.Click += new System.EventHandler(this.ButtonAfiseazaParticipanti_Click);
        AfiseazaParticipanti();
        MesajRaspuns.Text = "";
        cmdSend.Visible = false;
        lblFile.Visible = false;
        filMyFile.Visible = false;
        numeEveniment.Visible = false;
        descriereEveniment.Visible = false;
        ziuaEveniment.Visible = false;
        lista_luni.Visible = false;
        anEveniment.Visible = false;
        lunaEveniment.Visible = false;
        oraEveniment.Visible = false;
        minutEveniment.Visible = false;
        etichetaEveniment.Visible = false;
        orasEveniment.Visible = false;
        judetEveniment.Visible = false;
        taraEveniment.Visible = false;
        ButtonEditeazaNume.Visible = false;
       ButtonEditeazaEtichete.Visible = false;
        ButtonEditeazaDescriere.Visible = false;
        ButtonEditeazaData.Visible = false;
        ButtonEditeazaOras.Visible = false;
        ButtonEditeazaJudet.Visible = false;
        ButtonEditeazaTara.Visible = false;

        ButtonAdaugaRol2.Visible = false;
        LabelDenumire.Visible = false;
        LabelDescriere.Visible = false;
        LabelMaxUsers.Visible = false;
        DenumireRol.Visible = false;
        DescriereRol.Visible = false;
        MaxUsers.Visible = false;
        ButtonSubmitRol2.Visible = false;
        
        ButtonSubmitNume.Visible = false;
        ButtonSubmitEtichete.Visible = false;
        ButtonSubmitDescriere.Visible = false;
        ButtonSubmitData.Visible = false;
        ButtonSubmitOras.Visible = false;
        ButtonSubmitJudet.Visible = false;
        ButtonSubmitTara.Visible = false;
        ButtonInvitaOrganizatori.Visible = false;

        textbox_latitudine.Visible = false;
        textbox_longitudine.Visible = false;
        ButtonIntroduceDateLocatie.Visible = false;

        VerificareProprietar(sender, e);

        if (Session["nume"] == null)
            ButtonComentariu.Visible = false;
        else ButtonComentariu.Visible = true;

        if (Proprietar)
        {
            ButtonAdaugaRol2.Visible = true;
            ButtonEditeazaNume.Visible = true;
            ButtonEditeazaEtichete.Visible = true;
            ButtonEditeazaDescriere.Visible = true;
            ButtonEditeazaData.Visible = true;
            ButtonEditeazaOras.Visible = true;
            ButtonEditeazaJudet.Visible = true;
            ButtonEditeazaTara.Visible = true;
            ButtonInvitaOrganizatori.Visible = true;
            cmdSend.Visible = true;
            lblFile.Visible = true;
            filMyFile.Visible = true;
            ButtonStergeEveniment.Visible = true;

            editareLocatie();
            textbox_latitudine.Visible = true;
            textbox_longitudine.Visible = true;
            ButtonIntroduceDateLocatie.Visible = true;

           // ButtonLocatie.Visible = true;
        }
        else
        {

            VerificareOrganizator(sender, e);
            // MesajRaspuns.Text += "* "+Session["idEvenimentSelectat"]+Session["Id"];
            if (Organizator)
            {
                //  MesajRaspuns.Text += "lala";
                ButtonAdaugaRol2.Visible = true;
                ButtonEditeazaNume.Visible = true;
                ButtonEditeazaEtichete.Visible = true;
                ButtonEditeazaDescriere.Visible = true;
                ButtonEditeazaData.Visible = true;
                ButtonEditeazaOras.Visible = true;
                ButtonEditeazaJudet.Visible = true;
                ButtonEditeazaTara.Visible = true;
                ButtonInvitaOrganizatori.Visible = false;
                cmdSend.Visible = true;
                lblFile.Visible = true;
                filMyFile.Visible = true;
            }
        }

        UmplereInformatii(sender, e);
        ////----------------------------------------------------------------------
        afisareHarta();
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        Afiseaza_Fisiere();
        Afiseaza_Roluri();
        Afiseaza_NumarParticipanti();
        if (PanelForum.Visible == true)
            ButtonForum_Click(sender, e);
    }

    protected void ButtonIntrebareSterge_Click(object sender, EventArgs e)
    {
        ButtonStergeEveniment.Visible = false;
        Label11sterge.Visible = true;
        ButtonStergeEvenimentOK.Visible = true;
        ButtonStergeEvenimentNO.Visible = true;
    }

    protected void ButtonStergeEveniment_Click(object sender, EventArgs e)
    {
        string nume;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select document from documente where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                nume = SRD.GetString(SRD.GetOrdinal("document")).ToString();
                File.Delete(Server.MapPath(nume));
            }
        }
        sqlConnection1.Close();
        SqlCommand cmd2 = new SqlCommand();
        cmd2.Connection = sqlConnection1;
        cmd2.CommandType = CommandType.Text;
        cmd2.CommandText = "delete from documente where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        cmd2.ExecuteNonQuery();
        sqlConnection1.Close();

        SqlCommand cmd3 = new SqlCommand();
        cmd3.Connection = sqlConnection1;
        cmd3.CommandType = CommandType.Text;
        cmd3.CommandText = "delete from organizeaza where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        cmd3.ExecuteNonQuery();
        sqlConnection1.Close();

        SqlCommand cmd4 = new SqlCommand();
        cmd4.Connection = sqlConnection1;
        cmd4.CommandType = CommandType.Text;
        cmd4.CommandText = "delete from participa where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        cmd4.ExecuteNonQuery();
        sqlConnection1.Close();

        SqlCommand cmd5 = new SqlCommand();
        cmd5.Connection = sqlConnection1;
        cmd5.CommandType = CommandType.Text;
        cmd5.CommandText = "delete from replici where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        cmd5.ExecuteNonQuery();
        sqlConnection1.Close();

        SqlCommand cmd6 = new SqlCommand();
        cmd6.Connection = sqlConnection1;
        cmd6.CommandType = CommandType.Text;
        cmd6.CommandText = "delete from roluri where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        cmd6.ExecuteNonQuery();
        sqlConnection1.Close();

        SqlCommand cmd7 = new SqlCommand();
        cmd7.Connection = sqlConnection1;
        cmd7.CommandType = CommandType.Text;
        cmd7.CommandText = "delete from evenimente where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        cmd7.ExecuteNonQuery();
        sqlConnection1.Close();

        Response.Redirect("homepage.aspx");
    }

    protected void ButtonIntrebareReturn_Click(object sender, EventArgs e)
    {
        ButtonStergeEveniment.Visible = true;
        Label11sterge.Visible = false;
        ButtonStergeEvenimentOK.Visible = false;
        ButtonStergeEvenimentNO.Visible = false;
    }

    protected void UmplereInformatii(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        sqlConnection1.Open();
        cmd.CommandText = "select nume+' '+prenume from utilizator where id=(select id_proprietar from evenimente where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ");";
        cmd.CommandType = CommandType.Text;
        proprietarLabel.Text = cmd.ExecuteScalar().ToString();
        cmd.CommandText = "select * from evenimente where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;

        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                numeEvenimentLabel.Text = SRD.GetString(SRD.GetOrdinal("nume"));

                if (Convert.IsDBNull(SRD["data_inceperii"]))
                {
                    ziuaEvenimentLabel.Text = "";
                    anEvenimentLabel.Text = "";
                    lunaEvenimentLabel.Text = "";
                    oraEvenimentLabel.Text = "";
                    minutEvenimentLabel.Text = "";
                }
                else
                {
                    ziuaEvenimentLabel.Text = (SRD.GetDateTime(SRD.GetOrdinal("data_inceperii")).Day).ToString();
                    anEvenimentLabel.Text = (SRD.GetDateTime(SRD.GetOrdinal("data_inceperii")).Year).ToString();
                    lunaEvenimentLabel.Text = (SRD.GetDateTime(SRD.GetOrdinal("data_inceperii")).Month).ToString();
                    oraEvenimentLabel.Text = (SRD.GetDateTime(SRD.GetOrdinal("data_inceperii")).Hour).ToString();
                    minutEvenimentLabel.Text = (SRD.GetDateTime(SRD.GetOrdinal("data_inceperii")).Minute).ToString();

                }

              //  etichetaEvenimentLabel.Text = SRD.GetString(SRD.GetOrdinal("etichete"));

                if (Convert.IsDBNull(SRD["descriere"]))
                {
                    descriereEvenimentLabel.Text = "Nu este stabilita.";
                }
                else
                {
                    descriereEvenimentLabel.Text = SRD.GetString(SRD.GetOrdinal("descriere"));
                }

                if (Convert.IsDBNull(SRD["oras"]))
                {
                    orasEvenimentLabel.Text = "Nu este stabilit.";
                }
                else
                {
                    orasEvenimentLabel.Text = SRD.GetString(SRD.GetOrdinal("oras"));
                }

                if (Convert.IsDBNull(SRD["judet"]))
                {
                    judetEvenimentLabel.Text = "Nu este stabilit.";
                }
                else
                {
                    judetEvenimentLabel.Text = SRD.GetString(SRD.GetOrdinal("judet"));
                }

                if (Convert.IsDBNull(SRD["tara"]))
                {
                    taraEvenimentLabel.Text = "Nu este stabilit.";
                }
                else
                {
                    taraEvenimentLabel.Text = SRD.GetString(SRD.GetOrdinal("tara"));
                }
            }
        }

        sqlConnection1.Close();
    }
    //----------------------------------------------------------------------------------
    private void afisareHarta()
    { /*
        double lat = 45.662943, longi = 25.612564;
        SqlCommand cmd = new SqlCommand();
        try
        {
            sqlConnection1.Open();
            cmd.Connection = sqlConnection1;

            cmd.CommandText = "select count(*) from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
            cmd.CommandType = CommandType.Text;

            if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
            {
                cmd.CommandText = "select latitudine,longitudine from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
                cmd.CommandType = CommandType.Text;
                using (SqlDataReader SRD = cmd.ExecuteReader())
                {
                    while (SRD.Read())
                    {
                       lat = SRD.GetDouble(SRD.GetOrdinal("latitudine"));
                       longi = SRD.GetDouble(SRD.GetOrdinal("longitudine"));
                    }
                }
            }         
           
        }
        catch (Exception)
        {

            MesajRaspuns.Text = "eroareeeeeeeeeeeee";
        }
        sqlConnection1.Close();

         GMap1.Key = "gmapkey_localhost";

        GMarker marker = new GMarker(new GLatLng(39.5, -3.2));
        GInfoWindow window = new GInfoWindow(marker, "<center><b>GoogleMaps.Subgurim.NET</b></center>", true);

        GMap1.addInfoWindow(window);
    */
        /*
        GMap1.setCenter(new GLatLng(41, 3), 3);
        GMap1.Add(new GControl(GControl.preBuilt.LargeMapControl));

        GMarker m1 = new GMarker(new GLatLng(41, 3));

        MarkerManager mManager = new MarkerManager();

        mManager.Add(m1, 2);

        List<GMarker> mks = new List<GMarker>();

        List<GInfoWindow> iws = new List<GInfoWindow>();

        Random r = new Random();
        double ir1, ir2;
        GMarker mkr;
        for (int i = 0; i < 10; i++)
        {
            ir1 = (double)r.Next(40) / 10.0 - 2.0;
            ir2 = (double)r.Next(40) / 10.0 - 2.0;

            mkr = new GMarker(m1.point + new GLatLng(ir1, ir2));
            mks.Add(mkr);

            GMap1.Add(new GListener(mkr.ID, GListener.Event.click, "function(){alert('" + i + "');}"));
        }

        for (int i = 0; i < 5; i++)
        {
             ir1 = (double)r.Next(40) / 20.0 - 1;
            ir2 = (double)r.Next(40) / 20.0 - 1;

            mkr = new GMarker(m1.point + new GLatLng(ir1, ir2));
            GInfoWindow window = new GInfoWindow(mkr, i.ToString());
            iws.Add(window);
        }

        mManager.Add(mks, 6, 8);
        mManager.Add(iws, 7, 8);

        GMap1.markerManager = mManager; 
        */
        string skey = "AIzaSyD2MtIvFfWaDxXGXssGyo0wOTWnYJcbddc";
         GMap1.Key = skey;
        GMap1.addControl(new GControl(GControl.preBuilt.GOverviewMapControl));
        GMap1.addControl(new GControl(GControl.preBuilt.LargeMapControl));

        double lat = 45.662943, longi = 25.612564;
        SqlCommand cmd = new SqlCommand();
        try
        {
            sqlConnection1.Open();
            cmd.Connection = sqlConnection1;

            cmd.CommandText = "select count(*) from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
            cmd.CommandType = CommandType.Text;

            if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
            {

                cmd.CommandText = "select latitudine,longitutine from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader SRD = cmd.ExecuteReader())
                {
                    while (SRD.Read())
                    {
                        //  MesajRaspuns.Text = "2";
                        lat = (SRD.GetDouble(0));
                        longi = (SRD.GetDouble(1));
                        //  MesajRaspuns.Text += "3";
                    }
                }

                GMarker marker = new GMarker(new GLatLng(lat, longi));

                //creating pushpin window with content
                GInfoWindow window = new GInfoWindow(marker, "<center><b></b></center>", true);

                //creating new marker for second location
                GMarker marker1 = new GMarker(new GLatLng(16.3, 79.4));

                //creating second pushpin window
                //  GInfoWindow windo1 = new GInfoWindow(marker1, "<center><b>Loyapalli, India </b></center>", true);

                //adding windows in GMap control
                GMap1.addInfoWindow(window);
                // GMap1.addInfoWindow(windo1);
            }
            else
            {
                GMap1.Visible = false;
            }

        }
        catch (Exception ex)
        {

            MesajRaspuns.Text += "eroareeeeeeeeeeeee in afisare harta " + ex.Message;
        }
        sqlConnection1.Close();

        //creating marker with latitude and logitude
      
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

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

    private void VerificareOrganizator(object sender, EventArgs e)
    {
        if (Session["nume"] != null)
        {
            int Id_curent = Convert.ToInt32(Session["Id"]);
            int Id_organizator = 0;
            // MesajRaspuns.Text += "&";
            try
            {
                SqlCommand cmd = new SqlCommand();
                sqlConnection1.Open();
                cmd.Connection = sqlConnection1;
                //   MesajRaspuns.Text += "1";
                cmd.CommandText = "select id_organizator from organizeaza where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + " and id_organizator = " + Session["Id"] + " and aprobat=1;";
                cmd.CommandType = CommandType.Text;
                using (SqlDataReader SRD = cmd.ExecuteReader())
                {
                    while (SRD.Read())
                    {
                        //      MesajRaspuns.Text += "2-";
                        Id_organizator = SRD.GetInt32(SRD.GetOrdinal("id_organizator"));
                        //   MesajRaspuns.Text += "3-";
                    }
                }
            }
            catch (Exception ex)
            {
                MesajRaspuns.Text += " " + ex.Message;
            }
            sqlConnection1.Close();
            //    MesajRaspuns.Text += Id_organizator + " = " + Id_curent;
            if (Id_organizator == Id_curent)
                Organizator = true;
        }
    }


    protected void ButtonEditeazaNume_Click(object Sender, EventArgs e)
    {
        numeEveniment.Visible = true;
        ButtonSubmitNume.Visible = true;
        ButtonEditeazaNume.Visible = false;
        numeEveniment.Text = numeEvenimentLabel.Text;
    }

    protected void ButtonEditeazaData_Click(object Sender, EventArgs e)
    {
        ziuaEveniment.Visible = true;
        lista_luni.Visible = true;
        anEveniment.Visible = true;
        oraEveniment.Visible = true;
        minutEveniment.Visible = true;
        ButtonSubmitData.Visible = true;
        ButtonEditeazaData.Visible = false;
        ziuaEveniment.Text = ziuaEvenimentLabel.Text;
        anEveniment.Text = anEvenimentLabel.Text;
        oraEveniment.Text = oraEvenimentLabel.Text;
        minutEveniment.Text = minutEvenimentLabel.Text;

    }

    protected void ButtonEditeazaDescriere_Click(object Sender, EventArgs e)
    {
        descriereEveniment.Visible = true;
        ButtonSubmitDescriere.Visible = true;
        ButtonEditeazaDescriere.Visible = false;
        descriereEveniment.Text = descriereEvenimentLabel.Text;
    }

    protected void ButtonEditeazaEtichete_Click(object Sender, EventArgs e)
    {
        etichetaEveniment.Visible = true;
        ButtonSubmitEtichete.Visible = true;
        ButtonEditeazaEtichete.Visible = false;
        etichetaEveniment.Text = etichetaEvenimentLabel.Text;
    }

    protected void ButtonEditeazaOras_Click(object Sender, EventArgs e)
    {
        orasEveniment.Visible = true;
        ButtonSubmitOras.Visible = true;
        ButtonEditeazaOras.Visible = false;
        orasEveniment.Text = orasEvenimentLabel.Text;
    }

    protected void ButtonEditeazaJudet_Click(object Sender, EventArgs e)
    {
        judetEveniment.Visible = true;
        ButtonSubmitJudet.Visible = true;
        ButtonEditeazaJudet.Visible = false;
        judetEveniment.Text = judetEvenimentLabel.Text;
    }

    protected void ButtonEditeazaTara_Click(object Sender, EventArgs e)
    {
        taraEveniment.Visible = true;
        ButtonSubmitTara.Visible = true;
        ButtonEditeazaTara.Visible = false;
        taraEveniment.Text = taraEvenimentLabel.Text;
    }

    protected void ButtonSubmitNume_Click(object Sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "update evenimente set nume='" + numeEveniment.Text + "' where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;
        if (numeEveniment.Text != "")
        {
            cmd.ExecuteNonQuery();
            numeEvenimentLabel.Text = numeEveniment.Text;
            MesajRaspuns.Text = "Editare efectuata cu succes!";
        }
        else MesajRaspuns.Text = "Eroare!";
        sqlConnection1.Close();
    }

    protected void ButtonSubmitData_Click(object Sender, EventArgs e)
    {
        int zi = DateTime.Now.Day, luna = DateTime.Now.Month, an = DateTime.Now.Year, ora = DateTime.Now.Hour, minut = DateTime.Now.Minute;

        if (Int32.TryParse(oraEveniment.Text, out ora) == false)
        {
            MesajRaspuns.Text = "Introduceti o ora numar intreg";
            return;
        }
        else
        {
            if (ora > 23 || ora < 0)
            {
                MesajRaspuns.Text = "Introduceti o ora numar intreg intre 0 si 23 ";
                return;
            }
        }

        if (Int32.TryParse(minutEveniment.Text, out minut) == false)
        {
            MesajRaspuns.Text = "Introduceti un minut numar intreg";
            return;
        }
        else
        {
            if (ora > 59 || ora < 0)
            {
                MesajRaspuns.Text = "Introduceti un minut numar intreg intre 0 si 59 ";
                return;
            }
        }

        if (Int32.TryParse(anEveniment.Text, out an) == false)
        {
            MesajRaspuns.Text = "Introduceti un an numar intreg";
            return;
        }
        else
        {
            if (an < DateTime.Now.Year || an > 3000)
            {
                MesajRaspuns.Text = "Introduceti un an numar intreg intre " + DateTime.Now.Year + " si 3000 ";
                return;
            }
        }

        luna = lista_luni.SelectedIndex + 1;
        bool paritateLuna = false; // false e luna impara, true e luna para

        if (luna % 2 == 0)
        {
            paritateLuna = true;
        }


        if (Int32.TryParse(ziuaEveniment.Text, out zi) == false)
        {
            MesajRaspuns.Text = "Introduceti o zi numar intreg";
            return;
        }
        else
        {
            if (paritateLuna == true)
            {
                if (luna == 2)
                { // daca e februarie
                    if ((an % 100 == 0) && (an % 400 != 0))
                    { // verific daca e an bisect
                        if (zi < 1 || zi > 29)
                        {
                            MesajRaspuns.Text = "Introduceti o zi numar intreg intre 1 si 29 ";
                            return;
                        }
                    }
                    else
                        if (an % 4 == 0)
                        {
                            if (zi < 1 || zi > 29)
                            {
                                MesajRaspuns.Text = "Introduceti o zi numar intreg intre 1 si 29 ";
                                return;
                            }
                        }
                        else
                        {
                            if (zi < 1 || zi > 28)
                            {
                                MesajRaspuns.Text = "Introduceti o zi numar intreg intre 1 si 28 ";
                                return;
                            }
                        }
                }
                if (zi < 1 || zi > 30)
                {
                    MesajRaspuns.Text = "Introduceti o zi numar intreg intre 1 si 30 ";
                    return;
                }
            }
            else
                if (zi < 1 || zi > 31)
                {
                    MesajRaspuns.Text = "Introduceti o zi numar intreg intre 1 si 31 ";
                    return;
                }
        }

        DateTime data = DateTime.Now;
        string dataString = luna + "." + zi + "." + an + " " + ora + ":" + minut;
        MesajRaspuns.Text = dataString;
        try
        {
            data = Convert.ToDateTime(dataString);
        }
        catch (Exception ex)
        {
            MesajRaspuns.Text += ex.Message;
        }

        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "update evenimente set data_inceperii='" + data + "' where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;
        try
        {
            cmd.ExecuteNonQuery();
            ziuaEvenimentLabel.Text = zi.ToString();
            lunaEvenimentLabel.Text = luna.ToString();
            anEvenimentLabel.Text = an.ToString();
            oraEvenimentLabel.Text = ora.ToString();
            minutEvenimentLabel.Text = minut.ToString();
            MesajRaspuns.Text = "Editare efectuata cu succes!";
        }
        catch (Exception ex)
        {
            MesajRaspuns.Text = ex.Message;
        }
        sqlConnection1.Close();

    }

    protected void ButtonSubmitDescriere_Click(object Sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "update evenimente set descriere='" + descriereEveniment.Text + "' where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;
        if (descriereEveniment.Text != "")
        {
            cmd.ExecuteNonQuery();
            descriereEvenimentLabel.Text = descriereEveniment.Text;
            MesajRaspuns.Text = "Editare efectuata cu succes!";
        }
        else
            MesajRaspuns.Text = "Eroare!";
        sqlConnection1.Close();
    }

    protected void ButtonSubmitEtichete_Click(object Sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "update evenimente set etichete='" + etichetaEveniment.Text + "' where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;
        if (etichetaEveniment.Text != "")
        {
            cmd.ExecuteNonQuery();
            etichetaEvenimentLabel.Text = etichetaEveniment.Text;
            MesajRaspuns.Text = "Editare efectuata cu succes!";
        }
        else MesajRaspuns.Text = "Eroare!";
        sqlConnection1.Close();
    }

    protected void ButtonSubmitOras_Click(object Sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "update evenimente set oras='" + orasEveniment.Text + "' where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;
        if (orasEveniment.Text != "")
        {
            cmd.ExecuteNonQuery();
            orasEvenimentLabel.Text = orasEveniment.Text;
            MesajRaspuns.Text = "Editare efectuata cu succes!";
        }
        else MesajRaspuns.Text = "Eroare!";
        sqlConnection1.Close();
    }

    protected void ButtonSubmitJudet_Click(object Sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "update evenimente set judet='" + judetEveniment.Text + "' where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;
        if (judetEveniment.Text != "")
        {
            cmd.ExecuteNonQuery();
            judetEvenimentLabel.Text = judetEveniment.Text;
            MesajRaspuns.Text = "Editare efectuata cu succes!";
        }
        else MesajRaspuns.Text = "Eroare!";
        sqlConnection1.Close();
    }

    protected void ButtonSubmitTara_Click(object Sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "update evenimente set tara='" + taraEveniment.Text + "' where id=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;
        if (taraEveniment.Text != "")
        {
            cmd.ExecuteNonQuery();
            taraEvenimentLabel.Text = taraEveniment.Text;
            MesajRaspuns.Text = "Editare efectuata cu succes!";
        }
        else MesajRaspuns.Text = "Eroare!";
        sqlConnection1.Close();
    }


    protected void ButonInvitaOrganizatori_Click(object sender, EventArgs e)
    {
        Response.Redirect("OrganizatoriEveniment.aspx");
    }

    protected void ButtonInformatii_Click(object sender, EventArgs e)
    {
        PanelInformatii.Visible = true;
        PanelInscriere.Visible = false;
        PanelResurse.Visible = false;
        PanelForum.Visible = false;
    }
    protected void ButtonInscriere_Click(object sender, EventArgs e)
    {
        PanelInformatii.Visible = false;
        PanelInscriere.Visible = true;
        PanelResurse.Visible = false;
        PanelForum.Visible = false;
    }
    protected void ButtonResurse_Click(object sender, EventArgs e)
    {
        PanelInformatii.Visible = false;
        PanelInscriere.Visible = false;
        PanelResurse.Visible = true;
        PanelForum.Visible = false;
    }
    protected void ButtonForum_Click(object sender, EventArgs e)
    {
        //   if (PanelForum.Visible == true) PanelComentarii.Controls.Clear();
        //   if (PanelComentarii.Controls.Count > 0) return;
        PanelInformatii.Visible = false;
        PanelInscriere.Visible = false;
        PanelResurse.Visible = false;
        PanelForum.Visible = true;


        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;
        cmd.CommandText = "select * from replici where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + " and id_parinte=-1 order by data desc;";
        cmd.CommandType = CommandType.Text;

        if ((PanelComentarii.FindControl("kid")) == null)
        {
            Label k = new Label();
            k.Text = "Comentarii:";
            k.ID = "kid";
            PanelComentarii.Controls.Add(k);
            PanelComentarii.Controls.Add(new LiteralControl("</br>"));
            PanelComentarii.Controls.Add(new LiteralControl("</br>"));
        }

        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                TextBox t2 = new TextBox();
                t2.ID = "Comentariu" + SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
                if ((PanelComentarii.FindControl(t2.ID)) == null)
                {

                    Label l = new Label();
                    Label l2 = new Label();
                    TextBox t = new TextBox();
                    Button btn = new Button();
                    Button btn2 = new Button();

                    l.Text = "Autor: " + SRD.GetString(SRD.GetOrdinal("autor")).ToString();
                    PanelComentarii.Controls.Add(l);
                    PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                    l2.Text = "Data: " + SRD.GetDateTime(SRD.GetOrdinal("data")).ToString();
                    PanelComentarii.Controls.Add(l2);
                    PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                    t.ID = "Comentariu" + SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
                    t.Text = SRD.GetString(SRD.GetOrdinal("text"));
                    t.Enabled = false;
                    t.BorderWidth = 0;
                    t.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    PanelComentarii.Controls.Add(t);
                    PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                    btn.ID = "Sterge" + SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
                    btn.Text = "Sterge";
                    btn.CssClass = "btn btn-danger";
                    btn.Click += new EventHandler(ComentariuSterge);
                    if (Session["nume"] == null)
                        btn.Visible = false;
                    else if (SRD.GetString(SRD.GetOrdinal("autor")).ToString().ToUpper() != Session["nume"].ToString().ToUpper())
                        btn.Visible = false;
                    if (Proprietar || Organizator)
                        btn.Visible = true;
                    PanelComentarii.Controls.Add(btn);

                    btn2.ID = "Raspunde" + SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
                    btn2.Text = "Raspunde";
                    btn2.CssClass = "btn btn-warning";
                    btn2.ToolTip = SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
                    if (Session["nume"] == null)
                        btn2.Visible = false;
                    btn2.Click += new EventHandler(ComentariuRaspunde);
                    PanelComentarii.Controls.Add(btn2);

                    PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                    if (!(Session["idtata"] == null))
                        if (Session["idtata"].ToString() == SRD.GetInt32(SRD.GetOrdinal("id")).ToString())
                        {
                            TextBox rsp = new TextBox();
                            rsp.ID = "rsp2";
                            rsp.Columns = 100;
                            rsp.MaxLength = 500;
                            rsp.Rows = 5;
                            rsp.TextMode = TextBoxMode.MultiLine;
                            PanelComentarii.Controls.Add(rsp);
                            PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                            Button btn3 = new Button();
                            btn3.ID = "rsp";
                            btn3.Text = "Posteaza";
                            btn3.ToolTip = SRD.GetInt32(SRD.GetOrdinal("id")).ToString();
                            btn3.Click += new EventHandler(ComentariuRaspundePosteaza);
                            PanelComentarii.Controls.Add(btn3);
                            PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                        }

                    PanelForum.Controls.Add(new LiteralControl("</br>"));

                    sqlConnection2.Open();
                    SqlCommand cmd5 = new SqlCommand();
                    cmd5.Connection = sqlConnection2;
                    cmd5.CommandText = "select * from replici where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + " and id_parinte=" + SRD.GetInt32(SRD.GetOrdinal("id")) + " order by data desc;";
                    cmd5.CommandType = CommandType.Text;

                    //////////////////////////////////////////////////////


                    using (SqlDataReader SRD5 = cmd5.ExecuteReader())
                    {
                        while (SRD5.Read())
                        {
                            TextBox t25 = new TextBox();
                            t25.ID = "Comentariu" + SRD5.GetInt32(SRD5.GetOrdinal("id")).ToString();
                            if ((PanelComentarii.FindControl(t25.ID)) == null)
                            {

                                Label l5 = new Label();
                                Label l25 = new Label();
                                TextBox t5 = new TextBox();
                                Label l255 = new Label();
                                Button btn5 = new Button();

                                l5.Style.Add("padding-left", "100px");
                                l25.Style.Add("padding-left", "100px");
                                t5.Style.Add("padding-left", "100px");
                                l255.Style.Add("padding-left", "100px");


                                l5.Text = "Autor: " + SRD5.GetString(SRD5.GetOrdinal("autor")).ToString();
                                PanelComentarii.Controls.Add(l5);
                                PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                                l25.Text = "Data: " + SRD5.GetDateTime(SRD5.GetOrdinal("data")).ToString();
                                PanelComentarii.Controls.Add(l25);
                                PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                                t5.ID = "Comentariu" + SRD5.GetInt32(SRD5.GetOrdinal("id")).ToString();
                                t5.Text = SRD5.GetString(SRD5.GetOrdinal("text"));
                                t5.Enabled = false;
                                t5.BorderWidth = 0;
                                t5.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                                PanelComentarii.Controls.Add(t5);
                                PanelComentarii.Controls.Add(new LiteralControl("</br>"));

                                l255.Text = "";
                                PanelComentarii.Controls.Add(l255);
                                btn5.ID = "Sterge" + SRD5.GetInt32(SRD5.GetOrdinal("id")).ToString();
                                btn5.Text = "Sterge";
                                if (Session["nume"] == null)
                                    btn5.Visible = false;
                                if (Session["nume"] != null)
                                    if (SRD5.GetString(SRD5.GetOrdinal("autor")).ToString().ToUpper() != Session["nume"].ToString().ToUpper())
                                        btn5.Visible = false;
                                if (Organizator || Proprietar)
                                    btn5.Visible = true;
                                btn5.CssClass = "btn btn-danger";
                                btn5.Click += new EventHandler(ComentariuSterge);

                                PanelComentarii.Controls.Add(btn5);
                                PanelComentarii.Controls.Add(new LiteralControl("</br>"));


                                PanelForum.Controls.Add(new LiteralControl("</br>"));


                                //     PanelForum.Controls.Add(new LiteralControl("<p></p>"));

                            }
                            else break;
                        }
                    }


                    sqlConnection2.Close();

                    //     PanelForum.Controls.Add(new LiteralControl("<p></p>"));

                }
                else break;
            }
        }
        sqlConnection1.Close();

    }

    protected void ButtonAdaugaRol2_Click(object sender, EventArgs e)
    {
        LabelDenumire.Visible = true;
        LabelDescriere.Visible = true;
        LabelMaxUsers.Visible = true;
        DenumireRol.Visible = true;
        DescriereRol.Visible = true;
        MaxUsers.Visible = true;
        ButtonSubmitRol2.Visible = true;
        ButtonAdaugaRol2.Visible = false;
        div_rol.Visible = true;
    }

    protected void ButtonSubmitRol2_Click(object sender, EventArgs e)
    {
        string den = "", desc = "", max_u = "";
        int max = 0;
        den = DenumireRol.Text;
        desc = DescriereRol.Text;
        if (MaxUsers.Text != "")
        {
            try
            {
                max_u = MaxUsers.Text;
                max = Convert.ToInt32(max_u);
            }
            catch (Exception ex)
            {
                LabelEroareRol.Text = "Valoare utilizatori maximi nu este valida!";
            }
        }
        if (den == "" || desc == "")
        {
            LabelEroareRol.Text = "Campurile Denumire si Descriere sunt obligatorii!";
        }
        else
        {
            int nr = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection1;
            cmd.CommandText = "select max(Id) as numar from roluri";
            cmd.CommandType = CommandType.Text;
            sqlConnection1.Open();
            using (SqlDataReader SRD = cmd.ExecuteReader())
            {
                while (SRD.Read())
                {
                    try
                    {
                        nr = SRD.GetInt32(SRD.GetOrdinal("numar"));
                    }
                    catch (Exception ex)
                    {
                        nr = 0;
                    }
                }
            }
            sqlConnection1.Close();
            nr++;
            cmd = new SqlCommand("INSERT INTO roluri(Id, id_eveniment, denumire, descriere, max_participanti) values(" + nr + ", " + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ", '" + den + "','" + desc + "', " + max + ")", sqlConnection1);
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();
            LabelEroareRol.Text = "Rol adaugat cu succes!";
            LabelDenumire.Visible = false;
            LabelDescriere.Visible = false;
            LabelMaxUsers.Visible = false;
            DenumireRol.Visible = false;
            DescriereRol.Visible = false;
            MaxUsers.Visible = false;
            ButtonSubmitRol2.Visible = false;
            ButtonAdaugaRol2.Visible = true;
            Afiseaza_Roluri();
        }

    }

    private void Afiseaza_Roluri()
    {
        PanelRoluri.Controls.Clear();
        int nr = 0, max = 0, ok = 0, rol = 0;

        SqlCommand cmd3 = new SqlCommand();
        cmd3.Connection = sqlConnection1;
        cmd3.CommandType = CommandType.Text;
        cmd3.CommandText = "select count(*) as numar from participa where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + " and id_utilizator=" + Convert.ToInt32(Session["Id"]);
        sqlConnection1.Open();
        using (SqlDataReader SRD = cmd3.ExecuteReader())
        {
            while (SRD.Read())
            {
                ok = SRD.GetInt32(SRD.GetOrdinal("numar"));
            }
        }
        sqlConnection1.Close();

        if (ok != 0)
        {
            SqlCommand cmd4 = new SqlCommand();
            cmd4.Connection = sqlConnection1;
            cmd4.CommandType = CommandType.Text;
            cmd4.CommandText = "select id_rol from participa where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + " and id_utilizator=" + Convert.ToInt32(Session["Id"]);
            sqlConnection1.Open();
            using (SqlDataReader SRD = cmd4.ExecuteReader())
            {
                while (SRD.Read())
                {
                    rol = SRD.GetInt32(SRD.GetOrdinal("id_rol"));
                }
            }
            sqlConnection1.Close();
        }

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select * from roluri where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        PanelRoluri.Controls.Add(new LiteralControl("</br></br>"));
        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                Label l1 = new Label();
                l1.Text = SRD.GetString(SRD.GetOrdinal("denumire")).ToString() + " ";
                PanelRoluri.Controls.Add(l1);

                if (SRD.GetString(SRD.GetOrdinal("descriere")).ToString() != "")
                {
                    Label l3 = new Label();
                    l3.Text = "<br />Descriere: " + SRD.GetString(SRD.GetOrdinal("descriere")).ToString() + " ";
                    PanelRoluri.Controls.Add(l3);
                }

                max = SRD.GetInt32(SRD.GetOrdinal("max_participanti"));

                sqlConnection2.Open();
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = sqlConnection2;
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "select count(*) as numar from participa where id_rol=" + SRD.GetInt32(SRD.GetOrdinal("Id"));
                using (SqlDataReader SRD2 = cmd2.ExecuteReader())
                {
                    while (SRD2.Read())
                    {
                        nr = SRD2.GetInt32(SRD2.GetOrdinal("numar"));
                    }
                }
                sqlConnection2.Close();

                if (Session["Nume"] != null && ok == 0 && nr < max)
                {
                    Button Button1 = new Button();
                    Button1.Text = "Inrolare";
                    Button1.ID = SRD.GetInt32(SRD.GetOrdinal("Id")).ToString();
                    Button1.Click += new EventHandler(Inrolare);
                    Button1.CssClass = "btn btn-success";
                    PanelRoluri.Controls.Add(Button1);
                }
                else
                    if (Session["Nume"] != null && ok == 0 && max == 0)
                    {
                        Button Button1 = new Button();
                        Button1.Text = "Inrolare";
                        Button1.ID = SRD.GetInt32(SRD.GetOrdinal("Id")).ToString();
                        Button1.CssClass = "btn btn-success";
                        Button1.Click += new EventHandler(Inrolare);
                        PanelRoluri.Controls.Add(Button1);
                    }

                if (rol == SRD.GetInt32(SRD.GetOrdinal("Id")))
                {
                    Button Button2 = new Button();
                    Button2.Text = "Refuza";
                    Button2.CssClass = "btn btn-danger";
                    Button2.ID = SRD.GetInt32(SRD.GetOrdinal("Id")).ToString();
                    Button2.Click += new EventHandler(RefuzaInrolare);
                    PanelRoluri.Controls.Add(Button2);
                }

                if ((Proprietar || Organizator) && (SRD.GetString(SRD.GetOrdinal("denumire")).ToString() != "Participant") && nr == 0)
                {
                    Button Button3 = new Button();
                    Button3.Text = "Sterge Rol";
                    Button3.ID = SRD.GetInt32(SRD.GetOrdinal("Id")).ToString() + "a";
                    Button3.CssClass = "btn btn-danger";
                    Button3.Click += new EventHandler(StergeRol);
                    PanelRoluri.Controls.Add(Button3);
                }

                /*if (Proprietar || Organizator)
                {
                    Button Button4 = new Button();
                    Button4.Text = "Participanti";
                    Button4.ID = SRD.GetInt32(SRD.GetOrdinal("Id")).ToString() + "b";
                    Button4.CssClass = "btn btn-warning";
                    Button4.Click += new EventHandler(AfiseazaUtilizatoriRol);
                    PanelRoluri.Controls.Add(Button4);
                }*/

                if (max != 0)
                {
                    Label l2 = new Label();
                    l2.Text = "<br />Deja inrolati: " + nr + "/" + max;
                    PanelRoluri.Controls.Add(l2);
                }
                else
                {
                    Label l2 = new Label();
                    l2.Text = "<br />Deja inrolati: " + nr;
                    PanelRoluri.Controls.Add(l2);
                }
                PanelRoluri.Controls.Add(new LiteralControl("<br /><br />"));
            }
        }
        PanelRoluri.Controls.Add(new LiteralControl("<br /><br />"));
        sqlConnection1.Close();
    }

    private void Inrolare(object sender, EventArgs e)
    {
        Button x = (Button)sender;
        int id = 0;
        id = Convert.ToInt32(x.ID);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "insert into participa(id_utilizator, id_rol, id_eveniment) values(" + Convert.ToInt32(Session["Id"]) + ", " + id + ", " + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ")";
        sqlConnection1.Open();
        cmd.ExecuteNonQuery();
        sqlConnection1.Close();
        Afiseaza_Roluri();
    }

    private void RefuzaInrolare(object sender, EventArgs e)
    {
        Button x = (Button)sender;
        int id = 0;
        id = Convert.ToInt32(x.ID);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "delete from participa where id_utilizator=" + Convert.ToInt32(Session["Id"]) + " and id_rol=" + id + " and id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        cmd.ExecuteNonQuery();
        sqlConnection1.Close();
        Afiseaza_Roluri();
    }

    private void StergeRol(object sender, EventArgs e)
    {
        Button x = (Button)sender;
        int id = 0;
        string temp = x.ID;
        temp = temp.TrimEnd('a');
        id = Convert.ToInt32(temp);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "delete from roluri where Id=" + id;
        sqlConnection1.Open();
        cmd.ExecuteNonQuery();
        sqlConnection1.Close();
        Afiseaza_Roluri();
        LabelEroareRol.Text = "Rol sters cu succes!";
    }

    /*private void AfiseazaUtilizatoriRol(object sender, EventArgs e)
    {
        PanelParticipanti.Visible = true;
        PanelParticipanti.Controls.Clear();
        Button x = (Button)sender;
        int id = 0, id_util = 0;
        string temp = x.ID;
        temp = temp.TrimEnd('b');
        id = Convert.ToInt32(temp);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select * from participa where id_rol=" + id;
        sqlConnection1.Open();
        PanelParticipanti.Controls.Add(new LiteralControl("<div class='panel panel-success' style='max-width:40%'><div class='panel-heading'></br>"));
        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                id_util = SRD.GetInt32(SRD.GetOrdinal("id_utilizator"));

                sqlConnection2.Open();
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = sqlConnection2;
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "select id,  acont from utilizator where id=" + id_util;
                using (SqlDataReader SRD2 = cmd2.ExecuteReader())
                {
                    while (SRD2.Read())
                    {
                        Label l1 = new Label();
                        l1.Text =SRD2.GetString(SRD2.GetOrdinal("acont")).ToString() + " ";
                        PanelParticipanti.Controls.Add(l1);

                        Button Button5 = new Button();
                        Button5.Text = "Elimina";
                        Button5.ID = SRD2.GetInt32(SRD2.GetOrdinal("id")).ToString() + "c";
                        Button5.Click += new EventHandler(EliminaParticipant);
                        Button5.CssClass = "btn btn-danger";
                        PanelParticipanti.Controls.Add(Button5);

                        PanelParticipanti.Controls.Add(new LiteralControl("</br></br>"));
                    }
                }
                sqlConnection2.Close();
            }
        }
        PanelParticipanti.Controls.Add(new LiteralControl("</div></div>"));
        Button Button2 = new Button();
        Button2.Text = "Ascunde";
        Button2.Click += new EventHandler(AscundeParticipanti);
        Button2.CssClass = "btn btn-success";
        PanelParticipanti.Controls.Add(Button2);
        LabelEroareRol.Text = "weird";
        sqlConnection1.Close();
    }*/

    private void EliminaParticipant(object sender, EventArgs e)
    {
        
        Button x = (Button)sender;
        int id = 0;
        string temp = x.ID;
        temp = temp.TrimEnd('c');
        id = Convert.ToInt32(temp);
        SqlCommand cmd7 = new SqlCommand();
        cmd7.Connection = sqlConnection1;
        cmd7.CommandType = CommandType.Text;
        cmd7.CommandText = "delete from participa where id_utilizator=" + id + " and id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        cmd7.ExecuteNonQuery();
        sqlConnection1.Close();
        AfiseazaParticipanti();
        Afiseaza_Roluri();
    }

    private void AscundeParticipanti(object sender, EventArgs e)
    {
       PanelParticipanti.Controls.Clear();
        PanelParticipanti.Visible = false;
    }

    private void ButtonAfiseazaParticipanti_Click(object sender, EventArgs e)
    {
        PanelParticipanti.Visible = true;
    }

    protected void AfiseazaParticipanti()
    {
        int id_util = 0;
        PanelParticipanti.Controls.Clear();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select * from participa where id_eveniment =" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        PanelParticipanti.Controls.Add(new LiteralControl("<div class='panel panel-success' style='max-width:40%'><div class='panel-heading'></br>"));
        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                id_util = SRD.GetInt32(SRD.GetOrdinal("id_utilizator"));

                sqlConnection2.Open();
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = sqlConnection2;
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "select id,  acont from utilizator where id=" + id_util;
                using (SqlDataReader SRD2 = cmd2.ExecuteReader())
                {
                    while (SRD2.Read())
                    {
                        Label l1 = new Label();
                        l1.Text = SRD2.GetString(SRD2.GetOrdinal("acont")).ToString() + " ";
                        PanelParticipanti.Controls.Add(l1);

                        if (Proprietar || Organizator)
                        {
                            Button Button5 = new Button();
                            Button5.Text = "Elimina";
                            Button5.ID = SRD2.GetInt32(SRD2.GetOrdinal("id")).ToString() + "c";
                            Button5.Click += new EventHandler(EliminaParticipant);
                            Button5.CssClass = "btn btn-danger";
                            PanelParticipanti.Controls.Add(Button5);
                        }

                        PanelParticipanti.Controls.Add(new LiteralControl("</br></br>"));
                    }
                }
                sqlConnection2.Close();
            }
        }
        PanelParticipanti.Controls.Add(new LiteralControl("</div></div>"));
        Button Button2 = new Button();
        Button2.Text = "Ascunde";
        Button2.Click += new EventHandler(AscundeParticipanti);
        Button2.CssClass = "btn btn-success";
        PanelParticipanti.Controls.Add(Button2);
        sqlConnection1.Close();
    }

    private void Afiseaza_Fisiere()
    {
        PanelShow.Controls.Clear();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select * from documente where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]);
        sqlConnection1.Open();
        PanelShow.Controls.Add(new LiteralControl("</br><div class='panel panel-info'> <div class='panel-heading'></br>"));
        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                Label l1 = new Label();
                l1.Text = "Fisier: " + SRD.GetString(SRD.GetOrdinal("document")).ToString() + " ";
                PanelShow.Controls.Add(l1);

                Label l2 = new Label();
                l2.Text = "Data: " + SRD.GetString(SRD.GetOrdinal("data_incarcare")).ToString() + " ";
                PanelShow.Controls.Add(l2);

                if (Proprietar || Organizator)
                {
                    Button Button1 = new Button();
                    Button1.Text = "Delete";
                    Button1.CssClass = "btn btn-danger";
                    Button1.ID = "delete_" + SRD.GetInt32(SRD.GetOrdinal("Id")).ToString();
                    Button1.Click += new EventHandler(DeleteFisier);
                    PanelShow.Controls.Add(Button1);
                }

                Button Button2 = new Button();
                Button2.Text = "Download";
                Button2.ID = "download_" + SRD.GetInt32(SRD.GetOrdinal("Id")).ToString();
                Button2.CssClass = "btn btn-success";
                Button2.Click += new EventHandler(DownloadFisier);
                PanelShow.Controls.Add(Button2);

                PanelShow.Controls.Add(new LiteralControl("</br>"));
            }
        }
        PanelShow.Controls.Add(new LiteralControl("</br></div></div></br>"));
        sqlConnection1.Close();
    }

    private void DeleteFisier(object sender, EventArgs e)
    {
        Button x = (Button)sender;
        int id = 0; string nume = "";
        id = Convert.ToInt32(x.ID.Substring(7));
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select document from documente where Id=" + id;
        sqlConnection1.Open();
        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                nume = SRD.GetString(SRD.GetOrdinal("document")).ToString();
            }
        }
        sqlConnection1.Close();
        File.Delete(Server.MapPath(nume));
        SqlCommand cmd2 = new SqlCommand();
        cmd2.Connection = sqlConnection1;
        cmd2.CommandType = CommandType.Text;
        cmd2.CommandText = "delete from documente where Id=" + id;
        sqlConnection1.Open();
        cmd2.ExecuteNonQuery();
        sqlConnection1.Close();
        Afiseaza_Fisiere();
        lblInfo.Text = "Fisier sters cu succes!";
    }

    private void DownloadFisier(object sender, EventArgs e)
    {
        Button x = (Button)sender;
        int id = 0; string nume = "";
        id = Convert.ToInt32(x.ID.Substring(9));
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select document from documente where Id=" + id;
        sqlConnection1.Open();
        using (SqlDataReader SRD = cmd.ExecuteReader())
        {
            while (SRD.Read())
            {
                nume = SRD.GetString(SRD.GetOrdinal("document")).ToString();
            }
        }
        sqlConnection1.Close();
        if (nume.Contains(".htm") || nume.Contains(".html"))
        {
            Response.ContentType = "text/HTML";
        }
        if (nume.Contains(".txt"))
        {
            Response.ContentType = "text/plain";
        }
        if (nume.Contains(".doc") || nume.Contains(".rtf") || nume.Contains(".docx"))
        {
            Response.ContentType = "Application/msword";
        }
        if (nume.Contains(".xls") || nume.Contains(".xlsx"))
        {
            Response.ContentType = "Application/x-msexcel";
        }
        if (nume.Contains(".jpg") || nume.Contains(".jpeg"))
        {
            Response.ContentType = "image/jpeg";
        }
        if (nume.Contains(".gif"))
        {
            Response.ContentType = "image/GIF";
        }
        if (nume.Contains(".pdf"))
        {
            Response.ContentType = "application/pdf";
        }
        Response.AppendHeader("Content-Disposition", "attachment; filename="+nume+"");
        Response.TransmitFile(Server.MapPath(nume));
        Response.End();
    }

    private void cmdSend_Click(object sender, System.EventArgs e)
    {
        int ok = 0;
        if (filMyFile.PostedFile != null)
        {
            HttpPostedFile myFile = filMyFile.PostedFile;
            int nFileLen = myFile.ContentLength;
            if (nFileLen > 0)
            {
                byte[] myData = new byte[nFileLen];
                string dt = DateTime.Now.ToString("d"); ;
                myFile.InputStream.Read(myData, 0, nFileLen);
                string strFilename = Path.GetFileName(myFile.FileName);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection1;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select count(*) as numar from documente where document='" + strFilename + "'";
                sqlConnection1.Open();
                using (SqlDataReader SRD = cmd.ExecuteReader())
                {
                    while (SRD.Read())
                    {
                        ok = SRD.GetInt32(SRD.GetOrdinal("numar"));
                    }
                }
                sqlConnection1.Close();
                if (ok == 0)
                {
                    WriteToFile(Server.MapPath(strFilename), ref myData);
                    UpdateDatabaseDoc(strFilename, dt);
                    lblInfo.Text =
                        "Filename: " + strFilename + "<br>" +
                        "Date: " + dt + "<br> Fisier uploadat cu succes!<p>";
                    Afiseaza_Fisiere();
                }
                else
                {
                    lblInfo.Text = "Fisier deja existent!";
                }
            }
        }
    }

    private void UpdateDatabaseDoc(string Name, string date)
    {
        int nr = 0;
        SqlCommand cmd5 = new SqlCommand();
        cmd5.Connection = sqlConnection1;
        cmd5.CommandText = "select max(Id) as numar from documente";
        cmd5.CommandType = CommandType.Text;
        sqlConnection1.Open();
        using (SqlDataReader SRD5 = cmd5.ExecuteReader())
        {
            while (SRD5.Read())
            {
                try { nr = SRD5.GetInt32(SRD5.GetOrdinal("numar")); }
                catch (Exception ex) { nr = 0; }

            }
        }
        sqlConnection1.Close();
        nr++;
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd = new SqlCommand("INSERT INTO documente(Id, id_eveniment, document, data_incarcare) VALUES(" + nr + ", " + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ", '" + Name + "', '" + date + "')", sqlConnection1);
        cmd.ExecuteNonQuery();
        sqlConnection1.Close();
    }

    private void WriteToFile(string strPath, ref byte[] Buffer)
    {
        FileStream newFile = new FileStream(strPath, FileMode.Create);
        newFile.Write(Buffer, 0, Buffer.Length);
        newFile.Close();
    }

    private string GetMyName()
    {
        string strScript = Request.ServerVariables["SCRIPT_NAME"];
        int nPos = strScript.LastIndexOf("/");
        if (nPos > -1)
            strScript = strScript.Substring(nPos + 1);
        return strScript;
    }
    protected void ButtonComentariu_Click(object sender, EventArgs e)
    {
        PanelComentariuNou.Visible = true;
        ButtonForum_Click(sender, e);
    }
    protected void ButtonScrieComentariu_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;

        int idev = Convert.ToInt32(Session["IdEvenimentSelectat"]);
        string text = TextBoxComentariuNou.Text, autor = Session["nume"].ToString();

        cmd.CommandText = "INSERT INTO replici (id_eveniment,text,autor) VALUES (" + idev + ",'" + text + "','" + autor + "');";
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        sqlConnection1.Close();
        PanelComentariuNou.Visible = false;
        ButtonForum_Click(sender, e);
    }

    protected void ComentariuSterge(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;

        Button btn = (Button)sender;
        int id = Convert.ToInt32(btn.ID.Substring(6));

        cmd.CommandText = "DELETE FROM replici WHERE id=" + id + ";";
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        sqlConnection1.Close();
        PanelComentariuNou.Visible = false;
        ButtonForum_Click(sender, e);

    }
    protected void ComentariuRaspunde(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        //   sqlConnection1.Open();
        //   cmd.Connection = sqlConnection1;

        Button btn = (Button)sender;

        int idev = Convert.ToInt32(Session["IdEvenimentSelectat"]);
        //    string text = TextBoxComentariuNou.Text, autor = Session["nume"].ToString();

        //   cmd.CommandText = "INSERT INTO replici (id_eveniment,text,autor,id_parinte) VALUES (" + idev + ",'" + text + "','" + autor + "',"+Convert.ToInt32(btn.ToolTip)+");";
        //   cmd.CommandType = CommandType.Text;
        //  cmd.ExecuteNonQuery();
        Session["idtata"] = btn.ID.ToString().Substring(8);

        //   sqlConnection1.Close();
        PanelComentariuNou.Visible = false;
        Page_Load(sender, e);


    }
    protected void ComentariuRaspundePosteaza(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        sqlConnection1.Open();
        cmd.Connection = sqlConnection1;

        Button btn = (Button)sender;
        TextBox t = (TextBox)PanelComentarii.FindControl("rsp2");

        int idev = Convert.ToInt32(Session["IdEvenimentSelectat"]);
        string text = t.Text;
        string autor = Session["nume"].ToString();

        cmd.CommandText = "INSERT INTO replici (id_eveniment,text,autor,id_parinte) VALUES (" + idev + ",'" + text + "','" + autor + "'," + Convert.ToInt32(btn.ToolTip) + ");";
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        Session["idtata"] = null;

        sqlConnection1.Close();
        PanelComentariuNou.Visible = false;
        ButtonForum_Click(sender, e);

    }

    protected void ButtonAdaugaRol2_Click1(object sender, EventArgs e)
    {

    }

    protected void Button_EvenimenteFeatured_Click(object sender, EventArgs e)
    {
        Response.Redirect("EvenimenteFeatured.aspx");
    }
    protected void Afiseaza_NumarParticipanti()
    {
         SqlCommand cmd = new SqlCommand();
        cmd.Connection = sqlConnection1;
        try 
	{	        
		sqlConnection1.Open();
        cmd.CommandText = "select count(id_utilizator) from participa where id_eveniment = " + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
        cmd.CommandType = CommandType.Text;
        nr_part.Text = cmd.ExecuteScalar().ToString();

        sqlConnection1.Close();
	}
	catch (Exception ex)
	{
		
		MesajRaspuns.Text = " eroare in functia de afisare participanti "+ex.Message;
	}
       
    }

    //protected void ButtonEditeazaLocatie_Click(object sender, EventArgs e)
    //{
    //    textbox_latitudine.Visible = true;
    //    textbox_longitudine.Visible = true;
    //    ButtonIntroduceDateLocatie.Visible = true;
    //    ButtonLocatie.Visible = false;

    //    SqlCommand cmd = new SqlCommand();
    //    try
    //    {
    //        sqlConnection1.Open();
    //        cmd.Connection = sqlConnection1;

    //        cmd.CommandText = "select count(*) from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
    //        cmd.CommandType = CommandType.Text;

    //        if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
    //        {
    //            cmd.CommandText = "select latitudine,longitutine from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
    //            cmd.CommandType = CommandType.Text;

    //            using (SqlDataReader SRD = cmd.ExecuteReader())
    //            {
    //                SRD.Read();
    //                {
    //                    //MesajRaspuns.Text = "2";
    //                    textbox_latitudine.Text = (SRD.GetDouble(0).ToString());
    //                    textbox_longitudine.Text = (SRD.GetDouble(1).ToString());
    //                    //MesajRaspuns.Text += "3";
    //                    Session["locatie"] = "da";
    //                }
    //            }


    //        }
    //        else
    //            Session["locatie"] = "nu";
          

    //    }
    //    catch (Exception ex)
    //    {

    //        MesajRaspuns.Text = "eroareeeeeeeeeeeee in ButtonEditeazaLocatie_Click " + ex.Message;
    //    }
    //    sqlConnection1.Close();
    //   // MesajRaspuns.Text = Session["locatie"].ToString();
    //}

    protected void editareLocatie() // apelez in page load daca este proprietar
    {
        SqlCommand cmd = new SqlCommand();
        try
        {
            sqlConnection1.Open();
            cmd.Connection = sqlConnection1;

            cmd.CommandText = "select count(*) from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
            cmd.CommandType = CommandType.Text;

            if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
            {
                cmd.CommandText = "select latitudine,longitutine from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader SRD = cmd.ExecuteReader())
                {
                    SRD.Read();
                    {
                        //MesajRaspuns.Text = "2";
                        textbox_latitudine.Text = (SRD.GetDouble(0).ToString());
                        textbox_longitudine.Text = (SRD.GetDouble(1).ToString());
                        MesajRaspuns.Text += "3";
                        Session["locatie"] = "da";
                    }
                }


            }
            else
                Session["locatie"] = "nu";


        }
        catch (Exception ex)
        {

            MesajRaspuns.Text = "eroareeeeeeeeeeeee in editareLocatie " + ex.Message;
        }
        sqlConnection1.Close();
    }

    protected void ButtonSubmitLocatie_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();

        double lat = 90.0, longi = -90.0;

        if (Double.TryParse(textbox_latitudine.Text, out lat) == false)
        {
            MesajRaspuns.Text = "introduceti 2 numere intre -90 si 90";
            return;
        }
        else if (lat > 90 || lat < -90)
        {
            MesajRaspuns.Text = "introduceti 2 numere intre -90 si 90";
            return;
        }

        if (Double.TryParse(textbox_longitudine.Text, out longi) == false)
        {
            MesajRaspuns.Text = "introduceti 2 numere intre -90 si 90";
            return;
        }
        else if (longi > 90 || longi < -90)
        {
            MesajRaspuns.Text = "introduceti 2 numere intre -90 si 90";
            return;
        }

       
        try
        {

            sqlConnection1.Open();
            cmd.Connection = sqlConnection1;

            if (Session["locatie"] == "nu")
            {
                MesajRaspuns.Text = Session["locatie"].ToString();
                cmd.CommandText = "insert into locatie(latitudine,longitutine,id_eveniment) values(" + lat + "," + longi + "," + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ");";
                cmd.CommandType = CommandType.Text;
            }
            else if (Session["locatie"] == "da")
            {
                cmd.CommandText = "update locatie set latitudine = " + lat + ", longitutine = " + longi + " where id_eveniment = " + Convert.ToInt32(Session["IdEvenimentSelectat"]) + " ;";
                cmd.CommandType = CommandType.Text;
                MesajRaspuns.Text += Session["locatie"].ToString();
            }
            cmd.ExecuteNonQuery();
            MesajRaspuns.Text += Session["locatie"].ToString();

            //if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
            //{

            //    cmd.CommandText = "select latitudine,longitutine from locatie where id_eveniment=" + Convert.ToInt32(Session["IdEvenimentSelectat"]) + ";";
            //    cmd.CommandType = CommandType.Text;

            //    using (SqlDataReader SRD = cmd.ExecuteReader())
            //    {
            //        while (SRD.Read())
            //        {
            //            MesajRaspuns.Text = "2";
            //            lat = (SRD.GetDouble(0));
            //            longi = (SRD.GetDouble(1));
            //            MesajRaspuns.Text += "3";
            //        }
            //    }
            //}

        }
        catch (Exception ex)
        {

            MesajRaspuns.Text += "eroareeeeeeeeeeeee in editeaza locatie " + ex.Message;
        }
        sqlConnection1.Close();

       // Response.Redirect("Eveniment.aspx");
    }
}