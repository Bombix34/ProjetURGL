using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FafaTools.Audio;
using FafaTools.Utils.Enum;

[CustomEditor(typeof(AudioDatabase))]
public class AudioDataBaseEditor : Editor
{
    private AudioDatabase m_Source;
    private string m_FilePath = "Assets/FafaTools/AudioEvent/";
    private string m_FileName="AudioFieldEnum";

    private void OnEnable()
    {
        m_Source = (AudioDatabase)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(25f);

        if (GUILayout.Button("SAVE"))
        {
            List<string> enumValues = new List<string>();
            foreach(var field in m_Source.m_Datas)
            {
                enumValues.Add(field.m_EnumFieldName);
            }
            EnumEditor.WriteToEnum(m_FilePath, m_FileName, enumValues);

            Debug.Log("File " + m_FileName +".cs created at " + m_FilePath);
        }
    }
}
