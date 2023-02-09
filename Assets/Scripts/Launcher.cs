using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    const string TITLE = "TitleMenu", LOADING = "LoadingMenu", ROOM = "RoomMenu", ERROR = "ErrorMenu";
    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] TMP_Text errortext;
    [SerializeField] TMP_Text roomNametext;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startgamebutton;

    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuController.Instance.OpenMenu(TITLE);
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInput.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInput.text);
        MenuController.Instance.OpenMenu(LOADING);
    }

    public override void OnJoinedRoom()
    {
        MenuController.Instance.OpenMenu(ROOM);
        roomNametext.text = PhotonNetwork.CurrentRoom.Name;

        foreach(Transform t in playerListContent)           // it destroys previously created players when starting room
        {
            Destroy(t.gameObject);
        }

        foreach(Player p in PhotonNetwork.PlayerList)
        {
            Instantiate(PlayerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().Setup(p);
        }

        startgamebutton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startgamebutton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errortext.text = "Room Creation Failed: " + message;
        MenuController.Instance.OpenMenu(ERROR);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuController.Instance.OpenMenu(LOADING);
    }
    
    public void JoinRooom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuController.Instance.OpenMenu(LOADING); 
    }

    public override void OnLeftRoom()
    {
        MenuController.Instance.OpenMenu(TITLE);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform t in roomListContent)
        {
            Destroy(t.gameObject);
        }

        foreach(RoomInfo ri in roomList)
        {
            if(ri.RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab,roomListContent).GetComponent<RoomListItem>().Setup(ri);
            
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }
}
