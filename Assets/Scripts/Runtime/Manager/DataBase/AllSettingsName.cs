using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������õ�����
/// </summary>
[Serializable]
public class AllSettingsName
{
	[Header("��������ѡ�������")]

	[Header("��Ϸ����")]
	[Header("���������")]
	public string mouseSensitivity = "Mouse_Sensitivity";
	[Header("׼��")]
	public string frontSight = "Front_Sight";
	[Header("��Ұ�Ƕ�")]
	public string fieldOfView = "Field_Of_View";
	[Header("��Ļ������ʾ")]
	public string screenInjuryWarning = "Screen_Injury_Warning";
	[Header("�ӽ�ҡ��")]
	public string shakingPerspective = "Shaking_Perspective";
	[Header("��ʾ�̳���Ϣ")]
	public string displayTutorialInformation = "Display_Tutorial_Information";

	[Header("ͼ������")]
	[Header("��ʾģʽ")]
	public string displayMode = "Display_Mode";
	[Header("�ֱ���")]
	public string resolutionRatio = "Resolution_Ratio";
	[Header("��Ⱦ����")]
	public string renderScale = "Render_Scale";
	[Header("���֡��")]
	public string maximumFrameRate = "Maximum_Frame_Rate";
	[Header("��ֱͬ��")]
	public string vSync = "VSync";
	[Header("���ڴ������")]
	public string postProcessingAntiAliasing = "Post_Processing_Anti_Aliasing";

	[Header("��Ӱ����")]
	public string shadowQuality = "Shadow_Quality";

	[Obsolete("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����")]
	[Header("ƽ�й���Ӱ����")]
	public string directionalLightShadowQuality = "Directional_Light_Shadow_Quality";

	[Obsolete("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����")]
	[Header("��������Ӱ����")]
	public string sceneLightShadowQuality = "Scene_Light_Shadow_Quality";

	[Obsolete("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����")]
	[Header("��Ӱ��Ⱦ����")]
	public string shadowRenderDistance = "Shadow_Render_Distance";

	[Header("��������")]
	public string textureQuality = "Texture_Quality";
	[Header("�������ڱ�")]
	public string ambientOcclusion = "Ambient_Occlusion";
	[Header("�������")]
	public string textureFiltering = "Texture_Filtering";

	[Header("��Ƶ����")]
	[Header("ȫ������")]
	public string globalVolume = "Global_Volume";
	[Header("��������")]
	public string bgmVolume = "Music_Volume";
	[Header("��Ч����")]
	public string sfxVolume = "SFX_Volume";
	[Header("ϵͳ����")]
	public string systemSFXVolume = "System_SFX_Volume";

}
