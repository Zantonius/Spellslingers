using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerScoreboardItem : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [SerializeField]
    Text usernameText;

    [SerializeField]
    Text killsText;

    [SerializeField]
    Text deathsText;

    public void Setup(string username, int deaths, int kills)
    {
        usernameText.text = username;
        deathsText.text = "Deaths: " + deaths.ToString();
        killsText.text = "Kills: " + kills.ToString();
    }

}
