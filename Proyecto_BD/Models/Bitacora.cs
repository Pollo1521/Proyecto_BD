using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Bitacora
    {
        [Key]
        public int ID_Bitacora { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Movimiento")]
        public DateTime Fecha_Movimiento { get; set; }

        public int ID_Medico { get; set; }
        [ForeignKey("ID_Medico")]
        public Medico Medico { get; set; }

        public int ID_Paciente { get; set; }
        [ForeignKey("ID_Paciente")]
        public Paciente Paciente { get; set; }

        public int ID_Cita { get; set; }
        [ForeignKey("ID_Cita")]
        public Cita Cita { get; set; }
    }
}
