using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{

    public GameObject spellslingerPrefab;
    public SpellslingerScript mySlinger;

    // SyncVars are variable where if their value changes on the SERVER,
    // then all clients are automatically informed of the new value.
    [SyncVar(hook = "OnPlayerNameChanged")]
    public string playerName = "Anonymous";

   
    // Use this for initialization
    void Start()
    {
        // Is this actually my own local PlayerConnectionObject?
        if (isServer == true)
        {
            SpawnSpellslinger();
            // This object belongs to another player
            return;
        }

        // Since the PlayerConnectionObject is invisible and not part of the world,
        // give me something physical to move around

        Debug.Log("PlayerObject::Start -- Spawning my own personal unit.");

        // Instantiate() only creates and object on the LOCAL COMPUTER.
        // Even if it has a NetworkIdentity it still will NOT exist on
        // the network (and therefore not on any other client) UNLESS
        // NetworkServer.Spawn() is called on this object.

        //Instantiate(PlayerUnitPrefab);

        // Command (politely) the server to SPAWN our unit

        
    }

    // Update is called once per frame
    void Update()
    {
        // Remember Update runes on EVERYONE's computer, whether or not they own this
        // particular player object.

        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            CmdDisableMyUnit(mySlinger.GetComponent<NetworkIdentity>().netId);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            CmdEnableMyUnit(mySlinger.GetComponent<NetworkIdentity>().netId);
            Debug.Log("SetHealth and EnableMe");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            string n = "Zant" + Random.Range(1, 100);

            Debug.Log("Sending the server a request to change our name to: " + n);
            CmdChangePlayerName(n);
        }
    }

    void SpawnSpellslinger()
    {
        if (isServer == false)
        {
            Debug.LogError("SpawnSpellSlinger: Can only do this from the server.");
            return;
        }

        GameObject go = Instantiate(spellslingerPrefab);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        CmdSetConnectedPlayer(go.GetComponent<NetworkIdentity>().netId);
    }

    [Command]
    void CmdSetConnectedPlayer(NetworkInstanceId spellslingerNetid)
    {
        GameObject mySlingerPrefab = ClientScene.FindLocalObject(spellslingerNetid);
        RpcSetConnectedPlayer(mySlingerPrefab);
    }

    [ClientRpc]
    void RpcSetConnectedPlayer(GameObject myConnectedSlinger)
    {
        mySlinger = myConnectedSlinger.GetComponent<SpellslingerScript>();
    }

    // WARNING: IF YOU USE A HOOK ON A SyncVar, then our local value does NOT get
    // automatically updated.
    void OnPlayerNameChanged(string newName)
    {
        Debug.Log("OnPlayerNameChange: OldName:  " + playerName + " NewName: " + newName);

        gameObject.name = "PlayerConnectionObject [" + newName + "]";

        playerName = newName;
    }

    /////////////////////////////////// COMMANDS
    // Commands are special functions that ONLY get executed on the server

    [Command]
    void CmdDisableMyUnit(NetworkInstanceId spellslingerNetid)
    {
        GameObject myConnectedSlinger = ClientScene.FindLocalObject(spellslingerNetid);
        RpcDisableMyUnit(myConnectedSlinger);
    }

    [ClientRpc]
    void RpcDisableMyUnit(GameObject myConnectedSlinger)
    {
        myConnectedSlinger.gameObject.SetActive(false);
    }

    [Command]
    void CmdEnableMyUnit(NetworkInstanceId spellslingerNetid)
    {
        GameObject myConnectedSlinger = ClientScene.FindLocalObject(spellslingerNetid);
        RpcEnableMyUnit(myConnectedSlinger);
    }

    [ClientRpc]
    void RpcEnableMyUnit(GameObject myConnectedSlinger)
    {
        myConnectedSlinger.SetActive(true);
        myConnectedSlinger.GetComponent<Health>().CmdSetHealth(100);
    }

    [Command]
    void CmdChangePlayerName(string n)
    {
        Debug.Log("CmdChangePlayerName: " + n);

        // Maybe check for bad names?
        // Do we ignore it in that case? Or do we still call the Rpc? But with the orignal name?

        playerName = n;       
    }
}
