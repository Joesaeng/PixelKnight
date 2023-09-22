using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : Singleton<PortalManager>
{
    [Header("# index ��ȣ�� ����� ��Ż�� ID �Դϴ�.")]
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
