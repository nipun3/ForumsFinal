using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System;
using System.Transactions;

namespace ForumsFinalApi._3325.Repository.DataProcessor
{
    public class RepositoryConnector : DataHelper
    {
        public new Database Database;

        public RepositoryConnector(string connectionName)
        {
            Database = new DatabaseProviderFactory().Create(connectionName);
        }

        public new DataSet ExecuteDataSet(string spName, params  SqlParameterHolder[] parameterList)
        {
            using (var dbCommand = GetSqlCommand(spName, CommandType.StoredProcedure))
            {
                foreach (var parameter in parameterList)
                {
                    if (parameter.DataType != null && parameter.Direction != null)
                    {

                        dbCommand.AddParameter(GetSqlParameter(parameter.Parameter, (ParameterDirection)parameter.Direction, (DbType)parameter.DataType, parameter.ParameterValue));
                    }
                    else
                        dbCommand.AddParameter(new SqlParameter(parameter.Parameter, parameter.ParameterValue));
                }

                using (var ds = Database.ExecuteDataSet(dbCommand))
                {
                    return ds;
                }
            }
        }
        public bool ExecuteNonQuery(string spName, params  SqlParameterHolder[] parameterList)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (var dbCommand = GetSqlCommand(spName, CommandType.StoredProcedure))
                {

                    foreach (var parameter in parameterList)
                    {
                        if (parameter.DataType != null && parameter.Direction != null)
                        {

                            dbCommand.AddParameter(GetSqlParameter(parameter.Parameter, (ParameterDirection)parameter.Direction, (DbType)parameter.DataType, parameter.ParameterValue));
                        }
                        else
                            dbCommand.AddParameter(new SqlParameter(parameter.Parameter, parameter.ParameterValue));
                    }

                    int rowsAffected = Database.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        transaction.Complete();
                    }
                    return rowsAffected > 0;
                }
            }
        }
    }

    public class DataHelper
    {
        public static Database Database
        {
            get
            {
                return new DatabaseProviderFactory().Create("DbConnection");
            }
        }

        public static SqlParameter GetSqlParameter(string parameterName, ParameterDirection parameterDirection, DbType type, int size, object value)
        {
            return new SqlParameter()
            {
                Value = (value == null || (type == DbType.DateTime || type == DbType.Date) && (DateTime)value == DateTime.MinValue) ? DBNull.Value : value,
                Direction = parameterDirection,
                DbType = type,
                ParameterName = parameterName,
                Size = size
            };
        }

        public static SqlParameter GetSqlParameter(string parameterName, ParameterDirection parameterDirection, DbType type, object value)
        {
            return new SqlParameter()
            {
                Value =
                    (value == null ||
                     (type == DbType.DateTime || type == DbType.Date) && (DateTime)value == DateTime.MinValue)
                        ? DBNull.Value
                        : value,
                Direction = parameterDirection,
                DbType = type,
                ParameterName = parameterName
            };
        }

        public static SqlParameter GetSqlParameter(string parameterName, DbType type, object value)
        {
            return new SqlParameter()
            {
                Value = (value == null || (type == DbType.DateTime || type == DbType.Date) && (DateTime)value == DateTime.MinValue) ? DBNull.Value : value,
                DbType = type,
                ParameterName = parameterName
            };
        }

        public static SqlParameter GetSqlParameter(string parameterName, SqlDbType type, object value)
        {
            return new SqlParameter()
            {
                Value = (value == null || (type == SqlDbType.DateTime || type == SqlDbType.Date) && (DateTime)value == DateTime.MinValue) ? DBNull.Value : value,
                SqlDbType = type,
                ParameterName = parameterName
            };
        }


        public static SqlParameter GetSqlParameter(string parameterName, object value)
        {
            return new SqlParameter()
            {
                Value = value ?? DBNull.Value,
                ParameterName = parameterName
            };
        }

        //Suppressed this issue as we are aware that we will passed only the SP names as CommandText.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static SqlCommand GetSqlCommand(string commandText, CommandType type)
        {

            return new SqlCommand()
            {
                CommandText = commandText,
                CommandType = type,
                CommandTimeout = 60
            };
        }

        public static DataSet ExecuteCommand(string SpName, params SqlParameterHolder[] parameterList)
        {
            using (var dbCommand = GetSqlCommand(SpName, CommandType.StoredProcedure))
            {
                foreach (var parameter in parameterList)
                {
                    if (parameter.DataType != null && parameter.Direction != null)
                    {

                        dbCommand.AddParameter(GetSqlParameter(parameter.Parameter, (ParameterDirection)parameter.Direction, (DbType)parameter.DataType, parameter.ParameterValue));
                    }
                    else
                        dbCommand.AddParameter(new SqlParameter(parameter.Parameter, parameter.ParameterValue));
                }

                return Database.ExecuteDataSet(dbCommand);
            }
        }

        public static bool ExecuteNonQuery(string spName, int commandTimeout, params  SqlParameterHolder[] parameterList)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromSeconds(commandTimeout)))
            {
                using (var dbCommand = GetSqlCommand(spName, CommandType.StoredProcedure))
                {
                    dbCommand.CommandTimeout = commandTimeout;
                    foreach (var parameter in parameterList)
                    {
                        if (parameter.DataType != null && parameter.Direction != null)
                        {

                            dbCommand.AddParameter(GetSqlParameter(parameter.Parameter, (ParameterDirection)parameter.Direction, (DbType)parameter.DataType, parameter.ParameterValue));
                        }
                        else
                            dbCommand.AddParameter(new SqlParameter(parameter.Parameter, parameter.ParameterValue));
                    }

                    int rowsAffected = Database.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        transaction.Complete();
                    }
                    return rowsAffected > 0;
                }
            }
        }

        public static DataSet ExecuteDataSet(string SpName, params  SqlParameterHolder[] parameterList)
        {
            using (var dbCommand = GetSqlCommand(SpName, CommandType.StoredProcedure))
            {
                foreach (var parameter in parameterList)
                {
                    if (parameter.DataType != null && parameter.Direction != null)
                    {

                        dbCommand.AddParameter(GetSqlParameter(parameter.Parameter, (ParameterDirection)parameter.Direction, (DbType)parameter.DataType, parameter.ParameterValue));
                    }
                    else
                        dbCommand.AddParameter(new SqlParameter(parameter.Parameter, parameter.ParameterValue));
                }

                using (var ds = Database.ExecuteDataSet(dbCommand))
                {
                    return ds;
                }
            }
        }

        public static IDataReader ExecuteReader(string commandText, CommandType type, SqlParameter[] parameters = null)
        {
            using (var dbCommand = GetSqlCommand(commandText, type))
            {
                if (parameters != null)
                    dbCommand.AddParameters(parameters);
                return Database.ExecuteReader(dbCommand);
            }
        }

        public static int ExecuteNonQuery(string commandText, CommandType type, SqlParameter[] parameters)
        {
            using (var dbCommand = GetSqlCommand(commandText, type))
            {
                dbCommand.AddParameters(parameters);
                return Database.ExecuteNonQuery(dbCommand);
            }
        }
        public static object ExecuteScalar(string commandText, CommandType type, params SqlParameterHolder[] parameterList)
        {
            using (var dbCommand = GetSqlCommand(commandText, type))
            {
                foreach (var parameter in parameterList)
                {
                    if (parameter.DataType != null && parameter.Direction != null)
                    {

                        dbCommand.AddParameter(GetSqlParameter(parameter.Parameter, (ParameterDirection)parameter.Direction, (DbType)parameter.DataType, parameter.ParameterValue));
                    }
                    else
                        dbCommand.AddParameter(new SqlParameter(parameter.Parameter, parameter.ParameterValue));
                }
                return Database.ExecuteScalar(dbCommand);
            }
        }

        public static IDataReader ExecuteReaderWithoutTimeout(string commandText, CommandType type, SqlParameter[] parameters = null)
        {
            using (var dbCommand = GetSqlCommand(commandText, type))
            {
                dbCommand.CommandTimeout = 300;
                if (parameters != null)
                    dbCommand.AddParameters(parameters);
                return Database.ExecuteReader(dbCommand);
            }
        }

        public static DataSet ProspectSearchExecuteCommand(string SpName, params SqlParameterHolder[] parameterList)
        {
            using (var dbCommand = new SqlCommand()
            {
                CommandText = SpName,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 300
            })
            {
                foreach (var parameter in parameterList)
                {
                    if (parameter.DataType != null && parameter.Direction != null)
                    {

                        dbCommand.AddParameter(GetSqlParameter(parameter.Parameter, (ParameterDirection)parameter.Direction, (DbType)parameter.DataType, parameter.ParameterValue));
                    }
                    else
                        dbCommand.AddParameter(new SqlParameter(parameter.Parameter, parameter.ParameterValue));
                }

                return Database.ExecuteDataSet(dbCommand);
            }

        }


    }
}



public static class DataExtensions
{
    public static void AddParameter(this DbCommand command, SqlParameter parameter)
    {
        command.Parameters.Add(parameter);
    }

    public static void AddParameters(this DbCommand command, SqlParameter[] parameters)
    {
        command.Parameters.AddRange(parameters);
    }
}

public class SqlParameterHolder
{

    string param;
    private object paramValue;
    ParameterDirection? paramDirection = null;
    DbType? dbType = null;
    public string Parameter
    {

        get { return "@" + param.ToString().Trim(); }
        set { param = value; }
    }
    public object ParameterValue
    {

        get
        {
            if (paramValue is string && string.IsNullOrEmpty((string)paramValue)) return null;
            return paramValue;
        }
        set { paramValue = value; }
    }

    public Nullable<ParameterDirection> Direction
    {
        get { return paramDirection; }
        set { paramDirection = value; }
    }

    public Nullable<DbType> DataType
    {
        get { return dbType; }
        set { dbType = value; }
    }


}