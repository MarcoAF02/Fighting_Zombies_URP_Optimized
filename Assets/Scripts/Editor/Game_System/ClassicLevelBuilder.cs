using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// ����ؿ�������
/// </summary>
public class ClassicLevelBuilder
{
	#region �༭����չ���ܺ���

	[MenuItem("Level Builder/Fast Build/Classic Level")]
	private static void FastBuildClassicLevel() // �༭�����������Ǿ�̬��
	{
		Debug.Log("���ٹ��������ֹؿ�");

		// ������Ҽ��������������
		GameObject newCharacterRoot = new GameObject();
		newCharacterRoot.name = "====== Character ======";
		GameObject newPlayer = AssetDatabase.LoadAssetAtPath<GameObject>(ResourceAssetPath.characterPlayerLoadPath);
		newPlayer = GameObject.Instantiate(newPlayer);
		newPlayer.transform.SetParent(newCharacterRoot.transform);

		// ����������������
		GameObject newManagerParent = new GameObject();
		newManagerParent.name = "====== Manager ======";

		// �ֱ���ļ����м��ز�ʵ����������
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
