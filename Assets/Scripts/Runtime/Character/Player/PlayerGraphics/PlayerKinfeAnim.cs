using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小刀的动画控制器
/// </summary>
public class PlayerKinfeAnim : MonoBehaviour
{
	#region 基本组件和变量

	[Header("基本组件和变量")]
	[Header("小刀第一人称角色模型")]
	[SerializeField] private GameObject kinfeArm;
	[Header("小刀动画控制器")]
	[SerializeField] private Animator kinfeAnimator;
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
			kinfeAnimator.SetFloat("moveSpeed", 0f);
		}
		else
		{
			kinfeAnimator.SetFloat("moveSpeed", playerController.moveVelocity);
		}
	}

	public void PlayAttackAnim()
	{
		kinfeAnimator.SetTrigger("attack");
	}

	public void PlayHideAnim()
	{
		kinfeAnimator.SetTrigger("hide");
	}

	#endregion
}
