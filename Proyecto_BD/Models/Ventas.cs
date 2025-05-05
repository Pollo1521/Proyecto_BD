using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Ventas
    {
        [Key]
        public int ID_Ventas { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Venta")]
        public DateTime Fecha_Venta { get; set; }

        public int ID_Recepcionista { get; set; }
        [ForeignKey("ID_Recepcionista")]
        [Required]
        public Recepcionista Recepcionista { get; set; }
    }
}
