using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �����ǹ����ű�
/// </summary>
public class PlayerPistolShooting : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("�������������")]
	[SerializeField] private PlayerCameraAnim playerCameraAnim;
	[Header("��ǹ����������")]
	public PlayerPistolAnim playerPistolAnim;
	[Header("��ǹ��ɫģ������")]
	public GameObject pistolArmModel;

	[Header("��ǹ��Ч������")]
	[SerializeField] private PlayerPistolSound playerPistolSound;

	[Header("��������")]
	[SerializeField] private GameObject bulletShell;
	[Header("������Ч")]
	[SerializeField] private GameObject hitVFX;

	#endregion

	#region ��ǹ��������

	[Header("��ǹ�����˺�ֵ")]
	[SerializeField] private float gunDamage;

	[Header("ǹ�ں�������ͷ��������")]
	[SerializeField] private float recoilForce;

	[Header("����ɢ�����")]
	[Header("�ƶ�ʱ��ɢ�����")]
	[SerializeField] private Vector3 moveSpreadValue;
	[Header("����׼״̬�µ�ɢ�����")]
	[SerializeField] private Vector3 notAimSpreadValue;

	private Vector3 computeMoveSpreadValue;
	private Vector3 computeNotAimSpreadValue;
	private Vector3 usedSpreadValue; // ���ռ���õ���ɢ�����

	[Header("��� CD ʱ��")]
	[SerializeField] private float shootCDTime;
	private float shootTotalTime;

	[Header("��ϻ��պ��Զ����� CD ʱ��")]
	[SerializeField] private float autoReloadCDTime;
	private float autoReloadTotalTime;

	[Header("����ȡ������Ҫ��ʱ��")]
	public float weaponGetTime;
	[Header("������������Ҫ��ʱ��")]
	public float weaponHideTime;

	[Header("��ǹ������������ǰ�Ƶ�����")]
	[SerializeField] private float hitPhysicsObjForce;

	[Header("�ӵ��ǵĴ�С")]
	[SerializeField] private Vector3 bulletShellSize;
	[Header("�ӵ��׿���С����")]
	[SerializeField] private float throwBulletShellForceMin;
	[Header("�ӵ��׿��������")]
	[SerializeField] private float throwBulletShellForceMax;
	[Header("�ӵ��׿ǵ���ת��")]
	[SerializeField] private float bulletShellRotateAngle;
	[Header("�ӵ��׿���ʼ��")]
	[SerializeField] private Transform bulletShellStartTrans;
	[Header("�ӵ��׿Ƿ����")]
	[SerializeField] private Transform bulletShellDirTrans;

	[Header("ǹ�ڻ�����Ч")]
	[SerializeField] private List<ParticleSystem> pistolMuzzleFlash;

	[Header("ǹ�ڻ����Ч")]
	[SerializeField] private Light muzzleFlashLight;

	[Header("������Ч�Ĵ�С")]
	[SerializeField] private Vector3 hitVFXSize;

	[Header("������Ч���в�ͬ�����ķŴ���")]
	[Header("���л�������")]
	[Range(0, 2)]
	[SerializeField] private float hitEnvirObjVFXMag;
	[Header("���н�ʬͷ��")]
	[Range(0, 2)]
	[SerializeField] private float hitZombieHeadVFXMag;
	[Header("���н�ʬ����")]
	[Range(0, 2)]
	[SerializeField] private float hitZombieBodyVFXMag;
	[Header("���н�ʬ��֫")]
	[Range(0, 2)]
	[SerializeField] private float hitZombieFourLimbsVFXMag;

	#endregion

	#region ǹе��ҩ����

	[Header("��������ʱ��")]
	[SerializeField] private float reloadTime;

	[Header("������")]
	public int allBulletCount;
	[Header("��ϻ�ﻹʣ�����ӵ�")]
	public int currentBulletCountInMagazine;
	[Header("��ϻ�ݵ���")]
	[SerializeField] private int magazineCapacity;

	#endregion

	#region �����������ں���

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnEnable() // �ýű�����ʱ��ʾ����ó�����ǹ
	{
		shootTotalTime = shootTotalTime + shootCDTime;
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region ������ GamePlay ����

	/// <summary>
	/// ������ҵ���ɢ��
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

	// ============================== ��׼����� ============================== //

	/// <summary>
	/// ��Ҿ�ǹ��׼
	/// </summary>
	public void PlayerPerformAim()
	{
		// ����ͣ�˵�����ʱ���зǳ����ݵ�ʱ�䲻����׼
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
	/// �����ǹ���
	/// </summary>
	public void PlayerPerformShoot()
	{
		shootTotalTime = shootTotalTime + Time.deltaTime; // �ۼ���� CD ʱ��
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
			// Debug.Log("��ϻ��û���ӵ��ˣ��� R ������");

			if (allBulletCount <= 0)
			{
				playerPistolSound.PlayEmptyTriggerSound(); // ����ײ����Ч
			}

			return;
		}

		autoReloadTotalTime = 0f; // �����Զ�������ʱ�䣬��ֹͬһ֡�ڼ�ִ�п�ǹ��ִ�л���

		playerPistolSound.PlayPistolShootSound(); // ���������Ч

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

			// ������ͨ��ʬ����
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

			// ������������
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
			// ���һ�����������Ĭ�ϻ�������
			else
			{
				Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
				PlayPistolHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.BulletDefaultEnvir, true);
			}
		}

		playerController.weaponManager.playerGunState = PlayerGunState.Standby;
	}

	// ============================== �ӵ��������� ============================== //

	private void ReduceCurrentBulletCount()
	{
		currentBulletCountInMagazine--;

		if (currentBulletCountInMagazine <= 0) // ��ֹ�����ӵ���Ϊ�������
		{
			currentBulletCountInMagazine = 0;
		}

		playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.SecondaryWeapon, currentBulletCountInMagazine, allBulletCount);
	}

	/// <summary>
	/// ��ϻ�յ�ʱ���ɿ��������Զ�����
	/// </summary>
	public void PlayerPerformReloadWhenEmpty()
	{
		autoReloadTotalTime += Time.deltaTime;
		if (autoReloadTotalTime > 32768f) autoReloadTotalTime = 32768f;

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Shoot.WasPressedThisFrame() &&
			autoReloadTotalTime > autoReloadCDTime &&
			currentBulletCountInMagazine <= 0 &&
			allBulletCount > 0) // �󱸵�ҩ���˾Ͳ��ܻ�����
		{
			// �жϵ�ǰ����״̬
			if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;

			autoReloadTotalTime = 0f;
			StartCoroutine(PlayerReload());
		}
	}

	/// <summary>
	/// ��Ұ��»�������������
	/// </summary>
	public void PlayerPerformReload()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Reload.WasPressedThisFrame() &&
			currentBulletCountInMagazine < magazineCapacity &&
			allBulletCount > 0 // �󱸵�ҩ���˾Ͳ��ܻ�����
			)
		{
			// �жϵ�ǰ����״̬
			if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;
			StartCoroutine(PlayerReload());
		}
	}

	private IEnumerator PlayerReload()
	{
		playerController.weaponManager.playerGunState = PlayerGunState.Reloading;

		playerPistolAnim.PlayReloadAnim();
		playerPistolSound.PlayPistolReloadSound(); // ���Ŷ�������Ч

		yield return new WaitForSeconds(reloadTime);

		// ����װ����Ҫ�ĵ�ҩ������ϻ������ȥ��ǰ�ӵ�����
		int needBulletCount = magazineCapacity - currentBulletCountInMagazine;

		if (needBulletCount < allBulletCount)
		{
			// �󱸵�ҩ����ȥ��ϻȱ�ٵĵ�ҩ��
			allBulletCount = allBulletCount - needBulletCount;
		}
		else
		{
			// ����󱸵�ҩװ����һ����ϻ���Ͱ����к󱸵�ҩ����װ����Ҫ�ĵ�ҩ��
			needBulletCount = allBulletCount;
			allBulletCount = 0;
		}

		// ִ��װ��
		currentBulletCountInMagazine = currentBulletCountInMagazine + needBulletCount;

		// ���²���ʾ�ӵ��� UI
		playerController.eventHandler_Player.InvokeChangeEquipWeapon(false, false, WeaponTypeInHand.SecondaryWeapon, currentBulletCountInMagazine, allBulletCount);

		playerController.weaponManager.playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region ��ǹʱ�ᷢ��������

	/// <summary>
	/// ��ǹʱ�׳�����
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
	/// ����ǹ�ڻ�����Ч�͹�Ч
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
	/// ��������Ŀ��ʱ������Ч
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

	#region ֱ�Ӳ�����ҩ
	
	/// <summary>
	/// ������ǹ��ҩ������ UI
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
