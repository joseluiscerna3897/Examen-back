using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Utilities.Ado
{
    public class SQLServer : IDisposable
    {
        // Internal members
        protected string _connString = null;
        protected SqlConnection _conn = null;
        protected SqlTransaction _trans = null;
        protected bool _disposed = false;

        /// <summary>
        /// Sets or returns the connection string use by all instances of this class.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Returns the current SqlTransaction object or null if no transaction
        /// is in effect.
        /// </summary>
        public SqlTransaction Transaction { get { return _trans; } }

        /// <summary>
        /// Constructor using global connection string.
        /// </summary>
        public SQLServer()
        {
            _connString = ConnectionString;
            Connect().Wait();
        }

        /// <summary>
        /// Constructure using connection string override
        /// </summary>
        /// <param name="connString">Connection string for this instance</param>
        public SQLServer(string connString)
        {
            _connString = connString;
            Connect().Wait();
        }

        public SQLServer(SqlConnection Connection)
        {
            _connString = Connection.ConnectionString;
            _conn = Connection;
        }

        // Creates a SqlConnection using the current connection string
        protected async Task Connect()
        {
            _conn = new SqlConnection(_connString);
            await _conn.OpenAsync();
        }

        /// <summary>
        /// Constructs a SqlCommand with the given parameters. This method is normally called
        /// from the other methods and not called directly. But here it is if you need access
        /// to it.
        /// </summary>
        /// <param name="qry">SQL query or stored procedure name</param>
        /// <param name="type">Type of SQL command</param>
        /// <param name="args">Query arguments. Arguments should be in pairs where one is the
        /// name of the parameter and the second is the value. The very last argument can
        /// optionally be a SqlParameter object for specifying a custom argument type</param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string qry, CommandType type, params object[] args)
        {
            SqlCommand cmd = new SqlCommand(qry, _conn);

            // Associate with current transaction, if any
            if (_trans != null)
                cmd.Transaction = _trans;

            // Set command type
            cmd.CommandType = type;

            // Construct SQL parameters
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is string && i < (args.Length - 1))
                    {
                        SqlParameter parm = new SqlParameter();
                        parm.ParameterName = (string)args[i];
                        parm.Value = args[++i];
                        cmd.Parameters.Add(parm);
                    }
                    else if (args[i] is SqlParameter)
                    {
                        cmd.Parameters.Add((SqlParameter)args[i]);
                    }
                    else throw new ArgumentException("Invalid number or type of arguments supplied");
                }
            }
            return cmd;
        }

        public SqlCommand CreateCommandTransaction(string qry, CommandType type, params object[] args)
        {
            SqlCommand cmd = new SqlCommand(qry, _conn);
            cmd.Transaction = _trans;

            // Set command type
            cmd.CommandType = type;

            // Construct SQL parameters
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is string && i < (args.Length - 1))
                    {
                        SqlParameter parm = new SqlParameter();
                        parm.ParameterName = (string)args[i];
                        parm.Value = args[++i];
                        cmd.Parameters.Add(parm);
                    }
                    else if (args[i] is SqlParameter)
                    {
                        cmd.Parameters.Add((SqlParameter)args[i]);
                    }
                    else throw new ArgumentException("Invalid number or type of arguments supplied");
                }
            }
            return cmd;
        }

        public SqlConnection GetConnection()
        {
            return _conn;
        }

        #region Exec Members

        /// <summary>
        /// Executes a query that returns no results
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>The number of rows affected</returns>
        public async Task<int> ExecNonQuery(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> ExecNonQueryProcTransaction(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommandTransaction(qry, CommandType.StoredProcedure, args))
            {
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task ExecNonQueryBulkCopy(List<SqlBulkCopyColumnMapping> columnsmap, string table, DataTable dt)
        {
            using (SqlBulkCopy copy = new SqlBulkCopy(GetConnection()))
            {
                foreach (SqlBulkCopyColumnMapping itm in columnsmap)
                {
                    copy.ColumnMappings.Add(itm);
                }
                copy.DestinationTableName = table;
                await copy.WriteToServerAsync(dt);
            }

        }

        /// <summary>
        /// Executes a stored procedure that returns no results
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>The number of rows affected</returns>
        public async Task<int> ExecNonQueryProc(string proc, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(proc, CommandType.StoredProcedure, args))
            {
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public async Task<object> ExecScalar(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                return await cmd.ExecuteScalarAsync();
            }
        }

        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public async Task<object> ExecScalarProc(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
            {
                return await cmd.ExecuteScalarAsync();
            }
        }

        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public async Task<object> ExecScalarProcTransaction(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommandTransaction(qry, CommandType.StoredProcedure, args))
            {
                return await cmd.ExecuteScalarAsync();
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a SqlDataReader
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a SqlDataReader</returns>
        public async Task<SqlDataReader> ExecDataReader(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                return await cmd.ExecuteReaderAsync();
            }
        }

        public async Task<SqlDataReader> ExecDataReaderTransaction(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommandTransaction(qry, CommandType.Text, args))
            {
                return await cmd.ExecuteReaderAsync();
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the results as a SqlDataReader
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a SqlDataReader</returns>
        public async Task<SqlDataReader> ExecDataReaderProc(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
            {
                return await cmd.ExecuteReaderAsync();
            }
        }

        public async Task<SqlDataReader> ExecDataReaderProcTrans(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommandTransaction(qry, CommandType.StoredProcedure, args))
            {
                return await cmd.ExecuteReaderAsync();
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a DataSet
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataSet</returns>
        public DataSet ExecDataSet(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                return ds;
            }
        }



        /// <summary>
        /// Executes a stored procedure and returns the results as a Data Set
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataSet</returns>
        public async Task<DataSet> ExecDataSetProc(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommandTransaction(qry, CommandType.StoredProcedure, args))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                await Task.Run(() => adapt.Fill(ds));
                return ds;
            }
        }

        #endregion

        #region Transaction Members

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <returns>The new SqlTransaction object</returns>
        public async Task<SqlTransaction> BeginTransaction()
        {
            await Rollback();
            _trans = await Task.Run<SqlTransaction>(() => _conn.BeginTransaction());
            return Transaction;
        }

        /// <summary>
        /// Commits any transaction in effect.
        /// </summary>
        public async Task Commit()
        {
            if (_trans != null)
            {
                await Task.Run(() => _trans.Commit());
                _trans = null;
            }
        }

        /// <summary>
        /// Rolls back any transaction in effect.
        /// </summary>
        public async Task Rollback()
        {
            if (_trans != null)
            {
                await Task.Run(() => _trans.Rollback());
                _trans = null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true).Wait();
            GC.SuppressFinalize(this);
        }

        protected virtual async Task Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Need to dispose managed resources if being called manually
                if (disposing)
                {
                    if (_conn != null)
                    {
                        await Rollback();
                        await Task.Run(() => _conn.Dispose());
                        _conn = null;
                    }
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
