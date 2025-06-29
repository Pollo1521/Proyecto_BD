using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Proyecto_BD.Models;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_BD.Utilities
{
    public class RecetaPdfDocument : IDocument
    {
        private readonly Receta _receta;
        private readonly Cita _cita;
        private readonly Paciente _paciente;
        private readonly Medico _doctor;

        public RecetaPdfDocument(Receta receta, Cita cita, Paciente paciente, Medico medico)
        {
            _receta = receta;
            _cita = cita;
            _paciente = paciente;
            _doctor = medico;

        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            var hoy = DateTime.Today;
            int edad = hoy.Year - _paciente.Usuario.Fecha_Nacimiento.Year;
            if (_paciente.Usuario.Fecha_Nacimiento.Date > hoy.AddYears(-edad)) edad--;

            container.Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4);
                page.Content()
                    .Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Receta Médica").FontSize(20).Bold().AlignCenter();
                        col.Item().Text($"Folio: {_receta.ID_Cita}").Bold();
                        col.Item().Text($"Fecha: {_cita.Fecha_Cita.ToString("dd/MM/yyyy")}");
                        col.Item().Text($"Doctor: {_doctor.Usuario.Nombre + " " + _doctor.Usuario.Apellido_Paterno} Cédula: {_doctor.Cedula}");
                        col.Item().Text($"Paciente: {_paciente.Usuario.Nombre + " " + _paciente.Usuario.Apellido_Paterno} Edad: {edad}");
                        col.Item().Text($"Diagnóstico: {_receta.Diagnostico}").Italic();
                        col.Item().Text("Tratamientos:").Bold();

                        foreach (var tratamiento in _receta.Tratamiento)
                        {
                            col.Item().Text($"- {tratamiento.Medicamento}: {tratamiento.Indicaciones}");
                        }
                    });
            });
        }
    }
}
