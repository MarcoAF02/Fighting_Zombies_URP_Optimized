using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ч������Ϸ��Ч
// ���� 3D ������Ч

/// <summary>
/// ������Ч������
/// </summary>
public class HitEffectSound : MonoBehaviour
{
	#region ��������ͱ���

	// ע�����������ǹ̶��ģ����øı�

	[Header("����Ч����Դ���")]
	[SerializeField] private AudioSource hitAudioSource;

	[Header("�ӵ������Ƶ")]
	[Header("���л���ʱ��Ĭ����Ƶ")]
	[SerializeField] private List<AudioClip> bulletDefHitClipList = new List<AudioClip>();
	[Header("��������")]
	[SerializeField] private List<AudioClip> bulletHitDirtClipList = new List<AudioClip>();
	[Header("���в���")]
	[SerializeField] private List<AudioClip> bulletHitGlassClipList = new List<AudioClip>();
	[Header("����ľͷ")]
	[SerializeField] private List<AudioClip> bulletHitWoodClipList = new List<AudioClip>();
	[Header("���е���")]
	[SerializeField] private List<AudioClip> bulletHitEnemyClipList = new List<AudioClip>();

	[Header("С�������Ƶ")]
	[Header("���л���ʱ��Ĭ����Ƶ")]
	[SerializeField] private List<AudioClip> kinfeDefHitClipList = new List<AudioClip>();
	[Header("��������")]
	[SerializeField] private List<AudioClip> kinfeHitDirtClipList = new List<AudioClip>();
	[Header("���в���")]
	[SerializeField] private List<AudioClip> kinfeHitGlassClipList = new List<AudioClip>();
	[Header("����ľͷ")]
	[SerializeField] private List<AudioClip> kinfeHitWoodClipList = new List<AudioClip>();
	[Header("���е���")]
	[SerializeField] private List<AudioClip> kinfeHitEnemyClipList = new List<AudioClip>();

	#endregion

	#region ��Ч���Ź���

	#region �ӵ���Ч

	/// <summary>
	/// �����ӵ����л�����Ĭ����Ч
	/// </summary>
	public void PlayBulletDefHitSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletDefHitClipList.Count);
		hitAudioSource.clip = bulletDefHitClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// �����ӵ�������������Ч
	/// </summary>
	public void PlayBulletHitDirtSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletHitDirtClipList.Count);
		hitAudioSource.clip = bulletHitDirtClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// �����ӵ�����ľͷ����Ч
	/// </summary>
	public void PlayBulletHitWoodSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletHitWoodClipList.Count);
		hitAudioSource.clip = bulletHitWoodClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// �����ӵ����в�������Ч
	/// </summary>
	public void PlayBulletHitGlassSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletHitGlassClipList.Count);
		hitAudioSource.clip = bulletHitGlassClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// �����ӵ����е��˵���Ч
	/// </summary>
	public void PlayBulletHitEnemySound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletHitEnemyClipList.Count);
		hitAudioSource.clip = bulletHitEnemyClipList[randomIndex];
		hitAudioSource.Play();
	}

	#endregion

	#region С����Ч

	#endregion

	/// <summary>
	/// ����С�����л�����Ĭ����Ч
	/// </summary>
	public void PlayKinfeDefHitSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeDefHitClipList.Count);
		hitAudioSource.clip = kinfeDefHitClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// ����С��������������Ч
	/// </summary>
	public void PlayKinfeHitDirtSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeHitDirtClipList.Count);
		hitAudioSource.clip = kinfeHitDirtClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// ����С������ľͷ����Ч
	/// </summary>
	public void PlayKinfeHitWoodSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeHitWoodClipList.Count);
		hitAudioSource.clip = kinfeHitWoodClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// ����С�����в�������Ч
	/// </summary>
	public void PlayKinfeHitGlassSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeHitGlassClipList.Count);
		hitAudioSource.clip = kinfeHitGlassClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// �����ӵ����е��˵���Ч
	/// </summary>
	public void PlayKinfeHitEnemySound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeHitEnemyClipList.Count);
		hitAudioSource.clip = kinfeHitEnemyClipList[randomIndex];
		hitAudioSource.Play();
	}

	#endregion
}
