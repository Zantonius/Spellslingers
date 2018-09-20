using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    Health health;
    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {


		if (health == null)
        {
            // We need to find our player's spellslinger
            if (SpellslingerScript.localSlinger != null)
            {
                health = SpellslingerScript.localSlinger.GetComponent<Health>();                
            }

            if (health == null)
            {
                text.text = "DEAD";
                return;
            }
        }

        text.text = health.GetHitPoints().ToString("#");
	}
}
