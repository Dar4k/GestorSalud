using GestorSalud.Models;
using GestorSalud.Services;
using System;

namespace GestorSalud.Controllers
{
    public class RegistroPesoController
    {
        private DBRegistroPesoService dbRegistroPeso = new DBRegistroPesoService();

        public bool GuardarRegistroPeso(int usuarioId, double peso, double altura, DateTime fechaRegistro, string notas = "")
        {
            // Calcular IMC
            double imc = CalcularIMC(peso, altura);
            string clasificacion = ClasificarIMC(imc);

            var registro = new RegistroPesoModel
            {
                UsuarioId = usuarioId,
                Peso = peso,
                Altura = altura,
                IMCCalculado = imc,
                ClasificacionIMC = clasificacion,
                FechaRegistro = fechaRegistro,
                Notas = notas
            };

            return dbRegistroPeso.GuardarRegistroPeso(registro);
        }

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
                Notas = "Registro automático desde calculadora IMC"
            };

            return dbRegistroPeso.GuardarRegistroPeso(registro);
        }

        public double CalcularIMC(double peso, double altura)
        {
            return Math.Round(peso / (altura * altura), 2);
        }

        public string ClasificarIMC(double imc)
        {
            if (imc < 18.5) return "Bajo peso";
            else if (imc >= 18.5 && imc <= 24.9) return "Normal";
            else if (imc >= 25 && imc <= 29.9) return "Sobrepeso";
            else if (imc >= 30 && imc <= 34.9) return "Obesidad I";
            else if (imc >= 35 && imc <= 39.9) return "Obesidad II";
            else return "Obesidad III";
        }

        public RegistroPesoModel ObtenerUltimoRegistro(int usuarioId)
        {
            return dbRegistroPeso.ObtenerUltimoRegistro(usuarioId);
        }
    }
}
