using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInput;

    
    void Start()
    {
        if(PlayerPrefs.HasKey("username"))
        {
            userNameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            userNameInput.text = "Player"+Random.Range(0,9999).ToString("0000");
            OnUserNameInputValurChanged();
        }
    }

    public void OnUserNameInputValurChanged()
    {
        PhotonNetwork.NickName = userNameInput.text;
        PlayerPrefs.SetString("username",userNameInput.text);
    }
}
