using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[AddComponentMenu("AR/AR Device Cam")]
public class ARDeviceCam : MonoBehaviour
{

    #region vars

    [Header("Parameters")]
    public float maxVideoDistance = 200f;
    private float videoDistance = 50f;
    private float heightDistance = 3.3f;
    public float maxHeightDistance = 300f;

    [Header("References")]
    public GameObject camPlane;
    public GameObject videoPlane;
    private GameObject camParent;

    public Slider heightDistanceSlider;
    public Slider videoDistanceSlider;
    public Text gyroWarning;

    public GUISkin skin;

    private Camera cam;

    #endregion

    #region init

    void Start()
    {
        cam = Camera.main; //get main cam

        videoPlane.transform.position = new Vector3(videoPlane.transform.position.x, videoPlane.transform.position.y, videoDistance); //cam.transform.position + cam.transform.forward * videoDistance; //set video distance
         
        camParent = new GameObject("CamParent"); //create parent
        camParent.transform.position = transform.position; //set parent pos
        transform.SetParent(camParent.transform, true); //parent 
//        camParent.transform.Rotate(Vector3.right, 0f); //preset rot
        //camParent.AddComponent<UserMovement>(); //add user move script

        if (Input.isGyroAvailable) {
            Input.gyro.enabled = true; //toggle gyro
        }

        InitUI(); 

        WebCamTexture camTexture = new WebCamTexture(); //create new cam texture
        camPlane.GetComponent<Renderer>().material.mainTexture = camTexture; //set to device cam texture
        camTexture.Play(); //play device cam

        VideoPlayer vp = videoPlane.transform.GetChild(0).GetComponent<VideoPlayer>(); //get video player
        vp.Play(); //play video
    }

    private void InitUI() {
        heightDistanceSlider.minValue = heightDistance;
        heightDistanceSlider.maxValue = maxHeightDistance;

        videoDistanceSlider.minValue = videoDistance;
        videoDistanceSlider.maxValue = maxVideoDistance;

        gyroWarning.enabled = !Input.isGyroAvailable? true : false;
    }

    #endregion

    #region sm

    void Update() {
        Quaternion stableRot = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w); //rot fix
        transform.localRotation = stableRot; //Quaternion.Lerp(transform.localRotation, stableRot, 5f * Time.deltaTime); //set rot fix

        if (heightDistance != camParent.transform.position.y) { //update height
            Vector3 newPos = camParent.transform.position;
            newPos.y = heightDistance; //set
            camParent.transform.position = newPos; //update pos
        }

        UpdateUI(); //update ui
    }

    /*void OnGUI() {
        GUI.skin = skin; //set custom skin

        Color defColor = GUI.color; //get default color

        GUI.color = ConvertHexToColor("#4c4c4c"); //set area color (dark)
        GUILayout.BeginArea(new Rect(25, 25, Screen.width / 4, Screen.height / 2)); //pos area
        GUI.color = defColor; //reset color
       
        GUILayout.BeginVertical("Box", GUILayout.MaxHeight(Screen.height / 3));

        //alert
        if (!Input.isGyroAvailable) {
            GUILayout.Label("Gyro Not Found!");
        }

        //cam height
        GUILayout.Label("Calibrate Height");
        heightDistance = GUILayout.HorizontalSlider(heightDistance, 3f, maxHeightCalibration); //height slider

        //video distance
        GUILayout.Label("Calibrate Video Distance");
        videoDistance = GUILayout.HorizontalSlider(videoDistance, 50f, maxVideoDistance); //video distance slider

        GUILayout.Space(10);

        //re-position video
        bool moveClick = GUILayout.Button("Move Video"); 
        if(moveClick){ //reposition video
            videoPlane.transform.position = transform.position + transform.forward * videoDistance; //move position
            videoPlane.transform.rotation = transform.rotation; //move rotation
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/

    #endregion

    #region methods

    private void UpdateUI() {
        heightDistance = heightDistanceSlider.value;
        videoDistance = videoDistanceSlider.value;
    }

    public void MoveVideo() {
        videoPlane.transform.position = transform.position + transform.forward * videoDistance; //move position
        videoPlane.transform.rotation = transform.rotation; //move rotation
    }

    private Color ConvertHexToColor(string hex) { //get color from hex value
        Color c = new Color();
        ColorUtility.TryParseHtmlString(hex, out c);
        return c;
    }

    #endregion
}
