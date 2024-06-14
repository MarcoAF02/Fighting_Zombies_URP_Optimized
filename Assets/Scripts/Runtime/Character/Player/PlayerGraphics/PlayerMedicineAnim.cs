using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 医疗包动画控制器
/// </summary>
public class PlayerMedicineAnim : MonoBehaviour
{
	#region 基本组件和变量

	[Header("基本组件和变量")]
	[Header("医疗包第一人称模型")]
	[SerializeField] private GameObject syringeArm;
	[Header("医疗包动画控制器")]
	[SerializeField] private Animator syringeAnimator;
	[Header("角色控制器")]
	[SerializeField] private PlayerController playerController;

	#endregion

	#region 基本生命周期函数

	private void Update()
	{
		MoveAnimation();
	}

	#endregion

	#region 动画更新函数

	private void MoveAnimation()
	{
		if (playerController.playerIsSliding)
		{
			syringeAnimator.SetFloat("moveSpeed", 0f);
		}
		else
		{
			syringeAnimator.SetFloat("moveSpeed", playerController.moveVelocity);
		}
	}

	public void PlayUseAnim()
	{
		syringeAnimator.SetTrigger("use");
	}

	public void PlayHideAnim()
	{
		syringeAnimator.SetTrigger("hide");
	}

	#endregion
}
