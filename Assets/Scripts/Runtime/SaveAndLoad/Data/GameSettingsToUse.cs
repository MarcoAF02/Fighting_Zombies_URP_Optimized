using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettingsToUse
{
	#region ����ʹ�ü�¼

	public string _settingName; // ���汾���õ����ƣ������û������޸ģ�
	public int _settingListIndex; // �����������Ѱ�������б��ж�Ӧ��ѡ��

	#endregion
}

[System.Serializable]
public class GameSettingsToUseList
{
	#region ���ô����¼

	// ��������������Ϣ���б�
	public List<GameSettingsToUse> gameSettingsSaveList;

	#endregion
}
