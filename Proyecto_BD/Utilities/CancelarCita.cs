using Proyecto_BD.Models;

namespace Proyecto_BD.Utilities
{
    public class CancelarCita
    {
        private readonly ContextoBaseDatos _context;

        public CancelarCita(ContextoBaseDatos context)
        {
            _context = context;
        }
        public void CancelarCitaNoPagada(int idCita)
        {
            var cita = _context.Cita.FirstOrDefault(c => c.ID_Cita == idCita);
            var pago = _context.Pago.FirstOrDefault(p => p.ID_Cita == idCita);

            if (cita != null && pago != null && !pago.Estado_Pago)
            {
                cita.ID_Estatus_Cita = 5; // Suponiendo que 5 = Cancelada
                _context.SaveChanges();
            }
        }
    }
}
