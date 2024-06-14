using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 收集通用管理类的文件路径
/// </summary>
[Serializable]
public static class ResourceAssetPath
{
	// 角色加载路径
	public static string characterPlayerLoadPath = "Assets/Prefabs/Character/Player/Player.prefab";

	public static string publicSettingsLoaderPath = "Assets/Prefabs/Manager/PublicSettingsLoader.prefab";
	public static string eventHandler_gameManagerPath = "Assets/Prefabs/Manager/EventHandler_GameManager.prefab";
	public static string gameProgressManagerPath = "Assets/Prefabs/Manager/GameProgressManager.prefab";
	public static string bgmManager_gamePlayPath = "Assets/Prefabs/Manager/BGMManager_GamePlay.prefab";
	public static string environmentManagerPath = "Assets/Prefabs/Manager/EnvironmentManager.prefab";
	public static string enemyPatrolPointManagerPath = "Assets/Prefabs/Manager/EnemyPatrolPointManager.prefab";
	public static string gameCompleteManagerPath = "Assets/Prefabs/Manager/GameCompleteManager.prefab";
}
