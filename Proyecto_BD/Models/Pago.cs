using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Proyecto_BD.Models
{
    public class Pago
    {
        [Key]
        public int ID_Pago { get; set; }

        public int ID_Cita { get; set; }
        [ForeignKey("ID_Cita")]
        public Cita Cita { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool Estado_Pago { get; set; }

        [Required]
        public string ComprobantePago { get; set; }
    }
}
