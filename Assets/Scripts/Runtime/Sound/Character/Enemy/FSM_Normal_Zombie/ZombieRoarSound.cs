using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���˺����������Ϸ��Ч
// ���� 3D ��Ч

public class ZombieRoarSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��Ч������Դ���")]
	[SerializeField] private AudioSource roarAudioSource;

	[Header("����վ׮ʱ�ĺ������С")]
	[SerializeField] private float idleRoarAudioVolume;
	[Header("����Ѳ��ʱ�ĺ������С")]
	[SerializeField] private float patrolRoarAudioVolume;
	[Header("�������˲��ĺ������С")]
	[SerializeField] private float shockRoarAudioVolume;
	[Header("׷�����ʱ�ĺ������С")]
	[SerializeField] private float pursuitRoarAudioVolume;
	[Header("��������ʱ�ĺ������С")]
	[SerializeField] private float attackRoarAudioVolume;
	[Header("���˽���Ӳֱ�ĺ������С")]
	[SerializeField] private float hardStraightRoarAudioVolume;
	[Header("���������ĺ������С")]
	[SerializeField] private float deadRoarAudioVolume;

	[Header("վ׮�����Ч�ļ��ʱ��")]
	[SerializeField] private float idleSoundIntervalTime;
	private float idleSoundTotalTime;
	[Header("Ѳ�ߺ����Ч�ļ��ʱ��")]
	[SerializeField] private float patrolSoundIntervalTime;
	private float patrolSoundTotalTime;
	[Header("׷�����ʱ�ĺ����ʱ����")]
	[SerializeField] private float pursuitSoundIntervalTime;
	private float pursuitSoundTotalTime;

	[Header("������ҵ�˲������Ҫ�������")]
	[SerializeField] private float shockAudioSustainTime = 1f;
	[HideInInspector] public bool isShocked = false; // �Ƿ��й���

	[Header("����ʱ��Ч�б�")]
	[SerializeField] private List<AudioClip> idleAudioList = new List<AudioClip>();
	[Header("����Ѳ��ʱ��Ч�б�")]
	[SerializeField] private List<AudioClip> patrolAudioList = new List<AudioClip>();
	[Header("�������˲��ĺ�����б�")]
	[SerializeField] private List<AudioClip> shockAudioList = new List<AudioClip>();
	[Header("׷�����ʱ�ĺ�����б�")]
	[SerializeField] private List<AudioClip> pursuitAudioList = new List<AudioClip>();
	[Header("��������ʱ�ĺ�����б�")]
	[SerializeField] private List<AudioClip> attackAudioList = new List<AudioClip>();
	[Header("���˽���Ӳֱ�ĺ������Ч�б�")]
	[SerializeField] private List<AudioClip> hardStraightAudioList = new List<AudioClip>();
	[Header("����ʱ�ĺ������Ч�б�")]
	[SerializeField] private List<AudioClip> deadAudioList = new List<AudioClip>();

	// Э��
	private Coroutine playShockRoarSound_IECor;

	#endregion

	#region �����������ں���

	private void Start()
	{
		idleSoundTotalTime = idleSoundIntervalTime;
		patrolSoundTotalTime = patrolSoundIntervalTime;
	}

	#endregion

	#region ��Ч���Ź���

	/// <summary>
	/// ��ʬվ׮�ͺ���Ч����
	/// </summary>
	public void PlayIdleRoarSound()
	{
		idleSoundTotalTime += Time.deltaTime;
		if (idleSoundTotalTime > 32768f) idleSoundTotalTime = 32768f;

		roarAudioSource.volume = idleRoarAudioVolume;

		if (idleSoundTotalTime > idleSoundIntervalTime)
		{
			idleSoundTotalTime = 0f;
			int randomIndex = UnityEngine.Random.Range(0, idleAudioList.Count);
			roarAudioSource.clip = idleAudioList[randomIndex];
			roarAudioSource.Play();
		}
	}

	/// <summary>
	/// ��ʬѲ�ߵͺ���Ч����
	/// </summary>
	public void PlayPatrolRoarSound()
	{
		patrolSoundTotalTime += Time.deltaTime;
		if (patrolSoundTotalTime > 32768f) patrolSoundTotalTime = 32768f;

		roarAudioSource.volume = patrolRoarAudioVolume;

		if (patrolSoundTotalTime > patrolSoundIntervalTime)
		{
			patrolSoundTotalTime = 0f;
			int randomIndex = UnityEngine.Random.Range(0, patrolAudioList.Count);
			roarAudioSource.clip = patrolAudioList[randomIndex];
			roarAudioSource.Play();
		}
	}

	/// <summary>
	/// ��ʬ�������˲��ĺ����
	/// </summary>
	public void PlayShockRoarSound()
	{
		if (isShocked) return;
		if (!gameObject.activeInHierarchy) return;

		playShockRoarSound_IECor = StartCoroutine(PlayShockRoarSound_IE());
	}

	private IEnumerator PlayShockRoarSound_IE()
	{
		roarAudioSource.volume = shockRoarAudioVolume;

		int randomIndex = UnityEngine.Random.Range(0, shockAudioList.Count);
		roarAudioSource.clip = shockAudioList[randomIndex];
		roarAudioSource.Play();

		yield return new WaitForSeconds(shockAudioSustainTime);

		isShocked = true;
	}

	/// <summary>
	/// ��ʬ׷�����ʱ�ĺ����
	/// </summary>
	public void PlayPursuitRoarSound()
	{
		pursuitSoundTotalTime += Time.deltaTime;
		if (pursuitSoundTotalTime > pursuitSoundIntervalTime) pursuitSoundTotalTime = 32768f;
		roarAudioSource.volume = pursuitRoarAudioVolume;

		if (pursuitSoundTotalTime > pursuitSoundIntervalTime)
		{
			pursuitSoundTotalTime = 0f;

			int randomIndex = Random.Range(0, pursuitAudioList.Count);
			roarAudioSource.clip = pursuitAudioList[randomIndex];
			roarAudioSource.Play();
		}
	}

	/// <summary>
	/// ��ʬ��������ʱ�ĺ����
	/// </summary>
	public void PlayAttackRoarSound()
	{
		roarAudioSource.clip = null; // ����ձ����Ч
		roarAudioSource.volume = attackRoarAudioVolume;

		int randomIndex = UnityEngine.Random.Range(0, attackAudioList.Count);
		roarAudioSource.clip = attackAudioList[randomIndex];
		roarAudioSource.Play();
	}

	/// <summary>
	/// ��ʬ���˽���Ӳֱ״̬���������
	/// </summary>
	public void PlayHardStraightRoarSound()
	{
		roarAudioSource.clip = null; // ����ձ����Ч
		roarAudioSource.volume = hardStraightRoarAudioVolume;

		int randomIndex = UnityEngine.Random.Range(0, hardStraightAudioList.Count);
		roarAudioSource.clip = hardStraightAudioList[randomIndex];
		roarAudioSource.Play();
	}

	/// <summary>
	/// ��ʬ����ʱ���������
	/// </summary>
	public void PlayDeadRoarSound()
	{
		roarAudioSource.clip = null; // ����ձ����Ч
		roarAudioSource.volume = deadRoarAudioVolume;

		int randomIndex = UnityEngine.Random.Range(0, deadAudioList.Count);
		roarAudioSource.clip = deadAudioList[randomIndex];
		roarAudioSource.Play();
	}

	#endregion

}
