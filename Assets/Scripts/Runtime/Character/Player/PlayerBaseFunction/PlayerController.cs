using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��ҽ�ɫ������
/// </summary>
public class PlayerController : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction; // ����ϵͳ

	[Header("��ɫ�������")]
	[Header("��ɫ�����������������")]
	public CharacterController characterController;
	[SerializeField] private GameObject cameraControllerObj;

	[Header("Layer TAG �󼯺�")]
	public LayerAndTagCollection_Player layerAndTagCollection_Player;
	[Header("����¼�������")]
	public EventHandler_Player eventHandler_Player;

	[Header("��һ�˳��ֱۿ�����")]
	public FPSBodyController fpsBodyController;

	[Header("�����ʾ��Ϣ������")]
	public TipMessageController tipMessageController;
	[Header("��ҽ̴̳�����")]
	public TutorialTipTrigger tutorialTrigger;
	[Header("���ʱ�������")]
	public PlayerTimeManager playerTimeManager;

	[Header("��ҽŲ���������")]
	public PlayerFootstepSound playerFootstepSound;
	[Header("��Һ�����������")]
	public PlayerBreathSound playerBreathSound;

	[Header("�����Ʒ��")]
	public PlayerInventory playerInventory;

	[Header("��ҽ�ɫ�ĵƹ�")]
	public Light flashLight;
	public Light fpsItemViewLight;

	[Tooltip("���ڵ�����Ұ��� Trigger ����")]
	public GameObject enemyViewTarget;

	public PlayerCameraController playerCameraController;
	public WeaponManager weaponManager;
	public PlayerDoorInteractive playerDoorInteractive;
	public PlayerItemInteractive playerItemInteractive;
	public PlayerFurnitureInteractive playerFurnitureInteractive;

	[Header("��ɫ�˶���ز���")]
	[Header("�������")]
	[SerializeField] private Transform groundCheckTrans;
	[Header("�����ⷶΧ")]
	[SerializeField] private float groundCheckRadius;

	[Header("б�¼���")]
	[SerializeField] private Transform slopeCheckTrans;
	[Header("б�¼�ⷶΧ")]
	[SerializeField] private float slopeCheckRadius;
	private Vector3 groundNormal; // ��¼��⵽�ĵ��淨��

	// �ο�ֵ����·�ƶ��ٶ� 2.0 �ܲ��ƶ��ٶ� 3.0
	[Header("��ɫ��·�ٶ�")]
	[SerializeField] private float walkSpeed;
	[Header("��ɫ�ܲ��ٶ�")]
	[SerializeField] private float runSpeed;
	[Header("�¶�ʱ���ƶ��ٶ�")]
	[SerializeField] private float crouchSpeed;
	[Header("б���»��ٶ�")]
	[SerializeField] private float moveSpeedOnSlope;

	[Header("��Ҵ���ĳ��״̬�¶��ƶ��ٶȰ���������")]
	[Header("���̧ǹ��׼")]
	[SerializeField] private float multiReduceMoveSpeedAiming;
	[Header("��Һ�������")]
	[SerializeField] private float multiReduceMoveSpeedMoveBack;
	private float multiReduceMoveSpeed;

	[Header("��ֵ�����ƶ��ٶȣ�ʵ��������ҵ��ٶȲ�����")]
	[Header("��һ�����ʱ�ĸ����ٶ�")]
	[SerializeField] private float moveLerpSpeedOnGround;
	[Header("��б��ʱ�ĸ����ٶ�")]
	[SerializeField] private float moveLerpSpeedOnSlope;
	private Vector3 slidingMoveVector; // б�»����ƶ��ٶ�

	private float moveLerpSpeedUse; // ʵ��ʹ�õĲ�ֵ�ƶ��ٶ�
	private float moveSpeedUse; // ʵ��ʹ�õ��ƶ��ٶ�

	private Vector3 targetMoveVector; // ����豸�����ֵ
	[HideInInspector] public Vector3 performMoveVector; // ���ռ���õ��ģ�����ƶ�������

	[Header("��ɫ���������ò���")]
	[Header("ͨ��״̬�µĽ�ɫ����")]
	[SerializeField] private float playerGravity; // ��ɫ������С
	private Vector3 playerGravityVector; // ��ɫ��������

	[Header("��ɫ�¶׺��¶� CD ʱ��")]
	[Header("��ɫվ��ʱ�����ǽ���������ĵ�ʹ�С")]
	[SerializeField] private Vector3 standCCCenter;
	[SerializeField] private float standCCHeight;

	[Header("��ɫ�¶�ʱ�����ǽ���������ĵ�ʹ�С")]
	[SerializeField] private Vector3 crouchCCCenter;
	[SerializeField] private float crouchCCHeight;

	[Header("�����վ�����¶׷ֱ���ʲôλ��")]
	[SerializeField] private Transform cameraStandTrans;
	[SerializeField] private Transform cameraCrouchTrans;
	private Transform cameraTargetTransform;

	[Header("��������¶׺�����ʱ�ĸı��ٶ�")]
	[SerializeField] private float cameraChangeHeightSpeed;

	[Header("��ɫ�¶׵� CD ʱ��")]
	[SerializeField] private float crouchCDTime;
	private float crouchTotalTime; // ������һ���¶׹�ȥ�˶��

	// ��¼��ҵ��ж�״̬
	[HideInInspector] public bool playerIsCrouch;
	[HideInInspector] public bool playerIsRun;
	[HideInInspector] public bool playerIsMove;
	[HideInInspector] public bool playerIsMoveBack;
	[HideInInspector] public bool playerIsMoveForward;
	[HideInInspector] public bool playerIsGrounded; // ����Ƿ���ڵ�����
	[HideInInspector] public bool playerIsOnSlope; // ����Ƿ����б����
	[HideInInspector] public bool playerIsSliding; // ����Ƿ����ڻ���

	// ע�⣺Aiming ������ PlayerController ��һ��״̬��������״ֻ̬���ڳ�ǹʱ���ܼ���
	[HideInInspector] public bool isAiming;
	[HideInInspector] public float moveVelocity;

	private Vector3 capsuleBottomHemisphere;
	private Vector3 capsuleTopHemisphere; // �����ײ���߼�⣨ײǽ�����ƶ��ٶȣ�

	// �������״̬��FSM��
	public PlayerBaseState playerCurrentState; // ��ǰ����״̬
	public PlayerBaseState playerLastState; // ��һ��״̬
	public PlayerControlState playerControlState = new PlayerControlState(); // �����������״̬
	public PlayerViewState playerViewState = new PlayerViewState(); // ��Ҷ���ĳ��Ʒʱ��״̬
	public PlayerOpenInventoryState playerOpenInventoryState = new PlayerOpenInventoryState(); // ��Ҵ���Ʒ����״̬ 
	public PlayerCompleteState playerCompleteState = new PlayerCompleteState(); // ��ҳɹ������Ϸ״̬
	public PlayerDeadState playerDeadState = new PlayerDeadState(); // �������״̬
	public GamePauseState gamePauseState = new GamePauseState(); // ����ͣ�˵�

	[Header("覴��޸�����Ҵ���ͣ�˵�����ʱ�зǳ��̵�ʱ�䲻����׼")]
	public float fixAimActionMaxTime = 0.2f;
	public float fixAimActionCurTime = 0f;

	#endregion

	#region �����������ں���

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void Start()
	{
		crouchTotalTime = crouchCDTime;
		SwitchState(playerControlState); // ��Ϸ��ʼʱ����������ɲ���״̬

		if (GameProgressManager.Instance != null)
		{
			GameProgressManager.Instance.playerController = this; // ע���Լ�
		}
	}

	private void Update()
	{
		playerCurrentState.OnUpdate(this);
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region ��ҽ�ɫ״̬�л�

	public void SwitchState(PlayerBaseState _state)
	{
		if (playerCurrentState != null)
		{
			playerLastState = playerCurrentState;
		}

		playerCurrentState = _state;
		playerCurrentState.EnterState(this);
		eventHandler_Player.InvokePlayerStateChange(_state);
	}

	#endregion

	#region ��� GamePlay ��������

	/// <summary>
	/// �ƶ�״̬���
	/// </summary>
	public void PlayerActionCheck()
	{
		if (isAiming)
		{
			if (playerIsCrouch)
			{
				moveSpeedUse = crouchSpeed;
			}
			else
			{
				moveSpeedUse = walkSpeed;
			}

			multiReduceMoveSpeed = multiReduceMoveSpeedAiming;
			return;
		}

		if (playerIsMoveBack)
		{
			if (playerIsCrouch)
			{
				moveSpeedUse = crouchSpeed;
			}
			else
			{
				moveSpeedUse = walkSpeed;
			}

			multiReduceMoveSpeed = multiReduceMoveSpeedMoveBack;
			return;
		}

		multiReduceMoveSpeed = 1f;
	}

	/// <summary>
	/// ��ɫ�ϰ�����ĵײ�λ��
	/// </summary>
	private Vector3 GetCapsuleBottomHemisphere()
	{
		return transform.position + (transform.up * characterController.radius);
	}

	/// <summary>
	/// ��ɫ�ϰ�����Ķ���λ��
	/// </summary>
	/// <param name="characterHeight"></param>
	private Vector3 GetCapsuleTopHemisphere(float characterHeight)
	{
		return transform.position + (transform.up * (characterHeight - characterController.radius));
	}

	/// <summary>
	/// �жϽ�ɫ�ǲ��ǲ���б����
	/// </summary>
	/// <param name="normal"></param>
	/// <returns></returns>
	private bool IsNormalUnderSlopeLimit(Vector3 normal)
	{
		return Vector3.Angle(transform.up, normal) > characterController.slopeLimit;
	}

	/// <summary>
	/// �����б���ϵ��ض����ƶ��ٶȣ�����б���¶������ƶ��ٶȣ�
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="slopeNormal"></param>
	/// <returns></returns>
	public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
	{
		Vector3 directionRight = Vector3.Cross(direction, transform.up);
		return Vector3.Cross(slopeNormal, directionRight).normalized;
	}

	/// <summary>
	/// ���ʵ��ִ���ƶ�
	/// </summary>
	public void PlayerPerformMove()
	{
		// ��ȡ�����ֵ��ת��Ϊ��������
		targetMoveVector = new Vector3(fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Movement.ReadValue<Vector2>().x, 0f, fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Movement.ReadValue<Vector2>().y);
		targetMoveVector = transform.TransformVector(targetMoveVector);
		targetMoveVector = new Vector3(targetMoveVector.x * (moveSpeedUse * multiReduceMoveSpeed), targetMoveVector.y * (moveSpeedUse * multiReduceMoveSpeed), targetMoveVector.z * (moveSpeedUse * multiReduceMoveSpeed));

		// ��б���Ͻ�������ƶ��ٶ�
		targetMoveVector = GetDirectionReorientedOnSlope(targetMoveVector.normalized, groundNormal) * targetMoveVector.magnitude;

		if (playerIsSliding)
		{
			moveLerpSpeedUse = moveLerpSpeedOnSlope;
		}
		else
		{
			moveLerpSpeedUse = moveLerpSpeedOnGround;
		}

		performMoveVector = Vector3.Lerp(performMoveVector, targetMoveVector, moveLerpSpeedUse * Time.deltaTime);

		// ײǽ�����ƶ��ٶȣ��ٷ�������FPS_Microgame
		capsuleBottomHemisphere = GetCapsuleBottomHemisphere();
		capsuleTopHemisphere = GetCapsuleTopHemisphere(characterController.height);

		// HACK: ��һ�������
		if (Physics.CapsuleCast(capsuleBottomHemisphere, capsuleTopHemisphere, characterController.radius,
			performMoveVector.normalized, out RaycastHit hit, performMoveVector.magnitude * Time.deltaTime, -1,
			QueryTriggerInteraction.Ignore))
		{
			performMoveVector = Vector3.ProjectOnPlane(performMoveVector, hit.normal);
		}

		characterController.Move(performMoveVector * Time.deltaTime);

		// ��ȡ��ɫ���˶��ٶȲ����ڶ�������
		Vector3 characterVelocityVector = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);
		moveVelocity = characterVelocityVector.magnitude;

		if (moveVelocity > 0f)
		{
			playerIsMove = true;
		}
		else
		{
			playerIsMove = false;
		}
	}
	
	/// <summary>
	/// �����б���ϻ���
	/// </summary>
	public void PlayerSlideOnSlope()
	{
		playerIsSliding = false;

		if (playerIsGrounded && playerIsOnSlope)
		{
			playerIsSliding = true;

			// ע�⣺�����˷�ĩβ��ϵ��������һ���Ĳ��л���Ч����������ҿ��Դ���������ȥ��
			slidingMoveVector.x = (1f - groundNormal.y) * groundNormal.x * moveSpeedOnSlope;
			slidingMoveVector.z = (1f - groundNormal.y) * groundNormal.z * moveSpeedOnSlope;

			performMoveVector += slidingMoveVector;
			performMoveVector = performMoveVector.normalized * moveSpeedOnSlope;
		}
	}

	/// <summary>
	/// ��ҵ�����
	/// </summary>
	public void PlayerGroundCheck()
	{
		playerIsGrounded = false;

		if (Physics.CheckSphere(groundCheckTrans.position, groundCheckRadius,
			1 << layerAndTagCollection_Player.groundLayerIndex, QueryTriggerInteraction.Ignore))
		{
			playerIsGrounded = true;
		}
	}

	/// <summary>
	/// ���б�¼��
	/// </summary>
	public void PlayerSlopeCheck()
	{
		groundNormal = Vector3.up;
		playerIsOnSlope = false;

		RaycastHit hitObj;

		if (Physics.Raycast(slopeCheckTrans.position, -slopeCheckTrans.up, out hitObj, slopeCheckRadius,
			1 << layerAndTagCollection_Player.groundLayerIndex, QueryTriggerInteraction.Ignore))
		{
			groundNormal = hitObj.normal; // ��¼��⵽�ĵ��淨��
			playerIsOnSlope = IsNormalUnderSlopeLimit(groundNormal);
		}
	}

	/// <summary>
	/// �������Ч��ʵ��
	/// </summary>
	public void PlayerPerformGravity()
	{
		playerGravityVector.y = playerGravity;
		characterController.Move(playerGravityVector * Time.deltaTime);
	}

	/// <summary>
	/// ��Ҳ��ڿ���״̬ʱֹͣ�ƶ�
	/// </summary>
	public void PlayerStopMove()
	{
		moveVelocity = 0f;
		performMoveVector = Vector3.zero;
		characterController.Move(performMoveVector * Time.deltaTime);
	}

	/// <summary>
	/// �ж�����Ƿ�����ǰ���ƶ�����
	/// </summary>
	public void CheckPlayerMoveForward()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveForward.phase == InputActionPhase.Performed)
		{
			playerIsMoveForward = true;
		}

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveForward.phase == InputActionPhase.Waiting)
		{
			if (playerIsCrouch)
			{
				moveSpeedUse = crouchSpeed;
			}
			else
			{
				moveSpeedUse = walkSpeed;
			}

			playerIsMoveForward = false;
		}
	}

	/// <summary>
	/// �ж�����Ƿ��������ƶ�����
	/// </summary>
	public void CheckPlayerMoveBack()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveBack.phase == InputActionPhase.Performed)
		{
			playerIsMoveBack = true;

			if (playerIsCrouch)
			{
				moveSpeedUse = crouchSpeed;
			}
			else
			{
				moveSpeedUse = walkSpeed;
			}
		}

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveBack.phase == InputActionPhase.Waiting)
		{
			playerIsMoveBack = false;
		}
	}

	/// <summary>
	/// ���ִ���ܲ�
	/// </summary>
	public void PlayerPerformRun()
	{
		if (playerIsCrouch) return; // �¶�״̬�����ܲ�
		if (!playerIsMoveForward) return; // ��ɫ������ǰ�ƶ�ʱ�������ܲ�
		if (isAiming) return; // ��׼ʱ�����ܲ�

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Run.phase == InputActionPhase.Performed)
		{
			playerIsRun = true;
			moveSpeedUse = runSpeed;
		}

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Run.phase == InputActionPhase.Waiting)
		{
			playerIsRun = false;
			moveSpeedUse = walkSpeed;
		}
	}

	/// <summary>
	/// ����¶�
	/// </summary>
	public void PlayerPerformCrouch()
	{
		if (playerIsRun) return; // �ܲ���ʱ�����¶�

		crouchTotalTime = crouchTotalTime + Time.deltaTime; // �ۼ��¶� CD ʱ��

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Crouch.phase == InputActionPhase.Performed &&
			crouchTotalTime > crouchCDTime)
		{
			playerIsCrouch = true; // ����¶�

			moveSpeedUse = crouchSpeed;

			characterController.center = crouchCCCenter; // �ı���ײ��ߴ�
			characterController.height = crouchCCHeight;

			crouchTotalTime = 0f;
		}

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Crouch.phase == InputActionPhase.Waiting)
		{
			playerIsCrouch = false;

			moveSpeedUse = walkSpeed;

			characterController.center = standCCCenter; // �ָ���ײ��ߴ�
			characterController.height = standCCHeight;
		}
	}

	/// <summary>
	/// ����¶׻�����ʱ���ı�������ĸ߶�
	/// </summary>
	public void ChangeCameraPos()
	{
		if (playerIsCrouch) cameraTargetTransform = cameraCrouchTrans;
		else cameraTargetTransform = cameraStandTrans;

		cameraControllerObj.transform.localPosition = Vector3.MoveTowards(cameraControllerObj.transform.localPosition, cameraTargetTransform.localPosition, cameraChangeHeightSpeed * Time.deltaTime);
	}

	#endregion

	#region DEBUG �ú���

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere(groundCheckTrans.position, groundCheckRadius);
		Debug.DrawRay(slopeCheckTrans.position, -slopeCheckTrans.up * slopeCheckRadius);
	}

	#endregion

}
