using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class CreazaEveniment : System.Web.UI.Page
{
    SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Domanitos\Downloads\PIPv36\PIP\App_Data\PIP.mdf';Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void creazaEveniment(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        LabelCreareEveniment.Text = "";

        if (numeEveniment.Text == "")
        {
            LabelCreareEveniment.Text = "Introduceti numele evenimentului";
            return;
        }
        else
            if (numeEveniment.Text.Length > 50)
            {
                LabelCreareEveniment.Text = "Denumirea evenimentului este prea lunga";
                return;
            }
        if (ziuaEveniment.Text == "")
        {
            LabelCreareEveniment.Text = "Introduceti o zi!";
            return;
        }
        if (anEveniment.Text == "")
        {
            LabelCreareEveniment.Text = " Introduceti un an!";
            return;
        }

        if (etichetaEveniment.Text == "")
        {
            LabelCreareEveniment.Text = "Introduceti etichete!";
            return;
        }
        else
            if (etichetaEveniment.Text.Length > 128)
            {
                LabelCreareEveniment.Text = "Eticheta evenimentului este prea lunga";
                return;
            }

        if (descriereEveniment.Text == "")
        {
            LabelCreareEveniment.Text = "Introduceti descrierea!";
            return;
        }
        else
            if (descriereEveniment.Text.Length > 128)
            {
                LabelCreareEveniment.Text = "Descrierea evenimentului este prea lunga";
                return;
            }

        int zi = DateTime.Now.Day, luna = DateTime.Now.Month, an = DateTime.Now.Year,ora=DateTime.Now.Hour,minut=DateTime.Now.Minute;

        if (Int32.TryParse(oraEveniment.Text, out ora) == false)
        {
            LabelCreareEveniment.Text = "Introduceti o ora numar intreg";
            return;
        }
        else
        {
            if ( ora>23 || ora <0 )
            {
                LabelCreareEveniment.Text = "Introduceti o ora numar intreg intre 0 si 23 ";
                return;
            }
        }

        if (Int32.TryParse(minutEveniment.Text, out minut) == false)
        {
            LabelCreareEveniment.Text = "Introduceti un minut numar intreg";
            return;
        }
        else
        {
            if (ora > 59 || ora < 0)
            {
                LabelCreareEveniment.Text = "Introduceti un minut numar intreg intre 0 si 59 ";
                return;
            }
        }

        if (Int32.TryParse(anEveniment.Text, out an) == false)
        {
            LabelCreareEveniment.Text = "Introduceti un an numar intreg";
            return;
        }
        else
        {
            if (an < DateTime.Now.Year || an > 3000)
            {
                LabelCreareEveniment.Text = "Introduceti un an numar intreg intre " + DateTime.Now.Year + " si 3000 ";
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
            LabelCreareEveniment.Text = "Introduceti o zi numar intreg";
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
                            LabelCreareEveniment.Text = "Introduceti o zi numar intreg intre 1 si 29 ";
                            return;
                        }
                    }
                    else
                        if (an % 4 == 0)
                        {
                            if (zi < 1 || zi > 29)
                            {
                                LabelCreareEveniment.Text = "Introduceti o zi numar intreg intre 1 si 29 ";
                                return;
                            }
                        }
                        else
                        {
                            if (zi < 1 || zi > 28)
                            {
                                LabelCreareEveniment.Text = "Introduceti o zi numar intreg intre 1 si 28 ";
                                return;
                            }
                        }
                }
                if (zi < 1 || zi > 30)
                {
                    LabelCreareEveniment.Text = "Introduceti o zi numar intreg intre 1 si 30 ";
                    return;
                }
            }
            else
                if (zi < 1 || zi > 31)
                {
                    LabelCreareEveniment.Text = "Introduceti o zi numar intreg intre 1 si 31 ";
                    return;
                }
        }

        DateTime data = DateTime.Now;
        string dataString = luna + "." + zi + "." + an+" "+ora+":"+minut;
        LabelCreareEveniment.Text = dataString;
        try
        {
            data = Convert.ToDateTime(dataString);
        }
        catch (Exception ex)
        {

        }
        //----------------------------------------------------------------------
        String etichete = etichetaEveniment.Text;
        etichete += " " + numeEveniment.Text.ToString() + " " + orasEveniment.Text.ToString() + " " + judetEveniment.Text.ToString() + " " + taraEveniment.Text.ToString();
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
       
        sqlConnection1.Close();
        sqlConnection1.Open();
        //----------------------------------------------------------------------
        cmd = new SqlCommand("INSERT INTO evenimente(nume, descriere, data_inceperii, etichete, oras, judet, tara, id_proprietar) VALUES('" + numeEveniment.Text.ToString() + "','" + descriereEveniment.Text.ToString() + "','" + data + "','" + etichete + "','" + orasEveniment.Text.ToString() + "','" + judetEveniment.Text.ToString() + "','" + taraEveniment.Text.ToString() + "'," + Session["id"] + ")", sqlConnection1);
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        try
        {
            cmd.ExecuteNonQuery();
            LabelCreareEveniment.Text = "Inregistrare reusita";
            numeEveniment.Text = "";
            descriereEveniment.Text = "";
            orasEveniment.Text = "";
            judetEveniment.Text = "";
            taraEveniment.Text = "";
            ziuaEveniment.Text = "";
            anEveniment.Text = "";
            etichetaEveniment.Text = "";
        }
        catch (Exception ex)
        {
            LabelCreareEveniment.Text = ex.Message;
            return;
        }
        sqlConnection1.Close();
        int nr2 = 0;
        SqlCommand cmd3 = new SqlCommand();
        cmd3.Connection = sqlConnection1;
        cmd3.CommandText = "select max(id) as numar from evenimente";
        cmd3.CommandType = CommandType.Text;
        sqlConnection1.Open();
        using (SqlDataReader SRD5 = cmd3.ExecuteReader())
        {
            while (SRD5.Read())
            {
                nr2 = SRD5.GetInt32(SRD5.GetOrdinal("numar"));
            }
        }
        sqlConnection1.Close();
        int nr = 0;
        SqlCommand cmd5 = new SqlCommand();
        cmd5.Connection = sqlConnection1;
        cmd5.CommandText = "select max(Id) as numar from roluri";
        cmd5.CommandType = CommandType.Text;
        sqlConnection1.Open();
        using (SqlDataReader SRD5 = cmd5.ExecuteReader())
        {
            while (SRD5.Read())
            {
                try
                {
                    nr = SRD5.GetInt32(SRD5.GetOrdinal("numar"));
                }
                catch (Exception ex)
                {
                    nr = 0;
                }
            }
        }
        sqlConnection1.Close();
        nr++;
        SqlCommand cmd4 = new SqlCommand();
        cmd4.Connection = sqlConnection1;
        cmd4.CommandText = "insert into roluri(Id, id_eveniment, denumire, descriere, max_participanti) values(" + nr + ", " + nr2 + ", 'Participant', '', 0)";

        cmd4.CommandType = CommandType.Text;
        sqlConnection1.Open();
        cmd4.ExecuteNonQuery();
        sqlConnection1.Close();
        Button_creazaEveniment.Enabled = false;
    }


}