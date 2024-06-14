using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 暂不打算使用 PoolManager

// TODO: 归还对象池的脚本必须实现一个清空数据的接口（使用 Interface）
// TODO: 注意考题：对象池归还的对象，数据没有清理干净怎么处理？
// TODO: 未来的更新：将对象池系统做成万金油式的对象池系统（根据指定的名称获取对象）

/// <summary>
/// 对象池管理器
/// </summary>
[Tooltip("不同功能的物体分别有自己的对象池")]
public class PoolManager : MonoSingleton<PoolManager>
{
	#region 基本组件和变量

	// 注意：不同功能的物体分别有自己的对象池

	[Header("武器击中物体特效的预制件")]
	[SerializeField] private GameObject hitVFXPrefab;
	[Header("武器击中特效的对象池大小")]
	[SerializeField] private int hitVFXPoolStartSize;

	[Header("手枪子弹抛壳的预制件")]
	[SerializeField] private GameObject pistolBulletShellPrefab;
	[Header("手枪子弹壳抛壳的对象池大小")]
	[SerializeField] private int pistolBulletShellPoolStartSize;

	private Queue<GameObject> hitVFXPool = new Queue<GameObject>(); // 子弹特效物体队列
	private Queue<GameObject> pistolBulletShellPool = new Queue<GameObject>(); // 手枪子弹壳物体队列

	#endregion

	#region 基本生命周期函数

	protected override void Awake()
	{
		base.Awake();

		FillHitVFXPool();
		FillPistolBulletShellPool();
	}

	#endregion

	#region 对象池基本功能

	#region 击中物体的特效

	/// <summary>
	/// 初始化击中特效对象池
	/// </summary>
	private void FillHitVFXPool()
	{
		for (int i = 0; i < hitVFXPoolStartSize; i ++)
		{
			GameObject newVFX = Instantiate(hitVFXPrefab);
			newVFX.transform.parent = transform;

			ReturnToPool_VFX(newVFX); // 初始化对象后将其放入对象池
		}
	}

	/// <summary>
	/// 将击中特效返回对象池
	/// </summary>
	/// <param name="VFXObject"></param>
	public void ReturnToPool_VFX(GameObject VFXObject)
	{
		VFXObject.SetActive(false);

		VFXObject.transform.localScale = Vector3.one;
		VFXObject.transform.position = Vector3.zero;
		VFXObject.transform.rotation = Quaternion.identity;

		hitVFXPool.Enqueue(VFXObject);
	}

	/// <summary>
	/// 从对象池中取出击中特效物体
	/// </summary>
	/// <returns></returns>
	public GameObject GetFromPool_VFX(Vector3 objSize)
	{
		if (hitVFXPool.Count <= 0)
		{
			FillHitVFXPool(); // 如果对象池中的物体数量不够了，则在此重新填充
		}

		GameObject newVFX = hitVFXPool.Dequeue();
		newVFX.transform.localScale = objSize;
		newVFX.SetActive(true);
		return newVFX;
	}

	#endregion

	#region 子弹抛壳的物理物体

	/// <summary>
	/// 初始化手枪子弹抛壳对象池
	/// </summary>
	private void FillPistolBulletShellPool()
	{
		for (int i = 0; i < pistolBulletShellPoolStartSize; i ++)
		{
			GameObject newPistolBulletShell = Instantiate(pistolBulletShellPrefab);
			newPistolBulletShell.transform.parent = transform;

			ReturnToPool_PistolBulletShell(newPistolBulletShell); // 初始化对象后将其放入对象池
		}
	}

	/// <summary>
	/// 从对象池中取出物理弹壳
	/// </summary>
	/// <param name="objSize"></param>
	/// <returns></returns>
	public GameObject GetFromPool_PistolBulletShell(Vector3 objSize)
	{
		if (pistolBulletShellPool.Count <= 0)
		{
			FillPistolBulletShellPool();
		}

		GameObject newPistolBulletShell = pistolBulletShellPool.Dequeue();
		newPistolBulletShell.transform.localScale = objSize;
		newPistolBulletShell.gameObject.SetActive(true);

		return newPistolBulletShell;
	}

	/// <summary>
	/// 将物理弹壳归还至对象池
	/// </summary>
	/// <param name="pistolBulletShellObj"></param>
	public void ReturnToPool_PistolBulletShell(GameObject pistolBulletShellObj)
	{
		pistolBulletShellObj.SetActive(false);

		pistolBulletShellObj.transform.localScale = Vector3.one;
		pistolBulletShellObj.transform.position = Vector3.zero;
		pistolBulletShellObj.transform.rotation = Quaternion.identity;

		pistolBulletShellPool.Enqueue(pistolBulletShellObj);
	}

	#endregion

	#endregion

}
