using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Helper/Background Scaler")]
public class BackgroundScaler : MonoBehaviour {

    #region vars

    [Header("References")]
    public Camera cam;

    #endregion

    #region init

    void Awake() {
        transform.localScale = Vector3.one; //preset

        float height = cam.orthographicSize * 2f; //compute height
        float width = height * Screen.width / Screen.height; //compute width

        transform.localScale = new Vector3(width, height, 0.1f); //scale based on screen size
    }

    #endregion
}
