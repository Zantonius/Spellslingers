using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[Serializable]
public class Spell : IMoveable {

    [SerializeField] public Sprite spellIcon;
    [SerializeField] public GameObject spellPrefab;
    [SerializeField] public string spellName;
    [SerializeField] public float spellCooldown;
    [SerializeField] public int spellId;
    [SerializeField] public float spellSpeed;

    public Spell(Spell d)
    {
        spellIcon = d.spellIcon;
        spellName = d.spellName;
        spellCooldown = d.spellCooldown;
        spellId = d.spellId;
        spellSpeed = d.spellSpeed;
    }

    public Sprite MyIcon
    {
        get
        {
            return spellIcon;
        }
    }

    public string MySpellName
    {
        get
        {
            return spellName;
        }
    }

    public int MySpellId
    {
        get
        {
            return spellId;
        }
    }

    public float MySpellCooldown
    {
        get
        {
            return spellCooldown;
        }
    }

    public float MySpellSpeed
    {
        get
        {
            return spellSpeed;
        }
    }
}


