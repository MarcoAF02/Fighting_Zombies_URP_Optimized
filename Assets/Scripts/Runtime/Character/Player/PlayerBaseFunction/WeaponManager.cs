using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 枚举：表示玩家手上拿着的枪械所处的状态
/// </summary>
public enum PlayerGunState
{
	Getting,
	Standby,
	Reloading,
	Shooting,
}

/// <summary>
/// 枚举：表示玩家手上拿着的武器的类型
/// </summary>
public enum WeaponTypeInHand
{
	None,
	PrimaryWeapon,
	SecondaryWeapon,
	MeleeWeapon,
	Medicine,
}

/// <summary>
/// 玩家武器管理器，管理所有玩家武器的状态和切换
/// </summary>
public class WeaponManager : MonoBehaviour
{
	#region 基本组件和变量

	// 注意：WeaponTypeInHand 实际上记录了玩家切换武器之前拿着的武器

	private FPS_Play_Action fpsPlayAction; // 玩家输入系统

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("武器管理器音效控制")]
	[SerializeField] private WeaponManagerSound weaponManagerSound;

	[Header("各个状态的情况（根据玩家操作自动变更）")]
	public PlayerGunState playerGunState = PlayerGunState.Standby;
	public WeaponTypeInHand weaponTypeInHand = WeaponTypeInHand.None;

	private WeaponTypeInHand lastWeaponTypeInHand = WeaponTypeInHand.None; // 玩家在把物品拿起来端详之前，手中拿的是什么武器

	[Header("各个武器的控制脚本")]
	[Header("手枪")]
	public PlayerPistolShooting playerPistolShooting;
	[Header("小刀")]
	public PlayerKinfeAttacking playerKinfeAttacking;
	[Header("医疗包")]
	public PlayerMedicineUsing playerSyringeUsing;

	// 协程
	private Coroutine getDownCurrentWeapon_IECor;
	private Coroutine playerSwitchToSecondaryWeapon_IECor;
	private Coroutine playerSwitchToMeleeWeapon_IECor;
	private Coroutine playerSwitchToMedicine_IECor;

	// TODO 未来的更新: 其他武器

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 玩家武器切换功能

	#region 玩家操作时暂时放下和拿起手中的武器

	/// <summary>
	/// 玩家放下手中拿着的武器
	/// </summary>
	public void GetDownCurrentWeapon()
	{
		if (weaponTypeInHand == WeaponTypeInHand.None) return;

		// 先停止其他的武器切换逻辑
		if (playerSwitchToSecondaryWeapon_IECor != null) StopCoroutine(playerSwitchToSecondaryWeapon_IECor);
		if (playerSwitchToMeleeWeapon_IECor != null) StopCoroutine(playerSwitchToMeleeWeapon_IECor);
		if (playerSwitchToMedicine_IECor != null) StopCoroutine(playerSwitchToMedicine_IECor);

		getDownCurrentWeapon_IECor = StartCoroutine(GetDownCurrentWeapon_IE());
	}

	private IEnumerator GetDownCurrentWeapon_IE()
	{
		lastWeaponTypeInHand = weaponTypeInHand; // 记录上次拿着的武器
		playerGunState = PlayerGunState.Getting;

		// 隐藏现有手持武器
		float hideTime = PlayCurrentWeaponHideAnimAndGetHideTime();
		yield return new WaitForSeconds(hideTime);

		HideCurrentWeapon();

		weaponTypeInHand = WeaponTypeInHand.None;
		playerGunState = PlayerGunState.Standby;
	}

	/// <summary>
	/// 玩家重新拿起手中的武器
	/// </summary>
	public void PlayerRestartWeapon()
	{
		if (lastWeaponTypeInHand == WeaponTypeInHand.None)
		{
			return;
		}
		if (lastWeaponTypeInHand == WeaponTypeInHand.PrimaryWeapon)
		{
			// TODO: 这是以后的事
		}
		if (lastWeaponTypeInHand == WeaponTypeInHand.SecondaryWeapon)
		{
			playerSwitchToSecondaryWeapon_IECor = StartCoroutine(PlayerSwitchToSecondaryWeapon());
		}
		if (lastWeaponTypeInHand == WeaponTypeInHand.MeleeWeapon)
		{
			playerSwitchToMeleeWeapon_IECor = StartCoroutine(PlayerSwitchToMeleeWeapon());
		}
		if (lastWeaponTypeInHand == WeaponTypeInHand.Medicine)
		{
			playerSwitchToMedicine_IECor = StartCoroutine(PlayerSwitchToMedicine());
		}
	}

	#endregion

	#region 玩家切换至手枪

	public void PlayerPerformSwitchToSecondaryWeapon()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_SwitchToSecondaryWeapon.WasPressedThisFrame())
		{
			if (weaponTypeInHand == WeaponTypeInHand.SecondaryWeapon) return; // 判断手持武器的类型
			if (playerGunState != PlayerGunState.Standby) return; // 判断当前武器的状态
			if (playerController.isAiming) return; // 判断瞄准状态

			playerController.eventHandler_Player.InvokeChangeEquipWeapon(true, false, WeaponTypeInHand.SecondaryWeapon, playerPistolShooting.currentBulletCountInMagazine, playerPistolShooting.allBulletCount);
			playerSwitchToSecondaryWeapon_IECor = StartCoroutine(PlayerSwitchToSecondaryWeapon());
		}
	}

	private IEnumerator PlayerSwitchToSecondaryWeapon()
	{
		playerGunState = PlayerGunState.Getting;

		// 隐藏现有手持武器
		float hideTime = PlayCurrentWeaponHideAnimAndGetHideTime();
		yield return new WaitForSeconds(hideTime);

		HideCurrentWeapon();

		weaponManagerSound.PlayGetPistolSound(); // 播放音效

		playerPistolShooting.enabled = true;
		playerPistolShooting.playerPistolAnim.enabled = true;
		playerPistolShooting.pistolArmModel.SetActive(true);

		yield return new WaitForSeconds(playerPistolShooting.weaponGetTime);

		playerController.tutorialTrigger.TutorialPlayerFirstUseGun(); // 显示武器使用教程

		weaponTypeInHand = WeaponTypeInHand.SecondaryWeapon;
		playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region 切换至小刀

	public void PlayerPerformSwitchToMeleeWeapon()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_SwitchToMeleeWeapon.WasPressedThisFrame())
		{
			if (weaponTypeInHand == WeaponTypeInHand.MeleeWeapon) return; // 判断手持武器的类型
			if (playerGunState != PlayerGunState.Standby) return; // 判断当前武器的状态
			if (playerController.isAiming) return; // 判断瞄准状态

			playerController.eventHandler_Player.InvokeChangeEquipWeapon(true, false, WeaponTypeInHand.MeleeWeapon, 0, 0);
			playerSwitchToMeleeWeapon_IECor = StartCoroutine(PlayerSwitchToMeleeWeapon());
		}
	}

	private IEnumerator PlayerSwitchToMeleeWeapon()
	{
		playerGunState = PlayerGunState.Getting;

		// 隐藏现有手持武器
		float hideTime = PlayCurrentWeaponHideAnimAndGetHideTime();
		yield return new WaitForSeconds(hideTime);

		HideCurrentWeapon();

		weaponManagerSound.PlayGetKinfeSound(); // 播放音效

		playerKinfeAttacking.enabled = true;
		playerKinfeAttacking.playerKinfeAnim.enabled = true;
		playerKinfeAttacking.kinfeArmModel.SetActive(true);

		yield return new WaitForSeconds(playerKinfeAttacking.weaponGetTime);

		weaponTypeInHand = WeaponTypeInHand.MeleeWeapon;
		playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region 切换至医疗包

	public void PlayerPerformSwitchToMedicine()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_SwitchToMedicine.WasPressedThisFrame())
		{
			if (weaponTypeInHand == WeaponTypeInHand.Medicine) return; // 判断手持武器的类型
			if (playerGunState != PlayerGunState.Standby) return; // 判断当前武器的状态
			if (playerController.isAiming) return; // 判断瞄准状态

			if (playerSyringeUsing.medicineCount <= 0) return;

			playerController.eventHandler_Player.InvokeChangeEquipWeapon(true, false, WeaponTypeInHand.Medicine, playerSyringeUsing.medicineCount, 0);
			playerSwitchToMedicine_IECor = StartCoroutine(PlayerSwitchToMedicine());
		}
	}

	private IEnumerator PlayerSwitchToMedicine()
	{
		playerGunState = PlayerGunState.Getting;

		// 隐藏现有手持武器
		float hideTime = PlayCurrentWeaponHideAnimAndGetHideTime();
		yield return new WaitForSeconds(hideTime);

		HideCurrentWeapon();

		weaponManagerSound.PlayGetSyringeSound(); // 播放音效

		playerSyringeUsing.enabled = true;
		playerSyringeUsing.playerSyringeAnim.enabled = true;
		playerSyringeUsing.syringeArmModel.SetActive(true);

		yield return new WaitForSeconds(playerSyringeUsing.weaponGetTime);

		weaponTypeInHand = WeaponTypeInHand.Medicine;
		playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region 玩家切换道具时对动画和模型进行处理

	/// <summary>
	/// 根据玩家手持武器类型播放对应隐藏动画并获取隐藏武器用的时间
	/// </summary>
	private float PlayCurrentWeaponHideAnimAndGetHideTime()
	{
		if (weaponTypeInHand == WeaponTypeInHand.None)
		{
			return 0f;
		}
		if (weaponTypeInHand == WeaponTypeInHand.SecondaryWeapon)
		{
			playerPistolShooting.playerPistolAnim.PlayHideAnim();
			return playerPistolShooting.weaponHideTime;
		}
		if (weaponTypeInHand == WeaponTypeInHand.MeleeWeapon)
		{
			playerKinfeAttacking.playerKinfeAnim.PlayHideAnim();
			return playerKinfeAttacking.weaponHideTime;
		}
		if (weaponTypeInHand == WeaponTypeInHand.Medicine)
		{
			playerSyringeUsing.playerSyringeAnim.PlayHideAnim();
			return playerSyringeUsing.weaponHideTime;
		}

		Debug.Log("手持武器状态有异常，请检查代码逻辑");
		return 0f;
	}

	/// <summary>
	/// 藏起现有武器
	/// </summary>
	private void HideCurrentWeapon()
	{
		if (weaponTypeInHand == WeaponTypeInHand.SecondaryWeapon)
		{
			playerPistolShooting.playerPistolAnim.enabled = false;
			playerPistolShooting.pistolArmModel.SetActive(false);
			playerPistolShooting.enabled = false;
		}
		if (weaponTypeInHand == WeaponTypeInHand.MeleeWeapon)
		{
			playerKinfeAttacking.playerKinfeAnim.enabled = false;
			playerKinfeAttacking.kinfeArmModel.SetActive(false);
			playerKinfeAttacking.enabled = false;
		}
		if (weaponTypeInHand == WeaponTypeInHand.Medicine)
		{
			playerSyringeUsing.playerSyringeAnim.enabled = false;
			playerSyringeUsing.syringeArmModel.SetActive(false);
			playerSyringeUsing.enabled = false;
		}
	}

	#endregion

	#endregion
}
