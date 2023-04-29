using System.Data.OleDb;
using System.Data;
using System.Data.Common;

namespace MVS.Database;

public enum MyEnum
{

}

public class Database
{
	public const double DbActivityFlushTimout = 60_000;
	internal const short MsToTickMult = 10_000;
	internal const float TickToMsMult = 0.0001f;

	public string FilePath { get; set; }
	public string Password { get; set; } = string.Empty;
	public bool PersistSecuriryinfo { get; set; } = false;
	public bool UseCache { get; init; } = false;
	public double CacheLifeTime { get; init; }
	public string[] CacheColExclude { get; init; }
	public Cache Cache { get; init; }
	public DatabaseSchema Schema { get; init; }

	private OleDbConnection connection;

	public string ConnectionString 
	{
		get => GetConnectionString();
	}

	/// <summary>
	/// Creates a new instance of the <see cref="Database"/> class based on the file path of the database file
	/// </summary>
	/// <param name="filePath"></param>
	/// <param name="useCache">Detirmines whether the read/write operations are cached (use <see langword="true"/> when requesting the same data often or when data doesn't change often)</param>
	/// <param name="cacheLifetime">Lifetime of data in cache before it's refreshed from the database, a value that's too high can cause downstream problems.<br/><see langword="default"/> is 1h (this value is given in <strong>MILLISECONDS</strong>)</param>
	/// <param name="excludeColumnsFromCache">Select which columns in the database are <strong>NOT</strong> cached (use this for columns with frequently changing data, like sensors)</param>
	/// <param name="schema">Tells the <see cref="Database"/> class how to interpret the data returned by the database. This schema can be changed dynamically if needed by for example adding columns.<br/><br/>See: <see cref="DatabaseSchema.DatabaseSchema(KeyValuePair{Type, string}[])"/> for an example</param>
	public Database(string filePath, ref DatabaseSchema schema, bool useCache = true, double cacheLifetime = 3_600_000d, params string[] excludeColumnsFromCache)
    {
        FilePath = filePath;
		Cache = new(new(ticks: (long)cacheLifetime * MsToTickMult), new(ticks: (long)DbActivityFlushTimout * MsToTickMult));
		UseCache = useCache;
		CacheLifeTime = cacheLifetime;
		CacheColExclude = excludeColumnsFromCache;
		Schema = schema;
    }

