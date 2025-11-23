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
            if (imc < 18.5)
                return "🔶 BAJO PESO\n💡 Consulta un nutricionista";
            else if (imc < 25)
                return "✅ PESO NORMAL\n¡Excelente! Mantén tus hábitos";
            else if (imc < 30)
                return "🔶 SOBREPESO\n💪 Más ejercicio y dieta balanceada";
            else
                return "🔴 OBESIDAD\n🏥 Consulta con un profesional de salud";
        }

        public RegistroPesoModel ObtenerUltimoRegistro(int usuarioId)
        {
            return dbRegistroPeso.ObtenerUltimoRegistro(usuarioId);
        }
    }
}
