using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

/// <summary>
/// Summary description for DBL
/// </summary>

public sealed class DBL
{
    public DBL() { }
    #region staticMethods
    private static void attachParameters(SqlCommand command, SqlParameter[] parameters)
    {
        if (command == null) throw new ArgumentNullException("command");
        if (parameters != null)
        {
            foreach (SqlParameter p in parameters)
            {
                if (p != null)
                {
                    if ((p.Direction == ParameterDirection.InputOutput ||
                        p.Direction == ParameterDirection.Input) &&
                        (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    command.Parameters.Add(p);
                }
            }
        }
    }
    private static void prepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
    {
        if (command == null) throw new ArgumentNullException("command");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
        if (connection.State != ConnectionState.Open)
        {
            mustCloseConnection = true;
            connection.Open();
        }
        else
        {
            mustCloseConnection = false;
        }
        command.Connection = connection;
        command.CommandText = commandText;
        if (transaction != null)
        {
            if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            command.Transaction = transaction;
        }
        command.CommandType = commandType;
        if (commandParameters != null)
        {
            attachParameters(command, commandParameters);
        }
        return;
    }
    private static void assignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
    {
        if ((commandParameters == null) || (parameterValues == null))
        {
            return;
        }
        if (commandParameters.Length != parameterValues.Length)
        {
            throw new ArgumentException("Parameter count does not match Parameter Value count.");
        }
        for (int i = 0, j = commandParameters.Length; i < j; i++)
        {
            if (parameterValues[i] is IDbDataParameter)
            {
                IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                if (paramInstance.Value == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = paramInstance.Value;
                }
            }
            else if (parameterValues[i] == null)
            {
                commandParameters[i].Value = DBNull.Value;
            }
            else
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }
    }
    #endregion staticMethods
    #region executeNonQuery
    public static int executeNonQuery(SqlConnection connection, CommandType commandType, string command, params SqlParameter[] commandParameters)
    {
        int _iResult = 0;
        if (connection == null) throw new ArgumentNullException("connection");
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        prepareCommand(cmd, connection, (SqlTransaction)null, commandType, command, commandParameters, out mustCloseConnection);
        _iResult = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        if (mustCloseConnection)
        {
            connection.Close();
        }
        return _iResult;
    }
    #endregion executeNonQuery

    #region executeDataSet
    public static DataSet executeDataSet(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return executeDataset(connection, commandType, commandText, commandParameters);
            //connection.Close();
        }
    }
    private static DataSet executeDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        cmd.CommandTimeout = 60;
        prepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return ds;
        }
    }
    #endregion executeDataSet
    #region executeDataReader
    private enum SqlConnectionOwnership
    {
        Internal,
        External
    }
    private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        bool mustCloseConnection = false;
        // Create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        try
        {
            prepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            SqlDataReader dataReader;
            // Call ExecuteReader with the appropriate CommandBehavior
            if (connectionOwnership == SqlConnectionOwnership.External)
            {
                dataReader = cmd.ExecuteReader();
            }
            else
            {
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            // Detach the SqlParameters from the command object, so they can be used again.
            // HACK: There is a problem here, the output parameter values are fletched 
            // when the reader is closed, so if the parameters are detached from the command
            // then the SqlReader can´t set its values. 
            // When this happen, the parameters can´t be used again in other command.
            bool canClear = true;
            foreach (SqlParameter commandParameter in cmd.Parameters)
            {
                if (commandParameter.Direction != ParameterDirection.Input)
                    canClear = false;
            }
            if (canClear)
            {
                cmd.Parameters.Clear();
            }
            return dataReader;
        }
        catch
        {
            if (mustCloseConnection)
                connection.Close();
            throw;
        }
    }
    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of SqlParameters
        return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
    }
    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(connectionString);
            connection.Open();

            // Call the private overload that takes an internally owned connection in place of the connection string
            //2
            return ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
        }
        catch
        {
            // If we fail to return the SqlDatReader, we need to close the connection ourselves
            if (connection != null) connection.Close();
            throw;
        }
    }
    private static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        // If we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            assignParameterValues(commandParameters, parameterValues);


            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // Otherwise we can just call the SP without params
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }
    }
    private static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of SqlParameters
        return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
    }
    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        // Pass through the call to the private overload using a null transaction value and an externally owned connection
        return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
    }
    private static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        // If we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            assignParameterValues(commandParameters, parameterValues);

            return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // Otherwise we can just call the SP without params
            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }
    }
    private static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of SqlParameters
        return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
    }
    private static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        // Pass through to private overload, indicating that the connection is owned by the caller
        return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
    }
    private static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        // If we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

            assignParameterValues(commandParameters, parameterValues);

            return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // Otherwise we can just call the SP without params
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion executeDataReader
    #region executeScalar
    public static object executeScalar(string _connectionString, CommandType _sCommandType, string _sCommandText)
    {
        return executeScalar(_connectionString, _sCommandType, _sCommandText, (SqlParameter[])null);
    }
    private static object executeScalar(string _connectionString, CommandType _sCommandType, string _sCommandText, params SqlParameter[] _commandParameters)
    {
        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            return executeScalar(connection, _sCommandType, _sCommandText);
        }
    }
    public static object executeScalar(SqlConnection _Connection, CommandType _sCommandType, string _sCommandText, params SqlParameter[] parameters)
    {
        if (_Connection == null) throw new ArgumentNullException("Connection");
        SqlCommand _sCommand = new SqlCommand();
        bool mustCloseConnection = false;
        prepareCommand(_sCommand, _Connection, (SqlTransaction)null, _sCommandType, _sCommandText, parameters, out mustCloseConnection);
        object result = _sCommand.ExecuteScalar();
        _sCommand.Parameters.Clear();
        if (mustCloseConnection)
            _Connection.Close();
        return result;
    }
    #endregion executeScalar
}
public sealed class SqlHelperParameterCache
{
    #region private methods, variables, and constructors

