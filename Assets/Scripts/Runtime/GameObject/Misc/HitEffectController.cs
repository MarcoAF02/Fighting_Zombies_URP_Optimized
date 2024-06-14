using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 记录特效命中物体的种类
/// </summary>
public enum HitState
{
	None,

	// 子弹
	BulletDefaultEnvir, // 子弹默认环境
	BulletHitDirt, // 子弹击中泥土
	BulletHitGlass, // 子弹击中玻璃
	BulletHitWood, // 子弹击中木头
	BulletEnemy, // 子弹击中敌人

	// 小刀
	KinfeDefaultEnvir, // 小刀默认环境
	KinfeHitDirt, // 小刀击中泥土
	KinfeHitGlass, // 小刀击中玻璃
	KinfeHitWood, // 小刀击中木头
	KinfeEnemy // 小刀击中敌人
}

/// <summary>
/// 击中特效控制器
/// </summary>
public class HitEffectController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("击中特效过多久消失")]
	[SerializeField] private float VFXObjSustainTime;

	[Header("击中贴花控制器")]
	[SerializeField] private HitDecalController hitDecalController;
	[Header("击中音效控制器")]
	[SerializeField] private HitEffectSound hitEffectSound;

	[Header("击中部分物体时发出的亮光")]
	[SerializeField] private Light hitLight;
	[Header("亮光持续时间")]
	[SerializeField] private float hitLightSustainTime;

	[Header("环境击中特效一览")]
	[Header("子弹击中环境的主特效")]
	[SerializeField] private ParticleSystem bulletHitMainVFX;
	[Header("小刀砍中环境的主特效")]
	[SerializeField] private ParticleSystem kinfeHitMainVFX;

	[Header("击中泥土")]
	[SerializeField] private ParticleSystem hitDirtVFX;
	[Header("击中玻璃")]
	[SerializeField] private ParticleSystem hitGlassVFX;
	[Header("击中木头")]
	[SerializeField] private ParticleSystem hitWoodVFX;

	[Header("敌人击中特效一览")]
	[Header("子弹击中敌人的主特效")]
	[SerializeField] private ParticleSystem bulletHitEnemyVFX;
	[Header("小刀砍中敌人的主特效")]
	[SerializeField] private ParticleSystem kinfeHitEnemyVFX;
	[Header("命中头部击杀特效")]
	[Tooltip("这个特效爆头独有")]
	[SerializeField] private ParticleSystem hitEnemyHeadVFX;

	// 协程
	private Coroutine playHitLight_IECor;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		// Debug.Log(transform.localScale);
		Destroy(gameObject, VFXObjSustainTime); // 隔一段时间后删除自己
	}

	#endregion

	#region 特效播放功能

	public void PlayHitVFX(HitState _hitState, Transform _hitObjTrans, float _playerRotateY, float _decalSize, bool _makeHitDecal)
	{
		// 特效大小由玩家的武器脚本决定

		if (_hitState == HitState.None)
		{
			Debug.LogWarning("攻击击中了未知材质，暂不播放击中特效");
			return;
		}

		// ==================== 子弹 ==================== //

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
			PlayHitDirtVFX(); // 只播放击中泥土的特效

			hitEffectSound.PlayBulletHitDirtSound();
		}

		if (_hitState == HitState.BulletEnemy)
		{
			PlayBulletHitEnemyVFX();

			hitEffectSound.PlayBulletHitEnemySound();
		}

		// ==================== 小刀 ==================== //

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
			PlayHitDirtVFX(); // 只播放击中泥土的特效

			hitEffectSound.PlayKinfeHitDirtSound();
		}

		if (_hitState == HitState.KinfeEnemy)
		{
			PlayKinfeHitEnemyVFX();

			hitEffectSound.PlayKinfeHitEnemySound();
		}

		//if (_makeHitDecal)
		//{
		//	hitDecalController.ApplyHitDecal(_hitState, _hitObjTrans, _playerRotateY, _decalSize); // 应用贴花
		//}
	}

	#region 子弹独有击中特效播放

	// 播放子弹击中环境的主特效
	private void PlayMainBulletEnvirVFX()
	{
		bulletHitMainVFX.Emit(1);
		bulletHitMainVFX.Play();
	}

	// 播放子弹击中敌人的特效
	private void PlayBulletHitEnemyVFX()
	{
		bulletHitEnemyVFX.Emit(1);
		bulletHitEnemyVFX.Play();
	}

	#endregion

	#region 小刀独有击中特效播放

	// 播放小刀击中环境的主特效
	private void PlayMainKinfeEnvirVFX()
	{
		kinfeHitMainVFX.Emit(1);
		kinfeHitMainVFX.Play();
	}

	// 播放小刀砍中敌人的特效
	private void PlayKinfeHitEnemyVFX()
	{
		kinfeHitEnemyVFX.Emit(1);
		kinfeHitEnemyVFX.Play();
	}

	#endregion

	#region 击中环境特效播放

	// 播放击中泥土的特效
	private void PlayHitDirtVFX()
	{
		hitDirtVFX.Emit(1);
		hitDirtVFX.Play();
	}

	// 播放击中玻璃的特效
	private void PlayHitGlassVFX()
	{
		hitGlassVFX.Emit(1);
		hitGlassVFX.Play();
	}

	// 播放击中木头的特效
	private void PlayHitWoodVFX()
	{
		hitWoodVFX.Emit(1);
		hitWoodVFX.Play();
	}

	#endregion

	/// <summary>
	/// 播放击中亮光
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
