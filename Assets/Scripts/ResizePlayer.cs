using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using System;

public class ResizePlayer : MonoBehaviour
{
    private bool resizing = false;
    public float sizeRatio = 2.0f;
    // private GameObject target;
    public SteamVR_Action_Boolean resizeUp;
    public SteamVR_Action_Boolean resizeDown;
    public Hand hand;
    public GameObject player;

    private void OnEnable()
    {
        if (resizeUp == null || resizeDown == null)
        {
            Debug.LogError("<b>[SteamVR Interaction]</b> No resize action assigned", this);
            return;
        }

        resizeUp.AddOnChangeListener(OnResizeUpChange, hand.handType);
        resizeDown.AddOnChangeListener(OnResizeDownChange, hand.handType);
    }

    private void OnDisable()
    {
        if (resizeUp != null)
        {
            resizeUp.RemoveOnChangeListener(OnResizeUpChange, hand.handType);
        }
        if ( resizeDown != null)
        {
            resizeUp.RemoveOnChangeListener(OnResizeDownChange, hand.handType);
        }

    }

    private void OnResizeUpChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        if (newValue)
        {
            Resize(true);
        }
    }

    private void OnResizeDownChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        if (newValue)
        {
            Resize(false);
        }
    }

    public void Resize(bool scalingUp)
    {
        StartCoroutine(startResize(scalingUp));
    }

    private IEnumerator startResize(bool scalingUp)
    {
        // scaleDirection should be 1 or -1
        if (!resizing)
        {
            resizing = true;
            float xscale = player.transform.localScale.x;
            float yscale= player.transform.localScale.y;
            float zscale = player.transform.localScale.z;

            float xPos = player.transform.position.x;
            float yPos = player.transform.position.y;
            float zPos = player.transform.position.z;

            GameObject currentObj = hand.currentAttachedObject;

            //DO resize
            if (scalingUp && yscale < .35 * sizeRatio) // Scale should be uniform for x, y, and z
            {
                if (currentObj != null)
                {
                    currentObj.transform.SetParent(null);
                }
                player.transform.localScale = new Vector3(xscale * sizeRatio, yscale * sizeRatio, zscale * sizeRatio);
                player.transform.position = new Vector3(xPos, yPos + (float)0.5, zPos);
                Debug.Log("Condition check: " + (xscale));
            }
            else if (!scalingUp && yscale > .35 / sizeRatio)
            {
                player.transform.localScale = new Vector3(xscale / sizeRatio, yscale / sizeRatio, zscale / sizeRatio);
                player.transform.position = new Vector3(xPos, yPos + (float)0.5, zPos);
            }
            resizing = false;
        }
        yield return null;
    }
}
