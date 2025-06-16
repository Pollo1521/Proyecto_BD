using System.ComponentModel.DataAnnotations;

namespace Proyecto_BD.Models
{
    public class CitasHorario
    {
        [Key]
        public int ID_Horario { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de Cita")]
        public TimeOnly Hora_Cita { get; set; }
        
        [Required]
        public bool JornadaHorario { get; set; }

        public ICollection<Cita> Cita { get; set; }
    }
}
