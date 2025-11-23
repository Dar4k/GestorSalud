using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorSalud.Models;
using GestorSalud.Services;

namespace GestorSalud.Controllers
{
    public class RegistroComidaController
    {
        private readonly DBRegistroComidaService _service = new DBRegistroComidaService();

        public bool GuardarComida(RegistroComidaModel comida) => _service.GuardarRegistroComida(comida);
        public bool ActualizarComida(RegistroComidaModel comida) => _service.ActualizarRegistroComida(comida);
        public bool EliminarComida(int id, int usuarioId) => _service.EliminarRegistroComida(id, usuarioId);
        public List<RegistroComidaModel> ObtenerComidasHoy(int usuarioId) => _service.ObtenerComidasHoy(usuarioId);
    }
}