    //Since this class provides only static methods, make the default constructor private to prevent 
    //instances from being created with "new SqlHelperParameterCache()"
    private SqlHelperParameterCache() { }

    private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

    private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        SqlCommand cmd = new SqlCommand(spName, connection);
        cmd.CommandType = CommandType.StoredProcedure;

        connection.Open();
        SqlCommandBuilder.DeriveParameters(cmd);
        connection.Close();

        if (!includeReturnValueParameter)
        {
            cmd.Parameters.RemoveAt(0);
        }

        SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];

        cmd.Parameters.CopyTo(discoveredParameters, 0);

        // Init the parameters with a DBNull value
        foreach (SqlParameter discoveredParameter in discoveredParameters)
        {
            discoveredParameter.Value = DBNull.Value;
        }
        return discoveredParameters;
    }

    private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
    {
        SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

        for (int i = 0, j = originalParameters.Length; i < j; i++)
        {
            clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
        }

        return clonedParameters;
    }

    #endregion private methods, variables, and constructors

    #region caching functions


    public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

        string hashKey = connectionString + ":" + commandText;

        paramCache[hashKey] = commandParameters;
    }

    public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

        string hashKey = connectionString + ":" + commandText;

        SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
        if (cachedParameters == null)
        {
            return null;
        }
        else
        {
            return CloneParameters(cachedParameters);
        }
    }

    #endregion caching functions

    #region Parameter Discovery Functions

    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
    {
        return GetSpParameterSet(connectionString, spName, false);
    }
    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
        }
    }
    internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
    {
        return GetSpParameterSet(connection, spName, false);
    }
    internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
        {
            return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
        }
    }
    private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

        SqlParameter[] cachedParameters;

        cachedParameters = paramCache[hashKey] as SqlParameter[];
        if (cachedParameters == null)
        {
            SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
            paramCache[hashKey] = spParameters;
            cachedParameters = spParameters;
        }

        return CloneParameters(cachedParameters);
    }
    #endregion Parameter Discovery Functions
}