using GestorSalud.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Services
{
    public class DBHidratacionService
    {
        string conexion = "server=localhost; user=root; database=salud; password= ; port=3306; ";
        public bool RegistrarHidratacion(HidratacionModel hidratacion)
        {
            string procedimiento = "INSERT INTO registro_hidratacion (usuario_id, fecha_registro, vasos_agua, otros_liquidos, total_ml) VALUES (@usuario_id, @fecha_registro, @vasos_agua, @otros_liquidos, @total_ml)";

            using (MySqlConnection mySqlConnection = new MySqlConnection(conexion))
            {
                using (MySqlCommand command = new MySqlCommand(procedimiento, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@usuario_id", hidratacion.UsuarioId);
                    command.Parameters.AddWithValue("@fecha_registro", DateTime.Now);
                    command.Parameters.AddWithValue("@vasos_agua", hidratacion.VasosAgua);
                    command.Parameters.AddWithValue("@otros_liquidos", hidratacion.OtrosLiquidos);
                    command.Parameters.AddWithValue("@total_ml", hidratacion.TotalMl);

                    mySqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool EliminarHidratacion(int id)
        {
            using var conn = new MySqlConnection(conexion);
            conn.Open();

            string sql = "DELETE FROM registro_hidratacion WHERE id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public bool EditarHidratacion(HidratacionModel hidratacion)
        {
            using var conn = new MySqlConnection(conexion);
            conn.Open();

            string sql = "UPDATE registro_hidratacion SET vasos_agua=@v, otros_liquidos=@o, total_ml=@t WHERE usuario_id=@id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@v", hidratacion.VasosAgua);
            cmd.Parameters.AddWithValue("@o", hidratacion.OtrosLiquidos);
            cmd.Parameters.AddWithValue("@t", hidratacion.TotalMl);
            cmd.Parameters.AddWithValue("@id", hidratacion.Id);

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public List<HidratacionModel> ObtenerHidratacionPorUsuario(int usuarioId)
        {
            List<HidratacionModel> listaHidratacion = new List<HidratacionModel>();

            using var conn = new MySqlConnection(conexion);
            conn.Open();

            string query = "SELECT * FROM registro_hidratacion WHERE usuario_id = @usuarioId ORDER BY fecha_registro DESC";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var hidratacion = new HidratacionModel
                        {
                            Id = reader.GetInt32("id"),
                            UsuarioId = reader.GetInt32("usuario_id"),
                            FechaRegistro = reader.GetDateTime("fecha_registro"),
                            VasosAgua = reader.GetInt32("vasos_agua"),
                            OtrosLiquidos = reader.IsDBNull(reader.GetOrdinal("otros_liquidos")) ? string.Empty : reader.GetString("otros_liquidos"),
                            TotalMl = reader.GetInt32("total_ml")
                        };

                        listaHidratacion.Add(hidratacion);
                    }
                }
            }
            return listaHidratacion;
        }
    }
}
