using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorSalud.Models;
using GestorSalud.Services;

namespace GestorSalud.Controllers
{
    public class PerfilSaludController
    {
        private DBPerfilSaludService dbPerfilSalud = new DBPerfilSaludService();

        public bool GuardarPerfilSalud(PerfilSaludModel perfil)
        {
            // Verificar si ya existe un perfil para este usuario
            var perfilExistente = dbPerfilSalud.ObtenerPerfilSalud(perfil.UsuarioId);

            if (perfilExistente != null)
            {
                // Actualizar perfil existente
                perfil.Id = perfilExistente.Id;
                return dbPerfilSalud.ActualizarPerfilSalud(perfil);
            }
            else
            {
                // Crear nuevo perfil
                return dbPerfilSalud.GuardarPerfilSalud(perfil);
            }
        }

        public PerfilSaludModel ObtenerPerfilSalud(int usuarioId)
        {
            return dbPerfilSalud.ObtenerPerfilSalud(usuarioId);
        }
    }
}
