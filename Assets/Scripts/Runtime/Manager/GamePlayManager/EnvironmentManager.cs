using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������
/// </summary>
public class EnvironmentManager : MonoBehaviour
{
	// ������Ϸ���ȵ��ƽ��ı��Χ�ƵȻ������

	#region ��������ͱ���

	[Header("��Ϸ�����¼�������")]
	[SerializeField] private EventHandler_GameManager eventHandler_GameManager;

	[Header("�����ƹ�")]
	[SerializeField] private List<Light> envirLightList = new List<Light>();

	[Header("�����ƹ�����Ϸǰ�ڵ���ɫ")]
	[SerializeField] private Color earlyStageLightColor;

	[Header("�����ƹ�����Ϸ���ڵ���ɫ")]
	[SerializeField] private Color laterStageLightColor;

	#endregion

	#region �����������ں���

	private void Awake()
	{
		eventHandler_GameManager = GameObject.FindGameObjectWithTag("EventHandler_GameManager").GetComponent<EventHandler_GameManager>();
	}

	private void OnEnable()
	{
		eventHandler_GameManager.ChangeGameProgressEvent += ChangeEnvirLightColor; // �����¼�
	}

	private void OnDisable()
	{
		eventHandler_GameManager.ChangeGameProgressEvent -= ChangeEnvirLightColor; // ȡ�������¼�
	}

	#endregion

	/// <summary>
	/// ������Ϸ���̸ı��Χ�Ƶ���ɫ
	/// </summary>
	/// <param name="_gameProgress"></param>
	private void ChangeEnvirLightColor(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.None)
		{
			Debug.LogWarning("��Ϸ����Ϊ None��������Ϸ��Ƶ���ȷ��");
			return;
		}

		if (_gameProgress == GameProgress.EarlyStage)
		{
			for (int i = 0; i < envirLightList.Count; i ++)
			{
				try
				{
					envirLightList[i].color = earlyStageLightColor;
				}
				catch
				{
					Debug.Log("��Ϸ�Ѿ��������������������");
				}
			}
		}

		if (_gameProgress == GameProgress.LaterStage)
		{
			for (int i = 0; i < envirLightList.Count; i ++)
			{
				try
				{
					envirLightList[i].color = laterStageLightColor;
				}
				catch
				{
					Debug.Log("��Ϸ�Ѿ��������������������");
				}
			}
		}
	}

}
