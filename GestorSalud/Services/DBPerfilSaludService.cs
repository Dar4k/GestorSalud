using GestorSalud.Models;
using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace GestorSalud.Services
{
    public class DBPerfilSaludService
    {
        string conexion = "server=localhost; user=root; database=salud; password= ; port=3306; ";

        public bool GuardarPerfilSalud(PerfilSaludModel perfil)
        {
            try
            {
                // Primero verificar si ya existe un perfil para este usuario
                var perfilExistente = ObtenerPerfilSalud(perfil.UsuarioId);

                if (perfilExistente != null)
                {
                    // ACTUALIZAR perfil existente
                    return ActualizarPerfilSalud(perfil);
                }
                else
                {
                    // INSERTAR nuevo perfil
                    string query = @"INSERT INTO perfiles_salud 
                           (usuario_id, fecha_nacimiento, genero, altura, objetivo_peso, 
                            nivel_actividad, condiciones_medicas) 
                           VALUES (@usuario_id, @fecha_nacimiento, @genero, @altura, @objetivo_peso, 
                                   @nivel_actividad, @condiciones_medicas)";

                    using (MySqlConnection conn = new MySqlConnection(conexion))
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario_id", perfil.UsuarioId);
                        cmd.Parameters.AddWithValue("@fecha_nacimiento", perfil.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@genero", perfil.Genero);
                        cmd.Parameters.AddWithValue("@altura", perfil.Altura);
                        cmd.Parameters.AddWithValue("@objetivo_peso", perfil.ObjetivoPeso ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@nivel_actividad", perfil.NivelActividad);
                        cmd.Parameters.AddWithValue("@condiciones_medicas", perfil.CondicionesMedicas);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar perfil: {ex.Message}", "Error BD", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public PerfilSaludModel ObtenerPerfilSalud(int usuarioId)
        {
            try
            {
                string query = @"SELECT * FROM perfiles_salud 
                        WHERE usuario_id = @usuario_id 
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
                            return new PerfilSaludModel
                            {
                                Id = reader.GetInt32("id"),
                                UsuarioId = reader.GetInt32("usuario_id"),
                                FechaNacimiento = reader.GetDateTime("fecha_nacimiento"),
                                Genero = reader.IsDBNull(reader.GetOrdinal("genero")) ? "" : reader.GetString("genero"),
                                Altura = reader.GetDecimal("altura"),
                                ObjetivoPeso = reader.IsDBNull(reader.GetOrdinal("objetivo_peso")) ? null : reader.GetDecimal("objetivo_peso"),
                                NivelActividad = reader.IsDBNull(reader.GetOrdinal("nivel_actividad")) ? "" : reader.GetString("nivel_actividad"),
                                CondicionesMedicas = reader.IsDBNull(reader.GetOrdinal("condiciones_medicas")) ? "" : reader.GetString("condiciones_medicas")
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener perfil: {ex.Message}", "Error BD", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public bool ActualizarPerfilSalud(PerfilSaludModel perfil)
        {
            try
            {
                string query = @"UPDATE perfiles_salud SET 
                        fecha_nacimiento = @fecha_nacimiento,
                        genero = @genero,
                        altura = @altura,
                        objetivo_peso = @objetivo_peso,
                        nivel_actividad = @nivel_actividad,
                        condiciones_medicas = @condiciones_medicas
                        WHERE id = @id";

                using (MySqlConnection conn = new MySqlConnection(conexion))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", perfil.Id);
                    cmd.Parameters.AddWithValue("@fecha_nacimiento", perfil.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@genero", perfil.Genero);
                    cmd.Parameters.AddWithValue("@altura", perfil.Altura);
                    cmd.Parameters.AddWithValue("@objetivo_peso", perfil.ObjetivoPeso ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nivel_actividad", perfil.NivelActividad);
                    cmd.Parameters.AddWithValue("@condiciones_medicas", perfil.CondicionesMedicas);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar perfil: {ex.Message}", "Error BD", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}