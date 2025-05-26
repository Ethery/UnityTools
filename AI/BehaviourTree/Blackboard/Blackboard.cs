using System;
using System.Collections.Generic;

namespace UnityTools.AI.BehaviourTree
{
	public class Blackboard
	{
		[Serializable]
		public struct BlackboardValue
		{
			public BlackboardValue(string key, object value)
			{
				Key = key;
				Value = value;
			}

			public string Key;
			public object Value;
		}

		public T GetValue<T>(string key)
		{
			if (Values.TryGetValue(key, out object value))
			{
				if (value.GetType().IsAssignableFrom(typeof(T)))
					return (T)value;
			}
			return default(T);
		}
		
		public object GetValue(string key)
		{
			if (Values.TryGetValue(key, out object value))
			{
				return value;
			}
			return null;
		}

		public Dictionary<string, object> Values = new Dictionary<string, object>();
	}
}
