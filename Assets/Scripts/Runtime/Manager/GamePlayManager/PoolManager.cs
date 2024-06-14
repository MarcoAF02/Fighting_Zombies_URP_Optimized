using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: �ݲ�����ʹ�� PoolManager

// TODO: �黹����صĽű�����ʵ��һ��������ݵĽӿڣ�ʹ�� Interface��
// TODO: ע�⿼�⣺����ع黹�Ķ�������û������ɾ���ô����
// TODO: δ���ĸ��£��������ϵͳ���������ʽ�Ķ����ϵͳ������ָ�������ƻ�ȡ����

/// <summary>
/// ����ع�����
/// </summary>
[Tooltip("��ͬ���ܵ�����ֱ����Լ��Ķ����")]
public class PoolManager : MonoSingleton<PoolManager>
{
	#region ��������ͱ���

	// ע�⣺��ͬ���ܵ�����ֱ����Լ��Ķ����

	[Header("��������������Ч��Ԥ�Ƽ�")]
	[SerializeField] private GameObject hitVFXPrefab;
	[Header("����������Ч�Ķ���ش�С")]
	[SerializeField] private int hitVFXPoolStartSize;

	[Header("��ǹ�ӵ��׿ǵ�Ԥ�Ƽ�")]
	[SerializeField] private GameObject pistolBulletShellPrefab;
	[Header("��ǹ�ӵ����׿ǵĶ���ش�С")]
	[SerializeField] private int pistolBulletShellPoolStartSize;

	private Queue<GameObject> hitVFXPool = new Queue<GameObject>(); // �ӵ���Ч�������
	private Queue<GameObject> pistolBulletShellPool = new Queue<GameObject>(); // ��ǹ�ӵ����������

	#endregion

	#region �����������ں���

	protected override void Awake()
	{
		base.Awake();

		FillHitVFXPool();
		FillPistolBulletShellPool();
	}

	#endregion

	#region ����ػ�������

	#region �����������Ч

	/// <summary>
	/// ��ʼ��������Ч�����
	/// </summary>
	private void FillHitVFXPool()
	{
		for (int i = 0; i < hitVFXPoolStartSize; i ++)
		{
			GameObject newVFX = Instantiate(hitVFXPrefab);
			newVFX.transform.parent = transform;

			ReturnToPool_VFX(newVFX); // ��ʼ����������������
		}
	}

	/// <summary>
	/// ��������Ч���ض����
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
	/// �Ӷ������ȡ��������Ч����
	/// </summary>
	/// <returns></returns>
	public GameObject GetFromPool_VFX(Vector3 objSize)
	{
		if (hitVFXPool.Count <= 0)
		{
			FillHitVFXPool(); // ���������е��������������ˣ����ڴ��������
		}

		GameObject newVFX = hitVFXPool.Dequeue();
		newVFX.transform.localScale = objSize;
		newVFX.SetActive(true);
		return newVFX;
	}

	#endregion

	#region �ӵ��׿ǵ���������

	/// <summary>
	/// ��ʼ����ǹ�ӵ��׿Ƕ����
	/// </summary>
	private void FillPistolBulletShellPool()
	{
		for (int i = 0; i < pistolBulletShellPoolStartSize; i ++)
		{
			GameObject newPistolBulletShell = Instantiate(pistolBulletShellPrefab);
			newPistolBulletShell.transform.parent = transform;

			ReturnToPool_PistolBulletShell(newPistolBulletShell); // ��ʼ����������������
		}
	}

	/// <summary>
	/// �Ӷ������ȡ��������
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
	/// �������ǹ黹�������
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
