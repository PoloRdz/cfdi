using cfdi.Exceptions;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Data.DAO
{
    public class RolDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<Rol> getRoles()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_ROLES", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = null;
            var roles = new List<Rol>();
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se han encontrado Roles");
                while (rdr.Read())
                    roles.Add(getRolesFromReader(rdr));
                return roles;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                rdr.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        private Rol getRolesFromReader(SqlDataReader rdr) {
            var rol = new Rol
            {
                id = rdr.GetInt32(0),
                rol = rdr.GetString(1),
                descripcion = rdr.GetString(2),
                identificador = rdr.GetString(3)
            };
            return rol;
        }
    }
}
