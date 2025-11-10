using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Models
{
    public class RegistroPesoModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public double Peso { get; set; }
        public double Altura { get; set; }
        public double? IMCCalculado { get; set; }
        public string ClasificacionIMC { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
    }

}
