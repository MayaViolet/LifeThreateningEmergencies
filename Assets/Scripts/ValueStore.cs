using System;
using System.Collections.Generic;

public class ValueStore
{
	static ValueStore _instance;
	static ValueStore Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ValueStore();
			}
			return _instance;
		}
	}

	Dictionary<string, bool> store;

	private ValueStore ()
	{
		store = new Dictionary<string, bool>();
	}

	public static void Store(string key, bool value)
	{
		if (!Instance.store.ContainsKey(key))
		{
			Instance.store.Add(key, value);
		}
		else
		{
			Instance.store[key] = value;
		}
	}

	public static bool Retrieve(string key)
	{
		if (!Instance.store.ContainsKey(key))
		{
			return false;
		}
		return Instance.store[key];
	}

	public static void Clear()
	{
		Instance.store = new Dictionary<string, bool>();
	}
}

