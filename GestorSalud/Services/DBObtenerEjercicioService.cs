using GestorSalud.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Services
{
    public class DBObtenerEjercicioService
    {
        string conexion = "server=localhost; user=root; database=salud; password= ; port=3306; ";

        public List<ActividadFisicaModel> ObtenerEjercicio(int usuarioId)
        {
            List<ActividadFisicaModel> ejercicios = new List<ActividadFisicaModel>();

            string consulta = @"
                                SELECT 
                                    us.nombre, 
                                    re.peso, 
                                    re.imc_calculado, 
                                    re.clasificacion_imc, 
                                    ce.nombre AS ejercicio_recomendado, 
                                    ce.intensidad, 
                                    ce.duracion_sugerida
                                FROM usuarios us
                                LEFT JOIN registros_peso re 
                                    ON us.id = re.usuario_id
                                LEFT JOIN catalogo_ejercicios ce
                                    ON ce.categoria_imc = re.clasificacion_imc
                                WHERE us.id = @usuarioId";

            using (MySqlConnection conn = new MySqlConnection(conexion))
            using (MySqlCommand cmd = new MySqlCommand(consulta, conn))
            {
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ejercicios.Add(new ActividadFisicaModel()
                        {
                            ClasificacionImc = reader["clasificacion_imc"].ToString(),
                            EjercicioRecomendado = reader["ejercicio_recomendado"].ToString(),
                            Intensidad = reader["intensidad"].ToString(),
                            DuracionSugerida = Convert.ToInt32(reader["duracion_sugerida"]),
                        });
                    }
                }
            }

            return ejercicios;
        }
    }

}
