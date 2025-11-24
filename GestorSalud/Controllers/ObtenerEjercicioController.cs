using GestorSalud.Models;
using GestorSalud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Controllers
{
    class ObtenerEjercicioController
    {
       private DBObtenerEjercicioService dbEjercicio = new DBObtenerEjercicioService();
       public List<ActividadFisicaModel> ObtenerEjerciciosPorCategoria(int usuarioId)
       {
           return dbEjercicio.ObtenerEjercicio(usuarioId);
        }
    }
}
