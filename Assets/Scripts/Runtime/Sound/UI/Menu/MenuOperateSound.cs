using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���˵���Ч����ϵͳ��Ч
// ϵͳ��Ч������ 3D ��Ч

/// <summary>
/// ���˵���Ч����
/// </summary>
public class MenuOperateSound : MonoSingleton<MenuOperateSound>
{
	#region ��������ͱ���

	// ��Ϊ��Щ��Ƶ�ǹ̶��ģ����Բ����κ����ȡ��

	[Header("ȷ����Դ���")]
	[SerializeField] private AudioSource checkAudioSource;
	[Header("ȡ����Դ���")]
	[SerializeField] private AudioSource cancelAudioSource;
	[Header("��껬��ѡ����Դ���")]
	[SerializeField] private AudioSource mouseUpAudioSource;

	[Header("��Ϸ�ڲ˵�����Ч")]
	[SerializeField] private AudioSource openMenuAudioSourceInGame;
	[Header("��Ϸ�ڲ˵��ر���Ч")]
	[SerializeField] private AudioSource closeMenuAudioSourceInGame;

	#endregion

	#region �����������ں���

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject); // �����Դ�������Ϸ�˵���Ҳ��ʹ��
	}

	private void Start()
	{
		// ʹ��Ƶ���� TimeScale
		checkAudioSource.ignoreListenerPause = true;
		cancelAudioSource.ignoreListenerPause = true;
		mouseUpAudioSource.ignoreListenerPause = true;
		openMenuAudioSourceInGame.ignoreListenerPause = true;
		closeMenuAudioSourceInGame.ignoreListenerPause = true;
	}

	#endregion

	#region UI ѡ����Ч

	/// <summary>
	/// ���Ű�ťȷ�ϵ���Ч
	/// </summary>
	public void PlayCheckSound()
	{
		checkAudioSource.Play();
	}

	/// <summary>
	/// ���Ű�ťȡ������Ч
	/// </summary>
	public void PlayCancelSound()
	{
		cancelAudioSource.Play();
	}
	
	/// <summary>
	/// ������������ڰ�ť�ϵ���Ч
	/// </summary>
	public void PlayMouseUpSound()
	{
		mouseUpAudioSource.Play();
	}

	#endregion

	#region ��Ϸ����ͣ�˵��򿪺͹ر���Ч

	/// <summary>
	/// ������Ϸ�ڲ˵���ʱ����Ч
	/// </summary>
	public void PlayMenuOpenSound_InGame()
	{
		openMenuAudioSourceInGame.Play();
	}

	/// <summary>
	/// ������Ϸ�ڲ˵��ر�ʱ����Ч
	/// </summary>
	public void PlayMenuCloseSound_InGame()
	{
		closeMenuAudioSourceInGame.Play();
	}

	#endregion

}
