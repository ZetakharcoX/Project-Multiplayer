using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
   [SerializeField] Transform container;
   [SerializeField] GameObject scoreBoardItemPrefab;
   [SerializeField] CanvasGroup canvasGroup;
   [SerializeField] GameObject[] topics;

   Dictionary<Player, ScoreBoardItem> scoreBoardItems = new Dictionary<Player, ScoreBoardItem>();

   void Start()
   {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            AddScoreBoardItem(player);
        }       
   }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreBoardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
    
   void AddScoreBoardItem(Player player)
   {
        ScoreBoardItem item = Instantiate(scoreBoardItemPrefab,container).GetComponent<ScoreBoardItem>();
        item.Initialize(player);
        scoreBoardItems[player] = item;
   }

   void RemoveScoreBoardItem(Player player)
   {
        Destroy(scoreBoardItems[player].gameObject);
        scoreBoardItems.Remove(player);
   }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            canvasGroup.alpha = 1;
            foreach(GameObject obj in topics)
            {
                obj.SetActive(true);
            }
        }

        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            canvasGroup.alpha = 0;
            foreach(GameObject obj in topics)
            {
                obj.SetActive(false);
            }
        }
    }
}
