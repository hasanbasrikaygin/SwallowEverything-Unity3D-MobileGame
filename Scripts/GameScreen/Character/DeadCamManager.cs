using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCamManager : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCameraBase killCam;

    public void EnabledKillCam()
    {
        Debug.Log(" �lmelisin laz�m");
       // killCam.Priority = SwitchVCam.totalPriorty +20;
    }
}
