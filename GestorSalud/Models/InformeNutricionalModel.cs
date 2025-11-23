using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Models
{
    public class InformeNutricionalModel
    {
        public int TotalCaloriasHoy { get; set; }
        public int MetaCalorica { get; set; } = 2000;
        public double? UltimoIMC { get; set; }
        public string ClasificacionIMC { get; set; } = "Sin datos";
        public string EstadoCalorias => TotalCaloriasHoy <= MetaCalorica ? "✅ Dentro de tu meta" : "⚠️ Superaste tu meta";
        public string Recomendacion { get; set; } = "Registra tus comidas para ver recomendaciones.";
    }
}

