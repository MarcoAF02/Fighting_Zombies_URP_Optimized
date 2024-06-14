using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸͨ�ع�����
/// </summary>
public class GameCompleteChecker : MonoBehaviour
{
	#region ��������ͱ���

	// ��鷽ʽ�����Ż�һ����������ҵ�Ŀ��ص��ϣ���ҵִ�ʱ������ɱ�����Ƿ�λ

	[Header("���ߵĳ���")]
	[SerializeField] private float checkRayDistance;
	[Header("��ҵ� LayerIndex")]
	[SerializeField] private int playerLayerIndex;

	#endregion

	#region �����������ں���

	private void Update()
	{
		CheckPlayerComplete();
	}

	#endregion

	#region ���ͨ�ؼ��

	private void CheckPlayerComplete()
	{
		if (GameProgressManager.Instance.CurrentGameProgress == GameProgress.GameComplete) return;

		if (Physics.Raycast(transform.position, transform.forward, checkRayDistance, 1 << playerLayerIndex))
		{
			if (GameProgressManager.Instance.isKillComplete)
			{
				GameProgressManager.Instance.playerController.SwitchState(GameProgressManager.Instance.playerController.playerCompleteState);
				Debug.Log("��Ϸͨ��");
			}
			else
			{
				Debug.Log("ɱ����δ��꣬��û��ͨ��");
			}
		}
	}

	#endregion

	#region DEBUG �ú���

	private void OnDrawGizmosSelected()
	{
		Debug.DrawRay(transform.position, transform.forward * checkRayDistance, Color.green);
	}

	#endregion
}
