using UnityEngine;
using UnityEngine.Video;

[AddComponentMenu("Video Element/Video Gesture")]
public class VideoGesture : MonoBehaviour {

    #region vars

    [Header("Parameters")]
    public float minScale = 7f;
    public float maxScale = 10f;

    [Header("References")]
    public VideoPlayer videoPlayer; //video player
    private Transform videoPlane; //video plane ref
    public Transform uiPanel;
    public GameObject dragIdentifier; 

    //private RaycastHit hit; 
    private bool drag; //drag identifier
    private bool clicked;

    private Vector3 dragOffset;
    private float dragDist;

    private float prevDist; //prev scale dist
    private float currDist; //prev scale dist
    private float scaleAdd = 0.75f; //scale add
    private float prevClick; 
    private float clickTime = 0.6f; //min click time
    private int clickCount = 0;

    #endregion

    #region init

    void Awake() {
        videoPlane = transform; //set to self 
        videoPlayer = videoPlane.GetChild(0).GetComponent<VideoPlayer>(); //get video player 
    }

    #endregion

    #region sm

    void Update() {
        //pinch scale
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(1).phase == TouchPhase.Began) {
            prevDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position); //begin 
        }
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) {
            currDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position); //end dist
            //prevDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).deltaPosition) - Vector2.Distance(Input.GetTouch(1).position, Input.GetTouch(1).deltaPosition); //prev dist

            if (currDist > prevDist) { //upscale
                float scale = videoPlane.localScale.x + scaleAdd > maxScale ? 0 : scaleAdd; //compute upscale (clamp to max scale)
                videoPlane.localScale += new Vector3(scale, scale, scale); //upscale video plane    
            } else if (prevDist > currDist) { //downscale
                float scale = videoPlane.localScale.x - scaleAdd < minScale ? 0 : -scaleAdd; //compute downscale (clamp to min scale)
                videoPlane.localScale += new Vector3(scale, scale, scale); //downscale video plane
            }
        }

        //click start / double click pause
        if (clickCount > 0 && Time.time - prevClick > clickTime) { //reset
            clickCount = 0;
            prevClick = 0;
        }

        if (Input.anyKeyDown && !isUIClicked(uiPanel.GetComponent<RectTransform>(), Input.GetTouch(0).position, Camera.main)) { //click (screen, not ui)
            clickCount++; 

            if (clickCount > 2) { //exceed clicks
                clickCount = 0; //reset
                prevClick = 0;
            } else if(clickCount == 2) { //two clicks - pause
                if (Time.time - prevClick < clickTime) { //valid click time
                    if (videoPlayer.isPlaying) { //playing
                        videoPlayer.Pause(); //pause
                    }
                }
                //reset
                clickCount = 0;
                prevClick = 0; 
            } else if(clickCount == 1) { //one click - play
                prevClick = Time.time; //store time

                if (!videoPlayer.isPlaying) { //not played yet
                    videoPlayer.Play(); //play
                }
            }
        }


        //move drag
        /*if (!drag && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); //convert screen to ray 

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 500f)) { //raycast to video plane
                if (hit.collider.CompareTag("Video")) { //video plane found
                    drag = true; //set drag identifier
                }
            }
        }
        if (drag && Input.GetTouch(0).phase == TouchPhase.Moved){ //dragging 
            //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); //convert screen to ray
            //videoPlane.transform.position = new Vector3(ray.direction.x, ray.direction.y, videoPlane.transform.position.z); //drag move video plane

            Vector3 direction = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, videoPlane.position.z);
            videoPlane.transform.position = Camera.main.ScreenToWorldPoint(direction);
        }

        //end
        if (drag && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)) {
            drag = false; //reset identifier
        }*/

        if (Input.touchCount != 1) {
            drag = false;
            dragIdentifier.SetActive(false);
            return;
        }

        Vector3 dragVector;
        Touch touch = Input.GetTouch(0);
        Vector3 touchPos = touch.position;

        if (touch.phase == TouchPhase.Began) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touchPos);

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Video")) {
                    dragDist = hit.transform.position.z - Camera.main.transform.position.z;
                    dragVector = new Vector3(touchPos.x, touchPos.y, dragDist);
                    dragVector = Camera.main.ScreenToWorldPoint(dragVector);
                    dragOffset = videoPlane.position - dragVector;
                    drag = true;
                }
            }
        }

        if (drag && touch.phase == TouchPhase.Moved) {
            dragVector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragDist);
            dragVector = Camera.main.ScreenToWorldPoint(dragVector);
            videoPlane.position = dragVector + dragOffset;
            dragIdentifier.SetActive(true);
        }
        if (drag && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) {
            drag = false;
            dragIdentifier.SetActive(false);
        }
    }

    #endregion

    #region methods

    private bool isUIClicked(RectTransform recT, Vector2 clickPos, Camera cam) {
        return RectTransformUtility.RectangleContainsScreenPoint(recT, clickPos, cam) ? true : false;
    }

    #endregion
}
