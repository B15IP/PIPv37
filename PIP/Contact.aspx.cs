using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.Mail;
using System.Net.Mail;

public partial class Contact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Button_sendEmail_Click(object sender, EventArgs e)
    {
        string fromEmail = email.Text;
        string BodyEmail = mesaj.Text;
        string nume = nume_sender.Text;
        string subject = subiect.Text;

        if ((fromEmail == "") || (BodyEmail == "") || (nume == "") || (subject == ""))
        {
            MesajEroare.Text = "Completati toate campurile";
            return;
        }

MailMessage msg = new MailMessage();
        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
        try {
            msg.Subject = subject;
            msg.Body = BodyEmail; 
            msg.From = new MailAddress(fromEmail); 
            msg.To.Add("cosmin.of@gmail.com");
            msg.IsBodyHtml = true; 
            client.Host = "smtp.gmail.com";
            System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("ingineria.programarii.b15@gmail.com", "ingineria"); 
            client.Port = int.Parse("587"); 
            client.EnableSsl = true; 
            client.UseDefaultCredentials = false;
            client.Credentials = basicauthenticationinfo;
            client.DeliveryMethod = SmtpDeliveryMethod.Network; 
            client.Send(msg); 
        } catch (Exception ex) {
            throw new Exception(); 
        }
        Response.Redirect("Contact.aspx");
        //return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri); 
    }
}