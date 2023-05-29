using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumpkin.Database;
public class Dataframe
{
	private Dictionary<string, dynamic> _data = new Dictionary<string, dynamic>();
	public ReadOnlyDictionary<string, dynamic> Data { get => _data.AsReadOnly(); }

    public Dataframe(params (string name, dynamic value)[] data) => _data = data.ToDictionary(d => d.name, d => d.value);

	public Dataframe(Dictionary<string, dynamic> data) => _data = data;

	public string GetString(string key) => (string)_data[key];

	public DateTime GetDateTime(string key) => (DateTime)_data[key];

	public float GetSingle(string key) => (float)_data[key];

	public double GetDouble(string key) => (double)_data[key];

	public decimal GetDecimal(string key) => (decimal)_data[key];

	public int GetInt32(string key) => (int)_data[key];

	public long GetInt64(string key) => (long)_data[key];

	public bool GetBoolean(string key) => (bool)_data[key];

	public Color GetColor(string key) => (Color)_data[key];

	//generic for whatever class i couldn't think of rn
	public TValue GetValue<TValue>(string key) => (TValue)_data[key];

	public dynamic this[int index]
	{
		get => _data.ElementAt(index);
		set => _data[_data.Keys.ElementAt(index)] = value;
	}

	public dynamic this[string key]
	{
		get => _data[key];
		set => _data[key] = value;
	}
}
