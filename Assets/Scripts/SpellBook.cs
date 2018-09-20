using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpellBook : NetworkBehaviour
{
    private static SpellBook instance;

    public static SpellBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Spell[] spells;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Spell GetSpell(string chosenSpellName)
    {
        Debug.Log("Chosen spellname was: " + chosenSpellName);
        for (int i = 0; i < spells.Length; i++)
        {
            if (chosenSpellName == spells[i].spellName)
            {
                return spells[i];
            }
        }
        Debug.Log("Should not get here");
        return spells[0];
    }

    public GameObject GetSpellPrefab(string chosenSpellName)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (chosenSpellName == spells[i].spellName)
            {
                return spells[i].spellPrefab;
            }
        }
        Debug.Log("No Spell was found with matching name. Should not get here.");
        return null ;
    }

    public float GetSpellSpeed(string chosenSpellName)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (chosenSpellName == spells[i].spellName)
            {
                return spells[i].spellSpeed;
            }
        }
        Debug.Log("No Spell was found with matching name. Should not get here.");
        return 0f;
    }

    public float GetSpellCooldown(string chosenSpellName)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (chosenSpellName == spells[i].spellName)
            {
                return spells[i].spellCooldown;
            }
        }
        Debug.Log("No Spell was found with matching name. Should not get here.");
        return 0f;
    }

    public string GetSpellName(string chosenSpellName)
    {
        return chosenSpellName;
    }
}
