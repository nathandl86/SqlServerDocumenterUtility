using SqlServerDocumenterUtility.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlServerDocumenterUtility.Data
{
    /// <summary>
    /// Helper class for abstracting the guts of common database interactions when 
    /// creating new Dal class allowing easy reuse and preventing largs amounts
    /// of code doing very similar things.
    /// </summary>
    public static class DalHelper
    {
        /// <summary>
        /// Method to handle inserts
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Nullable Int indicating the number of affected rows</returns>
        public static int? Insert(DalHelperModel dto)
        {
            try
            {
                using (var cmd = BuildCommand(dto))
                {
                    if (dto.Parameters != null)
                    {
                        foreach (var parameter in dto.Parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    dto.Connection.Open();

                    var result = cmd.ExecuteScalar();

                    int newId;
                    if (result != null && int.TryParse(result.ToString(), out newId))
                    {
                        return newId;
                    }
                }
            }
            finally
            {
                if (dto.Connection.State == ConnectionState.Open)
                {
                    dto.Connection.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Method handling updates
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Boolean indicating success or failure of the update operation</returns>
        public static bool Update(DalHelperModel dto)
        {
            try
            {
                using (var cmd = BuildCommand(dto))
                {
                    if (dto.Parameters != null)
                    {
                        foreach (var parameter in dto.Parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    dto.Connection.Open();

                    var result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            finally
            {
                if (dto.Connection.State == ConnectionState.Open)
                {
                    dto.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Method handling deletion of a row.
        /// </summary>
        /// <param name="dto"></param>
        public static void Delete(DalHelperModel dto)
        {
            try
            {
                using (var cmd = BuildCommand(dto))
                {
                    if (dto.Parameters != null)
                    {
                        foreach (var parameter in dto.Parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    dto.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (dto.Connection.State == ConnectionState.Open)
                {
                    dto.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Method handling the retrieval of 0 to many records based
        /// on the parameters. 
        /// </summary>
        /// <typeparam name="T">Type of the items in the IList returned</typeparam>
        /// <param name="dto">Contains information needed to make the database call and a mapper
        ///     used to convert the sql data records to the list
        /// </param>
        /// <returns></returns>
        public static IList<T> RetrieveList<T>(DalHelperModel<T> dto)
        {
            var items = new List<T>();
            try
            {
                using (var cmd = BuildCommand(dto))
                {
                    if (dto.Parameters != null)
                    {
                        foreach (var parameter in dto.Parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    dto.Connection.Open();

                    if (dto.Mapper != null)
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    items.Add(dto.Mapper(reader));
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No usable mapper was defined for the retrieve.");
                    }
                }
            }
            finally
            {
                if (dto.Connection.State == ConnectionState.Open)
                {
                    dto.Connection.Close();
                }
            }
            return items;
        }       
        
        /// <summary>
        /// Method to build the SqlCommand and configure it as a procedure call or command text.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>SqlCommand with a configured command type</returns>
        private static SqlCommand BuildCommand(DalHelperModel dto)
        {
            if (!String.IsNullOrWhiteSpace(dto.CommandText))
            {
                return new SqlCommand(dto.CommandText, dto.Connection) { CommandType = CommandType.Text };
            }

            if (!String.IsNullOrWhiteSpace(dto.ProcName))
            {
                return new SqlCommand(dto.ProcName, dto.Connection) { CommandType = CommandType.StoredProcedure };
            }

            throw new Exception("No command text or stored procedure name found in arguments");
        }
    }
}
