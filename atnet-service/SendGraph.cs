using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace atnet_service
{
    class SendGraph
    {
        public void NewMessage()
        {
            MailMessage mail = new MailMessage(
                new MailAddress("atnet@spanhel.eu", "system service"),
                new MailAddress("spanhel@spanhel.eu", "Jan Špaňhel")
            );
            mail.Subject = "Předmět zprávy";
            //mail.Body = "Toto je tělo";

            //mail.Attachments.Add(new Attachment(@"C:\Users\spanhel\Pictures\2416585_0.jpg"));

            // PŘÍLOHA V JPEG
            Stream sr = new FileStream(@".\atnet.jpg", FileMode.Open, FileAccess.Read);
            mail.Attachments.Add(new Attachment(sr, "image.jpg", MediaTypeNames.Image.Jpeg));

            // TĚLO
            mail.IsBodyHtml = true;
            string html = "<bold>TOTO JE PĚKNĚ TUČNÉ</bold>";
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            string text = "Toto je tělo bez formátování (sprostý text) :-)";
            AlternateView textView = AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain);
            mail.AlternateViews.Add(htmlView);
            mail.AlternateViews.Add(textView);

            SmtpClient client = new SmtpClient("mail.hukot.net");
            client.UseDefaultCredentials = false;
            var basicCredential = new NetworkCredential("atnet@spanhel.eu", "spa0138");
            client.Credentials = basicCredential;
            client.EnableSsl = true;
            
            try
            {
                client.Send(mail);
            }
            catch(Exception ex)
            {

            }
            sr.Dispose();
            client.Dispose();
            mail.Dispose();
        }
    }
}
