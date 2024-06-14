using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ǹ��Ч����Ϸ��Ч
// ������ 3D ��Ч

/// <summary>
/// �����ǹ��Ч����
/// </summary>
public class PlayerPistolSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ǹ��Դ���")]
	[SerializeField] private AudioSource pistolShootAudioSource;
	[Header("������Դ���")]
	[SerializeField] private AudioSource pistolReloadAudioSource;

	[Header("��Դ�����������")]
	[SerializeField] private float shootVolumeLevel;
	[SerializeField] private float reloadVolumeLevel;
	[SerializeField] private float triggerVolumeLevel;

	[Header("��ǹ������Ч")]
	[Header("��ϻ�˳�")]
	[SerializeField] private AudioClip magOutAudio;
	[Header("��ϻװ��ȥ")]
	[SerializeField] private AudioClip magInAudio;
	[Header("�ָ��ղֹһ�")]
	[SerializeField] private AudioClip magResetAudio;

	[Header("��ϻ�˳����ӳ�ʱ��")]
	[SerializeField] private float magOutDelayTime;
	[Header("��ϻ�˳���װ��ȥ�����೤ʱ��")]
	[SerializeField] private float magOutSustainTime;
	[Header("��ϻװ��ȥ��ûָ��ղֹһ�")]
	[SerializeField] private float magInResetTime;

	[Header("��ϻ���ʱ��ײ����Ч")]
	[SerializeField] private AudioClip emptyTriggerAudio;

	[Header("��ǹʹ�õĿ�ǹ��Ч�б�")]
	[Tooltip("��ǹʱ���ȡһ����Ч")]
	[SerializeField] private List<AudioClip> shootAudioList = new List<AudioClip>();

	// Э��
	private Coroutine PlayPistolReloadSound_IECor;

	#endregion

	#region ��Ч���Ź���

	/// <summary>
	/// ��ǹ��Ч
	/// </summary>
	public void PlayPistolShootSound()
	{
		pistolShootAudioSource.volume = shootVolumeLevel;

		int randomIndex = UnityEngine.Random.Range(0, shootAudioList.Count);
		pistolShootAudioSource.clip = shootAudioList[randomIndex];

		pistolShootAudioSource.Play();
	}

	/// <summary>
	/// ��ϻ���ʱ����ײ����Ч
	/// </summary>
	public void PlayEmptyTriggerSound()
	{
		pistolShootAudioSource.volume = triggerVolumeLevel;
		pistolShootAudioSource.clip = emptyTriggerAudio;
		pistolShootAudioSource.Play();
	}

	/// <summary>
	/// ��ǹ����ʱ���Ż�����Ч
	/// </summary>
	public void PlayPistolReloadSound()
	{
		PlayPistolReloadSound_IECor = StartCoroutine(PlayPistolReloadSound_IE());
	}

	private IEnumerator PlayPistolReloadSound_IE()
	{
		pistolReloadAudioSource.volume = reloadVolumeLevel;

		yield return new WaitForSeconds(magOutDelayTime);

		pistolReloadAudioSource.clip = magOutAudio;
		pistolReloadAudioSource.Play();

		yield return new WaitForSeconds(magOutSustainTime);

		pistolReloadAudioSource.clip = magInAudio;
		pistolReloadAudioSource.Play();

		yield return new WaitForSeconds(magInResetTime);

		pistolReloadAudioSource.clip = magResetAudio;
		pistolReloadAudioSource.Play();
	}

	#endregion
}
