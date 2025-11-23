using GestorSalud.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace GestorSalud.Services
{
    public class DBRegistroComidaService
    {
        string conexion = "server=localhost; user=root; database=salud; password= ; port=3306; ";

        public bool GuardarRegistroComida(RegistroComidaModel registro)
        {
            const string query = @"
                INSERT INTO registro_comidas 
                (usuario_id, fecha_comida, tipo_comida, alimentos, calorias, notas)
                VALUES (@usuario_id, @fecha_comida, @tipo_comida, @alimentos, @calorias, @notas)";

            try
            {
                using var connection = new MySqlConnection(conexion);
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@usuario_id", registro.UsuarioId);
                command.Parameters.AddWithValue("@fecha_comida", registro.FechaComida);
                command.Parameters.AddWithValue("@tipo_comida", registro.TipoComida);
                command.Parameters.AddWithValue("@alimentos", registro.Alimentos ?? "");
                command.Parameters.AddWithValue("@calorias", registro.Calorias ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@notas", registro.Notas ?? "");

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        public List<RegistroComidaModel> ObtenerComidasHoy(int usuarioId)
        {
            var registros = new List<RegistroComidaModel>();
            var fechaHoy = DateTime.Today;

            const string query = @"
                SELECT id, usuario_id, fecha_comida, tipo_comida, alimentos, calorias, notas
                FROM registro_comidas
                WHERE usuario_id = @usuario_id AND DATE(fecha_comida) = @fecha_hoy
                ORDER BY FIELD(tipo_comida, 'Desayuno', 'Almuerzo', 'Cena', 'Snack')";

            try
            {
                using var connection = new MySqlConnection(conexion);
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@usuario_id", usuarioId);
                command.Parameters.AddWithValue("@fecha_hoy", fechaHoy);

                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    registros.Add(new RegistroComidaModel
                    {
                        Id = reader.GetInt32("id"),
                        UsuarioId = reader.GetInt32("usuario_id"),
                        FechaComida = reader.GetDateTime("fecha_comida"),
                        TipoComida = reader.GetString("tipo_comida"),
                        Alimentos = reader.IsDBNull("alimentos") ? "" : reader.GetString("alimentos"),
                        Calorias = reader.IsDBNull("calorias") ? null : reader.GetInt32("calorias"),
                        Notas = reader.IsDBNull("notas") ? "" : reader.GetString("notas")
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener comidas: {ex.Message}", "Error BD",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<RegistroComidaModel>();
            }
            return registros;
        }

        public bool ActualizarRegistroComida(RegistroComidaModel registro)
        {
            const string query = @"
                UPDATE registro_comidas 
                SET tipo_comida = @tipo_comida, alimentos = @alimentos, calorias = @calorias, notas = @notas
                WHERE id = @id AND usuario_id = @usuario_id";

            try
            {
                using var connection = new MySqlConnection(conexion);
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", registro.Id);
                command.Parameters.AddWithValue("@usuario_id", registro.UsuarioId);
                command.Parameters.AddWithValue("@tipo_comida", registro.TipoComida);
                command.Parameters.AddWithValue("@alimentos", registro.Alimentos ?? "");
                command.Parameters.AddWithValue("@calorias", registro.Calorias ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@notas", registro.Notas ?? "");

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarRegistroComida(int id, int usuarioId)
        {
            const string query = "DELETE FROM registro_comidas WHERE id = @id AND usuario_id = @usuario_id";
            try
            {
                using var connection = new MySqlConnection(conexion);
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@usuario_id", usuarioId);
                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
