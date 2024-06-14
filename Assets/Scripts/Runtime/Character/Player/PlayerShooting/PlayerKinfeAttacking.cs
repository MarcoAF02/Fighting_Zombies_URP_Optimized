using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家小刀攻击脚本
/// </summary>
public class PlayerKinfeAttacking : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("相机动画控制器")]
	[SerializeField] private PlayerCameraAnim playerCameraAnim;
	[Header("小刀动画控制器")]
	public PlayerKinfeAnim playerKinfeAnim;
	[Header("小刀模型角色物体")]
	public GameObject kinfeArmModel;

	[Header("小刀挥舞音效管理")]
	[SerializeField] private PlayerKinfeSound playerKinfeSound;

	[Header("击中特效")]
	[SerializeField] private GameObject hitVFX;

	#endregion

	#region 小刀基本数据

	[Header("小刀攻击的射线发射点")]
	[SerializeField] private List<Transform> attackRayStartTrans = new List<Transform>();

	[Header("小刀基本伤害值")]
	[SerializeField] private float kinfeDamage;
	[Header("小刀攻击范围")]
	[SerializeField] private float kinfeAttackDistance;
	[Header("小刀攻击 CD 时间")]
	[SerializeField] private float attackCDTime;
	private float attackTotalTime;
	[Header("小刀攻击延迟触发时间")]
	[SerializeField] private float delayAttackTime;
	[Header("武器取出来需要的时间")]
	public float weaponGetTime;
	[Header("武器收起来需要的时间")]
	public float weaponHideTime;

	[Header("击中物理物体时施加的力度")]
	[SerializeField] private float hitPhysicsObjForce;

	[Header("击中特效的大小")]
	[SerializeField] private Vector3 hitVFXSize;
	[Header("击中特效命中不同东西的放大倍率")]
	[Header("击中环境物体")]
	[Range(0, 2)]
	[SerializeField] private float hitEnvirObjVFXMag;
	[Header("击中僵尸头部")]
	[Range(0, 2)]
	[SerializeField] private float hitZombieHeadVFXMag;
	[Header("击中僵尸躯干")]
	[Range(0, 2)]
	[SerializeField] private float hitZombieBodyVFXMag;
	[Header("击中僵尸四肢")]
	[Range(0, 2)]
	[SerializeField] private float hitZombieFourLimbsVFXMag;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnEnable() // 改脚本激活时表示玩家拿出了小刀
	{
		attackTotalTime = attackTotalTime + attackCDTime;
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 小刀攻击 GamePlay 功能

	/// <summary>
	/// 小刀执行攻击
	/// </summary>
	public void PlayerPerformKinfeAttack()
	{
		attackTotalTime = attackTotalTime + Time.deltaTime;

		if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;
		if (playerController.fpsBodyController.currentArmState == ArmState.TopWall) return;

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Shoot.WasPressedThisFrame() &&
			attackTotalTime > attackCDTime)
		{
			StartCoroutine(PlayerKinfeAttack());
		}
	}

	private IEnumerator PlayerKinfeAttack()
	{
		playerController.weaponManager.playerGunState = PlayerGunState.Shooting;

		attackTotalTime = 0f;

		playerCameraAnim.KinfeAttackVibrateAnim();
		playerKinfeAnim.PlayAttackAnim();

		yield return new WaitForSeconds(delayAttackTime);

		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex) | (1 << playerController.layerAndTagCollection_Player.doorInteractiveLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		for (int i = 0; i < attackRayStartTrans.Count; i ++)
		{
			if (Physics.Raycast(attackRayStartTrans[i].position, attackRayStartTrans[i].forward, out hitObj, kinfeAttackDistance, ignoreLayerIndex))
			{
				// Debug.Log(hitObj.collider.name);

				// 击中普通僵尸敌人
				if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.zombieHeadTag))
				{
					hitObj.collider.GetComponentInParent<ZombieView>()?.DirectLockPlayer(playerController.transform);
					hitObj.collider.GetComponentInParent<ZombieHealth>()?.TakeDamage(hitObj.collider.tag, kinfeDamage);
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitZombieHeadVFXMag, hitVFXSize.y * hitZombieHeadVFXMag, hitVFXSize.z * hitZombieHeadVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeEnemy, false);

					break;
				}
				else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.zombieBodyTag))
				{
					hitObj.collider.GetComponentInParent<ZombieView>()?.DirectLockPlayer(playerController.transform);
					hitObj.collider.GetComponentInParent<ZombieHealth>()?.TakeDamage(hitObj.collider.tag, kinfeDamage);
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitZombieBodyVFXMag, hitVFXSize.y * hitZombieBodyVFXMag, hitVFXSize.z * hitZombieBodyVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeEnemy, false);

					break;
				}
				else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.zombieFourLimbsTag))
				{
					hitObj.collider.GetComponentInParent<ZombieView>()?.DirectLockPlayer(playerController.transform);
					hitObj.collider.GetComponentInParent<ZombieHealth>()?.TakeDamage(hitObj.collider.tag, kinfeDamage);
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitZombieFourLimbsVFXMag, hitVFXSize.y * hitZombieFourLimbsVFXMag, hitVFXSize.z * hitZombieFourLimbsVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeEnemy, false);

					break;
				}

				// 击中其他环境
				else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.envirDirt))
				{
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeHitDirt, true);

					break;
				}
				else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.envirWood))
				{
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeHitWood, true);

					break;
				}
				else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.envirGlass))
				{
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeHitGlass, true);

					break;
				}
				else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.envirPhysicsObj))
				{
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeDefaultEnvir, false);

					Rigidbody body = hitObj.collider.GetComponent<Rigidbody>();

					if (body != null)
					{
						Vector3 hitDir = (body.transform.position - transform.position).normalized;
						body.AddForce(hitDir * hitPhysicsObjForce, ForceMode.Impulse);
					}

					break;
				}
				// 最后一种情况：击中默认环境物体
				else
				{
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeDefaultEnvir, true);

					break;
				}
			}
		}

		playerController.playerBreathSound.PlayAttackRoarSound(); // 播放攻击吼叫音效
		playerKinfeSound.PlayKinfeWaveSound(); // 播放挥刀音效
		playerController.weaponManager.playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region 攻击时会发生的事情

	/// <summary>
	/// 武器击中目标时播放特效
	/// </summary>
	/// <param name="_pos"></param>
	/// <param name="_normal"></param>
	/// <param name="_vfxSize"></param>
	/// <param name="_hitState"></param>
	/// <returns></returns>
	private void PlayKinfeHitVFX(Transform _hitObjTrans, float _playerRotateY, Vector3 _pos, Vector3 _normal, Vector3 _vfxSize, HitState _hitState, bool _makeHitDecal)
	{
		GameObject hitVFXObj = Instantiate(hitVFX, null);

		hitVFXObj.transform.localScale = _vfxSize;
		hitVFXObj.transform.SetLocalPositionAndRotation(_pos, Quaternion.LookRotation(_normal));

		hitVFXObj.GetComponent<HitEffectController>().PlayHitVFX(_hitState, _hitObjTrans, _playerRotateY, _vfxSize.x, _makeHitDecal);
	}

	#endregion
}
