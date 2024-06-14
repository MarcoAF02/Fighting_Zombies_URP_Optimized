using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���С�������ű�
/// </summary>
public class PlayerKinfeAttacking : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("�������������")]
	[SerializeField] private PlayerCameraAnim playerCameraAnim;
	[Header("С������������")]
	public PlayerKinfeAnim playerKinfeAnim;
	[Header("С��ģ�ͽ�ɫ����")]
	public GameObject kinfeArmModel;

	[Header("С��������Ч����")]
	[SerializeField] private PlayerKinfeSound playerKinfeSound;

	[Header("������Ч")]
	[SerializeField] private GameObject hitVFX;

	#endregion

	#region С����������

	[Header("С�����������߷����")]
	[SerializeField] private List<Transform> attackRayStartTrans = new List<Transform>();

	[Header("С�������˺�ֵ")]
	[SerializeField] private float kinfeDamage;
	[Header("С��������Χ")]
	[SerializeField] private float kinfeAttackDistance;
	[Header("С������ CD ʱ��")]
	[SerializeField] private float attackCDTime;
	private float attackTotalTime;
	[Header("С�������ӳٴ���ʱ��")]
	[SerializeField] private float delayAttackTime;
	[Header("����ȡ������Ҫ��ʱ��")]
	public float weaponGetTime;
	[Header("������������Ҫ��ʱ��")]
	public float weaponHideTime;

	[Header("������������ʱʩ�ӵ�����")]
	[SerializeField] private float hitPhysicsObjForce;

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

	#region �����������ں���

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnEnable() // �Ľű�����ʱ��ʾ����ó���С��
	{
		attackTotalTime = attackTotalTime + attackCDTime;
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region С������ GamePlay ����

	/// <summary>
	/// С��ִ�й���
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

				// ������ͨ��ʬ����
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

				// ������������
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
				// ���һ�����������Ĭ�ϻ�������
				else
				{
					Vector3 vfxSize = new Vector3(hitVFXSize.x * hitEnvirObjVFXMag, hitVFXSize.y * hitEnvirObjVFXMag, hitVFXSize.z * hitEnvirObjVFXMag);
					PlayKinfeHitVFX(hitObj.transform, playerController.transform.eulerAngles.y, hitObj.point, hitObj.normal, vfxSize, HitState.KinfeDefaultEnvir, true);

					break;
				}
			}
		}

		playerController.playerBreathSound.PlayAttackRoarSound(); // ���Ź��������Ч
		playerKinfeSound.PlayKinfeWaveSound(); // ���Żӵ���Ч
		playerController.weaponManager.playerGunState = PlayerGunState.Standby;
	}

	#endregion

	#region ����ʱ�ᷢ��������

	/// <summary>
	/// ��������Ŀ��ʱ������Ч
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
