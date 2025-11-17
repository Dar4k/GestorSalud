using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Models
{
    public class HidratacionModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int VasosAgua { get; set; }
        public string OtrosLiquidos { get; set; }
        public int TotalMl { get; set; }
    }
}
