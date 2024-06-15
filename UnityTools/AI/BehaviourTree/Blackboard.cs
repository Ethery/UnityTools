using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

		public T GetValue<T>(string key) where T : class
		{
			if(Values.TryGetValue(key, out object value))
			{
				if(value.GetType().IsAssignableFrom(typeof(T)))
					return value as T;
			}
			return default(T);
		}

		public Dictionary<string, object> Values = new Dictionary<string, object>();
	}
}
