using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Systems.UI
{
	public class Tooltip : Page
	{
		Transform LinkedTransform;
		Vector3 Offset;

		public void BindToTransform(Transform transform)
		{
			LinkedTransform = transform;
		}

		public void ApplyOffset(Vector3 offset)
		{
			Offset = offset;
		}

		public virtual void Update()
		{
			if (LinkedTransform != null)
			{
				transform.position = Camera.main.WorldToScreenPoint(LinkedTransform.position) + Offset;
			}
		}
	}
}
