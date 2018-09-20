using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserStats : NetworkBehaviour {
    
    [SerializeField]
    public Spell[] allSpells;
    public Spell[] playerSpells;
    public Spell[] spellShop;
    public bool spellLearnMenu;

    public Texture BlankIcon;

    // Player Menus
    public bool playerSpellBookShow;

    public Texture barsBackgroundTexture;
	// Use this for initialization
	void Start () {

        //Add spells
        spellShop[0] = allSpells[0];
        spellShop[1] = allSpells[1];
        spellLearnMenu = false;

        /*playerSpells[0].spellId = allSpells[0].spellId;
        playerSpells[0].spellIcon = allSpells[0].spellIcon;
        playerSpells[0].spellName = allSpells[0].spellName;
        playerSpells[0].spellDescription = allSpells[0].spellDescription;

        playerSpells[1].spellId = allSpells[1].spellId;
        playerSpells[1].spellIcon = allSpells[1].spellIcon;
        playerSpells[1].spellName = allSpells[1].spellName;
        playerSpells[1].spellDescription = allSpells[1].spellDescription;*/

        //OnGUI();
    }
	
	// Update is called once per frame
	void Update () {
		
        //Test spells
        if(Input.GetKeyDown("5"))
        {
            UsedSpell(playerSpells[0].spellId);
        }

        //Test spells
        if (Input.GetKeyDown("6"))
        {
            UsedSpell(playerSpells[1].spellId);
        }

        if (Input.GetButtonDown("Shop"))
        {
            if (spellLearnMenu == false)
            {
                Debug.Log("Show shop");
                spellLearnMenu = true;
            }
            else 
            {
                Debug.Log("Hide shop");              
                spellLearnMenu = false;
            }
        }
    }

    private void OnMouseOver()
    {
        
    }

    /*private void OnGUI()
    {
        Rect rect1 = new Rect(Screen.width / 2, Screen.height - 64, 32, 32);

        //Rect rect2 = new Rect(Screen.width / 2, Screen.height - 64, 32, 32);

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height - 64, 32, 32), "5"))
        {
            UsedSpell(playerSpells[0].spellId);
        }

        if (rect1.Contains(Event.current.mousePosition))
        {
            GUI.DrawTexture(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 150, 200, 200), barsBackgroundTexture);
            GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 150, 200, 200),
                "Spell name " + playerSpells[0].spellName + "\n" + 
                //"Spell description: " + playerSpells[0].spellDescription + "\n" +
                "Spell id : " + playerSpells[0].spellId);
        }

        if (spellLearnMenu == true)
        {
            // Show buy spell menu. Show spells that you haven't bought yet. Add spell to array when bought.

            GUI.DrawTexture(new Rect(100, 200, 300, 400), barsBackgroundTexture);

            for (int i = 0; i < spellShop.Length; i++)
            {
                if (GUI.Button(new Rect(100, 200 + (i*50), 100, 32), "" + spellShop[i].spellName))
                {
                    // Buy spell    
                    for (int j = 0; j < playerSpells.Length; j++)
                    {
                        if (playerSpells[j].spellName == "")
                        {
                            playerSpells[j] = spellShop[i];
                            break;
                        }
                    }
                }
            }
        }

        //if (playerSpellBookShow)

    }*/

    void UsedSpell (int spellId)
    {
        switch (spellId)
        {
            case 0:
                print("Used spell 0");
                break;
            case 1:
                print("Used spell 1");
                break;
            case 2:
                print("Used spell 2");
                break;
            case 3:
                print("Used spell 3");
                break;
            case 4:
                print("Used spell 4");
                break;
            case 5:
                print("Used spell 5");
                break;
            default:
                print("Spell Error");
                break;
        }
    }
}
