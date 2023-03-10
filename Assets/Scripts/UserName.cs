using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class UserName : MonoBehaviour
{
    [SerializeField] PhotonView playerPv;
    [SerializeField] TMP_Text text;

    void Start()
    {
        if(playerPv.IsMine)
        {
            gameObject.SetActive(false);
        }
        text.text = playerPv.Owner.NickName;
    }
}
