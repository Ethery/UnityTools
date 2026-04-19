using UnityEngine;

public class CameraBounds : MonoBehaviour
{
	public Bounds Bounds => m_Bounds;

	[SerializeField]
	private Bounds m_Bounds;
}