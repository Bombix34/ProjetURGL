using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            if(m_Instance==null)
            {
                Debug.LogError("Your singleton scriptable object is null !");
            }
            return m_Instance;
        }
    }

}
