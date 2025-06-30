using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Medico
    {
        [Key]
        [Display(Name = "Medico")]
        public int ID_Medico { get; set; }

        [Required]
        public int ID_Usuario { get; set; }
        [ForeignKey("ID_Usuario")]
        public Usuario Usuario { get; set; }

        [Required]
        public string Cedula { get; set; }

        [Display(Name = "Especialidad")]
        public int ID_Especialidad { get; set; }
        [ForeignKey("ID_Especialidad")]
        [Required]
        public Especialidad Especialidad { get; set; }

        [Display(Name = "Consultorio")]
        public int ID_Consultorio { get; set; }
        [ForeignKey("ID_Consultorio")]
        public Consultorio Consultorio { get; set; }

        [Required]
        [Display(Name = "Jornada")]
        public int ID_Jornada { get; set; }
        [ForeignKey("ID_Jornada")]
        [Required]
        public Jornada Jornada { get; set; }

        public ICollection<Bitacora> Bitacoras { get; set; }
    }
}
