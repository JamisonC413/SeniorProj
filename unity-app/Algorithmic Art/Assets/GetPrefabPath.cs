using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetPrefabPath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("Custom/Get Prefab Path")]
    public static void GetPrefabPathMenuItem()
    {
        GameObject prefab = Resources.Load<GameObject>("drawBlock"); // Replace with the name of your prefab
        if (prefab != null)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            Debug.Log("Prefab path: " + path);
        }
        else
        {
            Debug.LogError("Prefab not found in Resources.");
        }
    }
}
