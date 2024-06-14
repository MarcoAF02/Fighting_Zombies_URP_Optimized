using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��¼��Ч�������������
/// </summary>
public enum HitState
{
	None,

	// �ӵ�
	BulletDefaultEnvir, // �ӵ�Ĭ�ϻ���
	BulletHitDirt, // �ӵ���������
	BulletHitGlass, // �ӵ����в���
	BulletHitWood, // �ӵ�����ľͷ
	BulletEnemy, // �ӵ����е���

	// С��
	KinfeDefaultEnvir, // С��Ĭ�ϻ���
	KinfeHitDirt, // С����������
	KinfeHitGlass, // С�����в���
	KinfeHitWood, // С������ľͷ
	KinfeEnemy // С�����е���
}

/// <summary>
/// ������Ч������
/// </summary>
public class HitEffectController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("������Ч�������ʧ")]
	[SerializeField] private float VFXObjSustainTime;

	[Header("��������������")]
	[SerializeField] private HitDecalController hitDecalController;
	[Header("������Ч������")]
	[SerializeField] private HitEffectSound hitEffectSound;

	[Header("���в�������ʱ����������")]
	[SerializeField] private Light hitLight;
	[Header("�������ʱ��")]
	[SerializeField] private float hitLightSustainTime;

	[Header("����������Чһ��")]
	[Header("�ӵ����л���������Ч")]
	[SerializeField] private ParticleSystem bulletHitMainVFX;
	[Header("С�����л���������Ч")]
	[SerializeField] private ParticleSystem kinfeHitMainVFX;

	[Header("��������")]
	[SerializeField] private ParticleSystem hitDirtVFX;
	[Header("���в���")]
	[SerializeField] private ParticleSystem hitGlassVFX;
	[Header("����ľͷ")]
	[SerializeField] private ParticleSystem hitWoodVFX;

	[Header("���˻�����Чһ��")]
	[Header("�ӵ����е��˵�����Ч")]
	[SerializeField] private ParticleSystem bulletHitEnemyVFX;
	[Header("С�����е��˵�����Ч")]
	[SerializeField] private ParticleSystem kinfeHitEnemyVFX;
	[Header("����ͷ����ɱ��Ч")]
	[Tooltip("�����Ч��ͷ����")]
	[SerializeField] private ParticleSystem hitEnemyHeadVFX;

	// Э��
	private Coroutine playHitLight_IECor;

	#endregion

	#region �����������ں���

	private void Start()
	{
		// Debug.Log(transform.localScale);
		Destroy(gameObject, VFXObjSustainTime); // ��һ��ʱ���ɾ���Լ�
	}

	#endregion

	#region ��Ч���Ź���

	public void PlayHitVFX(HitState _hitState, Transform _hitObjTrans, float _playerRotateY, float _decalSize, bool _makeHitDecal)
	{
		// ��Ч��С����ҵ������ű�����

		if (_hitState == HitState.None)
		{
			Debug.LogWarning("����������δ֪���ʣ��ݲ����Ż�����Ч");
			return;
		}

		// ==================== �ӵ� ==================== //

		if (_hitState == HitState.BulletDefaultEnvir)
		{
			PlayMainBulletEnvirVFX();
			PlayHitLight();

			hitEffectSound.PlayBulletDefHitSound();
		}

		if (_hitState == HitState.BulletHitGlass)
		{
			PlayMainBulletEnvirVFX();
			PlayHitGlassVFX();

			hitEffectSound.PlayBulletHitGlassSound();
		}

		if (_hitState == HitState.BulletHitWood)
		{
			PlayMainBulletEnvirVFX();
			PlayHitWoodVFX();
			PlayHitLight();

			hitEffectSound.PlayBulletHitWoodSound();
		}

		if (_hitState == HitState.BulletHitDirt)
		{
			PlayHitDirtVFX(); // ֻ���Ż�����������Ч

			hitEffectSound.PlayBulletHitDirtSound();
		}

		if (_hitState == HitState.BulletEnemy)
		{
			PlayBulletHitEnemyVFX();

			hitEffectSound.PlayBulletHitEnemySound();
		}

		// ==================== С�� ==================== //

		if (_hitState == HitState.KinfeDefaultEnvir)
		{
			PlayMainKinfeEnvirVFX();
			PlayHitLight();

			hitEffectSound.PlayKinfeDefHitSound();
		}

		if (_hitState == HitState.KinfeHitGlass)
		{
			PlayMainKinfeEnvirVFX();
			PlayHitGlassVFX();

			hitEffectSound.PlayKinfeHitGlassSound();
		}

		if (_hitState == HitState.KinfeHitWood)
		{
			PlayMainKinfeEnvirVFX();
			PlayHitWoodVFX();
			PlayHitLight();

			hitEffectSound.PlayKinfeHitWoodSound();
		}

		if (_hitState == HitState.KinfeHitDirt)
		{
			PlayHitDirtVFX(); // ֻ���Ż�����������Ч

			hitEffectSound.PlayKinfeHitDirtSound();
		}

		if (_hitState == HitState.KinfeEnemy)
		{
			PlayKinfeHitEnemyVFX();

			hitEffectSound.PlayKinfeHitEnemySound();
		}

		//if (_makeHitDecal)
		//{
		//	hitDecalController.ApplyHitDecal(_hitState, _hitObjTrans, _playerRotateY, _decalSize); // Ӧ������
		//}
	}

	#region �ӵ����л�����Ч����

	// �����ӵ����л���������Ч
	private void PlayMainBulletEnvirVFX()
	{
		bulletHitMainVFX.Emit(1);
		bulletHitMainVFX.Play();
	}

	// �����ӵ����е��˵���Ч
	private void PlayBulletHitEnemyVFX()
	{
		bulletHitEnemyVFX.Emit(1);
		bulletHitEnemyVFX.Play();
	}

	#endregion

	#region С�����л�����Ч����

	// ����С�����л���������Ч
	private void PlayMainKinfeEnvirVFX()
	{
		kinfeHitMainVFX.Emit(1);
		kinfeHitMainVFX.Play();
	}

	// ����С�����е��˵���Ч
	private void PlayKinfeHitEnemyVFX()
	{
		kinfeHitEnemyVFX.Emit(1);
		kinfeHitEnemyVFX.Play();
	}

	#endregion

	#region ���л�����Ч����

	// ���Ż�����������Ч
	private void PlayHitDirtVFX()
	{
		hitDirtVFX.Emit(1);
		hitDirtVFX.Play();
	}

	// ���Ż��в�������Ч
	private void PlayHitGlassVFX()
	{
		hitGlassVFX.Emit(1);
		hitGlassVFX.Play();
	}

	// ���Ż���ľͷ����Ч
	private void PlayHitWoodVFX()
	{
		hitWoodVFX.Emit(1);
		hitWoodVFX.Play();
	}

	#endregion

	/// <summary>
	/// ���Ż�������
	/// </summary>
	private void PlayHitLight()
	{
		playHitLight_IECor = StartCoroutine(PlayHitLight_IE());
	}

	private IEnumerator PlayHitLight_IE()
	{
		hitLight.enabled = true;
		yield return new WaitForSeconds(hitLightSustainTime);
		hitLight.enabled = false;
	}

	#endregion

}
