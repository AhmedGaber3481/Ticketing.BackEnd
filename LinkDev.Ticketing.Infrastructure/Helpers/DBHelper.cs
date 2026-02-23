using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LinkDev.Ticketing.Infrastructure.Helpers
{
    public class DBHelper
    {
        private string? connectionString = null;

        public DBHelper(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("TicketingConnection");
        }

        public SqlTransaction CreateDBTransaction()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn.BeginTransaction();
        }

        public SqlParameter CreateParam(string name, object value)
        {
            return new SqlParameter(name, value == null ? (object)DBNull.Value : value);
        }

        public object ExecuteScalar(string commandTxt, CommandType cmdType, params IDbDataParameter[] parameters)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(connectionString);
                cmd = conn.CreateCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = commandTxt;
               
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch(Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        public DataSet GetDataSetFromSP(SqlTransaction? trans, string cmdText,params SqlParameter[] parameters)
        {
            SqlConnection? conn = null;
            SqlCommand? cmd = null;
            try
            {
                if (trans != null)
                {
                    conn = trans.Connection;
                }
                else
                {
                    conn = new SqlConnection(connectionString);
                }

                cmd = conn.CreateCommand();
                //cmd = new SqlCommand();
                //cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = cmdText;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                DbDataAdapter adapter = new SqlDataAdapter();
                cmd.CommandTimeout = 1000000;
                adapter.SelectCommand = cmd;

                DataSet ds = new DataSet();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (trans == null)
                {
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                }
            }
        }
        public void ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] parameters)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {

                if (trans != null)
                {
                    conn = trans.Connection;
                }
                else
                {
                    conn = new SqlConnection(connectionString);
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                }
                cmd = conn.CreateCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;
                cmd.CommandTimeout = 1000000;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                if (trans != null)
                {
                    cmd.Transaction = trans;
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (trans == null)
                {
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                }
            }
        }
    }
}
