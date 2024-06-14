using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������
/// </summary>
public class HitDecalObj : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�������ɺ����������ʧ")]
	[SerializeField] private float decalSustainTime;
	[Header("ģ����Ⱦ��")]
	[SerializeField] private List<MeshRenderer> meshRendererList = new List<MeshRenderer>();

	[HideInInspector] public Material decalMaterial;

	#endregion

	#region �����������ں���

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
