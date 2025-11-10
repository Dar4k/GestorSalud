using GestorSalud.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestorSalud.Services
{
    public class DBRegistroPesoService
    {
        string conexion = "server=localhost; user=root; database=salud; password= ; port=3306; ";

        public bool GuardarRegistroPeso(RegistroPesoModel registro)
        {
            try
            {
                string query = @"INSERT INTO registros_peso 
                       (usuario_id, fecha_registro, peso, altura, imc_calculado, clasificacion_imc, notas) 
                       VALUES (@usuario_id, @fecha_registro, @peso, @altura, @imc_calculado, @clasificacion_imc, @notas)";

                using (MySqlConnection conn = new MySqlConnection(conexion))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario_id", registro.UsuarioId);
                    cmd.Parameters.AddWithValue("@fecha_registro", registro.FechaRegistro);
                    cmd.Parameters.AddWithValue("@peso", registro.Peso);
                    cmd.Parameters.AddWithValue("@altura", registro.Altura);
                    cmd.Parameters.AddWithValue("@imc_calculado", registro.IMCCalculado);
                    cmd.Parameters.AddWithValue("@clasificacion_imc", registro.ClasificacionIMC);
                    cmd.Parameters.AddWithValue("@notas", registro.Notas);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar registro: {ex.Message}", "Error BD", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public RegistroPesoModel ObtenerUltimoRegistro(int usuarioId)
        {
            try
            {
                string query = @"SELECT * FROM registros_peso 
                        WHERE usuario_id = @usuario_id 
                        ORDER BY fecha_registro DESC, id DESC 
                        LIMIT 1";

                using (MySqlConnection conn = new MySqlConnection(conexion))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new RegistroPesoModel
                            {
                                Id = reader.GetInt32("id"),
                                UsuarioId = reader.GetInt32("usuario_id"),
                                FechaRegistro = reader.GetDateTime("fecha_registro"),
                                Peso = reader.GetDouble("peso"),
                                Altura = reader.GetDouble("altura"),
                                IMCCalculado = reader.GetDouble("imc_calculado"),
                                ClasificacionIMC = reader.GetString("clasificacion_imc"),
                                Notas = reader.IsDBNull(reader.GetOrdinal("notas")) ? "" : reader.GetString("notas")
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener último registro: {ex.Message}", "Error BD", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
