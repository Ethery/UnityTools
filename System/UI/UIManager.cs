using System;
using System.Collections.Generic;
using UnityEngine;
using UnityTools.Systems.Collections;
using UnityTools.Systems.Inputs;

namespace UnityTools.Systems.UI
{
	public class UIManager<ETYPE> : Singleton<UIManager<ETYPE>>, ISerializationCallbackReceiver
	{
		public T Page<T>(Guid pageId) where T : Page
		{
			return m_loaded[pageId] as T;
		}

		[SerializeField]
		private SerializableDictionary<ETYPE, Page> m_prefabs;

		[SerializeField]
		private Transform UICanvas;


		[SerializeField]
		private TMPro.TextMeshProUGUI m_DebugInfos;

		private void Update()
		{
			InputManager.Instance.
			foreach ()
		}

		#region Datas

		private Dictionary<Guid, Page> m_loaded = new Dictionary<Guid, Page>();

		#endregion

		public Guid CreatePage(ETYPE page)
		{
			Guid newId = Guid.NewGuid();
			m_loaded.Add(newId, Instantiate(m_prefabs[page], UICanvas));
			return newId;
		}

		public void DestroyPage(Guid pageId)
		{
			Page page = Page<Page>(pageId);
			GameObject.Destroy(page.gameObject);
			m_loaded.Remove(pageId);
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			foreach (ETYPE type in Enum.GetValues(typeof(ETYPE)))
			{
				if (!m_prefabs.ContainsKey(type))
				{
					m_prefabs.Add(type, null);
				}
			}
		}

	}
}
