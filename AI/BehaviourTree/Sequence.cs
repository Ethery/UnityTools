using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.AI.BehaviourTree.Tasks
{
	[Serializable]
	public class Sequence : Task, IList<Task>
	{
		public sealed override ETaskStatus Tick(Blackboard blackboard)
		{
			for (int i = 0; i < SubTasks.Count; ++i)
			{
				ETaskStatus subTaskStatus = SubTasks[i].Tick(blackboard);

				if (!m_taskStatuses.ContainsKey(SubTasks[i]))
				{
					m_taskStatuses.Add(SubTasks[i], subTaskStatus);
				}
				else
				{
					m_taskStatuses[SubTasks[i]] = subTaskStatus;
				}
				if (subTaskStatus == ETaskStatus.Running)
				{
					return ETaskStatus.Running;
				}
				else if (subTaskStatus == ETaskStatus.Failed)
				{
					return ETaskStatus.Failed;
				}
			}
			return ETaskStatus.Success;
		}

#if UNITY_EDITOR
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			UnityEditor.EditorGUI.indentLevel++;
			for (int i = 0; i < SubTasks.Count; ++i)
			{
				if (m_taskStatuses.ContainsKey(SubTasks[i]))
				{
					UnityEditor.EditorGUILayout.LabelField(m_taskStatuses[SubTasks[i]].ToString());
				}
				else
				{
					UnityEditor.EditorGUILayout.LabelField($"notProcessed");
				}

				SubTasks[i].OnInspectorGUI();
			}
			UnityEditor.EditorGUI.indentLevel--;
		}
#endif

		[NonSerialized]
		private Dictionary<Task, ETaskStatus> m_taskStatuses = new Dictionary<Task, ETaskStatus>();
		
		[SerializeField]
		public List<Task> SubTasks = new List<Task>();

		#region IList Implementations

		public int Count => ((ICollection<Task>)SubTasks).Count;

		public bool IsReadOnly => ((ICollection<Task>)SubTasks).IsReadOnly;

		public Task this[int index] { get => ((IList<Task>)SubTasks)[index]; set => ((IList<Task>)SubTasks)[index] = value; }

		public int IndexOf(Task item)
		{
			return ((IList<Task>)SubTasks).IndexOf(item);
		}

		public void Insert(int index, Task item)
		{
			((IList<Task>)SubTasks).Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			((IList<Task>)SubTasks).RemoveAt(index);
		}

		public void Add(Task item)
		{
			((ICollection<Task>)SubTasks).Add(item);
		}

		public void Clear()
		{
			((ICollection<Task>)SubTasks).Clear();
		}

		public bool Contains(Task item)
		{
			return ((ICollection<Task>)SubTasks).Contains(item);
		}

		public void CopyTo(Task[] array, int arrayIndex)
		{
			((ICollection<Task>)SubTasks).CopyTo(array, arrayIndex);
		}

		public bool Remove(Task item)
		{
			return ((ICollection<Task>)SubTasks).Remove(item);
		}

		public IEnumerator<Task> GetEnumerator()
		{
			return ((IEnumerable<Task>)SubTasks).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)SubTasks).GetEnumerator();
		}

		#endregion
	}
}
