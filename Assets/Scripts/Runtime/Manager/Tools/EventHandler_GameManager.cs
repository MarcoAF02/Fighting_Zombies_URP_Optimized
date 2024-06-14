using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ���ȸı���¼�������
/// </summary>
public class EventHandler_GameManager : MonoBehaviour
{
	#region ��Ϸ�����ƽ�ʱ���������¼�

	// ��Ϸ�����ƽ��¼�
	public delegate void ChangeGameProgress(GameProgress gameProgress); // ί��
	public event ChangeGameProgress ChangeGameProgressEvent; // ���������ί�ж����¼�

	/// <summary>
	/// ��Ӧ��Ϸ���ȸı��¼��ĺ���
	/// </summary>
	/// <param name="gameProgress"></param>
	public void InvokeChangeGameProgress(GameProgress gameProgress)
	{
		ChangeGameProgressEvent(gameProgress);
	}

	#endregion

	#region ���ÿ��ɱ����һ�����˷������¼�

	public delegate void KillEnemy();

	public event KillEnemy OnKillEnemyEvent;

	/// <summary>
	/// ��Ӧ��һ�ɱ���˵��¼�
	/// </summary>
	public void InvokeKillEnemyEvent()
	{
		OnKillEnemyEvent();
	}

	#endregion

}
