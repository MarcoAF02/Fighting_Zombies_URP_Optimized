using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��������������Ϸ����
// ��������ȫ�ֲ��ţ������� 3D ��Ч

/// <summary>
/// �������ֹ�����
/// </summary>
public class GamePlayMusic : MonoBehaviour
{
	#region ��������ͱ���

	[Header("����������Դ���")]
	[SerializeField] private AudioSource musicAudioSource;

	[Header("ǰ����������")]
	[SerializeField] private float earlyBGMVolume;
	[Header("������������")]
	[SerializeField] private float laterBGMVolume;
	[Header("��Ϸͨ��ʱ����������")]
	[SerializeField] private float gameCompleteBGMVolume;
	[Header("��Ϸʧ��ʱ����������")]
	[SerializeField] private float gameFailBGMVolume;

	[Header("ǰ�ڱ�������Ƭ��")]
	[SerializeField] private AudioClip earlyStageBGM;
	[Header("���ڱ�������Ƭ��")]
	[SerializeField] private AudioClip laterStageBGM;
	[Header("��Ϸͨ��ʱ������Ƭ��")]
	[SerializeField] private AudioClip gameCompleteBGM;
	[Header("��Ϸʧ��ʱ������Ƭ��")]
	[SerializeField] private AudioClip gameFailBGM;

	#endregion

	#region �����������ں���

	private void Start()
	{
		GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent += ChangeAndPlayBGM;
	}

	private void OnDisable()
	{
		GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent -= ChangeAndPlayBGM;
	}

	#endregion

	#region �������ֵĲ��ź��л�����

	// ������Ϸ���ȸı䱳������
	private void ChangeAndPlayBGM(GameProgress gameProgress)
	{
		if (gameProgress == GameProgress.None)
		{
			Debug.LogWarning("��Ϸ����δ���壬������Ϸ�����߼�");
			return;
		}

		if (gameProgress == GameProgress.EarlyStage)
		{
			musicAudioSource.loop = true;
			musicAudioSource.volume = earlyBGMVolume;
			musicAudioSource.clip = earlyStageBGM;
			musicAudioSource.Play();
		}

		if (gameProgress == GameProgress.LaterStage)
		{
			musicAudioSource.loop = true;
			musicAudioSource.volume = laterBGMVolume;
			musicAudioSource.clip = laterStageBGM;
			musicAudioSource.Play();
		}

		if (gameProgress == GameProgress.GameComplete)
		{
			musicAudioSource.loop = true;
			musicAudioSource.volume = gameCompleteBGMVolume;
			musicAudioSource.clip = gameCompleteBGM;
			musicAudioSource.Play();
		}

		if (gameProgress == GameProgress.GameFail)
		{
			musicAudioSource.loop = false;
			musicAudioSource.volume = gameFailBGMVolume;
			musicAudioSource.clip = gameFailBGM;
			musicAudioSource.Play();
		}

	}

	#endregion
}
