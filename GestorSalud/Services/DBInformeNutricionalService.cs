using GestorSalud.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Services
{
    public class DBInformeNutricionalService
    {
        string conexion = "server=localhost; user=root; database=salud; password= ; port=3306; ";

        public int ObtenerTotalCaloriasHoy(int usuarioId)
        {
            const string query = @"
                SELECT COALESCE(SUM(calorias), 0)
                FROM registro_comidas
                WHERE usuario_id = @usuario_id AND DATE(fecha_comida) = CURDATE()";

            using var conn = new MySqlConnection(conexion);
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@usuario_id", usuarioId);
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public (double? imc, string clasificacion) ObtenerUltimoIMC(int usuarioId)
        {
            const string query = @"
                SELECT imc_calculado, clasificacion_imc
                FROM registros_peso
                WHERE usuario_id = @usuario_id
                ORDER BY fecha_registro DESC, id DESC
                LIMIT 1";

            using var conn = new MySqlConnection(conexion);
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@usuario_id", usuarioId);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var imc = reader.IsDBNull("imc_calculado") ? (double?)null : reader.GetDouble("imc_calculado");
                var clasif = reader.IsDBNull("clasificacion_imc") ? "Sin clasificación" : reader.GetString("clasificacion_imc");
                return (imc, clasif);
            }
            return (null, "Sin datos");
        }
    }
}
