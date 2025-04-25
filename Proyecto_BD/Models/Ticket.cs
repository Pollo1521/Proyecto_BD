using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Proyecto_BD.Controllers;

namespace Proyecto_BD.Models
{
    public class Ticket
    {
        [Key]
        public int ID_Ticket { get; set; }

        public int ID_Venta{ get; set; }
        [ForeignKey("ID_Venta")]
        public Ventas Venta { get; set; }

        [Required]
        public int ID_Medicina { get; set; }
        [ForeignKey("ID_Medicina")]
        [Required]
        public Medicina Medicina { get; set; }

        public int ID_Servicio { get; set; }
        [ForeignKey("ID_Servicio")]
        [Required]
        public Servicio Servicio { get; set; }
    }
}
