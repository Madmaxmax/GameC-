using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static T instance
    {
        get
        {
            if (_instance == null) 
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject rootObject = new GameObject("RootObject");
                    _instance = rootObject.AddComponent<T>();
                    DontDestroyOnLoad(rootObject);
                }
            }
            else if (_instance != FindObjectOfType<T>())
            {
                Destroy(FindObjectOfType<T>());
            }
            return _instance;
        }
    }

}
