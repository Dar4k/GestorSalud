using System;

namespace GestorSalud.Models
{
    public class PerfilSaludModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; } = string.Empty;
        public decimal Altura { get; set; }
        public decimal? ObjetivoPeso { get; set; }
        public string NivelActividad { get; set; } = string.Empty;
        public string CondicionesMedicas { get; set; } = string.Empty;
    }
}
//