using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Usuario
    {
        [Key]
        public int ID_Usuario { get; set; }

        [Required]
        public string Nombre { get; set; }
        
        [Required]
        [Display(Name = "Apellido Paterno")]
        public string Apellido_Paterno { get; set; }

        [Required]
        [Display(Name = "Apellido Materno")]
        public string Apellido_Materno { get; set; }

        [Required]
        public string Correo { get; set; }
        
        [Required]
        [MinLength(18)]
        [MaxLength(18)]
        public string CURP { get; set; }
       
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime Fecha_Nacimiento { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Registro")]
        public DateTime Fecha_Registro { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public int ID_Tipo_Usuario { get; set; }
        [ForeignKey("ID_Tipo_Usuario")]
        [Display(Name = "Tipo de Usuario")]
        public TipoUsuario TipoUsuario { get; set; }

        [Required]
        public bool Estado_Usuario { get; set; }

        //Llaves
        public ICollection<Paciente> Paciente { get; set; }
        public ICollection<Medico> Medico { get; set; }
        public ICollection<Recepcionista> Recepcionista { get; set; }
    }
}
