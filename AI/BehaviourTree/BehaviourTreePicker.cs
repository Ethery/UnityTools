using System;
using System.Linq;
using UnityEngine;

namespace UnityTools.AI.BehaviourTree
{
	[Serializable]
	public class BehaviourTreePicker
	{
		public const string TYPENAME_FIELDNAME = nameof(typeName);

		public BehaviourTree GetBehaviourTree()
		{
			if (m_behaviourTree == null)
			{
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
				{
					Type tt = assembly.GetType(typeName);
					if (tt != null)
					{
						m_behaviourTree = Activator.CreateInstance(tt) as BehaviourTree;
					}
				}
			}
			return m_behaviourTree;
		}

		[SerializeField]
		private string typeName;

		private BehaviourTree m_behaviourTree = null;
	}
}
