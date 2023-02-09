using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public RoomInfo rInfo;

    public void Setup(RoomInfo info)
    {
        rInfo = info;
        text.text = info.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinRooom(rInfo);
    }
}
