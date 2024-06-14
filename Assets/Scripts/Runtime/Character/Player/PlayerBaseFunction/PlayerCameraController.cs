using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
	// 基本视角旋转，后坐力枪口上跳由程序控制 CameraController 父物体
	// 视角摇晃，视角震动由 Animation 控制 PlayerCamera 子物体

	#region 基本组件和变量

	[Header("角色基本组件")]
	[Header("玩家摄像机控制器")]
	[SerializeField] private Transform playerCameraController;

	[Header("玩家视角摄像机")]
	public Camera playerCamera;

	[Header("摄像机 FOV，由设置读取")]
	public float normalFOV = 60f;
	private float aimingFOV;

	[Header("普通和瞄准 FOV 的距离")]
	[SerializeField] private float fovDistance;
	[Header("改变 FOV 的加速度")]
	[SerializeField] private float fovAddSpeed;

	// 相机 FOV 改变速度
	private float fovChangeSpeed;
	private float fovReturnSpeed;

	[Header("视角摇晃相关")]
	[Header("玩家死亡倒地时的视角改变")]
	[Tooltip("玩家死亡时的视角是先跪下再倒下，因此视角落地分为两段")]
	[Header("跪下时的摄像机位置")]
	[SerializeField] private Transform kneelDownCameraTrans;
	[Header("倒下时的摄像机位置")]
	[SerializeField] private Transform fallDownCameraTrans;

	[Header("跪下时摄像机的移动速度")]
	[SerializeField] private float kneelDownCameraSpeed;
	[Header("倒下时摄像机的移动速度")]
	[SerializeField] private float fallDownCameraSpeed;

	[Header("跪在地上后隔多长时间倒下")]
	[SerializeField] private float fallDownDelayTime;

	[Header("玩家角色的身体")]
	[SerializeField] private Transform playerBody;
	private FPS_Play_Action fpsPlayAction; // 输入系统

	[Header("鼠标灵敏度，取值范围为 (0, 0.1]")]
	[Range(0f, 0.1f)]
	public float mouseSpeed;

	[Header("视平线到最大角度的限制（角度制）")]
	[SerializeField] private float clampAngleValue;

	private float playerHorizontalRotate;
	private float playerVerticalRotate;
	private float cameraRotateTotal; // 相机旋转的累加值

	[Header("摄像机上下角度限制的值（弧度制，不用自己调，显示在此仅为观察）")]
	[SerializeField] private float maxCameraRotateTotal;
	[SerializeField] private float minCameraRotateTotal;

	[HideInInspector] public float cameraRecoilForce; // 用于后坐力的枪口上跳

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

	#region 相机 FOV 参数计算

	/// <summary>
	/// 读取摄像机设置的 FOV
	/// </summary>
	public void CalculateCameraFOV()
	{
		aimingFOV = normalFOV - 10f;

		fovChangeSpeed = (normalFOV - aimingFOV) * fovDistance + fovAddSpeed;
		fovReturnSpeed = (normalFOV - aimingFOV) * fovDistance - fovAddSpeed;
	}

	#endregion

	#region 玩家视角旋转逻辑

	public void SetupPlayerView()
	{
		// 根据鼠标灵敏度计算出上下旋转角度限制（弧度制）
		maxCameraRotateTotal = clampAngleValue / mouseSpeed;
		minCameraRotateTotal = -(clampAngleValue / mouseSpeed);
	}

	public void PlayerViewRotate()
	{
		// 鼠标在二维平面的移动会被拆分成两个 float
		playerHorizontalRotate = fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Rotate.ReadValue<Vector2>().x;
		playerVerticalRotate = fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Rotate.ReadValue<Vector2>().y;

		cameraRotateTotal = cameraRotateTotal + playerVerticalRotate;

		// 叠加枪械后坐力（枪口上跳与平移）
		cameraRotateTotal = cameraRotateTotal + cameraRecoilForce;
		playerHorizontalRotate = playerHorizontalRotate + Random.Range(-cameraRecoilForce, cameraRecoilForce);

		cameraRotateTotal = Mathf.Clamp(cameraRotateTotal, minCameraRotateTotal, maxCameraRotateTotal);

		// 上下旋转摄像机，左右旋转玩家身体
		playerCameraController.localEulerAngles = new Vector3(-cameraRotateTotal * mouseSpeed, 0f, 0f);
		playerBody.Rotate(0f, playerHorizontalRotate * mouseSpeed, 0f);

		// 慢慢减小枪口上跳的值，直到下一次开枪再次被重新设置
		cameraRecoilForce = cameraRecoilForce * 0.5f;
	}

	/// <summary>
	/// 设置枪口上跳大小
	/// </summary>
	/// <param name="recoilForce"></param>
	public void SetCameraRecoilForce(float recoilForce)
	{
		cameraRecoilForce = recoilForce;
	}

	#endregion

	#region 抬枪瞄准时改变摄像机 FOV

	public void ChangeCameraFOVWhenAiming(bool isAiming)
	{
		if (isAiming)
		{
			playerCamera.fieldOfView = Mathf.MoveTowards(playerCamera.fieldOfView, aimingFOV, fovChangeSpeed * Time.deltaTime);
		}
		else
		{
			playerCamera.fieldOfView = Mathf.MoveTowards(playerCamera.fieldOfView, normalFOV, fovReturnSpeed * Time.deltaTime);
		}
	}

	#endregion

	#region 玩家死亡倒地时的视角变化（摄像机位置变化）

	/// <summary>
	/// 玩家死亡时视角二段倒地
	/// </summary>
	public void ChangeCameraPosWhenDie()
	{
		StartCoroutine(CameraKneelDown());
	}

	private IEnumerator CameraKneelDown()
	{
		while (true)
		{
			playerCameraController.localPosition = Vector3.MoveTowards(playerCameraController.localPosition, kneelDownCameraTrans.localPosition, kneelDownCameraSpeed * Time.deltaTime);

			if (Vector3.Distance(playerCameraController.localPosition, kneelDownCameraTrans.localPosition) == 0f)
			{
				playerCameraController.localPosition = kneelDownCameraTrans.localPosition;
				StartCoroutine(CameraDelayFallDown());
				yield break;
			}

			yield return null;
		}
	}

	private IEnumerator CameraDelayFallDown()
	{
		yield return new WaitForSeconds(fallDownDelayTime);
		StartCoroutine(CameraFallDown());
	}

	private IEnumerator CameraFallDown()
	{
		while (true)
		{
			playerCameraController.localPosition = Vector3.MoveTowards(playerCameraController.localPosition, fallDownCameraTrans.localPosition, fallDownCameraSpeed * Time.deltaTime);

			if (Vector3.Distance(playerCameraController.localPosition, fallDownCameraTrans.localPosition) == 0f)
			{
				playerCameraController.localPosition = fallDownCameraTrans.localPosition;
				yield break;
			}

			yield return null;
		}
	}

	#endregion
}
