using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ҽ�ư�ʹ�ýű�
/// </summary>
public class PlayerMedicineUsing : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("�������ֵ������")]
	[SerializeField] private PlayerHealth playerHealth;
	[Header("ҽ�ư�����������")]
	public PlayerMedicineAnim playerSyringeAnim;
	[Header("ҽ�ư�ģ�ͽ�ɫ����")]
	public GameObject syringeArmModel;

	[Header("ҽ��ע������Ч����")]
	[SerializeField] private PlayerSyringeSound playerSyringeSound;

	#endregion

	#region ҽ�ư���������

	[Header("��ǰ����ж��ٸ�ҽ�ư�")]
	public int medicineCount;
	[Header("ÿ������Ļָ�ֵ")]
	[SerializeField] private float restoreValue;
	[Header("����ʱ�����೤ʱ��ʵ��ִ�л�Ѫ")]
	[SerializeField] private float restoreDelayTime;
	[Header("ҽ�ư�ʹ�� CD ʱ��")]
	[SerializeField] private float useCDTime;
	private float useTotalTime;
	[Header("����ȡ������Ҫ��ʱ��")]
	public float weaponGetTime;
	[Header("������������Ҫ��ʱ��")]
	public float weaponHideTime;

	#endregion

	#region �����������ں���

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

	#region ��һ�Ѫ GamePlay ����

	/// <summary>
	/// ��һ�Ѫ GamePlay ����
	/// </summary>
	public void PlayerPerformRestore()
	{
		useTotalTime = useTotalTime + Time.deltaTime;

		if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;
		if (medicineCount <= 0) return; // û��ҩ����ʱ������ʹ��ҩ��
		if (playerHealth.currentHealth >= playerHealth.maxHealth) return; // ��Ѫʱ���ָܻ�����ֵ

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Shoot.WasPressedThisFrame() &&
			useTotalTime > useCDTime)
		{
			StartCoroutine(PlayerRestore());
		}
	}

	private IEnumerator PlayerRestore()
	{
		playerController.weaponManager.playerGunState = PlayerGunState.Shooting;

		playerSyringeAnim.PlayUseAnim(); // ���Ż�Ѫ����
		yield return new WaitForSeconds(restoreDelayTime);
		medicineCount--; // ����ҩ������
		useTotalTime = 0f;

		if (medicineCount >= 1)
		{
			playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.Medicine, medicineCount, 0);
		}

		playerHealth.RecoverHealth(restoreValue);
		playerSyringeSound.PlaySyringeUseSound(); // ����ҽ��ע����ʹ����Ч
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

	#region ֱ�Ӳ���ҽ�ư�����

	/// <summary>
	/// ����ҽ�ư������������� UI
	/// </summary>
	/// <param name="supplies"></param>
	public void SupplementMedicine(int supplies)
	{
		if (medicineCount <= 0) // ���֮ǰ��ҽ�ư�������Ϊ 0 ����ʾ������¼�����ҽ�ư�
		{
			playerController.eventHandler_Player.InvokeGetNewItemOnEquipUI(WeaponTypeInHand.Medicine); // ��һ����ҽ�ư�
		}

		medicineCount += supplies;

		if (enabled) // ����ű�����������ˣ��ͱ�ʾ����������������װ��
		{
			playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.Medicine, medicineCount, 0);
		}
	}

	#endregion

}
