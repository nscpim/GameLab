using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class ResetVCam : MonoBehaviour
{
    public static Action VrCamForceResetAction;
    [SerializeField] CinemachineFreeLook cam;

    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }
    private void OnEnable()
    {
        VrCamForceResetAction += ForceResetCam;
    }
    private void OnDisable()
    {
        VrCamForceResetAction -= ForceResetCam;
    }
    public void ForceResetCam()
    {
        cam.PreviousStateIsValid = false;
    }
}
