using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityTools.Systems.Collections
{
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private List<TKey> keys = new List<TKey>();

		[SerializeField]
		private List<TValue> values = new List<TValue>();

		// save the dictionary to lists
		public void OnBeforeSerialize()
		{
			keys.Clear();
			values.Clear();
			foreach (KeyValuePair<TKey, TValue> pair in this)
			{
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}

		// load dictionary from lists
		public void OnAfterDeserialize()
		{
			this.Clear();

			for (int i = 0; i < keys.Count && i < values.Count; i++)
			{
				TKey key = default(TKey);
				if (i < keys.Count)
				{
					key = keys[i];
				}
				TValue value = default(TValue);
				if (i < values.Count)
				{
					value = values[i];
				}
				this.Add(key, value);
			}
		}

		public void AddDefaultValue()
		{
			keys.Add(default(TKey));
			values.Add(default(TValue));
		}
	}
}
