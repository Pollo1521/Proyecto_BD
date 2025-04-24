using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Cita
    {
        [Key]
        public int ID_Cita { get; set; }

        public int ID_Paciente { get; set; }
        [ForeignKey("ID_Paciente")]
        [Required]
        public Paciente Paciente { get; set; }

        //public int ID_Medico { get; set; }
        //[ForeignKey("ID_Medico")]
        //[Required]
        //public Medico Medico { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Registro")]
        public DateTime Fecha_Registro { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Cita")]
        public DateTime Fecha_Cita { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de Cita")]
        public DateTime Hora_Cita { get; set; }

        public int ID_Estatus_Cita { get; set; }
        [ForeignKey("ID_Estatus_Cita")]
        [Required]
        public EstatusCita EstatusCita { get; set; }
    }
}
