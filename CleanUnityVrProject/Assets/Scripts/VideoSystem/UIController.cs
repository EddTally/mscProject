﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class UIController : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean menuHiderAction;

    public LaserRaycast laserRaycast;
    public GameObject videoUI;
    public Collider leftVideoCube, rightVideoCube, playCol, ffCol;

    //Variables for Delays
    int count = 0;



    // Update is called once per frame
    void Update()
    {
        //Basically counting updates for delays on actions, disgusting but every other method was too bothersome
        if (count > 100)
        {
            count--;
        }
        else
        {



            Collider col = null;
            col = this.GetComponent<LaserRaycast>().GetColliderHit();
            if (col != null) //Don't have to go through the whole loop if its null, saves time
            {
                //If grabAction from SteamVR and Raycast Collider hits left or right screen, set col obj as col
                /* if (grabAction.GetState(handType) && (col == leftVideoCube || col == rightVideoCube))
                 {
                     this.GetComponent<ControllerGrabObject>().SetCollidingObject(col);
                     //Debug.Log(this.GetComponent<ControllerGrabObject>().getCollidingObject());
                     this.GetComponent<ControllerGrabObject>().GrabObject();
                 }*/
                if (grabAction.GetState(handType) && (col == leftVideoCube || col == rightVideoCube))
                {
                    Vector3 mOffset;
                    mOffset = col.gameObject.transform.position - laserRaycast.GetHitpoint();
                    col.gameObject.transform.position = laserRaycast.GetHitpoint() + mOffset;
                   // col.gameObject.transform.Translate(laserRaycast.GetHitpoint() + mOffset);
                    Debug.Log(mOffset);
                    //this.GetComponent<DragObjectAttempt1>().DragObject(col.gameObject);
                }

                //If triggerAction from SteamVR and Raycast Collider hits fastforwrad gameobject, fastforward
                if (triggerAction.GetState(handType) && col == ffCol)
                {
                    ffCol.GetComponent<FastForwardScript>().ff15s();


                    count = 200;
                }

                //If triggerAction from SteamVR and Raycast Collider hits PlayPause gameobject, Pause/Play
                if (triggerAction.GetState(handType) && col == playCol)
                {
                    playCol.GetComponent<PlayPauseScript>().PlayPause();


                    count = 200;
                }
                //Refreshing raycast collider so when raycast is no longer over an object it is null, this is important
                // as it stops functions from being activated when they aren't being pointed to.
                this.GetComponent<LaserRaycast>().SetColliderHit(null);
            }

            //If menuHiderAction from SteamVR is pressed, hide menu and raycasts until another gameobj is hit
            //Outside the Col != null statement because this doesn't need to interact with a collider to work.
            if (menuHiderAction.GetState(handType))
            {
                videoUI.gameObject.GetComponent<HideMenuScript>().HideMenu();


                count = 200;
            }
        }

    }

}
