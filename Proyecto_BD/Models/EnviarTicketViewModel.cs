using System.ComponentModel.DataAnnotations;

namespace Proyecto_BD.Models
{
    public class EnviarTicketViewModel
    {
        public int VentaId { get; set; }

        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; }
    }
}
