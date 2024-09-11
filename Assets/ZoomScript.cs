using Cinemachine;
using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ZoomScript : MonoBehaviour
{
    public SimplePlayerController controller;
    public static bool zoom;
    public CinemachineVirtualCamera cameraV2;
    public Animator anim;

    public GameObject zoomTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            anim.SetBool("isRun", false);
            controller.enabled = false;
            zoom = true;
            cameraV2.Priority = 11;

            StartCoroutine(CameraV1());
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
        yield return new WaitForSeconds(4);
        zoomTrigger.SetActive(false);
        controller.enabled = true;

    }

}
