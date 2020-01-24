using UnityEngine;
using UnityEditor;

namespace Assets.Editor.Menu { 
public class CreateObject
{
    [MenuItem("GameObject/Overlay AR/AR Device Cam")]
    private static void CreateARDeviceCam()
    {
        GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Camera/Main Camera.prefab", typeof(GameObject)); //get go
        PrefabUtility.InstantiatePrefab(go as GameObject); //instantiate
    }

    [MenuItem("GameObject/Overlay AR/BG Device Cam")]
    static void CreateBGCam()
    {
        GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Camera/BgCamera.prefab", typeof(GameObject)); //get go
        PrefabUtility.InstantiatePrefab(go as GameObject); //instantiate
    }

    [MenuItem("GameObject/Overlay AR/Video Element")]
    static void CreateVideoElement()
    {
        GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Objects/VideoPlane.prefab", typeof(GameObject)); //get go
        PrefabUtility.InstantiatePrefab(go as GameObject); //instantiate
    }
}

}
