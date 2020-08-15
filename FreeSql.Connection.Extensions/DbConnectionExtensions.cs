using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

public static class DbConnectionExtensions {

    static Dictionary<Type, IFreeSql> _dicCurd = new Dictionary<Type, IFreeSql>();
    static object _dicCurdLock = new object();
    internal static IFreeSql GetCurd(Type dbconType)
    {
        var connType = dbconType.UnderlyingSystemType;
        if (_dicCurd.TryGetValue(connType, out var fsql)) return fsql;

        Type providerType = null;
        switch (connType.Name)
        {
            case "MySqlConnection":
                providerType = Type.GetType("FreeSql.MySql.MySqlProvider`1,FreeSql.Provider.MySql")?.MakeGenericType(connType);
                if (providerType == null) providerType = Type.GetType("FreeSql.MySql.MySqlProvider`1,FreeSql.Provider.MySqlConnector")?.MakeGenericType(connType);
                if (providerType == null) throw new Exception("缺少 FreeSql 数据库实现包：FreeSql.Provider.MySql.dll，可前往 nuget 下载");
                break;
            case "SqlConnection":
                providerType = Type.GetType("FreeSql.SqlServer.SqlServerProvider`1,FreeSql.Provider.SqlServer")?.MakeGenericType(connType);
                if (providerType == null) throw new Exception("缺少 FreeSql 数据库实现包：FreeSql.Provider.SqlServer.dll，可前往 nuget 下载");
                break;
            case "NpgsqlConnection":
                providerType = Type.GetType("FreeSql.PostgreSQL.PostgreSQLProvider`1,FreeSql.Provider.PostgreSQL")?.MakeGenericType(connType);
                if (providerType == null) throw new Exception("缺少 FreeSql 数据库实现包：FreeSql.Provider.PostgreSQL.dll，可前往 nuget 下载");
                break;
            case "OracleConnection":
                providerType = Type.GetType("FreeSql.Oracle.OracleProvider`1,FreeSql.Provider.Oracle")?.MakeGenericType(connType);
                if (providerType == null) throw new Exception("缺少 FreeSql 数据库实现包：FreeSql.Provider.Oracle.dll，可前往 nuget 下载");
                break;
            case "SQLiteConnection":
                providerType = Type.GetType("FreeSql.Sqlite.SqliteProvider`1,FreeSql.Provider.Sqlite")?.MakeGenericType(connType);
                if (providerType == null) throw new Exception("缺少 FreeSql 数据库实现包：FreeSql.Provider.Sqlite.dll，可前往 nuget 下载");
                break;
            case "DmConnection":
                providerType = Type.GetType("FreeSql.Dameng.DamengProvider`1,FreeSql.Provider.Dameng")?.MakeGenericType(connType);
                if (providerType == null) throw new Exception("缺少 FreeSql 数据库实现包：FreeSql.Provider.Dameng.dll，可前往 nuget 下载");
                break;
            case "OscarConnection":
                providerType = Type.GetType("FreeSql.ShenTong.ShenTongProvider`1,FreeSql.Provider.ShenTong")?.MakeGenericType(connType);
                if (providerType == null) throw new Exception("缺少 FreeSql 数据库实现包：FreeSql.Provider.ShenTong.dll，可前往 nuget 下载");
                break;
            default:
                throw new Exception("未实现");
        }
        lock (_dicCurdLock)
        {
            if (_dicCurd.TryGetValue(connType, out fsql)) return fsql;
            lock (_dicCurdLock)
                _dicCurd.Add(connType, fsql = Activator.CreateInstance(providerType, new object[] { null, null, null }) as IFreeSql);
        }
        return fsql;
    }

    public static ISelect<T1> Select<T1>(this DbConnection that) where T1 : class => GetCurd(that?.GetType()).Select<T1>().WithConnection(that);
	public static ISelect<T1> Select<T1>(this DbConnection that, object dywhere) where T1 : class => GetCurd(that?.GetType()).Select<T1>(dywhere).WithConnection(that);
    public static IInsert<T1> Insert<T1>(this DbConnection that) where T1 : class => GetCurd(that?.GetType()).Insert<T1>().WithConnection(that);
    public static IInsert<T1> Insert<T1>(this DbConnection that, T1 source) where T1 : class => GetCurd(that?.GetType()).Insert<T1>(source).WithConnection(that);
    public static IInsert<T1> Insert<T1>(this DbConnection that, T1[] source) where T1 : class => GetCurd(that?.GetType()).Insert<T1>(source).WithConnection(that);
    public static IInsert<T1> Insert<T1>(this DbConnection that, IEnumerable<T1> source) where T1 : class => GetCurd(that?.GetType()).Insert<T1>(source).WithConnection(that);
    public static IUpdate<T1> Update<T1>(this DbConnection that) where T1 : class => GetCurd(that?.GetType()).Update<T1>().WithConnection(that);
    public static IUpdate<T1> Update<T1>(this DbConnection that, object dywhere) where T1 : class => GetCurd(that?.GetType()).Update<T1>(dywhere).WithConnection(that);
    public static IDelete<T1> Delete<T1>(this DbConnection that) where T1 : class => GetCurd(that?.GetType()).Delete<T1>().WithConnection(that);
    public static IDelete<T1> Delete<T1>(this DbConnection that, object dywhere) where T1 : class => GetCurd(that?.GetType()).Delete<T1>(dywhere).WithConnection(that);

    public static List<T> Query<T>(this DbConnection that, string cmdText, object parms = null) => GetCurd(that?.GetType()).Ado.Query<T>(that, null, cmdText, parms);
    public static List<T> Query<T>(this DbConnection that, CommandType cmdType, string cmdText, params DbParameter[] cmdParms) => GetCurd(that?.GetType()).Ado.Query<T>(that, null, cmdType, cmdText, cmdParms);
}
