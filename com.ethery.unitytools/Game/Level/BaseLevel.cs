using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLevel<DATA_TYPE> : MonoBehaviour
	where DATA_TYPE : BaseLevelDatas
{
	public DATA_TYPE Datas;

	public void InitLevel()
	{
		Datas.Reset();
	}

	public void CloseLevel()
	{
		Datas.Save();
	}
}
