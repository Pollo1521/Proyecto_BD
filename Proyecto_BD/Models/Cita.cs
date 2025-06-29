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
        public Paciente Paciente { get; set; }

        public int ID_Medico { get; set; }
        [ForeignKey("ID_Medico")]
        public Medico Medico { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Registro")]
        public DateTime Fecha_Registro { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Cita")]
        public DateTime Fecha_Cita { get; set; }

        public int ID_Cita_Horario { get; set; }
        [ForeignKey("ID_Cita_Horario")]
        [Display(Name = "Hora de Cita")]
        public CitasHorario CitasHorario { get; set; }

        public int ID_Estatus_Cita { get; set; }
        [ForeignKey("ID_Estatus_Cita")]
        [Display(Name = "Estatus")]
        public EstatusCita EstatusCita { get; set; }

        //Llave

        public ICollection<Receta> Recetas { get; set; }
        public ICollection<Pago> Pagos { get; set; }
        public ICollection<Bitacora> Bitacoras { get; set; }
    }
}
