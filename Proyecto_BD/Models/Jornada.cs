﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Jornada
    {
        [Key]
        public int ID_Jornada { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de Entrada")]
        public DateTime Hora_Entrada { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de Salida")]
        public DateTime Hora_Salida { get; set; }

        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        //Llave
        ICollection<Medico> Medico { get; set; } 
        ICollection<Recepcionista> Recepcionista { get; set; }
    }
}
