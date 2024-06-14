using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ö�٣���ʾ����������ŵ�ǹе������״̬
/// </summary>
public enum PlayerGunState
{
	Getting,
	Standby,
	Reloading,
	Shooting,
}

/// <summary>
/// ö�٣���ʾ����������ŵ�����������
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
/// ��������������������������������״̬���л�
/// </summary>
public class WeaponManager : MonoBehaviour
{
	#region ��������ͱ���

	// ע�⣺WeaponTypeInHand ʵ���ϼ�¼������л�����֮ǰ���ŵ�����

	private FPS_Play_Action fpsPlayAction; // �������ϵͳ

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("������������Ч����")]
	[SerializeField] private WeaponManagerSound weaponManagerSound;

	[Header("����״̬�������������Ҳ����Զ������")]
	public PlayerGunState playerGunState = PlayerGunState.Standby;
	public WeaponTypeInHand weaponTypeInHand = WeaponTypeInHand.None;

	private WeaponTypeInHand lastWeaponTypeInHand = WeaponTypeInHand.None; // ����ڰ���Ʒ����������֮ǰ�������õ���ʲô����

	[Header("���������Ŀ��ƽű�")]
	[Header("��ǹ")]
	public PlayerPistolShooting playerPistolShooting;
	[Header("С��")]
	public PlayerKinfeAttacking playerKinfeAttacking;
	[Header("ҽ�ư�")]
	public PlayerMedicineUsing playerSyringeUsing;

	// Э��
	private Coroutine getDownCurrentWeapon_IECor;
	private Coroutine playerSwitchToSecondaryWeapon_IECor;
	private Coroutine playerSwitchToMeleeWeapon_IECor;
	private Coroutine playerSwitchToMedicine_IECor;

	// TODO δ���ĸ���: ��������

	#endregion

	#region �����������ں���

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

	#region ��������л�����

	#region ��Ҳ���ʱ��ʱ���º��������е�����

	/// <summary>
	/// ��ҷ����������ŵ�����
	/// </summary>
	public void GetDownCurrentWeapon()
	{
		if (weaponTypeInHand == WeaponTypeInHand.None) return;

		// ��ֹͣ�����������л��߼�
		if (playerSwitchToSecondaryWeapon_IECor != null) StopCoroutine(playerSwitchToSecondaryWeapon_IECor);
		if (playerSwitchToMeleeWeapon_IECor != null) StopCoroutine(playerSwitchToMeleeWeapon_IECor);
		if (playerSwitchToMedicine_IECor != null) StopCoroutine(playerSwitchToMedicine_IECor);

		getDownCurrentWeapon_IECor = StartCoroutine(GetDownCurrentWeapon_IE());
	}

	private IEnumerator GetDownCurrentWeapon_IE()
	{
		lastWeaponTypeInHand = weaponTypeInHand; // ��¼�ϴ����ŵ�����
		playerGunState = PlayerGunState.Getting;

		// ���������ֳ�����
		float hideTime = PlayCurrentWeaponHideAnimAndGetHideTime();
		yield return new WaitForSeconds(hideTime);

		HideCurrentWeapon();

		weaponTypeInHand = WeaponTypeInHand.None;
		playerGunState = PlayerGunState.Standby;
	}

	/// <summary>
	/// ��������������е�����
	/// </summary>
	public void PlayerRestartWeapon()
	{
		if (lastWeaponTypeInHand == WeaponTypeInHand.None)
		{
			return;
		}
		if (lastWeaponTypeInHand == WeaponTypeInHand.PrimaryWeapon)
		{
			// TODO: �����Ժ����
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

	#region ����л�����ǹ

	public void PlayerPerformSwitchToSecondaryWeapon()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_SwitchToSecondaryWeapon.WasPressedThisFrame())
		{
			if (weaponTypeInHand == WeaponTypeInHand.SecondaryWeapon) return; // �ж��ֳ�����������
			if (playerGunState != PlayerGunState.Standby) return; // �жϵ�ǰ������״̬
			if (playerController.isAiming) return; // �ж���׼״̬

			playerController.eventHandler_Player.InvokeChangeEquipWeapon(true, false, WeaponTypeInHand.SecondaryWeapon, playerPistolShooting.currentBulletCountInMagazine, playerPistolShooting.allBulletCount);
			playerSwitchToSecondaryWeapon_IECor = StartCoroutine(PlayerSwitchToSecondaryWeapon());
		}
	}

	private IEnumerator PlayerSwitchToSecondaryWeapon()
	{
		playerGunState = PlayerGunState.Getting;

		// ���������ֳ�����
		float hideTime = PlayCurrentWeaponHideAnimAndGetHideTime();
		yield return new WaitForSeconds(hideTime);

		HideCurrentWeapon();

		weaponManagerSound.PlayGetPistolSound(); // ������Ч

		playerPistolShooting.enabled = true;
		playerPistolShooting.playerPistolAnim.enabled = true;
		playerPistolShooting.pistolArmModel.SetActive(true);

		yield return new WaitForSeconds(playerPistolShooting.weaponGetTime);

		playerController.tutorialTrigger.TutorialPlayerFirstUseGun(); // ��ʾ����ʹ�ý̳�

		weaponTypeInHand = WeaponTypeInHand.SecondaryWeapon;
		playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region �л���С��

	public void PlayerPerformSwitchToMeleeWeapon()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_SwitchToMeleeWeapon.WasPressedThisFrame())
		{
			if (weaponTypeInHand == WeaponTypeInHand.MeleeWeapon) return; // �ж��ֳ�����������
			if (playerGunState != PlayerGunState.Standby) return; // �жϵ�ǰ������״̬
			if (playerController.isAiming) return; // �ж���׼״̬

			playerController.eventHandler_Player.InvokeChangeEquipWeapon(true, false, WeaponTypeInHand.MeleeWeapon, 0, 0);
			playerSwitchToMeleeWeapon_IECor = StartCoroutine(PlayerSwitchToMeleeWeapon());
		}
	}

	private IEnumerator PlayerSwitchToMeleeWeapon()
	{
		playerGunState = PlayerGunState.Getting;

		// ���������ֳ�����
		float hideTime = PlayCurrentWeaponHideAnimAndGetHideTime();
		yield return new WaitForSeconds(hideTime);

		HideCurrentWeapon();

		weaponManagerSound.PlayGetKinfeSound(); // ������Ч

		playerKinfeAttacking.enabled = true;
		playerKinfeAttacking.playerKinfeAnim.enabled = true;
		playerKinfeAttacking.kinfeArmModel.SetActive(true);

		yield return new WaitForSeconds(playerKinfeAttacking.weaponGetTime);

		weaponTypeInHand = WeaponTypeInHand.MeleeWeapon;
		playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region �л���ҽ�ư�

	public void PlayerPerformSwitchToMedicine()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_SwitchToMedicine.WasPressedThisFrame())
		{
			if (weaponTypeInHand == WeaponTypeInHand.Medicine) return; // �ж��ֳ�����������
			if (playerGunState != PlayerGunState.Standby) return; // �жϵ�ǰ������״̬
			if (playerController.isAiming) return; // �ж���׼״̬

			if (playerSyringeUsing.medicineCount <= 0) return;

			playerController.eventHandler_Player.InvokeChangeEquipWeapon(true, false, WeaponTypeInHand.Medicine, playerSyringeUsing.medicineCount, 0);
			playerSwitchToMedicine_IECor = StartCoroutine(PlayerSwitchToMedicine());
		}
	}

	private IEnumerator PlayerSwitchToMedicine()
	{
		playerGunState = PlayerGunState.Getting;

		// ���������ֳ�����
		float hideTime = PlayCurrentWeaponHideAnimAndGetHideTime();
		yield return new WaitForSeconds(hideTime);

		HideCurrentWeapon();

		weaponManagerSound.PlayGetSyringeSound(); // ������Ч

		playerSyringeUsing.enabled = true;
		playerSyringeUsing.playerSyringeAnim.enabled = true;
		playerSyringeUsing.syringeArmModel.SetActive(true);

		yield return new WaitForSeconds(playerSyringeUsing.weaponGetTime);

		weaponTypeInHand = WeaponTypeInHand.Medicine;
		playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region ����л�����ʱ�Զ�����ģ�ͽ��д���

	/// <summary>
	/// ��������ֳ��������Ͳ��Ŷ�Ӧ���ض�������ȡ���������õ�ʱ��
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

		Debug.Log("�ֳ�����״̬���쳣����������߼�");
		return 0f;
	}

	/// <summary>
	/// ������������
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
