using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{

    public GameObject mainCameraRig;
    public GameObject aimCameraRig;
    private bool firstClick;

    void Start()
    {
        mainCameraRig.SetActive(true);
        aimCameraRig.SetActive(false);
    }
    void Update()
    {
        //This will toggle the enabled state of the two cameras between true and false each time
        //if (OVRInput.Get(OVRInput.Button.Back))
        //{
        //    mainCamera.enabled = false;
        //    aimCamera.enabled = true;
        //}
        //else
        //{
        //    mainCamera.enabled = true;
        //    aimCamera.enabled = false;
        //}
        
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        { // The back button is pressed.
            if (!firstClick)
            { // The first click is detected, so toggle aim mode on.
                firstClick = true;
                mainCameraRig.SetActive(false);
                aimCameraRig.SetActive(true);
            }
            else
            { // The second click detected, so toggle aim mode off.
                firstClick = false;
                mainCameraRig.SetActive(true);
                aimCameraRig.SetActive(false);
            }
        }
    }
}