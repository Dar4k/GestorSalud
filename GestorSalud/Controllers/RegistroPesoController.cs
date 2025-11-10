using GestorSalud.Models;
using GestorSalud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Controllers
{
    public class RegistroPesoController
    {
        private DBRegistroPesoService dbRegistroPeso = new DBRegistroPesoService();

        public bool GuardarRegistroIMC(int usuarioId, double peso, double altura, double imc, string clasificacion)
        {
            var registro = new RegistroPesoModel
            {
                UsuarioId = usuarioId,
                Peso = peso,
                Altura = altura,
                IMCCalculado = imc,
                ClasificacionIMC = clasificacion,
                FechaRegistro = DateTime.Now,
                Notas = $"Registro automático desde calculadora IMC"
            };

            return dbRegistroPeso.GuardarRegistroPeso(registro);
        }

        public RegistroPesoModel ObtenerUltimoRegistro(int usuarioId)
        {
            return dbRegistroPeso.ObtenerUltimoRegistro(usuarioId);
        }
    }
}
