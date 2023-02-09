using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    PhotonView pV;
    GameObject controller;
    int kills;
    int deaths;

    void Awake()
    {
        pV = GetComponent<PhotonView>();    
    }

    void Start()
    {
        if(pV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),spawnPoint.position,spawnPoint.rotation,0,new object[] {pV.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
        deaths++;
        Hashtable hash = new Hashtable();
        hash.Add("deaths",deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void GetKill()
    {
        pV.RPC(nameof(RPC_Kill),pV.Owner);
    }
    
    [PunRPC]
    void RPC_Kill()
    {
        kills++;
        Hashtable hash = new Hashtable();
        hash.Add("Kills",kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.pV.Owner == player);
    }
}
