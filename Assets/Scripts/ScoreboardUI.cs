using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreboardUI : NetworkBehaviour
{

    [SerializeField]
    GameObject[] players;

    [SerializeField]
    private CanvasGroup scoreboard;

    [SerializeField]
    public GameObject playerScoreboardItemPrefab;

    [SerializeField]
    public Transform playerScoreboardList;

    // Use this for initialization
    void Start()
    {

    }

        // Update is called once per frame
    void Update()
    {       
        OpenClose(scoreboard);       
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = 0;
                RemoveAllPlayer();
            }
            else
            {
                canvasGroup.alpha = 1;
                GetAllPlayers();
            }
            //canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
        }
    }

    //[Command]
    void GetAllPlayers()
    {
        SpellslingerScript[] spellSlingers = GameManager.GetAllPlayers();

        foreach (SpellslingerScript spellSlinger in spellSlingers)
        {
            
            if (spellSlinger != null)
            {
                GameObject itemGO = (GameObject)Instantiate(playerScoreboardItemPrefab, playerScoreboardList);
                PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
                
             
                if (item != null)
                {
                    Debug.Log("Spellslinger Name is: " + spellSlinger.name);
                    item.Setup(spellSlinger.name, spellSlinger.GetComponent<Health>().GetPlayerDeaths(), spellSlinger.GetSpellslingerKills());
                }
            }
        }
    }

    void RemoveAllPlayer()
    {
        foreach(Transform child in playerScoreboardList)
        {
            Debug.Log("Destroyed");
            Destroy(child.gameObject);
        }
    }
}
