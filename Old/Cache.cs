using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Transactions;
using System.Xml.Serialization;
using Timer = System.Timers.Timer;

namespace MVS.Database.Old;

public class Cache
{
	private ConcurrentDictionary<string, CacheItem> readCache = new ConcurrentDictionary<string, CacheItem>();
	private ConcurrentDictionary<string, CacheItem> writeCache = new ConcurrentDictionary<string, CacheItem>();
	private readonly TimeSpan lifetime;
	private readonly TimeSpan activityTimeout;
	private readonly TimeSpan forcedFlushTimeout;
	internal const byte retryUntilForceFlush = 5;
	private DateTime lastActivity;
	private Timer inactivityTimer;

	public Cache(TimeSpan lifetime, TimeSpan activityTimeout)
	{
		this.lifetime = lifetime;
		this.activityTimeout = activityTimeout;
		forcedFlushTimeout = new(activityTimeout.Ticks * retryUntilForceFlush);
		lastActivity = DateTime.Now;
		inactivityTimer = new Timer
		{
			Interval = this.activityTimeout.TotalMilliseconds,
			AutoReset = true,
		};
		inactivityTimer.Elapsed += (sender, e) => CheckActivity();
		inactivityTimer.Start();
	}

	//Read data from cache
	public T GetData<T>(string varName)
	{
		if (readCache.TryGetValue(varName, out CacheItem item) || writeCache.TryGetValue(varName, out item) && item is not null)
		{
			if (item.IsValid(lifetime))
			{
				item.Timestamp = DateTime.Now;
				return item.Data;
			}
			else readCache.Remove(varName, out _);
		}

		T data = _readDataInternal<T>(varName);
		readCache[varName] = new CacheItem(data);
		return data;
	}

	//Write data to cache, no need to flush bc the cache may be busy
	public void WriteData(string varName, dynamic data) => writeCache[varName] = new(data);


	//Crooked multithreaded flush implementation :),the original was worse, using 2 task completion sources to run 2 parallel for loops in parellel for flushing AND cache invalidation XD
	public async void Flush()
	{
		int itemCount = writeCache.Count;
		if (itemCount > 0)
		await Task.Run(() =>
		{
			Parallel.For(0, itemCount, (i) =>
			{
				KeyValuePair<string, CacheItem> item = readCache.ElementAt(i);
				_writeDataInternal(item.Key, item.Value.Data);
			});
		});
		lastActivity = DateTime.Now;
		writeCache.Clear();	
	}

	private T _readDataInternal<T>(string varName)
	{
		// Implementation for retrieving data from the data source goes here
		throw new NotImplementedException();
	}

	private void _writeDataInternal(string varName, dynamic data)
	{
		// Implementation for writing data to the data source goes here
		throw new NotImplementedException();
	}

	private void InvalidateCache()
	{
		Parallel.For(0, readCache.Count, (i) =>
		{
			KeyValuePair<string, CacheItem> item = readCache.ElementAt(i);
			if (!item.Value.IsValid(lifetime)) readCache.Remove(item.Key , out _);
		});
	}

	private void CheckActivity()
	{
		TimeSpan elapsedTime = DateTime.Now - lastActivity;
		if (elapsedTime >= activityTimeout || elapsedTime >= forcedFlushTimeout)
		{
			if (inactivityTimer.Interval != activityTimeout.TotalMilliseconds) inactivityTimer.Interval = activityTimeout.TotalMilliseconds;
			InvalidateCache();
			Flush();
			lastActivity = DateTime.Now;
		}
		else inactivityTimer.Interval = (elapsedTime - activityTimeout).TotalMilliseconds;
	}

	//primary constructors is only in c# 12 in .NET 8 :(
	internal class CacheItem
	{
		public dynamic Data { get; set; }
		public Type DataType { get; set; }
		public DateTime Timestamp { get; set; }

		public CacheItem(dynamic data)
		{
			Data = data;
			DataType = Data.GetType();
			Timestamp = DateTime.Now;
		}

		public bool IsValid(TimeSpan lifetime) => DateTime.Now - Timestamp < lifetime;
	}
}
