using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour {

    [SyncVar]
    public float fullHitpoints = 100;

    [SyncVar]
    public float currentHitpoints;

    [SyncVar]
    public int deaths;

    public bool isDead;

    public Slider slider;

    public Image fillImage;

    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;

    public NetworkInstanceId damagedByThisSlingerLast;

    //SpellslingerScript spellslingerWhoDamagedMe;
    
	// Use this for initialization
	void Start () {
		if (isServer)
        {
            currentHitpoints = fullHitpoints;
            deaths = 0;
            isDead = false;
        }        
    }
	
	// Update is called once per frame
	void Update () {

        if (isServer)
            RpcSetHealthSliderColor();

        if (SpellslingerScript.localSlinger.transform.position.x > 30 ||
            SpellslingerScript.localSlinger.transform.position.x < -30 ||
            SpellslingerScript.localSlinger.transform.position.z > 30 ||
            SpellslingerScript.localSlinger.transform.position.z < -30)
        {
            CmdLavaDamage();
        }
    }

    public float GetHitPoints()
    {
        return currentHitpoints;
    }

    public int GetPlayerDeaths()
    {
        return deaths;
    }

    [Command]
    public void CmdChangeHealthAndCheckKill(float amount, NetworkInstanceId spellSlingerWhoDamagedMe)
    {
        currentHitpoints -= amount;
        CmdSetHealthSliderColor();
        damagedByThisSlingerLast = spellSlingerWhoDamagedMe;

        if (currentHitpoints <= 0)
        {
            Die(damagedByThisSlingerLast);
            return;
        }       
    }

    [Command]
    public void CmdLavaDamage()
    {
        currentHitpoints -= 0.5f;
        if (currentHitpoints < 0)
        {
            Die(damagedByThisSlingerLast);
        }
    }

    void Die(NetworkInstanceId spellSlingerWhoDamagedMe)
    {
        if (isServer == false)
        {
            Debug.Log("Client called die");
            return;
        }

        if (!isDead)
        {
            isDead = true;
            Debug.Log("DIE");
            deaths++;
            GameObject spellSlinger = ClientScene.FindLocalObject(damagedByThisSlingerLast);
            spellSlinger.GetComponent<SpellslingerScript>().playerKills++;
            gameObject.SetActive(false);
            RpcSetSpellslingerAsInactive(damagedByThisSlingerLast);
        }
    }   

    [ClientRpc]
    void RpcSetSpellslingerAsInactive(NetworkInstanceId spellSlingerWhoDamagedMe)
    {
        GameObject spellSlinger = ClientScene.FindLocalObject(spellSlingerWhoDamagedMe);
        gameObject.SetActive(false);
    }

    [Command]
    private void CmdSetHealthSliderColor()
    {
        RpcSetHealthSliderColor();
    }

    [ClientRpc]
    private void RpcSetHealthSliderColor()
    {
        slider.value = currentHitpoints;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHitpoints / fullHitpoints);
    }

    [Command]
    public void CmdSetHealth(float health)
    {
        currentHitpoints = health;
        RpcSetHealth(health);
    }

    [ClientRpc]
    public void RpcSetHealth(float health)
    {
        currentHitpoints = health;
    }
}
