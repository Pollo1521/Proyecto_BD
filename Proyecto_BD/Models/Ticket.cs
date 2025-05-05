using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Proyecto_BD.Controllers;

namespace Proyecto_BD.Models
{
    public class Ticket
    {
        [Key]
        public int ID_Ticket { get; set; }

        public int ID_Venta { get; set; }
        [ForeignKey("ID_Venta")]
        public Ventas Venta { get; set; }

        public bool Tipo_item { get; set; } //true = medicina, false = servicio

        [Display(Name = "Item")]
        public int ID_Item { get; set; }

        [NotMapped]
        public Medicina? Medicina { get; set; }
        [NotMapped]
        public Servicio? Servicio { get; set; }
    }
}
