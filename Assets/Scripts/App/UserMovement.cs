using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour { //experimental!

    #region vars

    [Header("Parameters")]
    public float speed = 1f;

    public float sensitivityH = 1.2f;
    public float sensitivityV = 1.2f;
    public float smooth = 0.5f;

    private float axisH;
    private float axisV;

    private Vector3 zeroAccel;
    private Vector3 currentAccel;

    private Vector3 movePos;

    #endregion

    #region init

    void Start() {
        ResetAxes(); //reset
    }

    public void ResetAxes() {
        zeroAccel = Input.acceleration;
        currentAccel = Vector3.zero;
        movePos = Vector3.zero;
    }

    #endregion

    #region sm

    void Update() {
        currentAccel = Vector3.Lerp(currentAccel, Input.acceleration - zeroAccel, Time.deltaTime / smooth);
        axisH = Mathf.Clamp(currentAccel.x * sensitivityH, -1, 1);
        axisV = Mathf.Clamp(currentAccel.z * sensitivityV, -1, 1);

        movePos = new Vector3(-axisH, 0, 0); //get final move pos

        if(movePos.x > 0.500 || movePos.x < -0.500) {
            transform.Translate(movePos * speed); //move
        }
    }

    void OnGUI(){
        //test log
        GUI.Label(new Rect(10, 25, 200, 500), "X-Accel: " + movePos.x.ToString("F3"));
        GUI.Label(new Rect(100, 25, 200, 500), "Y-Accel: " + movePos.y.ToString("F3"));
        GUI.Label(new Rect(210, 25, 200, 500), "Z-Accel: " + movePos.z.ToString("F3"));
    }

    #endregion
}
