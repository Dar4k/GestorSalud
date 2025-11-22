using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Models
{
    public class RegistroComidaModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaComida { get; set; } = DateTime.Now;
        public string TipoComida { get; set; } = "Desayuno";
        public string Alimentos { get; set; } = string.Empty;
        public int? Calorias { get; set; }
        public string Notas { get; set; } = string.Empty;
    }
}
