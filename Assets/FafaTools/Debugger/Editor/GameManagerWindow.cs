using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManagerWindow : EditorWindow
{

    // Add menu item named "My Window" to the Window menu
    [MenuItem("FafaStudio/GameManager")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one. 
        EditorWindow.GetWindow(typeof(GameManagerWindow));
    }

    Texture circle;
    public delegate bool getVar();
    List<getVar> observedVar = new List<getVar>{};
    List<bool> previousObservedVar = new List<bool>();
    List<bool> observedVarPause = new List<bool>();
    void OnEnable()
    {        
        circle = AssetDatabase.LoadAssetAtPath("Assets/ScoreLib/Sprites/Voyant.png", typeof(Texture)) as Texture;
        
        //observedVar.Add(delegate{return instance.mavar;});

        for (int i = 0; i < observedVar.Count; i++)
        {
            previousObservedVar.Add(observedVar[i]());
            observedVarPause.Add(false);
        }
     }

    void Update()
    {
        Repaint();
    }

    void OnGUI()
    {
        EditorGUIUtility.labelWidth = 100;
        
        EditorGUILayout.BeginVertical();

        //displayVar("VarName",0);
        EditorGUILayout.EndVertical();
    }

    private void displayVar(string label, int index){
        bool state = observedVar[index]();
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(110));
            setColor(state);
            Rect rectTexture = new Rect(new Vector2(115, index * 18 + 2), new Vector2(15, 15));
            GUI.DrawTexture(rectTexture, circle, ScaleMode.ScaleToFit, true);//52 est parfait je suppose 50+1+1 donc height+margehaute+margebasse
            GUI.color = Color.white;
            GUILayout.Space(20);
            observedVarPause[index] = EditorGUILayout.Toggle("pauseOnChange", observedVarPause[index]);
            if (observedVarPause[index] && state != previousObservedVar[index])
            {
                Debug.Break();
            }

            previousObservedVar[index] = state;
        EditorGUILayout.EndHorizontal();
    }

    private void setColor(bool boolean)
    {
        if (boolean)
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.red;
        }
    }
}
