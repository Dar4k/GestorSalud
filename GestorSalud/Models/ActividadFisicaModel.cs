using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Models
{
    public class ActividadFisicaModel
    {
        public string ClasificacionImc { get; set; }
        public string EjercicioRecomendado { get; set; }
        public string Intensidad { get; set; }
        public int DuracionSugerida { get; set; }
    }
}
