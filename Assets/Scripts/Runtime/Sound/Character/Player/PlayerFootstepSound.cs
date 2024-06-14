using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// �Ų�����Ч����Ϸ��Ч
// ������ 3D ��Ч

public enum GroundMaterial // ���ö��û�� None�������⵽δ����ĵ�����ʣ�Ĭ�ϲ��Ż������Ų�����Ч
{
	Concrete, // ����������
	ConcreteStaircase, // ������̨��
	Brick, // ʯש
}

// TODO: δ���ĸ��£�����һ�������һ���µ� FSM ״̬�����Ϳ���Э�̿��ƻ�����Ч��������ϸ��

public class PlayerFootstepSound : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction; // ����ϵͳ

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;

	[Header("��ɫ�����⹦��")]
	[SerializeField] private Transform groundCheckPoint;
	[SerializeField] private float groundCheckRadius;

	[Header("�Ų�����Դ���")]
	[SerializeField] private AudioSource footstepAudioSource;

	[Header("��Ч���ŵ�ʱ����")]
	[Header("��ɫ��·ʱ�Ų����ļ��")]
	[SerializeField] private float walkFootstepIntervalTime;
	private float walkFootstepTotalTime;
	[Header("��ɫ�ܲ�ʱ�Ų����ļ��")]
	[SerializeField] private float runFootstepIntervalTime;
	private float runFootstepTotalTime;
	[Header("��ɫ����ʱ�Ų����ļ��")]
	[SerializeField] private float moveBackFootstepIntervalTime;
	private float moveBackFootstepTotalTime;
	[Header("��ɫ�¶�ʱ�ĽŲ������")]
	[SerializeField] private float crouchFootstepIntervalTime;
	private float crouchFootstepTotalTime;
	[Header("��ɫ�¶׺���ʱ�ĽŲ������")]
	[SerializeField] private float crouchBackFootstepIntervalTime;
	private float crouchBackFootstepTotalTime;
	[Header("��ɫ��׼ʱ�ĽŲ������")]
	[SerializeField] private float aimingFootstepIntervalTime;
	private float aimingFootstepTotalTime;
	[Header("��ɫ����ʱ�ĽŲ������")]
	public float slideFootstepIntervalTime;
	[HideInInspector] public float slideFootstepTotalTime;

	[Header("��ɫ��·ʱ�ĽŲ�������")]
	[SerializeField] private float walkFootstepVolume;
	[SerializeField] private float runFootstepVolume;
	[SerializeField] private float crouchFootstepVolume;
	[SerializeField] private float aimingFootstepVolume;
	[SerializeField] private float slideFootstepVolume;

	[Header("��б���ϻ������ĽŲ���")]
	[SerializeField] private AudioClip slideFootstepAudio;

	[Header("�������ذ���·��")]
	[SerializeField] private List<AudioClip> walkFootStepAudioList_Concrete = new List<AudioClip>();
	[Header("�������ذ��ܲ���")]
	[SerializeField] private List<AudioClip> runFootStepAudioList_Concrete = new List<AudioClip>();
	[Header("�������ذ���¥����")]
	[SerializeField] private List<AudioClip> footStepAudioList_ConcreteStaircase = new List<AudioClip>();

	private GroundMaterial groundMaterial = GroundMaterial.Concrete;

	#endregion

	#region �����������ں���

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

	#region �������ͼ��

	/// <summary>
	/// ִ�е����⣺�Ų������Ź���
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

	#region ִ�нŲ�����Ч����

	/// <summary>
	/// ִ�нŲ�����Ч����
	/// </summary>
	public void PerformFootstepSound()
	{
		if (playerController.moveVelocity <= 0.5) return; // �ƶ���ʱ����нŲ���

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

		// ���ж϶��£�Ȼ���ж��ǲ������˺�
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

		// ���ж��ǲ������˺�Ȼ���ж��ǲ������ܲ�
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

	#region �Ų�����Ч���Ź���

	// ��·��Ч
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

	// �ܲ���Ч
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

	// ������·��Ч
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

	// �¶���·��Ч
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

	// �¶׵�����·��Ч
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

	// ��׼��·��Ч
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

	// ��ɫ��б����Ч
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
