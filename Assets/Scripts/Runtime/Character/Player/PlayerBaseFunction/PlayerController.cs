using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家角色控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction; // 输入系统

	[Header("角色基本组件")]
	[Header("角色控制器和相机根物体")]
	public CharacterController characterController;
	[SerializeField] private GameObject cameraControllerObj;

	[Header("Layer TAG 大集合")]
	public LayerAndTagCollection_Player layerAndTagCollection_Player;
	[Header("玩家事件管理器")]
	public EventHandler_Player eventHandler_Player;

	[Header("第一人称手臂控制器")]
	public FPSBodyController fpsBodyController;

	[Header("玩家提示信息管理器")]
	public TipMessageController tipMessageController;
	[Header("玩家教程触发器")]
	public TutorialTipTrigger tutorialTrigger;
	[Header("玩家时间管理器")]
	public PlayerTimeManager playerTimeManager;

	[Header("玩家脚步声管理器")]
	public PlayerFootstepSound playerFootstepSound;
	[Header("玩家呼吸声管理器")]
	public PlayerBreathSound playerBreathSound;

	[Header("玩家物品栏")]
	public PlayerInventory playerInventory;

	[Header("玩家角色的灯光")]
	public Light flashLight;
	public Light fpsItemViewLight;

	[Tooltip("用于敌人视野检测 Trigger 对象")]
	public GameObject enemyViewTarget;

	public PlayerCameraController playerCameraController;
	public WeaponManager weaponManager;
	public PlayerDoorInteractive playerDoorInteractive;
	public PlayerItemInteractive playerItemInteractive;
	public PlayerFurnitureInteractive playerFurnitureInteractive;

	[Header("角色运动相关参数")]
	[Header("地面检测点")]
	[SerializeField] private Transform groundCheckTrans;
	[Header("地面检测范围")]
	[SerializeField] private float groundCheckRadius;

	[Header("斜坡检测点")]
	[SerializeField] private Transform slopeCheckTrans;
	[Header("斜坡检测范围")]
	[SerializeField] private float slopeCheckRadius;
	private Vector3 groundNormal; // 记录检测到的地面法线

	// 参考值：走路移动速度 2.0 跑步移动速度 3.0
	[Header("角色走路速度")]
	[SerializeField] private float walkSpeed;
	[Header("角色跑步速度")]
	[SerializeField] private float runSpeed;
	[Header("下蹲时的移动速度")]
	[SerializeField] private float crouchSpeed;
	[Header("斜坡下滑速度")]
	[SerializeField] private float moveSpeedOnSlope;

	[Header("玩家处在某种状态下对移动速度按比例削减")]
	[Header("玩家抬枪瞄准")]
	[SerializeField] private float multiReduceMoveSpeedAiming;
	[Header("玩家后退行走")]
	[SerializeField] private float multiReduceMoveSpeedMoveBack;
	private float multiReduceMoveSpeed;

	[Header("插值跟随移动速度（实际驱动玩家的速度参数）")]
	[Header("在一般地面时的跟随速度")]
	[SerializeField] private float moveLerpSpeedOnGround;
	[Header("滑斜坡时的跟随速度")]
	[SerializeField] private float moveLerpSpeedOnSlope;
	private Vector3 slidingMoveVector; // 斜坡滑动移动速度

	private float moveLerpSpeedUse; // 实际使用的插值移动速度
	private float moveSpeedUse; // 实际使用的移动速度

	private Vector3 targetMoveVector; // 玩家设备输入的值
	[HideInInspector] public Vector3 performMoveVector; // 最终计算得到的，玩家移动用向量

	[Header("角色自由落体用参数")]
	[Header("通常状态下的角色重力")]
	[SerializeField] private float playerGravity; // 角色重力大小
	private Vector3 playerGravityVector; // 角色重力向量

	[Header("角色下蹲和下蹲 CD 时间")]
	[Header("角色站立时，主角胶囊体的中心点和大小")]
	[SerializeField] private Vector3 standCCCenter;
	[SerializeField] private float standCCHeight;

	[Header("角色下蹲时，主角胶囊体的中心点和大小")]
	[SerializeField] private Vector3 crouchCCCenter;
	[SerializeField] private float crouchCCHeight;

	[Header("摄像机站立和下蹲分别在什么位置")]
	[SerializeField] private Transform cameraStandTrans;
	[SerializeField] private Transform cameraCrouchTrans;
	private Transform cameraTargetTransform;

	[Header("摄像机在下蹲和起立时的改变速度")]
	[SerializeField] private float cameraChangeHeightSpeed;

	[Header("角色下蹲的 CD 时间")]
	[SerializeField] private float crouchCDTime;
	private float crouchTotalTime; // 距离上一次下蹲过去了多久

	// 记录玩家的行动状态
	[HideInInspector] public bool playerIsCrouch;
	[HideInInspector] public bool playerIsRun;
	[HideInInspector] public bool playerIsMove;
	[HideInInspector] public bool playerIsMoveBack;
	[HideInInspector] public bool playerIsMoveForward;
	[HideInInspector] public bool playerIsGrounded; // 玩家是否踩在地面上
	[HideInInspector] public bool playerIsOnSlope; // 玩家是否踩在斜坡上
	[HideInInspector] public bool playerIsSliding; // 玩家是否正在滑坡

	// 注意：Aiming 是属于 PlayerController 的一种状态，但这种状态只有在持枪时才能激活
	[HideInInspector] public bool isAiming;
	[HideInInspector] public float moveVelocity;

	private Vector3 capsuleBottomHemisphere;
	private Vector3 capsuleTopHemisphere; // 玩家碰撞射线检测（撞墙重置移动速度）

	// 玩家所处状态（FSM）
	public PlayerBaseState playerCurrentState; // 当前所处状态
	public PlayerBaseState playerLastState; // 上一个状态
	public PlayerControlState playerControlState = new PlayerControlState(); // 玩家自主操作状态
	public PlayerViewState playerViewState = new PlayerViewState(); // 玩家端详某物品时的状态
	public PlayerOpenInventoryState playerOpenInventoryState = new PlayerOpenInventoryState(); // 玩家打开物品栏的状态 
	public PlayerCompleteState playerCompleteState = new PlayerCompleteState(); // 玩家成功完成游戏状态
	public PlayerDeadState playerDeadState = new PlayerDeadState(); // 玩家死亡状态
	public GamePauseState gamePauseState = new GamePauseState(); // 打开暂停菜单

	[Header("瑕疵修复：玩家从暂停菜单返回时有非常短的时间不能瞄准")]
	public float fixAimActionMaxTime = 0.2f;
	public float fixAimActionCurTime = 0f;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void Start()
	{
		crouchTotalTime = crouchCDTime;
		SwitchState(playerControlState); // 游戏开始时进入玩家自由操作状态

		if (GameProgressManager.Instance != null)
		{
			GameProgressManager.Instance.playerController = this; // 注册自己
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

	#region 玩家角色状态切换

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

	#region 玩家 GamePlay 基本功能

	/// <summary>
	/// 移动状态检测
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
	/// 角色障碍物检测的底部位置
	/// </summary>
	private Vector3 GetCapsuleBottomHemisphere()
	{
		return transform.position + (transform.up * characterController.radius);
	}

	/// <summary>
	/// 角色障碍物检测的顶部位置
	/// </summary>
	/// <param name="characterHeight"></param>
	private Vector3 GetCapsuleTopHemisphere(float characterHeight)
	{
		return transform.position + (transform.up * (characterHeight - characterController.radius));
	}

	/// <summary>
	/// 判断角色是不是踩在斜坡上
	/// </summary>
	/// <param name="normal"></param>
	/// <returns></returns>
	private bool IsNormalUnderSlopeLimit(Vector3 normal)
	{
		return Vector3.Angle(transform.up, normal) > characterController.slopeLimit;
	}

	/// <summary>
	/// 获得在斜坡上的重定向移动速度（根据斜坡坡度重设移动速度）
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
	/// 玩家实际执行移动
	/// </summary>
	public void PlayerPerformMove()
	{
		// 读取输入的值后转换为世界坐标
		targetMoveVector = new Vector3(fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Movement.ReadValue<Vector2>().x, 0f, fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Movement.ReadValue<Vector2>().y);
		targetMoveVector = transform.TransformVector(targetMoveVector);
		targetMoveVector = new Vector3(targetMoveVector.x * (moveSpeedUse * multiReduceMoveSpeed), targetMoveVector.y * (moveSpeedUse * multiReduceMoveSpeed), targetMoveVector.z * (moveSpeedUse * multiReduceMoveSpeed));

		// 在斜坡上降低玩家移动速度
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

		// 撞墙重置移动速度，官方案例：FPS_Microgame
		capsuleBottomHemisphere = GetCapsuleBottomHemisphere();
		capsuleTopHemisphere = GetCapsuleTopHemisphere(characterController.height);

		// HACK: 画一个和玩家胶囊体一样大的胶囊体射线检测
		if (Physics.CapsuleCast(capsuleBottomHemisphere, capsuleTopHemisphere, characterController.radius,
			performMoveVector.normalized, out RaycastHit hit, performMoveVector.magnitude * Time.deltaTime, -1,
			QueryTriggerInteraction.Ignore))
		{
			performMoveVector = Vector3.ProjectOnPlane(performMoveVector, hit.normal);
		}

		characterController.Move(performMoveVector * Time.deltaTime);

		// 获取角色的运动速度并用于动画更新
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
	/// 玩家在斜坡上滑动
	/// </summary>
	public void PlayerSlideOnSlope()
	{
		playerIsSliding = false;

		if (playerIsGrounded && playerIsOnSlope)
		{
			playerIsSliding = true;

			// 注意：两个乘法末尾的系数必须是一样的才有滑坡效果（否则玩家可以从坡下爬上去）
			slidingMoveVector.x = (1f - groundNormal.y) * groundNormal.x * moveSpeedOnSlope;
			slidingMoveVector.z = (1f - groundNormal.y) * groundNormal.z * moveSpeedOnSlope;

			performMoveVector += slidingMoveVector;
			performMoveVector = performMoveVector.normalized * moveSpeedOnSlope;
		}
	}

	/// <summary>
	/// 玩家地面检测
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
	/// 玩家斜坡检测
	/// </summary>
	public void PlayerSlopeCheck()
	{
		groundNormal = Vector3.up;
		playerIsOnSlope = false;

		RaycastHit hitObj;

		if (Physics.Raycast(slopeCheckTrans.position, -slopeCheckTrans.up, out hitObj, slopeCheckRadius,
			1 << layerAndTagCollection_Player.groundLayerIndex, QueryTriggerInteraction.Ignore))
		{
			groundNormal = hitObj.normal; // 记录检测到的地面法线
			playerIsOnSlope = IsNormalUnderSlopeLimit(groundNormal);
		}
	}

	/// <summary>
	/// 玩家重力效果实现
	/// </summary>
	public void PlayerPerformGravity()
	{
		playerGravityVector.y = playerGravity;
		characterController.Move(playerGravityVector * Time.deltaTime);
	}

	/// <summary>
	/// 玩家不在控制状态时停止移动
	/// </summary>
	public void PlayerStopMove()
	{
		moveVelocity = 0f;
		performMoveVector = Vector3.zero;
		characterController.Move(performMoveVector * Time.deltaTime);
	}

	/// <summary>
	/// 判断玩家是否有向前的移动输入
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
	/// 判断玩家是否有向后的移动输入
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
	/// 玩家执行跑步
	/// </summary>
	public void PlayerPerformRun()
	{
		if (playerIsCrouch) return; // 下蹲状态不能跑步
		if (!playerIsMoveForward) return; // 角色不是向前移动时不允许跑步
		if (isAiming) return; // 瞄准时不能跑步

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
	/// 玩家下蹲
	/// </summary>
	public void PlayerPerformCrouch()
	{
		if (playerIsRun) return; // 跑步的时候不能下蹲

		crouchTotalTime = crouchTotalTime + Time.deltaTime; // 累加下蹲 CD 时间

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Crouch.phase == InputActionPhase.Performed &&
			crouchTotalTime > crouchCDTime)
		{
			playerIsCrouch = true; // 玩家下蹲

			moveSpeedUse = crouchSpeed;

			characterController.center = crouchCCCenter; // 改变碰撞体尺寸
			characterController.height = crouchCCHeight;

			crouchTotalTime = 0f;
		}

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Crouch.phase == InputActionPhase.Waiting)
		{
			playerIsCrouch = false;

			moveSpeedUse = walkSpeed;

			characterController.center = standCCCenter; // 恢复碰撞体尺寸
			characterController.height = standCCHeight;
		}
	}

	/// <summary>
	/// 玩家下蹲或起立时，改变主相机的高度
	/// </summary>
	public void ChangeCameraPos()
	{
		if (playerIsCrouch) cameraTargetTransform = cameraCrouchTrans;
		else cameraTargetTransform = cameraStandTrans;

		cameraControllerObj.transform.localPosition = Vector3.MoveTowards(cameraControllerObj.transform.localPosition, cameraTargetTransform.localPosition, cameraChangeHeightSpeed * Time.deltaTime);
	}

	#endregion

	#region DEBUG 用函数

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere(groundCheckTrans.position, groundCheckRadius);
		Debug.DrawRay(slopeCheckTrans.position, -slopeCheckTrans.up * slopeCheckRadius);
	}

	#endregion

}
