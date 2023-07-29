using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCam : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void SetFollow(Transform player)
    {
        virtualCamera.m_Follow = player;
    }

}