	/// <summary>
	/// Creates a new instance of the <see cref="Database"/> class based on the file path of the database file
	/// </summary>
	/// <param name="filePath"></param>
	/// <param name="persistSecurityInfo"></param>
	/// <param name="useCache">Detirmines whether the read/write operations are cached (use <see langword="true"/> when requesting the same data often or when data doesn't change often)</param>
	/// <param name="cacheLifetime">Lifetime of data in cache before it's refreshed from the database, a value that's too high can cause downstream problems.<br/><see langword="default"/> is 1h (this value is given in <strong>MILLISECONDS</strong>)</param>
	/// <param name="excludeColumnsFromCache">Select which columns in the database are <strong>NOT</strong> cached (use this for columns with frequently changing data, like sensors)</param>
	/// <param name="schema">Tells the <see cref="Database"/> class how to interpret the data returned by the database.<br/><br/>See: <see cref="DatabaseSchema.DatabaseSchema(KeyValuePair{Type, string}[])"/> for an example</param>
	public Database(string filePath, ref DatabaseSchema schema, bool persistSecurityInfo = false, bool useCache = true, double cacheLifetime = 3_600_000d, params string[] excludeColumnsFromCache)
	{
		FilePath = filePath;
		PersistSecuriryinfo = persistSecurityInfo;
		Cache = new(new(ticks: (long)cacheLifetime * MsToTickMult), new(ticks: (long)DbActivityFlushTimout * MsToTickMult));
		UseCache = useCache;
		CacheLifeTime = cacheLifetime;
		CacheColExclude = excludeColumnsFromCache;
		Schema = schema;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="Database"/> class based on the file path of the database file and the userID and password
	/// </summary>
	/// <param name="filePath">Path of the db file</param>
	/// <param name="password">Password to login to the database</param>
	/// <param name="persistSecurityInfo"></param>
	/// <param name="useCache">Detirmines whether the read/write operations are cached (use <see langword="true"/> when requesting the same data often or when data doesn't change often)</param>
	/// <param name="cacheLifetime">Lifetime of data in cache before it's refreshed from the database, a value that's too high can cause downstream problems.<br/><see langword="default"/> is 1h (this value is given in <strong>MILLISECONDS</strong>)</param>
	/// <param name="excludeColumnsFromCache">Select which columns in the database are <strong>NOT</strong> cached (use this for columns with frequently changing data, like sensors)</param>
	/// <param name="schema">Tells the <see cref="Database"/> class how to interpret the data returned by the database.<br/><br/>See: <see cref="DatabaseSchema.DatabaseSchema(KeyValuePair{Type, string}[])"/> for an example</param>
	public Database(string filePath, string password, ref DatabaseSchema schema, bool persistSecurityInfo = false, bool useCache = true, double cacheLifetime = 3_600_000d, params string[] excludeColumnsFromCache)
	{
		FilePath = filePath;
		Password = password;
		PersistSecuriryinfo = persistSecurityInfo;
		Cache = new(new(ticks: (long)cacheLifetime * MsToTickMult), new(ticks: (long)DbActivityFlushTimout * MsToTickMult));
		UseCache = useCache;
		CacheLifeTime = cacheLifetime;
		CacheColExclude = excludeColumnsFromCache;
		Schema = schema;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="Database"/> class based on the file path of the database file and the userID and password
	/// </summary>
	/// <param name="filePath">Path of the db file</param>
	/// <param name="password">Password to login to the database</param>
	/// <param name="useCache">Detirmines whether the read/write operations are cached (use <see langword="true"/> when requesting the same data often or when data doesn't change often)</param>
	/// <param name="cacheLifetime">Lifetime of data in cache before it's refreshed from the database, a value that's too high can cause downstream problems.<br/><see langword="default"/> is 1h (this value is given in <strong>MILLISECONDS</strong>)</param>
	/// <param name="excludeColumnsFromCache">Select which columns in the database are <strong>NOT</strong> cached (use this for columns with frequently changing data, like sensors)</param>
	/// <param name="schema">Tells the <see cref="Database"/> class how to interpret the data returned by the database.<br/><br/>See: <see cref="DatabaseSchema(KeyValuePair{Type, string}[])"/> for an example</param>

	public Database(string filePath, string password, ref DatabaseSchema schema, bool useCache = true, double cacheLifetime = 3_600_000d, params string[] excludeColumnsFromCache)
	{
		FilePath = filePath;
		Password = password;
		Cache = new(new(ticks: (long)cacheLifetime * MsToTickMult), new(ticks: (long)DbActivityFlushTimout * MsToTickMult));
		UseCache = useCache;
		CacheLifeTime = cacheLifetime;
		CacheColExclude = excludeColumnsFromCache;
		Schema = schema;
	}

	public void Connect()
	{
		if (connection is null || connection?.State == (ConnectionState.Broken | ConnectionState.Closed)) (connection = new OleDbConnection(ConnectionString)).Open();
	}

	public string GetConnectionString() 
	{
		bool filePathInvalid = string.IsNullOrEmpty(FilePath.Trim());
		bool passwordInvalid = string.IsNullOrEmpty(Password.Trim());

		string connString = $"Provider=Microsoft.ACE.OLEDB.12.0; Persist Security Info={PersistSecuriryinfo}; ";
		if (!filePathInvalid) connString = string.Format(connString, FilePath); else throw new FileNotFoundException($"Database file: '{FilePath}'");
		if (!passwordInvalid) connString += $"Jet OLEDB: Database Password={Password};";
		return connString;
	}

	internal async Task<int> RunQuery(string query) 
	{
		Connect();
		OleDbCommand cmd = new(query, connection);
		return await cmd.ExecuteNonQueryAsync();
	}

	internal async Task<DbDataReader> RunQueryReader(string query)
	{
		Connect();
		OleDbCommand cmd = new(query, connection);
		return await cmd.ExecuteReaderAsync();
	}
	internal async Task<object> RunQueryScalar(string query)
	{
		Connect();
		OleDbCommand cmd = new(query, connection);
		return await cmd.ExecuteScalarAsync();
	}

	public async Task <DataTable> Select(string colName, string tableName, string where) 
	{
		DbDataReader reader = await RunQueryReader($"SELECT {colName} FROM {tableName} {(string.IsNullOrEmpty(where.Trim()) ? ';' : $"WHERE {where};")}");
		DataTable resultTable = new(tableName);
		resultTable.Load(reader);
		return resultTable;
	}

	public async void InsertInto(string colName, string[]) 
	{
		
	}

	public async void WriteData() 
	{
		
		connection.
	}
}
