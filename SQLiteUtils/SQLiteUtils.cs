using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Data.SQLite;
using NLog;

namespace VS2005Commons
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLiteUtils : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private String connectionString;
        private SQLiteConnection conn;
        private SQLiteTransaction transaction;
        private bool passedConnection = false;
        private bool transactionOpen = false;

        public static int contatoreConnessioniAperte;

        [DebuggerStepThroughAttribute]
        public SQLiteUtils(String connectionString)
        {
            this.connectionString = connectionString;
        }

        [DebuggerStepThrough]
        public SQLiteUtils(SQLiteConnection conn)
        {
            this.conn = conn;
            passedConnection = true;
        }

        [DebuggerStepThroughAttribute]
        public SQLiteUtils(SQLiteConnection conn, SQLiteTransaction transaction)
            : this(conn)
        {
            // this.conn = conn;
            this.transaction = transaction;
            // passedConnection = true;
        }

        /// <summary>
        /// Returns a new SQLiteConnection instance
        /// </summary>
        // [DebuggerStepThroughAttribute]
        public SQLiteConnection GetConnection()
        {
            // TODO: RETRIVE CONNECTION STRING
            // SQLiteConnection retVal = new SQLiteConnection(this.connectionString);
            //SQLiteConnection retVal = new SQLiteConnection();
            if (conn == null)
            {
                this.conn = new SQLiteConnection(this.connectionString);
                //logger.Debug(connectionString);
                //logger.Debug("Opening connection ... ");
                //this.conn.Open();

            }
            return this.conn;
        }

        /// <summary>
        /// Returns a new SQLiteCommand instance
        /// </summary>
        /// <param name="query">The text of the query</param>
        public SQLiteCommand CreateCommand(String query)
        {
            // return (GetTransaction() != null) ? new SQLiteCommand(query, GetConnection(), GetTransaction()) : new SQLiteCommand(query, GetConnection());
            return new SQLiteCommand(query, GetConnection(), GetTransaction());
        }

        /// <summary>
        /// Run a SQL Command INSERT, UPDATE, DELETE
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        public int ExecuteCommand(SQLiteCommand cmd)
        {
            cmd.Connection = GetConnection();
            cmd.Transaction = GetTransaction();
            return ExecuteCommand(cmd, false);
        }

        /// <summary>
        /// Run a SQL Command INSERT, UPDATE, DELETE
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        /// <param name="keepOpenConnection">If false the connection will close</param>
        private int ExecuteCommand(SQLiteCommand cmd, bool keepOpenConnection)
        {
            int res = -1;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }

                logger.Debug("Executing");
                logger.Debug(cmd.CommandText);

                res = cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
            finally
            {
                if ((cmd.Connection.State == ConnectionState.Open) && (!keepOpenConnection) && (!passedConnection) && (!transactionOpen))
                {
                    cmd.Connection.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }

            return res;
        }

        /// <summary>
        /// Aggiunto da Mirco per leggere Dati
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public SQLiteDataReader ExecuteReader(SQLiteCommand cmd, bool keepOpenConnection)
        {
            SQLiteDataReader res = null;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }

                logger.Debug(cmd.CommandText);

                // cmd.Transaction = GetTransaction();

                // res = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                res = cmd.ExecuteReader();
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
            finally
            {
                if ((cmd.Connection.State == ConnectionState.Open) && (!keepOpenConnection) && (!passedConnection) && (!transactionOpen))
                {
                    cmd.Connection.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }

            return res;
        }

        /// <summary>
        /// Run a SELECT comand which return a single value
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        public object ExecuteScalar(SQLiteCommand cmd)
        {
            cmd.Connection = GetConnection();
            cmd.Transaction = GetTransaction();
            return ExecuteScalar(cmd, false);
        }

        /// <summary>
        /// Run a SELECT comand which return a single value
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        /// <param name="keepOpenConnection">If false the connection will close</param>
        private object ExecuteScalar(SQLiteCommand cmd, bool keepOpenConnection)
        {
            object ret = null;
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }
                ret = cmd.ExecuteScalar();
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
            finally
            {
                if ((cmd.Connection.State == ConnectionState.Open) && (!keepOpenConnection) && (!passedConnection) && (!transactionOpen))
                {
                    cmd.Connection.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }
            return ret;
        }


        /// <summary>
        /// Execute more SQLiteCommands into a SQLiteTransaction
        /// </summary>
        /// <param name="commands"></param>
        public void ExecuteTransactedCommand(params SQLiteCommand[] commands)
        {

            SQLiteConnection conn = GetConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
            }

            SQLiteTransaction trans = conn.BeginTransaction();

            try
            {
                foreach (SQLiteCommand cmd in commands)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = trans;
                    ExecuteCommand(cmd, true);
                }
                trans.Commit();
            }
            catch (SQLiteException ex)
            {
                try
                {
                    trans.Rollback();
                }
                finally
                {
                    throw ex;
                }
            }
            finally
            {
                conn.Close();
                contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                conn.Dispose();
            }
        }

        public bool ExecuteExist(SQLiteCommand cmd)
        {
            bool res;

            cmd.Connection = GetConnection();
            SQLiteDataReader dr = null;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }

                // dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dr = cmd.ExecuteReader();
                res = dr.Read();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                // TODO: MAKE LOG
                // Utils.ManageException("SqlCeUtils.ExecuteExist()", ex);
                res = false;
            }
            finally
            {
                if ((dr != null) && (!dr.IsClosed))
                {
                    dr.Close();
                }
                if (cmd.Connection.State == ConnectionState.Open && (!passedConnection) && (!transactionOpen))
                {
                    cmd.Connection.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }

            return res;
        }

        public static SQLiteParameter CreateParameter(string parameterName, object value)
        {
            SQLiteParameter ret = new SQLiteParameter();

            // ret = new SQLiteParameter("Test", SqlDbType.DateTime);

            ret.ParameterName = parameterName;
            if (value != null)
            {
                /*
                 * Gestisco le DateTime di C# per SQLServer
                 * In SQL non possono essere valorizzate come DateTime.MinDate quindi le considero un null
                 * 
                 */
                if (value is DateTime)
                {
                    // ret.SqlDbType = SqlDbType.DateTime;
                    DateTime valueDate = (DateTime)value;
                    if (valueDate == DateTime.MinValue)
                    {
                        ret.Value = DBNull.Value;
                    }
                    else
                    {
                        ret.Value = value;
                    }
                }
                else
                {
                    ret.Value = value;
                }
            }
            else
            {
                ret.Value = DBNull.Value;
            }
            return ret;
        }

        public int EmptyTable(string tableName)
        {
            String SQL = "DELETE FROM " + tableName;
            SQLiteCommand cmd = this.CreateCommand(SQL);
            return this.ExecuteCommand(cmd);
        }

        public SQLiteDataReader SelectAllFromTable(string tableName, bool keepOpenConnection)
        {
            String SQL = "SELECT * FROM " + tableName;
            SQLiteCommand cmd = this.CreateCommand(SQL);
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public SQLiteDataReader SelectAllFromTable(string tableName, string where, string sort, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT * FROM " + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            if (StringUtils.isNotEmpty(sort))
            {
                SQL.Append(" ORDER BY " + sort);
            }

            SQLiteCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public SQLiteDataReader SelectTop1FromTable(string tableName, string where, string sort, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.AppendFormat("SELECT * FROM {0} ", tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            if (StringUtils.isNotEmpty(sort))
            {
                SQL.Append(" ORDER BY " + sort);
            }
            SQL.AppendFormat(" LIMIT {0}", 1);

            SQLiteCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public SQLiteDataReader SelectTopNFromTable(string tableName, string where, string sort, int topN, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.AppendFormat("SELECT * FROM {0} ", tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            if (StringUtils.isNotEmpty(sort))
            {
                SQL.Append(" ORDER BY " + sort);
            }
            SQL.AppendFormat(" LIMIT {0}", topN);

            SQLiteCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        /// <summary>
        /// Per compatibilta con la versione SQLServer teniamo campoCount anche se non viene usato
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="campoCount"></param>
        /// <param name="where"></param>
        /// <param name="keepOpenConnection"></param>
        /// <returns></returns>
        public long SelectCountFromTable(string tableName, string campoCount, string where, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT COUNT(*) FROM " + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }

            SQLiteCommand cmd = this.CreateCommand(SQL.ToString());
            return Convert.ToInt64(this.ExecuteScalar(cmd, keepOpenConnection));
        }

        //public void EmptyTable(string tableName,
        //    SQLiteConnection conn)
        //{
        //    String SQL = "DELETE FROM " + tableName;
        //    SQLiteCommand cmd = this.CreateCommand(SQL);
        //    cmd.Connection = conn;
        //    this.ExecuteCommand(cmd, true);
        //}

        /// <summary>
        /// Attenzione questo forza la Chiusura anche se c'e' una Transazione Aperta
        /// </summary>
        // [DebuggerStepThroughAttribute]
        public void CloseConnection()
        {
            // If Not conn Is Nothing AndAlso conn.State = ConnectionState.Open Then
            if (this.conn != null && this.conn.State == ConnectionState.Open)
            {
                if (!passedConnection)
                {
                    conn.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }
        }

        //public static SQLiteParameter CreateParameter(String name, Object value)
        //{
        //    SQLiteParameter param = new SQLiteParameter();
        //    param.ParameterName = name;
        //    param.Value = value;

        //    return param;
        //}

        // [DebuggerStepThroughAttribute]
        public void BeginTransaction()
        {
            //if (transaction == null)
            //{
            if (GetConnection().State != ConnectionState.Open)
            {
                GetConnection().Open(); // Devo Aprire prima di Iniziare la Transazione
                contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
            }
            transaction = GetConnection().BeginTransaction();
            transactionOpen = true;
            //}
        }

        // [DebuggerStepThroughAttribute]
        public void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
                /*
                 * Ripristino da Mirco il 27/3 dopo un commit non posso fare read con stesso SQLLiteHelper
                 */
                transaction = null;
                transactionOpen = false;
            }
        }

        [DebuggerStepThroughAttribute]
        public void RollBack()
        {
            /* 
             * Aggiunto transactionOpen perche' così posso richiamarlo sempre questo metodo
             * anche se dovessi fare il commit e poi succede una eccezione per qualche altro motivo
             * questo non solleva piu' l'eccezione che la transazione e' gia stata chiusa
             */

            if (transaction != null && transactionOpen)
            {
                transaction.Rollback();
                transaction = null;
                transactionOpen = false;
            }
        }

        [DebuggerStepThroughAttribute]
        public SQLiteTransaction GetTransaction()
        {
            return transaction;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.CloseConnection();
        }

        #endregion
    }
}