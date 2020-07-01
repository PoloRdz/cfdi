using cfdi.Exceptions;
using cfdi.Models.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Data.DAO
{
    public class UserDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Usuario GetUsuario(string username, string correo)
        {
            Usuario usuario = new Usuario();
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USUARIO_INFO", cnn);
            SqlDataReader reader = null;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            //////////////////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_NOMBRE_USUARIO", username == null ? "" : username);
            cmd.Parameters.AddWithValue("@PP_CORREO", correo == null ? "" : correo);
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("El usuario no se encuentra registrado o esta desactivado");
                reader.Read();
                usuario.id = reader.GetInt32(0);
                usuario.usuario = reader.GetValue(1).ToString();
                usuario.correo = reader.GetValue(2).ToString();
                usuario.password = reader.GetValue(3).ToString();
                usuario.nombre = reader.GetValue(4).ToString();
                usuario.apellidoP = reader.GetValue(5).ToString();
                usuario.apellidoM = reader.GetValue(6).ToString();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                reader.Close();
                cnn.Dispose();
                cmd.Dispose();
            }
            return usuario;
        }
    }
}
