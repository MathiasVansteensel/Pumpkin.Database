using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Pumpkin.Database.Old;

public enum MasterDbVarName
{
    ID,
    Username,
    PasswordHash,
    Language,
    Country,
    EnergyPrice,
    AutomationXml,
    UserId,
    ValuesPath
}

public enum UserDbVarName
{
    Powerfactor,
    Temperature,
    Light,
    Humidity,
    OutletState,
    LampColor
}

public class Database
{
    public string MasterDbPath { get; set; }
    public string Password { get; set; } = string.Empty;
    public bool PersistSecuriryinfo { get; set; } = false;

    public const string UserdataTableName = "UserVariables";
    public const string MasterTableName = "Users";

    private OleDbConnection masterConnection;
    public string MasterConnectionString
    {
        get => GetConnectionString(MasterDbPath, Password, PersistSecuriryinfo);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Database"/> class based on the file path of the database file
    /// </summary>
    /// <param name="masterDbPath"></param>
    public Database(string masterDbPath)
    {
        MasterDbPath = masterDbPath;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Database"/> class based on the file path of the database file
    /// </summary>
    /// <param name="masterDbPath"></param>
    /// <param name="persistSecurityInfo"></param>
    public Database(string masterDbPath, bool persistSecurityInfo = false)
    {
        MasterDbPath = masterDbPath;
        PersistSecuriryinfo = persistSecurityInfo;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Database"/> class based on the file path of the database file and the userID and password
    /// </summary>
    /// <param name="masterDbPath">Path of the db file</param>
    /// <param name="password">Password to login to the database</param>
    /// <param name="persistSecurityInfo"></param>
    public Database(string masterDbPath, string password, bool persistSecurityInfo = false)
    {
        MasterDbPath = masterDbPath;
        Password = password;
        PersistSecuriryinfo = persistSecurityInfo;
    }

    public OleDbConnection Connect(string dbPath = null)
    {
        if (string.IsNullOrEmpty(dbPath = dbPath.Trim()))
        {
            if (masterConnection is null || masterConnection?.State == (ConnectionState.Broken | ConnectionState.Closed)) (masterConnection = new OleDbConnection(MasterConnectionString)).Open();
            return null;
        }
        else
        {
            OleDbConnection nodeDbConnection = new(GetConnectionString(dbPath, Password, PersistSecuriryinfo));
            nodeDbConnection.Open();
            return nodeDbConnection;
        }
    }

    private int GetTimeStampIndex(int index) => index % 2 == 0 ? index + 1 : index - 1;

    public string GetConnectionString(string dbPath, string password = null, bool secuOptions = false)
    {
        bool filePathInvalid = string.IsNullOrEmpty(dbPath.Trim());
        bool passwordInvalid = string.IsNullOrEmpty(password.Trim());

        string connString = $"Provider=Microsoft.ACE.OLEDB.12.0; Persist Security Info={secuOptions}; ";
        if (!filePathInvalid) connString = string.Format(connString, dbPath); else throw new FileNotFoundException($"Database file: '{dbPath}'");
        if (!passwordInvalid) connString += $"Jet OLEDB: Database Password={password};";
        return connString;
    }

    internal async Task<int> RunQuery(string query, OleDbConnection connection)
    {
        Connect();
        OleDbCommand cmd = new(query, connection);
        return await cmd.ExecuteNonQueryAsync();
    }

    internal async Task<DbDataReader> RunQueryReader(string query, OleDbConnection connection)
    {
        Connect();
        OleDbCommand cmd = new(query, connection);
        return await cmd.ExecuteReaderAsync();
    }
    internal async Task<T> RunQueryScalar<T>(string query, OleDbConnection connection)
    {
        Connect();
        OleDbCommand cmd = new(query, connection);
        return (T)await cmd.ExecuteScalarAsync();
    }

    internal async Task<List<T>> GetVar<T>(MasterDbVarName varName, string where = "")
    {
        DbDataReader reader = await RunQueryReader($"SELECT {varName} FROM {MasterTableName} {(string.IsNullOrEmpty(where = where.Trim()) ? ';' : $"WHERE {where}")}", Connect());
        List<T> list = new();
        while (reader.Read()) list.Add((T)reader[0]);
        return list;
    }
    internal async Task<Dictionary<DateTime, T>> GetVar<T>(UserDbVarName varName, string userId, string where = "")
    {
        string dbPath = (await GetVar<string>(MasterDbVarName.ValuesPath, $"UserId= '{userId}'")).FirstOrDefault();
        DbDataReader reader = await RunQueryReader($"SELECT {varName},{Enum.GetName(typeof(UserDbVarName), GetTimeStampIndex((int)varName))}  FROM {UserdataTableName} {(string.IsNullOrEmpty(where = where.Trim()) ? ';' : $"WHERE {where}")}", Connect(dbPath));
        Dictionary<DateTime, T> list = new();
        while (reader.Read()) list.Add((DateTime)reader[1], (T)reader[0]);
        return list;
    }


}
