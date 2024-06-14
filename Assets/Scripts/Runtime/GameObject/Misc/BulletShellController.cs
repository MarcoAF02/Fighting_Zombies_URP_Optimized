using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹壳控制器
/// </summary>
public class BulletShellController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("弹壳物体过多久消失")]
	[SerializeField] private float destroyBulletShellTime;

	[Header("弹壳旋转速度")]
	[Tooltip("子弹抛壳是转着飞出去的，这个参数定义了旋转速度")]
	[SerializeField] private float rotateSpeed;

	[Header("音效控制器")]
	[SerializeField] private BulletShellSound bulletShellSound;

	private bool isGrounded; // 弹壳是否掉地上了

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		Destroy(gameObject, destroyBulletShellTime); // 隔一段时间后删除自己
	}

	private void Update()
	{
		RotateBulletShell();
	}

	#endregion

	#region 弹壳飞行功能

	private void RotateBulletShell()
	{
		// if (isGrounded) return;
		transform.Rotate(new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));
	}

	#endregion

	#region 物理碰撞相关函数

	private void OnTriggerEnter(Collider other)
	{
		if (!isGrounded) // 防止落地音效反复播放
		{
			bulletShellSound.PlayPistolBulletShellSound(); // 播放弹壳落地音效
		}

		isGrounded = true;
	}

	#endregion
}
