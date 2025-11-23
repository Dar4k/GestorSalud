using GestorSalud.Models;
using GestorSalud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Controllers
{
    public class InformeNutricionalController
    {
        private readonly DBInformeNutricionalService _service = new DBInformeNutricionalService();

        public InformeNutricionalModel GenerarInforme(int usuarioId)
        {
            var totalCalorias = _service.ObtenerTotalCaloriasHoy(usuarioId);
            var (imc, clasif) = _service.ObtenerUltimoIMC(usuarioId);

            
            string recomendacion = "Registra tus comidas para ver recomendaciones.";
            if (imc.HasValue)
            {
                if (imc > 25 && totalCalorias > 2000)
                    recomendacion = "⚠️ Tu IMC indica sobrepeso y estás consumiendo más de 2000 kcal. Considera reducir tu ingesta.";
                else if (imc < 18.5)
                    recomendacion = "⚠️ Tu IMC indica bajo peso. Asegúrate de consumir suficientes calorías.";
                else
                    recomendacion = "✅ Tu ingesta calórica está alineada con un IMC saludable.";
            }

            return new InformeNutricionalModel
            {
                TotalCaloriasHoy = totalCalorias,
                UltimoIMC = imc,
                ClasificacionIMC = clasif,
                Recomendacion = recomendacion
            };
        }
    }
}

