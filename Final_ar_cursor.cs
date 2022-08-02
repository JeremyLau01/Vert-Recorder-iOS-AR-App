using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI; // for test text

public class ARCursor : MonoBehaviour
{
    public GameObject cursorChildObject;

    public GameObject hoopObject;
    public GameObject rulerObject;

    public RecordToggle rT;

    // for switching current placed object
    public Button BtnRulerOn;
    public Button BtnHoopOn;



    public ARRaycastManager raycastManager;

    public Button stateButton; // for color differentiating

    public GameObject InstructionScreen; // for not tapping in instructions




    // trying to have the object point towards the viewer on place down
    public Transform target; // transform of the AR camera

    GameObject model = null; // temporary object so can place new hoops and destroy old ones


    // For old state before clicking different mode
    public Transform oldTarget; // old camera position
    Vector3 oldVector; // old model position


    // Start is called before the first frame update
    void Start()
    {
        BtnHoopOn.onClick.AddListener(ClickMode);
        BtnRulerOn.onClick.AddListener(ClickMode);
        //cursorChildObject.SetActive(true);
    }

    void ClickMode()
    {
        if (model != null)
        {
            Destroy(model); // destroy previous hoop, so that there will not be so many in one scene
            if (rT.BoolRulerMode)
            {
                model = GameObject.Instantiate(hoopObject, oldTarget.position, oldTarget.rotation); // how set reference to hoop as new game object
                                                                                                    // rotate on y axis towards user, using AR camera's position
            }
            else
            {
                model = GameObject.Instantiate(rulerObject, oldTarget.position, oldTarget.rotation); // how set reference to hoop as new game object
                                                                                                     // rotate on y axis towards user, using AR camera's position
            }
            model.transform.LookAt(oldVector);
        }
    }



    // Update is called once per frame
    void Update()
    {
        Vector4 stateButtonColor = stateButton.image.color;
        bool state = stateButtonColor[0].Equals((float)0.1);
        bool currentlyRecording = !state;

        //cursorChildObject.SetActive(currentlyRecording); // hide when recording


        UpdateCursor();

        

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (!InstructionScreen.activeSelf && currentlyRecording && touch.phase == TouchPhase.Began && touch.position.y > 200 && !(touch.position.y > 1200 && touch.position.x < 130) && !(touch.position.y > 1120 && touch.position.x > 535)) //screenPosThresh.y
            {
                Destroy(model); // destroy previous hoop, so that there will not be so many in one scene

                oldTarget.position = transform.position;
                oldTarget.rotation = transform.rotation;

                if (rT.BoolRulerMode)
                {
                    model = GameObject.Instantiate(hoopObject, transform.position, transform.rotation); // how set reference to hoop as new game object
                                                                                                          // rotate on y axis towards user, using AR camera's position
                }
                else
                {
                    model = GameObject.Instantiate(rulerObject, transform.position, transform.rotation); // how set reference to hoop as new game object
                                                                                                           // rotate on y axis towards user, using AR camera's position
                }
                oldVector = new Vector3(target.position.x, transform.position.y, target.position.z);
                model.transform.LookAt(oldVector);

                // save transform and target transforms to a separate variable, use for button press
            }

        }
    }

    void UpdateCursor()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
    }
}
