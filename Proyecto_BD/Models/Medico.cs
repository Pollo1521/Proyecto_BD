using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Medico
    {
        [Key]
        public int ID_Medico { get; set; }

        [Required]
        public int ID_Usuario { get; set; }
        [ForeignKey("ID_Usuario")]
        public Usuario Usuario { get; set; }

        [Required]
        public string CURP { get; set; }

        public int ID_Especialidad { get; set; }
        [ForeignKey("ID_Especialidad")]
        [Required]
        public Especialidad Especialidad { get; set; }

        public int ID_Consultorio { get; set; }
        [ForeignKey("ID_Consultorio")]
        public Consultorio Consultorio { get; set; }

        [Required]
        public int ID_Jornada { get; set; }
        [ForeignKey("ID_Jornada")]
        [Required]
        public Jornada Jornada { get; set; }
    }
}
