using System.Net;
using System.Net.Mail;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Proyecto_BD.Utilities
{
    public class CorreoElectronico
    {
        private readonly IConfiguration _config;

        public CorreoElectronico(IConfiguration config)
        {
            _config = config;
        }
        
        public async Task EnviarCorreo(string destinatario, string asunto, string cuerpo)
        {
            var smtp = _config.GetSection("Smtp");
            var mail = new MailMessage();
            mail.From = new MailAddress(smtp["User"]);
            mail.To.Add(destinatario);
            mail.Subject = asunto;
            mail.Body = cuerpo;
            mail.IsBodyHtml = true;

            using var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"]))
            {
                Credentials = new NetworkCredential(smtp["User"],
                smtp["Password"]),
                EnableSsl = bool.Parse(smtp["EnableSsl"])
            };

            await client.SendMailAsync(mail);
        }
    }
}
