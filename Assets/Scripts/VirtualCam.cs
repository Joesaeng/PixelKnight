using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCam : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Collider2D[] confinerColls;
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void SetFollow(Transform player)
    {
        virtualCamera.m_Follow = player;
    }
    public void SetConfiner(int index)
    {
        CinemachineConfiner2D confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = confinerColls[index];
    }
}
