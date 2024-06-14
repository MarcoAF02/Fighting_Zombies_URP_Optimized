using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 经典关卡创建器
/// </summary>
public class ClassicLevelBuilder
{
	#region 编辑器拓展功能函数

	[MenuItem("Level Builder/Fast Build/Classic Level")]
	private static void FastBuildClassicLevel() // 编辑器函数必须是静态的
	{
		Debug.Log("快速构建经典打怪关卡");

		// 创建玩家及其管理器根物体
		GameObject newCharacterRoot = new GameObject();
		newCharacterRoot.name = "====== Character ======";
		GameObject newPlayer = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.characterPlayerLoadPath);
		newPlayer = GameObject.Instantiate(newPlayer);
		newPlayer.transform.SetParent(newCharacterRoot.transform);

		// 创建管理器根物体
		GameObject newManagerParent = new GameObject();
		newManagerParent.name = "====== Manager ======";

		// 分别从文件夹中加载并实例化管理器
		GameObject newPublicSettingsLoader = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.publicSettingsLoaderPath);
		newPublicSettingsLoader = GameObject.Instantiate(newPublicSettingsLoader);
		newPublicSettingsLoader.transform.SetParent(newManagerParent.transform);

		GameObject newEventHandler_gameManager = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.eventHandler_gameManagerPath);
		newEventHandler_gameManager = GameObject.Instantiate(newEventHandler_gameManager);
		newEventHandler_gameManager.transform.SetParent(newManagerParent.transform);

		GameObject newGameProgressManager = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.gameProgressManagerPath);
		newGameProgressManager = GameObject.Instantiate(newGameProgressManager);
		newGameProgressManager.transform.SetParent(newManagerParent.transform);

		GameObject newBGMManager = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.bgmManager_gamePlayPath);
		newBGMManager = GameObject.Instantiate(newBGMManager);
		newBGMManager.transform.SetParent(newManagerParent.transform);

		GameObject newEnvironmentManager = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.environmentManagerPath);
		newEnvironmentManager = GameObject.Instantiate(newEnvironmentManager);
		newEnvironmentManager.transform.SetParent(newManagerParent.transform);

		GameObject newEnemyPatrolPointManager = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.enemyPatrolPointManagerPath);
		newEnemyPatrolPointManager = GameObject.Instantiate(newEnemyPatrolPointManager);
		newEnemyPatrolPointManager.transform.SetParent(newManagerParent.transform);

		GameObject newGameCompleteManager = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.gameCompleteManagerPath);
		newEnemyPatrolPointManager = GameObject.Instantiate(newGameCompleteManager);
		newEnemyPatrolPointManager.transform.SetParent(newManagerParent.transform);
	}

	#endregion
}
