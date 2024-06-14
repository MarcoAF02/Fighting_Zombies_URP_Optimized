using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������������������Ч���ɻ���������
/// </summary>
public class HitDecalController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�ӵ�������Ϸ����")]
	[SerializeField] private GameObject bulletDecalPlaneObj;
	[Header("С��������Ϸ����")]
	[SerializeField] private GameObject kinfeDecalPlaneObj;

	[Header("��������ķŴ���")]
	[Tooltip("�޸���������Դ���⵼�µ�������С��Ϸ")]
	[Header("�ӵ�����")]
	[Header("���л�����Ĭ�ϷŴ���")]
	[SerializeField] private float bulletDefHitDecalMag = 0.7f;
	[Header("����ľͷ�����ķŴ���")]
	[SerializeField] private float bulletHitWoodDecalMag = 0.7f;
	[Header("���в��������ķŴ���")]
	[SerializeField] private float bulletHitGlassDecalMag = 4.5f;
	[Header("�������������ķŴ���")]
	[SerializeField] private float bulletHitDirtDecalMag = 0.7f;
	[Header("���е��������ķŴ���")]
	[SerializeField] private float bulletHitEnemyDecalMag = 2.5f;

	// TODO: ���С��������Ҳ�ǲ�ͬ��׼��������Դ����Ҳ��Ҫ���벻ͬ�ķŴ���

	[Header("�ӵ���������")]
	[Header("���л�����Ĭ�ϲ���")]
	[SerializeField] private Material bulletDefHitMat;
	[Header("����ľͷ�Ĳ���")]
	[SerializeField] private Material bulletHitWoodMat;
	[Header("���в����Ĳ���")]
	[SerializeField] private Material bulletHitGlassMat;
	[Header("���������Ĳ���")]
	[SerializeField] private Material bulletHitDirtMat;
	[Header("���е��˵Ĳ���")]
	[SerializeField] private Material bulletHitEnemyMat;
	[Header("С����������")]
	[Header("���л�����Ĭ�ϲ���")]
	[SerializeField] private Material kinfeDefHitMat;
	[Header("����ľͷ�Ĳ���")]
	[SerializeField] private Material kinfeHitWoodMat;
	[Header("���������Ĳ���")]
	[SerializeField] private Material kinfeHitDirtMat;
	[Header("���е��˵Ĳ���")]
	[SerializeField] private Material kinfeHitEnemyMat;
	
	// ע�⣺���޷��ڲ��������»��ۣ���Ϊ������Ӳ��Զ���ڸ���

	#endregion

	#region ��������

	/// <summary>
	/// ������������
	/// </summary>
	/// <param name="_hitState"></param>
	/// <param name="_targetObjTrans"></param>
	/// <param name="_decalSize"></param>
	public void ApplyHitDecal(HitState _hitState, Transform _targetObjTrans, float _playerRotateY, float _decalSize)
	{
		if (_hitState == HitState.None)
		{
			Debug.LogWarning("����û��ָ���������������");
		}

		// ==================== �ӵ� ==================== //

		if (_hitState == HitState.BulletDefaultEnvir)
		{
			GenerateBulletDecal(_targetObjTrans, _playerRotateY, bulletDefHitMat, _decalSize, bulletDefHitDecalMag);
		}

		if (_hitState == HitState.BulletHitWood)
		{
			GenerateBulletDecal(_targetObjTrans, _playerRotateY, bulletHitWoodMat, _decalSize, bulletHitWoodDecalMag);
		}

		if (_hitState == HitState.BulletHitGlass)
		{
			GenerateBulletDecal(_targetObjTrans, _playerRotateY, bulletHitGlassMat, _decalSize, bulletHitGlassDecalMag);
		}

		if (_hitState == HitState.BulletHitDirt)
		{
			GenerateBulletDecal(_targetObjTrans, _playerRotateY, bulletHitDirtMat, _decalSize, bulletHitDirtDecalMag);
		}

		if (_hitState == HitState.BulletEnemy)
		{
			GenerateBulletDecal(_targetObjTrans, _playerRotateY, bulletHitEnemyMat, _decalSize, bulletHitEnemyDecalMag);
		}

		// ==================== С�� ==================== //

		if (_hitState == HitState.KinfeDefaultEnvir)
		{
			GenerateKinfeDecal(_targetObjTrans, _playerRotateY, kinfeDefHitMat, _decalSize);
		}

		if (_hitState == HitState.KinfeHitWood)
		{
			GenerateKinfeDecal(_targetObjTrans, _playerRotateY, kinfeHitWoodMat, _decalSize);
		}

		if (_hitState == HitState.KinfeHitDirt)
		{
			GenerateKinfeDecal(_targetObjTrans, _playerRotateY, kinfeHitDirtMat, _decalSize);
		}

		if (_hitState == HitState.KinfeEnemy)
		{
			GenerateKinfeDecal(_targetObjTrans, _playerRotateY, kinfeHitEnemyMat, _decalSize);
		}
	}

	// �����ӵ�������
	private void GenerateBulletDecal(Transform _targetObjTrans, float _playerRotateY, Material _decalMat, float _decalSize, float _decalMag)
	{
		GameObject newBulletDecal = Instantiate(bulletDecalPlaneObj);
		newBulletDecal.transform.localScale = new Vector3(_decalSize * _decalMag, _decalSize * _decalMag, _decalSize * _decalMag); // �ӵ�������Ӧ���˷Ŵ��ʣ���ΪҪӦ�Բ�һ�µ�������Դ��
		newBulletDecal.transform.SetPositionAndRotation(transform.position, transform.rotation);
		newBulletDecal.transform.eulerAngles = new Vector3(newBulletDecal.transform.eulerAngles.x, _playerRotateY, newBulletDecal.transform.eulerAngles.z);
		newBulletDecal.transform.SetParent(_targetObjTrans);
		newBulletDecal.GetComponent<HitDecalObj>().decalMaterial = _decalMat;
	}

	// ����С��������
	private void GenerateKinfeDecal(Transform _targetObjTrans, float _playerRotateY, Material _decalMat, float _decalSize)
	{
		GameObject newKinfeDecal = Instantiate(kinfeDecalPlaneObj);
		newKinfeDecal.transform.localScale = new Vector3(_decalSize, _decalSize, _decalSize);
		newKinfeDecal.transform.SetPositionAndRotation(transform.position, transform.rotation);
		newKinfeDecal.transform.eulerAngles = new Vector3(newKinfeDecal.transform.eulerAngles.x, _playerRotateY, newKinfeDecal.transform.eulerAngles.z);
		newKinfeDecal.transform.SetParent(_targetObjTrans);
		newKinfeDecal.GetComponent<HitDecalObj>().decalMaterial = _decalMat;
	}

	#endregion
}
