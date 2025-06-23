namespace Proyecto_BD.Models
{
    public class LineaPagoData
    {
        public string Paciente { get; set; }
        public string CURP { get; set; }
        public DateTime FechaCita { get; set; }
        public string HoraCita { get; set; }
        public string Medico { get; set; }
        public string Especialidad { get; set; }
        public string Consultorio { get; set; }
        public float Precio { get; set; }
        public string LinkPago { get; set; }
    }
}
