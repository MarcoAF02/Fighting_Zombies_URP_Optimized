using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家手枪射击脚本
/// </summary>
public class PlayerPistolShooting : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("相机动画控制器")]
	[SerializeField] private PlayerCameraAnim playerCameraAnim;
	[Header("手枪动画控制器")]
	public PlayerPistolAnim playerPistolAnim;
	[Header("手枪角色模型物体")]
	public GameObject pistolArmModel;

	[Header("手枪音效管理器")]
	[SerializeField] private PlayerPistolSound playerPistolSound;

	[Header("弹壳物体")]
	[SerializeField] private GameObject bulletShell;
	[Header("击中特效")]
	[SerializeField] private GameObject hitVFX;

	#endregion

	#region 手枪基本数据

	[Header("手枪基础伤害值")]
	[SerializeField] private float gunDamage;

	[Header("枪口后坐力镜头上跳幅度")]
	[SerializeField] private float recoilForce;

	[Header("弹道散射幅度")]
	[Header("移动时的散射幅度")]
	[SerializeField] private Vector3 moveSpreadValue;
	[Header("非瞄准状态下的散射幅度")]
	[SerializeField] private Vector3 notAimSpreadValue;

	private Vector3 computeMoveSpreadValue;
	private Vector3 computeNotAimSpreadValue;
	private Vector3 usedSpreadValue; // 最终计算得到的散射幅度

	[Header("射击 CD 时间")]
	[SerializeField] private float shootCDTime;
	private float shootTotalTime;

	[Header("弹匣打空后自动换弹 CD 时间")]
	[SerializeField] private float autoReloadCDTime;
	private float autoReloadTotalTime;

	[Header("武器取出来需要的时间")]
	public float weaponGetTime;
	[Header("武器收起来需要的时间")]
	public float weaponHideTime;

	[Header("手枪将物理物体向前推的力度")]
	[SerializeField] private float hitPhysicsObjForce;

	[Header("子弹壳的大小")]
	[SerializeField] private Vector3 bulletShellSize;
	[Header("子弹抛壳最小力度")]
	[SerializeField] private float throwBulletShellForceMin;
	[Header("子弹抛壳最大力度")]
	[SerializeField] private float throwBulletShellForceMax;
	[Header("子弹抛壳的旋转量")]
	[SerializeField] private float bulletShellRotateAngle;
	[Header("子弹抛壳起始点")]
	[SerializeField] private Transform bulletShellStartTrans;
	[Header("子弹抛壳方向点")]
	[SerializeField] private Transform bulletShellDirTrans;

	[Header("枪口火焰特效")]
	[SerializeField] private List<ParticleSystem> pistolMuzzleFlash;

	[Header("枪口火焰光效")]
	[SerializeField] private Light muzzleFlashLight;

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

	#region 枪械弹药数据

	[Header("换弹所需时间")]
	[SerializeField] private float reloadTime;

	[Header("备弹数")]
	public int allBulletCount;
	[Header("弹匣里还剩多少子弹")]
	public int currentBulletCountInMagazine;
	[Header("弹匣容弹量")]
	[SerializeField] private int magazineCapacity;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnEnable() // 该脚本激活时表示玩家拿出了手枪
	{
		shootTotalTime = shootTotalTime + shootCDTime;
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 玩家射击 GamePlay 功能

	/// <summary>
	/// 计算玩家弹道散射
	/// </summary>
	public void ComputeSpreadValue()
	{
		if (playerController.playerIsMove)
		{
			computeMoveSpreadValue = moveSpreadValue;
		}
		else
		{
			computeMoveSpreadValue = Vector3.zero;
		}

		if (playerController.isAiming)
		{
			computeNotAimSpreadValue = Vector3.zero;
		}
		else
		{
			computeNotAimSpreadValue = notAimSpreadValue;
		}

		usedSpreadValue = new Vector3
		{
			x = Random.Range(-computeMoveSpreadValue.x, computeMoveSpreadValue.x) + Random.Range(-computeNotAimSpreadValue.x, computeNotAimSpreadValue.x),
			y = Random.Range(-computeMoveSpreadValue.y, computeMoveSpreadValue.y) + Random.Range(-computeNotAimSpreadValue.y, computeNotAimSpreadValue.y),
			z = Random.Range(-computeMoveSpreadValue.z, computeMoveSpreadValue.z) + Random.Range(-computeNotAimSpreadValue.z, computeNotAimSpreadValue.z),
		};
	}

	// ============================== 瞄准和射击 ============================== //

	/// <summary>
	/// 玩家举枪瞄准
	/// </summary>
	public void PlayerPerformAim()
	{
		// 从暂停菜单返回时，有非常短暂的时间不能瞄准
		if (playerController.fixAimActionCurTime > 0)
		{
			playerController.isAiming = false;
			return;
		}

		if (playerController.weaponManager.playerGunState != PlayerGunState.Standby)
		{
			playerController.isAiming = false;
			return;
		}

		if (playerController.fpsBodyController.currentArmState == ArmState.TopWall)
		{
			playerController.isAiming = false;
			return;
		}

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Aim.phase == InputActionPhase.Performed)
		{
			playerController.isAiming = true;
		}

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Aim.phase == InputActionPhase.Waiting)
		{
			playerController.isAiming = false;
		}

		// Debug.Log(playerController.isAiming);
	}

	/// <summary>
	/// 玩家手枪射击
	/// </summary>
	public void PlayerPerformShoot()
	{
		shootTotalTime = shootTotalTime + Time.deltaTime; // 累加射击 CD 时间
		if (shootTotalTime > 32768f) shootTotalTime = 32768f;

		if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;
		if (playerController.fpsBodyController.currentArmState == ArmState.TopWall) return;

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Shoot.WasPressedThisFrame() &&
			shootTotalTime > shootCDTime)
		{
			PlayerPistolShoot();
		}
	}

	private void PlayerPistolShoot()
	{
		if (currentBulletCountInMagazine <= 0)
		{
			// Debug.Log("弹匣里没有子弹了，按 R 键换弹");

			if (allBulletCount <= 0)
			{
				playerPistolSound.PlayEmptyTriggerSound(); // 播放撞针音效
			}

			return;
		}

		autoReloadTotalTime = 0f; // 重置自动换弹的时间，防止同一帧内既执行开枪又执行换弹

		playerPistolSound.PlayPistolShootSound(); // 播放射击音效

		playerController.weaponManager.playerGunState = PlayerGunState.Shooting;
		shootTotalTime = 0f;

		playerController.playerCameraController.SetCameraRecoilForce(recoilForce);
		playerCameraAnim.PistolShootVibrateAnim();
		playerPistolAnim.PlayShootAnim();

		StartCoroutine(PlayMuzzleFlashVFX());
		ThrowBulletShell(bulletShellSize);
		ReduceCurrentBulletCount();

		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex) | (1 << playerController.layerAndTagCollection_Player.doorInteractiveLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		if (Physics.Raycast(playerController.playerCameraController.playerCamera.transform.position, playerController.playerCameraController.playerCamera.transform.forward + usedSpreadValue, out hitObj, Mathf.Infinity, ignoreLayerIndex))
		{
			// Debug.Log(hitObj.collider.name);

			// 击中普通僵尸敌人
			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.zombieHeadTag))
			{
				hitObj.collider.GetComponentInParent<ZombieView>()?.DirectLockPlayer(playerController.transform);
				hitObj.collider.GetComponentInParent<ZombieHealth>()?.TakeDamage(hitObj.collider.tag, gunDamage);
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitZombieHeadVFXMag, hitVFXSize.y * hitZombieHeadVFXMag, hitVFXSize.z * hitZombieHeadVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletEnemy, false);
			}
			else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.zombieBodyTag))
			{
				hitObj.collider.GetComponentInParent<ZombieView>()?.DirectLockPlayer(playerController.transform);
				hitObj.collider.GetComponentInParent<ZombieHealth>()?.TakeDamage(hitObj.collider.tag, gunDamage);
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitZombieBodyVFXMag, hitVFXSize.y * hitZombieBodyVFXMag, hitVFXSize.z * hitZombieBodyVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletEnemy, false);
			}
			else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.zombieFourLimbsTag))
			{
				hitObj.collider.GetComponentInParent<ZombieView>()?.DirectLockPlayer(playerController.transform);
				hitObj.collider.GetComponentInParent<ZombieHealth>()?.TakeDamage(hitObj.collider.tag, gunDamage);
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitZombieFourLimbsVFXMag, hitVFXSize.y * hitZombieFourLimbsVFXMag, hitVFXSize.z * hitZombieFourLimbsVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletEnemy, false);
			}

			// 击中其他环境
			else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.envirDirt))
			{
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletHitDirt, true);
			}
			else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.envirWood))
			{
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletHitWood, true);
			}
			else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.envirGlass))
			{
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletHitGlass, true);
			}
			else if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.envirPhysicsObj))
			{
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletDefaultEnvir, false);

				Rigidbody body = hitObj.collider.GetComponent<Rigidbody>();

				if (body != null)
				{
					Vector3 hitDir = (body.transform.position - transform.position).normalized;
					body.AddForce(hitDir * hitPhysicsObjForce, ForceMode.Impulse);
				}
			}
			// 最后一种情况：击中默认环境物体
			else
			{
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletDefaultEnvir, true);
			}
		}

		playerController.weaponManager.playerGunState = PlayerGunState.Standby;
	}

	// ============================== 子弹数量计算 ============================== //

	private void ReduceCurrentBulletCount()
	{
		currentBulletCountInMagazine--;

		if (currentBulletCountInMagazine <= 0) // 防止出现子弹数为负的情况
		{
			currentBulletCountInMagazine = 0;
		}

		playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.SecondaryWeapon, currentBulletCountInMagazine, allBulletCount);
	}

	/// <summary>
	/// 弹匣空的时候松开鼠标左键自动换弹
	/// </summary>
	public void PlayerPerformReloadWhenEmpty()
	{
		autoReloadTotalTime += Time.deltaTime;
		if (autoReloadTotalTime > 32768f) autoReloadTotalTime = 32768f;

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Shoot.WasPressedThisFrame() &&
			autoReloadTotalTime > autoReloadCDTime &&
			currentBulletCountInMagazine <= 0 &&
			allBulletCount > 0) // 后备弹药空了就不能换弹了
		{
			// 判断当前武器状态
			if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;

			autoReloadTotalTime = 0f;
			StartCoroutine(PlayerReload());
		}
	}

	/// <summary>
	/// 玩家按下换弹键主动换弹
	/// </summary>
	public void PlayerPerformReload()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Reload.WasPressedThisFrame() &&
			currentBulletCountInMagazine < magazineCapacity &&
			allBulletCount > 0 // 后备弹药空了就不能换弹了
			)
		{
			// 判断当前武器状态
			if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;
			StartCoroutine(PlayerReload());
		}
	}

	private IEnumerator PlayerReload()
	{
		playerController.weaponManager.playerGunState = PlayerGunState.Reloading;

		playerPistolAnim.PlayReloadAnim();
		playerPistolSound.PlayPistolReloadSound(); // 播放动画和音效

		yield return new WaitForSeconds(reloadTime);

		// 计算装弹需要的弹药数（弹匣容量减去当前子弹数）
		int needBulletCount = magazineCapacity - currentBulletCountInMagazine;

		if (needBulletCount < allBulletCount)
		{
			// 后备弹药数减去弹匣缺少的弹药数
			allBulletCount = allBulletCount - needBulletCount;
		}
		else
		{
			// 如果后备弹药装不满一个弹匣，就把所有后备弹药计入装弹需要的弹药数
			needBulletCount = allBulletCount;
			allBulletCount = 0;
		}

		// 执行装弹
		currentBulletCountInMagazine = currentBulletCountInMagazine + needBulletCount;

		// 更新并显示子弹数 UI
		playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.SecondaryWeapon, currentBulletCountInMagazine, allBulletCount);

		playerController.weaponManager.playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region 开枪时会发生的事情

	/// <summary>
	/// 开枪时抛出弹壳
	/// </summary>
	private void ThrowBulletShell(Vector3 bulletShellSize)
	{
		float throwForce = UnityEngine.Random.Range(throwBulletShellForceMin, throwBulletShellForceMax);
		float rotateAnlge = UnityEngine.Random.Range(-bulletShellRotateAngle, bulletShellRotateAngle);

		GameObject bulletShellObj = Instantiate(bulletShell, null);

		bulletShellObj.transform.SetParent(playerController.fpsBodyController.fpsArmRoot.transform);
		bulletShellObj.transform.localEulerAngles = new Vector3(rotateAnlge, rotateAnlge, rotateAnlge);
		bulletShellObj.transform.SetParent(null);

		bulletShellObj.transform.localScale = bulletShellSize;
		bulletShellObj.transform.position = bulletShellStartTrans.position;

		Vector3 throwDir = (bulletShellDirTrans.position - bulletShellStartTrans.position).normalized;
		bulletShellObj.GetComponent<Rigidbody>().AddForce(throwDir * throwForce, ForceMode.Impulse);
	}

	/// <summary>
	/// 播放枪口火焰特效和光效
	/// </summary>
	/// <returns></returns>
	private IEnumerator PlayMuzzleFlashVFX()
	{
		for (int i = 0; i < pistolMuzzleFlash.Count; i++)
		{
			pistolMuzzleFlash[i].Emit(1);
			pistolMuzzleFlash[i].Play();
		}

		muzzleFlashLight.enabled = true;
		yield return null;
		muzzleFlashLight.enabled = false;
	}

	/// <summary>
	/// 武器击中目标时播放特效
	/// </summary>
	/// <param name="_pos"></param>
	/// <param name="_normal"></param>
	/// <param name="_vfxSize"></param>
	/// <param name="_hitState"></param>
	/// <returns></returns>
	private void PlayPistolHitVFX(Transform _hitObjTrans, float _playerRotateY, Vector3 _pos, Vector3 _normal, Vector3 _vfxSize, HitState _hitState, bool _makeHitDecal)
	{
		GameObject hitVFXObj = Instantiate(hitVFX, null);

		hitVFXObj.transform.localScale = _vfxSize;
		hitVFXObj.transform.SetLocalPositionAndRotation(_pos, Quaternion.LookRotation(_normal));

		hitVFXObj.GetComponent<HitEffectController>().PlayHitVFX(_hitState, _hitObjTrans, _playerRotateY, _vfxSize.x, _makeHitDecal);
	}

	#endregion

	#region 直接补给弹药
	
	/// <summary>
	/// 补充手枪弹药并更新 UI
	/// </summary>
	/// <param name="supplies"></param>
	public void SupplementBullet(int supplies)
	{
		allBulletCount += supplies;

		if (enabled)
		{
			playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.SecondaryWeapon, currentBulletCountInMagazine, allBulletCount);
		}
	}

	#endregion

}
