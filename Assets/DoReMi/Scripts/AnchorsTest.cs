using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnchorsTest : MonoBehaviour
{
    public OVRHand lHand;
    public OVRHand rHand;

    public GameObject anchorParent;
    public GameObject anchorPrefab;

    private void OnEnable()
    {
    }

    void Update()
    {        
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //get the pose of the controller in local tracking coordinates
            OVRPose objectPose = new OVRPose()
            {
                position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
                orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch)
            };

            //Convert it to world coordinates
            OVRPose worldObjectPose = OVRExtensions.ToWorldSpacePose(objectPose, Camera.main);

            Instantiate(anchorPrefab, worldObjectPose.position, worldObjectPose.orientation, anchorParent.transform).AddComponent<OVRSpatialAnchor>();
        }
    }
}
