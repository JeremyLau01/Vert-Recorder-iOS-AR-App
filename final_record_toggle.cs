using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using System;

//using VideoCreator;

//[RequireComponent(typeof(AudioSource))]
public class RecordToggle : MonoBehaviour
{
    // For detecting click
    public Button StartRecord;
    public Button StopRecord;
    public Button ViewRecord;
    public Button SaveRecord; //

    // Asset button with watermark - off to the side
    public Button AssetStartRec;
    public Button AssetStopRec;
    public Button AssetViewRec;
    public Button AssetSaveRec;

    // For SetActive (can only do on game objects)
    public GameObject StartR;
    public GameObject StopR;
    public GameObject ViewR;
    public GameObject SaveR;

    // Keeps track of current states of buttons
    bool StartRecActive = true;
    bool StopRecActive = false;

    // Custom view button, save recording will have same behavior as view recording button
    bool ViewableRecording = false;
    bool ViewableSave = false;


    // For Instruction Screen
    public GameObject ObjQ;
    public Button BtnQ;
    bool BoolQ = false;

    public GameObject Instructions;
    public Button BtnGotIt;
    bool InstructionsOn = true;

    // For Modes
    public GameObject ObjHoopMode;
    public Button BtnHoopMode;
    public bool BoolHoopMode = false; // Start off both off

    public GameObject ObjRulerMode;
    public Button BtnRulerMode;
    public bool BoolRulerMode = false; // Start off both off

    bool RulerOn = true; // for tracking last button state before recording



    public GameObject BlueTarget;


    // FOR DISTINGUISHING WHICH BUTTON WAS PRESSED
    public GameObject stateGameobject;
    /// <summary>
	/// /////////
	/// </summary>


    // Start is called before the first frame update
    void Start()
    {
        //cachePath = Application.persistentDataPath + "/video.mov"; // maybe need to change this to .mpeg

        StartRecord.onClick.AddListener(ClickButton);
        StopRecord.onClick.AddListener(ClickButton);
        ViewRecord.onClick.AddListener(ViewRecording);
        SaveRecord.onClick.AddListener(SaveRecording); // copy view record

        // For Instruction UI
        BtnQ.onClick.AddListener(ClickHelp);
        BtnGotIt.onClick.AddListener(ClickGotIt);


        // For Modes
        BtnHoopMode.onClick.AddListener(ClickHoop);
        BtnRulerMode.onClick.AddListener(ClickRuler);

        UpdateButtons();
    }

    void ClickHoop()
    {
        BoolHoopMode = false;
        BoolRulerMode = true;
        UpdateButtons();
    }

    void ClickRuler()
    {
        BoolHoopMode = true;
        BoolRulerMode = false;
        UpdateButtons();
    }


    void ClickHelp()
    {
        BoolQ = false;
        InstructionsOn = true;

        SaveModeState();
        BoolHoopMode = false;
        BoolRulerMode = false;

        UpdateButtons();
    }

    void ClickGotIt()
    {
        BoolQ = true;
        InstructionsOn = false;

        BoolRulerMode = RulerOn;
        BoolHoopMode = !RulerOn;

        UpdateButtons();
    }

    void SaveModeState()
    {
        if (BoolRulerMode)
        {
            RulerOn = true;
        }
        else
        {
            RulerOn = false;
        }
    }


    public void SetStateColor(double colorVal)
    {


        stateGameobject.GetComponent<Image>().color = new Color((float)colorVal, 0, 0, 1);

    }


    void ClickButton()
    {
        StartRecActive = !StartRecActive;
        StopRecActive = !StopRecActive;
        // Click asset buttons according to the current boolean values
        if (StopRecActive)
        {
            SetStateColor(0.1); // Argument for don't do anything --> simple recorder . cs
            AssetStartRec.onClick.Invoke();
            ViewableRecording = false;
            ViewableSave = false;

            BoolQ = false;
            InstructionsOn = false;

            // for returning to the same state after recording
            SaveModeState();

            // when recording, set visibility off
            BoolHoopMode = false;
            BoolRulerMode = false;
        }
        else
        {
            SetStateColor(0.2); // Immediately save to camera roll --> simple recorder . cs
            AssetSaveRec.onClick.Invoke(); // Immediately save
            AssetStopRec.onClick.Invoke();
            ViewableRecording = true;
            ViewableSave = true;

            BoolQ = true;

            // Return to previous mode
            BoolRulerMode = RulerOn;
            BoolHoopMode = !RulerOn;
        }
        UpdateButtons();
    }

    void ViewRecording()
    {
        SetStateColor(0.3); // View Recording --> simple recorder . cs
        AssetViewRec.onClick.Invoke();
    }

    
    void SaveRecording() // Really should be share recording --> change names at the end
    {
        SetStateColor(0.4); // Share Recording --> simple recorder . cs
        AssetSaveRec.onClick.Invoke();

    }
    

    void UpdateButtons()
    {
        StartR.SetActive(StartRecActive);
        StopR.SetActive(StopRecActive);
        ViewR.SetActive(ViewableRecording);
        SaveR.SetActive(ViewableSave);

        ObjQ.SetActive(BoolQ);
        Instructions.SetActive(InstructionsOn);

        ObjHoopMode.SetActive(BoolHoopMode);
        ObjRulerMode.SetActive(BoolRulerMode);

        BlueTarget.SetActive(BoolQ); //redundancy when recording is going on

    }

    // Update is called once per frame - from medium
    void Update()
    {
        
    }

}
