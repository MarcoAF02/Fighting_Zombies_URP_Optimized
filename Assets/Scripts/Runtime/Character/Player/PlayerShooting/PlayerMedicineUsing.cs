using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家医疗包使用脚本
/// </summary>
public class PlayerMedicineUsing : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("玩家生命值管理类")]
	[SerializeField] private PlayerHealth playerHealth;
	[Header("医疗包动画控制器")]
	public PlayerMedicineAnim playerSyringeAnim;
	[Header("医疗包模型角色物体")]
	public GameObject syringeArmModel;

	[Header("医疗注射针音效管理")]
	[SerializeField] private PlayerSyringeSound playerSyringeSound;

	#endregion

	#region 医疗包基本数据

	[Header("当前玩家有多少个医疗包")]
	public int medicineCount;
	[Header("每次扎针的恢复值")]
	[SerializeField] private float restoreValue;
	[Header("扎针时，过多长时间实际执行回血")]
	[SerializeField] private float restoreDelayTime;
	[Header("医疗包使用 CD 时间")]
	[SerializeField] private float useCDTime;
	private float useTotalTime;
	[Header("武器取出来需要的时间")]
	public float weaponGetTime;
	[Header("武器收起来需要的时间")]
	public float weaponHideTime;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnEnable()
	{
		useTotalTime = useTotalTime + useCDTime;
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 玩家回血 GamePlay 功能

	/// <summary>
	/// 玩家回血 GamePlay 功能
	/// </summary>
	public void PlayerPerformRestore()
	{
		useTotalTime = useTotalTime + Time.deltaTime;

		if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;
		if (medicineCount <= 0) return; // 没有药包的时候不能再使用药包
		if (playerHealth.currentHealth >= playerHealth.maxHealth) return; // 满血时不能恢复生命值

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Shoot.WasPressedThisFrame() &&
			useTotalTime > useCDTime)
		{
			StartCoroutine(PlayerRestore());
		}
	}

	private IEnumerator PlayerRestore()
	{
		playerController.weaponManager.playerGunState = PlayerGunState.Shooting;

		playerSyringeAnim.PlayUseAnim(); // 播放回血动画
		yield return new WaitForSeconds(restoreDelayTime);
		medicineCount--; // 减少药包数量
		useTotalTime = 0f;

		if (medicineCount >= 1)
		{
			playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.Medicine, medicineCount, 0);
		}

		playerHealth.RecoverHealth(restoreValue);
		playerSyringeSound.PlaySyringeUseSound(); // 播放医疗注射器使用音效
		playerController.weaponManager.playerGunState = PlayerGunState.Standby;

		if (medicineCount <= 0)
		{
			medicineCount = 0;

			playerController.eventHandler_Player.InvokeChangeEquipWeapon(true, true, WeaponTypeInHand.Medicine, medicineCount, 0);

			playerSyringeAnim.PlayHideAnim();

			yield return new WaitForSeconds(weaponHideTime * 2);

			playerController.weaponManager.weaponTypeInHand = WeaponTypeInHand.None;
			playerController.weaponManager.playerGunState = PlayerGunState.Standby;

			playerSyringeAnim.enabled = false;
			syringeArmModel.SetActive(false);
			enabled = false;
		}
	}

	#endregion

	#region 直接补给医疗包数量

	/// <summary>
	/// 补充医疗包的数量并更新 UI
	/// </summary>
	/// <param name="supplies"></param>
	public void SupplementMedicine(int supplies)
	{
		if (medicineCount <= 0) // 如果之前的医疗包持有量为 0 ，表示玩家重新捡起了医疗包
		{
			playerController.eventHandler_Player.InvokeGetNewItemOnEquipUI(WeaponTypeInHand.Medicine); // 这一格是医疗包
		}

		medicineCount += supplies;

		if (enabled) // 这个脚本如果被启用了，就表示玩家手上正拿着这个装备
		{
			playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.Medicine, medicineCount, 0);
		}
	}

	#endregion

}
