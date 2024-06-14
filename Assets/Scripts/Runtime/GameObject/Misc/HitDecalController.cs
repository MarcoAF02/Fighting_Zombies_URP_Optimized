using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贴花生成器（击中特效生成击中贴花）
/// </summary>
public class HitDecalController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("子弹贴花游戏物体")]
	[SerializeField] private GameObject bulletDecalPlaneObj;
	[Header("小刀贴花游戏物体")]
	[SerializeField] private GameObject kinfeDecalPlaneObj;

	[Header("贴花物体的放大倍率")]
	[Tooltip("修复因美术资源问题导致的贴花大小出戏")]
	[Header("子弹贴花")]
	[Header("击中环境的默认放大倍率")]
	[SerializeField] private float bulletDefHitDecalMag = 0.7f;
	[Header("击中木头贴花的放大倍率")]
	[SerializeField] private float bulletHitWoodDecalMag = 0.7f;
	[Header("击中玻璃贴花的放大倍率")]
	[SerializeField] private float bulletHitGlassDecalMag = 4.5f;
	[Header("击中泥土贴花的放大倍率")]
	[SerializeField] private float bulletHitDirtDecalMag = 0.7f;
	[Header("击中敌人贴花的放大倍率")]
	[SerializeField] private float bulletHitEnemyDecalMag = 2.5f;

	// TODO: 如果小刀的贴花也是不同标准的美术资源，那也需要加入不同的放大倍率

	[Header("子弹贴花材质")]
	[Header("击中环境的默认材质")]
	[SerializeField] private Material bulletDefHitMat;
	[Header("击中木头的材质")]
	[SerializeField] private Material bulletHitWoodMat;
	[Header("击中玻璃的材质")]
	[SerializeField] private Material bulletHitGlassMat;
	[Header("击中泥土的材质")]
	[SerializeField] private Material bulletHitDirtMat;
	[Header("击中敌人的材质")]
	[SerializeField] private Material bulletHitEnemyMat;
	[Header("小刀贴花材质")]
	[Header("击中环境的默认材质")]
	[SerializeField] private Material kinfeDefHitMat;
	[Header("击中木头的材质")]
	[SerializeField] private Material kinfeHitWoodMat;
	[Header("击中泥土的材质")]
	[SerializeField] private Material kinfeHitDirtMat;
	[Header("击中敌人的材质")]
	[SerializeField] private Material kinfeHitEnemyMat;
	
	// 注意：刀无法在玻璃上留下划痕，因为玻璃的硬度远大于钢铁

	#endregion

	#region 贴花功能

	/// <summary>
	/// 创建击中贴花
	/// </summary>
	/// <param name="_hitState"></param>
	/// <param name="_targetObjTrans"></param>
	/// <param name="_decalSize"></param>
	public void ApplyHitDecal(HitState _hitState, Transform _targetObjTrans, float _playerRotateY, float _decalSize)
	{
		if (_hitState == HitState.None)
		{
			Debug.LogWarning("错误，没有指定击中物体的类型");
		}

		// ==================== 子弹 ==================== //

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

		// ==================== 小刀 ==================== //

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

	// 生成子弹的贴花
	private void GenerateBulletDecal(Transform _targetObjTrans, float _playerRotateY, Material _decalMat, float _decalSize, float _decalMag)
	{
		GameObject newBulletDecal = Instantiate(bulletDecalPlaneObj);
		newBulletDecal.transform.localScale = new Vector3(_decalSize * _decalMag, _decalSize * _decalMag, _decalSize * _decalMag); // 子弹的贴花应用了放大倍率（因为要应对不一致的美术资源）
		newBulletDecal.transform.SetPositionAndRotation(transform.position, transform.rotation);
		newBulletDecal.transform.eulerAngles = new Vector3(newBulletDecal.transform.eulerAngles.x, _playerRotateY, newBulletDecal.transform.eulerAngles.z);
		newBulletDecal.transform.SetParent(_targetObjTrans);
		newBulletDecal.GetComponent<HitDecalObj>().decalMaterial = _decalMat;
	}

	// 生成小刀的贴花
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
