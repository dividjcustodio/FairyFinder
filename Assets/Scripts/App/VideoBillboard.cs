using UnityEngine;

[AddComponentMenu("Video Element/Video Billboard")]
public class VideoBillboard : MonoBehaviour
{

    #region vars

    private Transform videoPlane; //video plane
    private Camera cam; //cam ref

    #endregion

    #region init

    void Awake(){
        videoPlane = transform; //set to self 
        cam = Camera.main; //ref main cam
    }

    #endregion

    #region sm

    void LateUpdate(){
        transform.LookAt(videoPlane.position + cam.transform.rotation * Vector3.down, cam.transform.rotation * Vector3.back); //face cam (billboarding)
        //transform.localEulerAngles = new Vector3(90f, cam.transform.parent.localEulerAngles.y - 180f, 0); //face cam (billboarding + rot axis lock)
    }

    #endregion
}
