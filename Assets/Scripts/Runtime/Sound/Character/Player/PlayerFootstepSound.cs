using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 脚步声音效是游戏音效
// 不属于 3D 音效

public enum GroundMaterial // 这个枚举没有 None，如果检测到未定义的地面材质，默认播放混凝土脚步声音效
{
	Concrete, // 混凝土地面
	ConcreteStaircase, // 混凝土台阶
	Brick, // 石砖
}

// TODO: 未来的更新：把玩家滑坡做成一个新的 FSM 状态机，就可以协程控制滑坡音效修正播放细节

public class PlayerFootstepSound : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction; // 输入系统

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;

	[Header("角色地面检测功能")]
	[SerializeField] private Transform groundCheckPoint;
	[SerializeField] private float groundCheckRadius;

	[Header("脚步声音源组件")]
	[SerializeField] private AudioSource footstepAudioSource;

	[Header("音效播放的时间间隔")]
	[Header("角色走路时脚步声的间隔")]
	[SerializeField] private float walkFootstepIntervalTime;
	private float walkFootstepTotalTime;
	[Header("角色跑步时脚步声的间隔")]
	[SerializeField] private float runFootstepIntervalTime;
	private float runFootstepTotalTime;
	[Header("角色后退时脚步声的间隔")]
	[SerializeField] private float moveBackFootstepIntervalTime;
	private float moveBackFootstepTotalTime;
	[Header("角色下蹲时的脚步声间隔")]
	[SerializeField] private float crouchFootstepIntervalTime;
	private float crouchFootstepTotalTime;
	[Header("角色下蹲后退时的脚步声间隔")]
	[SerializeField] private float crouchBackFootstepIntervalTime;
	private float crouchBackFootstepTotalTime;
	[Header("角色瞄准时的脚步声间隔")]
	[SerializeField] private float aimingFootstepIntervalTime;
	private float aimingFootstepTotalTime;
	[Header("角色滑坡时的脚步声间隔")]
	public float slideFootstepIntervalTime;
	[HideInInspector] public float slideFootstepTotalTime;

	[Header("角色走路时的脚步声音量")]
	[SerializeField] private float walkFootstepVolume;
	[SerializeField] private float runFootstepVolume;
	[SerializeField] private float crouchFootstepVolume;
	[SerializeField] private float aimingFootstepVolume;
	[SerializeField] private float slideFootstepVolume;

	[Header("从斜坡上滑下来的脚步声")]
	[SerializeField] private AudioClip slideFootstepAudio;

	[Header("混凝土地板走路声")]
	[SerializeField] private List<AudioClip> walkFootStepAudioList_Concrete = new List<AudioClip>();
	[Header("混凝土地板跑步声")]
	[SerializeField] private List<AudioClip> runFootStepAudioList_Concrete = new List<AudioClip>();
	[Header("混凝土地板上楼梯声")]
	[SerializeField] private List<AudioClip> footStepAudioList_ConcreteStaircase = new List<AudioClip>();

	private GroundMaterial groundMaterial = GroundMaterial.Concrete;

	#endregion

	#region 基本生命周期函数

	private void OnEnable()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 地面类型检测

	/// <summary>
	/// 执行地面检测：脚步声播放功能
	/// </summary>
	public void GroundMaterialCheck()
	{
		RaycastHit hitObj;

		if (Physics.Raycast(groundCheckPoint.position, -groundCheckPoint.up, out hitObj, groundCheckRadius))
		{
			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.concreteStaircase))
			{
				groundMaterial = GroundMaterial.ConcreteStaircase;
				return;
			}
			else
			{
				groundMaterial = GroundMaterial.Concrete;
				return;
			}
		}

		groundMaterial = GroundMaterial.Concrete;
	}

	#endregion

	#region 执行脚步声音效播放

	/// <summary>
	/// 执行脚步声音效播放
	/// </summary>
	public void PerformFootstepSound()
	{
		if (playerController.moveVelocity <= 0.5) return; // 移动的时候才有脚步声

		if (playerController.playerIsSliding)
		{
			PlaySlideFootstepSound();
			return;
		}

		if (playerController.isAiming)
		{
			PlayAimingFootstepSound();
			return;
		}

		// 先判断蹲下，然后判断是不是在退后
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Crouch.phase == InputActionPhase.Performed)
		{
			if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveBack.phase == InputActionPhase.Performed)
			{
				PlayCrouchMoveBackFootstepSound();
			}

			if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveBack.phase == InputActionPhase.Waiting)
			{
				PlayCrouchFootstepSound();
			}
		}

		// 先判断是不是在退后，然后判断是不是在跑步
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Crouch.phase == InputActionPhase.Waiting)
		{
			if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveBack.phase == InputActionPhase.Performed)
			{
				PlayMoveBackFootstepSound();
			}

			if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveBack.phase == InputActionPhase.Waiting)
			{
				if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Run.phase == InputActionPhase.Performed)
				{
					if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveForward.phase == InputActionPhase.Performed)
					{
						PlayRunFootstepSound();
					}
	
					if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_MoveForward.phase == InputActionPhase.Waiting)
					{
						PlayWalkFootstepSound();
					}
				}

				if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Run.phase == InputActionPhase.Waiting)
				{
					PlayWalkFootstepSound();
				}
			}
		}
	}

	#endregion

	#region 脚步声音效播放功能

	// 走路音效
	private void PlayWalkFootstepSound()
	{
		walkFootstepTotalTime += Time.deltaTime;
		if (walkFootstepTotalTime > 32768f) walkFootstepTotalTime = 32768f;

		if (walkFootstepTotalTime > walkFootstepIntervalTime)
		{
			walkFootstepTotalTime = 0f;

			footstepAudioSource.volume = walkFootstepVolume;

			if (groundMaterial == GroundMaterial.Concrete)
			{
				int randomIndex = UnityEngine.Random.Range(0, walkFootStepAudioList_Concrete.Count);
				footstepAudioSource.clip = walkFootStepAudioList_Concrete[randomIndex];
			}
			if (groundMaterial == GroundMaterial.ConcreteStaircase)
			{
				int randomIndex = UnityEngine.Random.Range(0, footStepAudioList_ConcreteStaircase.Count);
				footstepAudioSource.clip = footStepAudioList_ConcreteStaircase[randomIndex];
			}

			footstepAudioSource.Play();
		}
	}

	// 跑步音效
	private void PlayRunFootstepSound()
	{
		runFootstepTotalTime += Time.deltaTime;
		if (runFootstepTotalTime > 32768f) runFootstepTotalTime = 32768f;

		if (runFootstepTotalTime > runFootstepIntervalTime)
		{
			runFootstepTotalTime = 0f;

			footstepAudioSource.volume = runFootstepVolume;

			if (groundMaterial == GroundMaterial.Concrete)
			{
				int randomIndex = UnityEngine.Random.Range(0, runFootStepAudioList_Concrete.Count);
				footstepAudioSource.clip = runFootStepAudioList_Concrete[randomIndex];
			}
			if (groundMaterial == GroundMaterial.ConcreteStaircase)
			{
				int randomIndex = UnityEngine.Random.Range(0, footStepAudioList_ConcreteStaircase.Count);
				footstepAudioSource.clip = footStepAudioList_ConcreteStaircase[randomIndex];
			}

			footstepAudioSource.Play();
		}
	}

	// 倒退走路音效
	private void PlayMoveBackFootstepSound()
	{
		moveBackFootstepTotalTime += Time.deltaTime;
		if (moveBackFootstepTotalTime > 32768f) moveBackFootstepTotalTime = 32768f;

		if (moveBackFootstepTotalTime > moveBackFootstepIntervalTime)
		{
			moveBackFootstepTotalTime = 0f;

			footstepAudioSource.volume = walkFootstepVolume;

			if (groundMaterial == GroundMaterial.Concrete)
			{
				int randomIndex = UnityEngine.Random.Range(0, walkFootStepAudioList_Concrete.Count);
				footstepAudioSource.clip = walkFootStepAudioList_Concrete[randomIndex];
			}
			if (groundMaterial == GroundMaterial.ConcreteStaircase)
			{
				int randomIndex = UnityEngine.Random.Range(0, footStepAudioList_ConcreteStaircase.Count);
				footstepAudioSource.clip = footStepAudioList_ConcreteStaircase[randomIndex];
			}

			footstepAudioSource.Play();
		}
	}

	// 下蹲走路音效
	private void PlayCrouchFootstepSound()
	{
		crouchFootstepTotalTime += Time.deltaTime;
		if (crouchFootstepTotalTime > 32768f) crouchFootstepTotalTime = 32768f;

		if (crouchFootstepTotalTime > crouchFootstepIntervalTime)
		{
			crouchFootstepTotalTime = 0f;

			footstepAudioSource.volume = crouchFootstepVolume;

			if (groundMaterial == GroundMaterial.Concrete)
			{
				int randomIndex = UnityEngine.Random.Range(0, walkFootStepAudioList_Concrete.Count);
				footstepAudioSource.clip = walkFootStepAudioList_Concrete[randomIndex];
			}
			if (groundMaterial == GroundMaterial.ConcreteStaircase)
			{
				int randomIndex = UnityEngine.Random.Range(0, footStepAudioList_ConcreteStaircase.Count);
				footstepAudioSource.clip = footStepAudioList_ConcreteStaircase[randomIndex];
			}

			footstepAudioSource.Play();
		}
	}

	// 下蹲倒退走路音效
	private void PlayCrouchMoveBackFootstepSound()
	{
		crouchBackFootstepTotalTime += Time.deltaTime;
		if (crouchBackFootstepTotalTime > 32768f) crouchBackFootstepTotalTime = 32768f;

		if (crouchBackFootstepTotalTime > crouchBackFootstepIntervalTime)
		{
			crouchBackFootstepTotalTime = 0f;

			footstepAudioSource.volume = crouchFootstepVolume;

			if (groundMaterial == GroundMaterial.Concrete)
			{
				int randomIndex = UnityEngine.Random.Range(0, walkFootStepAudioList_Concrete.Count);
				footstepAudioSource.clip = walkFootStepAudioList_Concrete[randomIndex];
			}
			if (groundMaterial == GroundMaterial.ConcreteStaircase)
			{
				int randomIndex = UnityEngine.Random.Range(0, footStepAudioList_ConcreteStaircase.Count);
				footstepAudioSource.clip = footStepAudioList_ConcreteStaircase[randomIndex];
			}

			footstepAudioSource.Play();
		}
	}

	// 瞄准走路音效
	private void PlayAimingFootstepSound()
	{
		aimingFootstepTotalTime += Time.deltaTime;
		if (aimingFootstepTotalTime > 32768f) aimingFootstepTotalTime = 32768f;

		if (aimingFootstepTotalTime >  aimingFootstepIntervalTime)
		{
			aimingFootstepTotalTime = 0f;

			footstepAudioSource.volume = aimingFootstepVolume;

			if (groundMaterial == GroundMaterial.Concrete)
			{
				int randomIndex = UnityEngine.Random.Range(0, walkFootStepAudioList_Concrete.Count);
				footstepAudioSource.clip = walkFootStepAudioList_Concrete[randomIndex];
			}
			if (groundMaterial == GroundMaterial.ConcreteStaircase)
			{
				int randomIndex = UnityEngine.Random.Range(0, footStepAudioList_ConcreteStaircase.Count);
				footstepAudioSource.clip = footStepAudioList_ConcreteStaircase[randomIndex];
			}

			footstepAudioSource.Play();
		}
	}

	// 角色滑斜坡音效
	private void PlaySlideFootstepSound()
	{
		slideFootstepTotalTime += Time.deltaTime;
		if (slideFootstepTotalTime > 32768f) slideFootstepTotalTime = 32768f;

		if (slideFootstepTotalTime > slideFootstepIntervalTime)
		{
			slideFootstepTotalTime = 0f;

			footstepAudioSource.volume = slideFootstepVolume;
			footstepAudioSource.clip = slideFootstepAudio;
			footstepAudioSource.Play();
		}
	}

	#endregion

}
