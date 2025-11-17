using GestorSalud.Models;
using GestorSalud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Controllers
{
    public class HidratacionController
    {
        DBHidratacionService dbHidratacionService = new DBHidratacionService();
        public bool RegistrarHidratacion(HidratacionModel hidratacion)
        {
            return dbHidratacionService.RegistrarHidratacion(hidratacion);
        }

        public List<HidratacionModel> ObtenerHidratacionPorUsuario(int usuarioId)
        {
            return dbHidratacionService.ObtenerHidratacionPorUsuario(usuarioId);
        }

        public bool ActualizarHidratacion(HidratacionModel hidratacion)
        {
            return dbHidratacionService.EditarHidratacion(hidratacion);
        }

        public bool EliminarHidratacion(int Id)
        {
            return dbHidratacionService.EliminarHidratacion(Id);
        }
    }
}
