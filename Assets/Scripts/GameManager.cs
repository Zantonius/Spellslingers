using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class GameManager : NetworkBehaviour {

    [SyncVar]
    public float timeLeft = 180f;

    public float timer;

    [SerializeField]
    private static Dictionary<string, SpellslingerScript> players = new Dictionary<string, SpellslingerScript>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(isServer == false)
        {
            return;
        }

        timeLeftOfRound();
    }

    void timeLeftOfRound()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            // Do something perhaps.
        }
    }

    private const string PLAYER_ID_PREFIX = "Player ";

    public static void RegisterPlayer(string netID, SpellslingerScript player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        if (!players.ContainsKey(playerID))
        {
            players.Add(playerID, player);
            player.transform.name = playerID;
            Debug.Log(playerID);
        }
    }

    public static SpellslingerScript[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }
}
