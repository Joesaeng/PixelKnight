using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : Singleton<PortalManager>
{
    [Header("# index 번호에 연결된 포탈의 ID 입니다.")]
    public int[] connectedPortalID;

    public int curUsePortalID;
    public int GetConnectedPortal(int portalID)
    {
        return connectedPortalID[portalID];
    }
    public void SetCurUsePortalID(int ID)
    {
        curUsePortalID = ID;
    }
}
