using System;
using System.Collections.Generic;

public class ValueStore
{
	public static string ActivePlayer = "Enok";  // Or "Nalini".

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
	Dictionary<string, List<Action<bool>>> callbacks;

	private ValueStore ()
	{
		store = new Dictionary<string, bool>();
		callbacks = new Dictionary<string, List<Action<bool>>> ();
	}

	public static void Store(string key, bool value)
	{
		bool oldValue;
		if (!Instance.store.ContainsKey(key))
		{
			oldValue = false;
			Instance.store.Add(key, value);
		}
		else
		{
			oldValue = Instance.store[key];
			Instance.store[key] = value;
		}

		if (oldValue != value) {
			List<Action<bool>> callbacks;
			if (Instance.callbacks.TryGetValue(key, out callbacks)) {
				foreach (var callback in callbacks) {
					callback(value);
				}
			}
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

	public static void OnValueChanged(string key, Action<bool> callback) {
		List<Action<bool>> callbacks;
		if (!Instance.callbacks.TryGetValue (key, out callbacks)) {
			callbacks = Instance.callbacks [key] = new List<Action<bool>> ();
		}

		callbacks.Add (callback);
	}

	public static void RemoveValueChanged(string key, Action<bool> callback) {
		List<Action<bool>> callbacks;
		if (!Instance.callbacks.TryGetValue (key, out callbacks)) {
			return;
		}

		callbacks.Remove (callback);
	}
}

