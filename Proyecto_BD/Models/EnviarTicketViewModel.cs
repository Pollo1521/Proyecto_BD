using System.ComponentModel.DataAnnotations;

namespace Proyecto_BD.Models
{
    public class EnviarTicketViewModel
    {
        public int VentaId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string CorreoElectronico { get; set; }
    }
}
