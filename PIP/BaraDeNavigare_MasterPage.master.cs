using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

public class FaceBookUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string PictureUrl { get; set; }
    public string Email { get; set; }
}

public partial class BaraDeNavigare_MasterPage : System.Web.UI.MasterPage
{
    SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Domanitos\Downloads\PIPv36\PIP\App_Data\PIP.mdf';Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["nume"] == null)
        {
            LinkButtoninregistrare.Visible = true;
            LinkButtonlogin.Visible = true;
            LinkButtonlogout.Visible = false;
            LinkButtonprofil.Visible = false;
            
        }
        else
        {
            LinkButtoninregistrare.Visible = false;
            LinkButtonlogin.Visible = false;
            LinkButtonlogout.Visible = true;
            LinkButtonprofil.Visible = true;
            
            nume_logat.Text = "" + ((string)Session["nume"]);
        }
       
    }

    protected void profil_click(object sender, EventArgs e)
    {
        Response.Redirect("profil.aspx");
        LinkButtoninregistrare.Visible = false;
        LinkButtonlogin.Visible = false;
        LinkButtonlogout.Visible = true;
        LinkButtonprofil.Visible = true;
    }

    protected void inregistrare_click(object sender, EventArgs e)
    {
        Panellogin.Visible = false;
        Panelinreg.Visible = true;
    }

    protected void home_click(object sender, EventArgs e)
    {
        Response.Redirect("homepage.aspx");
    }

    protected void login_click(object sender, EventArgs e)
    {
        Panellogin.Visible = true;
        Panelinreg.Visible = false;
    }

    protected void logout_click(object sender, EventArgs e)
    {
        Session["nume"] = null;
        Session["id"] = null;
        nume_logat.Text = "";
        LinkButtoninregistrare.Visible = true;
        LinkButtonlogin.Visible = true;
        LinkButtonlogout.Visible = false;
        LinkButtonprofil.Visible = false;
        Response.Redirect("homepage.aspx"); // redirectionare catre home cand da logout
    }

    protected void Buttoncreare_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand();
        int nr = 0;
        Labelinregeroare.Text = "";

        if ((TextBoxacont.Text == "") || (TextBoxparola.Text == "") || (TextBoxparola2.Text == ""))
        {
            Labelinregeroare.Text = "Va rugam sa completati toate campurile obligatorii";
            return;
        }

        if (TextBoxparola.Text != TextBoxparola2.Text)
        {
            Labelinregeroare.Text = "Confirmarea parolei a esuat!";
            return;
        }
        sqlConnection1.Open();
        cmd.CommandText = "SELECT COUNT(acont) FROM utilizator where '" + TextBoxacont.Text + "'=acont";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;
        nr = Convert.ToInt32(cmd.ExecuteScalar());
        if (nr != 0)
        {
            Labelinregeroare.Text = "Numele acontului este deja luat!";
            return;
        }

        cmd.CommandText = "SELECT COUNT(acont) FROM utilizator where '" + TextBoxemail.Text + "'=email";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;
        nr = Convert.ToInt32(cmd.ExecuteScalar());
        if (nr != 0)
        {
            Labelinregeroare.Text = "Emailul este deja folosit!";
            return;
        }

        if (TextBoxnume.Text == "")
            TextBoxnume.Text = " ";
        if (TextBoxprenume.Text == "")
            TextBoxprenume.Text = " ";

        cmd = new SqlCommand("INSERT INTO utilizator(acont, nume, prenume, email, parola) VALUES('" + TextBoxacont.Text.ToString() + "','" + TextBoxnume.Text.ToString() + "','" + TextBoxprenume.Text.ToString() + "','" + TextBoxemail.Text.ToString() + "','" + TextBoxparola.Text.ToString() + "')", sqlConnection1);
        cmd.ExecuteNonQuery();
        sqlConnection1.Close();
        Labelinregeroare.Text = "Inregistrare reusita";
        Buttoncreare.Enabled = false;
    }

    protected void Buttonascunde_Click(object sender, EventArgs e)
    {
        Panelinreg.Visible = false;
    }

    protected void Buttonlogin_Click(object sender, EventArgs e)
    {
        // SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Domanitos\Downloads\creareevenimente\PIP\App_Data\PIP.mdf;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        int nr = 0;
        Labellogineroare.Text = "";

        if (TextBoxlogina.Text == "")
        {
            Labellogineroare.Text = "Introduceti acont!";
            return;
        }
        if (TextBoxloginp.Text == "")
        {
            Labellogineroare.Text = "Introduceti o parola!";
            return;
        }

        sqlConnection1.Open();
        cmd.CommandText = "SELECT COUNT(acont) FROM utilizator where '" + TextBoxloginp.Text + "'=parola AND '" + TextBoxlogina.Text + "'=acont;";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;
        nr = Convert.ToInt32(cmd.ExecuteScalar());
        if (nr == 0)
        {
            Labellogineroare.Text = "Acont sau parola gresita!";
            return;
        }

        cmd.CommandText = "SELECT id FROM utilizator where '" + TextBoxloginp.Text + "'=parola AND '" + TextBoxlogina.Text + "'=acont;";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;
        int id = Convert.ToInt32(cmd.ExecuteScalar());
        sqlConnection1.Close();

        Session["nume"] = TextBoxlogina.Text;
        Session["id"] = id;
        nume_logat.Text = "" + TextBoxlogina.Text;
        Panellogin.Visible = false;
        LinkButtoninregistrare.Visible = false;
        LinkButtonlogin.Visible = false;
        LinkButtonlogout.Visible = true;
        LinkButtonprofil.Visible = true;
        Response.Redirect("homepage.aspx");
    }

    protected void contact_click(object sender, EventArgs e)
    {
        Response.Redirect("Contact.aspx");
    }
}
