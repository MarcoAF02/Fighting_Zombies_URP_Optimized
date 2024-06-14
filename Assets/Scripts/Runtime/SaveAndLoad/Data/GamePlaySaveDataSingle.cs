using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GamePlaySaveDataList
{
	#region �ؿ���ɼ�¼�б�

	public List<GamePlaySaveDataSingle> gamePlaySaveDataList = new List<GamePlaySaveDataSingle>();

	#endregion
}

[Serializable]
public class GamePlaySaveDataSingle
{
	#region �ؿ���ɼ�¼��������¼��

	// �ؿ�����
	public string sceneName;

	// �ؿ����ʱ��
	public float useMinute;
	public float useSecond;

	#endregion
}
