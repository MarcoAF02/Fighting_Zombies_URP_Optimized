using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ʣ��ɱ����������
/// </summary>
public class NumberOfKillsUI : MonoBehaviour
{
	#region ��������ͱ���

	[Header("ʣ��ɱ�������� UI")]
	[SerializeField] private TextMeshProUGUI numberOfKillUI;

	#endregion

	#region �����������ں���

	private void Start()
	{
		GameProgressManager.Instance.eventHandler_GameManager.OnKillEnemyEvent += DisplayNumberOfKill;

		DisplayNumberOfKill(); // ��ֱ����ʾһ��
	}

	private void OnDestroy()
	{
		try
		{
			GameProgressManager.Instance.eventHandler_GameManager.OnKillEnemyEvent -= DisplayNumberOfKill;
		}
		catch
		{
			Debug.Log("��Ϸ�Ѿ�����������ȡ���¼�����");
		}
	}

	#endregion

	#region ʣ��ɱ������Ϣ��ʾ

	public void DisplayNumberOfKill()
	{
		numberOfKillUI.text = GameProgressManager.Instance.ResidueKillCount.ToString();
	}

	#endregion
}
