using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贴花控制器
/// </summary>
public class HitDecalObj : MonoBehaviour
{
	#region 基本组件和变量

	[Header("贴花生成后过多少秒消失")]
	[SerializeField] private float decalSustainTime;
	[Header("模型渲染器")]
	[SerializeField] private List<MeshRenderer> meshRendererList = new List<MeshRenderer>();

	[HideInInspector] public Material decalMaterial;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		for (int i = 0; i < meshRendererList.Count; i ++)
		{
			meshRendererList[i].material = decalMaterial;
		}

		Destroy(gameObject, decalSustainTime);
	}

	#endregion

}
