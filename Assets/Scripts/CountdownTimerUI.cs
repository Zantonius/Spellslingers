using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimerUI : MonoBehaviour {

    GameManager gameManager;
    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        getGamemanager();

        //text.text = gameManager.timeLeft.ToString("#.0");
    }

    void getGamemanager()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();

            if (gameManager == null)
            {
                // The game probably hasn't started yet.
                return;
            }
        }
    }
}
