using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityTools.Systems.Collections;

namespace UnityTools.Game
{
	public abstract class Wave<T> : MonoBehaviour where T : MonoBehaviour
	{
		[Serializable]
		public struct SpawnConfig
		{
			public T Prefab;
		}

		[SerializeField]
		protected List<SpawnConfig> SpawnConfigs;

		[SerializeField]
		protected Transform m_spawnPoint;

		protected int m_currentSpawnIndex;
		protected T LastSpawned;

		/// <summary>
		/// This is called each frame to check if the wave can spawn the next entity.
		/// </summary>
		protected abstract bool CanSpawn();

		public void OnEnable()
		{
			m_currentSpawnIndex = 0;
		}

		public void OnDisable()
		{
			m_currentSpawnIndex = 0;
		}

		public void Update()
		{
			if (!GameManager.IsInstanced)
				return;
			if (m_currentSpawnIndex < SpawnConfigs.Count)
			{
				if (CanSpawn())
				{
					Spawn();
				}
			}
			else
			{
				this.enabled = false;
			}
		}

		private void Spawn()
		{
			SpawnConfig spawnable = SpawnConfigs[m_currentSpawnIndex];
			Debug.Log($"Spawning {spawnable.Prefab.name}");
			if(m_spawnPoint == null)
			{
				Debug.LogWarning($"No spawn point referenced for {this}, using self to replace it.",this);
				m_spawnPoint = transform;
			}
			LastSpawned = Instantiate(spawnable.Prefab, m_spawnPoint.position, m_spawnPoint.rotation, m_spawnPoint);
			m_currentSpawnIndex++;
		}

	}
}