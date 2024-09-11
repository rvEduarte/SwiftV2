using Cinemachine;
using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ZoomScript : MonoBehaviour
{
    public CinemachineVirtualCamera cameraV2;
    public Animator anim;

    public GameObject zoomTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //Timer.countdownFinished = false;  //disableMove
            SimplePlayerController.grounded = false;
            cameraV2.Priority = 11;

            StartCoroutine(CameraV1());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            
        }
    }

    IEnumerator CameraV1()
    {
        yield return new WaitForSeconds(5);
        cameraV2.Priority = 0;
        //cameraV3.Priority = 11;
        
        StartCoroutine(enableMovement());
    }

    IEnumerator enableMovement()
    {
        yield return new WaitForSeconds(4.5f);
        //Timer.countdownFinished = true; //enableMove
        //triggerZoom.SetActive(false);
        
    }

}
