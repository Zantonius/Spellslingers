using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SpellslingerScript : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerNameChanged2")]
    public string playerName2 = "Anonymous";

    [Header("Camera Position Variables")]
    [SerializeField] float cameraDistance = 16f;
    [SerializeField] float cameraHeight = 16f;

    [SyncVar]
    string playerSpellName;

    [SyncVar]
    public int playerKills;
    
    // Player owned Spells. Stored as strings to be able to fetch other information
    public string[] playerSpellsName;
    public GameObject fireballPrefab;

    // Chosen spell to cast
    public GameObject chosenSpell;

    public float spellSpeed;
    public float timeBetweenFireballs = 1f;
    public float[] playerSpellCooldowns;
    //private float timer;
    public float[] timer;
    public int chosenActionButton;

    private bool teleportPrepare;
    private float teleportCooldown;
    public float timeBetweenTeleports = 3f;
    
    private Transform mainCamera;
    private Vector3 cameraOffset;
    private NavMeshAgent m_NavMeshAgent;
    private bool m_RightMouseClick;
    private Vector3 m_MousePosition;
    private Vector3 newPosition;

    private int floorMask;

    public Text playerScore;

    static public SpellslingerScript localSlinger { get; protected set; }


    public void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        floorMask = LayerMask.GetMask("Floor");       
    }

    // Use this for initialization
    void Start()
    {
        cameraOffset = new Vector3(0f, cameraHeight, cameraDistance);
        chosenSpell = null;
        mainCamera = Camera.main.transform;
        MoveCamera();
        playerSpellsName = new string[] { "No Spell", "No Spell", "No Spell" };
        playerSpellCooldowns = new float[] { 0f, 0f, 0f };
        timer = new float[] { 5f, 5f, 5f };
        teleportPrepare = false;

        playerKills = 0;

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        SpellslingerScript spellSlinger = GetComponent<SpellslingerScript>();
        GameManager.RegisterPlayer(netID, spellSlinger);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority == true)
        {
            AuthorityUpdate();
            localSlinger = this;          
        }        

        for (int i = 0; i < timer.Length; i++)
            timer[i] += Time.deltaTime;

        teleportCooldown += Time.deltaTime;

        SetPlayerName();
    }

    private void SetPlayerName()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            string n = "Zant" + Random.Range(1, 100);

            Debug.Log("Sending the server a request to change our name to: " + n);
            CmdChangePlayerName2(n);
        }
    }

    void AuthorityUpdate()
    {
        m_RightMouseClick = Input.GetMouseButton(1);
        m_MousePosition = Input.mousePosition;

        // Handles both movement and teleports
        MoveWithMouse();
        ClearMeshAgent();
        MoveCamera();

        ChooseSpell();
        CastSpell(chosenActionButton);
    }

    private void ChooseSpell()
    {
        if (Input.GetKeyDown("1"))
        {
            CmdChooseSpell(0);           
        }
        else if (Input.GetKeyDown("2"))
        {
            CmdChooseSpell(1);
        }
        else if (Input.GetKeyDown("3"))
        {
            CmdChooseSpell(2);
        }
        else if (Input.GetKeyDown("4"))
        {
            teleportPrepare = true;          
        }
    }

    public float GetPlayerSpellCooldows(int chosenSpellNumber)
    {
        return playerSpellCooldowns[chosenSpellNumber];
    }

    // Chooses spell to cast from the Spellbook and its' properties
    [Command]
    private void CmdChooseSpell(int chosenSpellNumber)
    {
        chosenActionButton = chosenSpellNumber;
        chosenSpell = SpellBook.MyInstance.GetSpellPrefab(playerSpellsName[chosenSpellNumber]);
        spellSpeed = SpellBook.MyInstance.GetSpellSpeed(playerSpellsName[chosenSpellNumber]);
        RpcChooseSpell(chosenSpellNumber);
    }

    [ClientRpc]
    private void RpcChooseSpell(int chosenSpellNumber)
    {
        chosenActionButton = chosenSpellNumber;
        chosenSpell = SpellBook.MyInstance.GetSpellPrefab(playerSpellsName[chosenSpellNumber]);
    }

    // Places spell from the spellbook into the action bar and registers it as a player owned spell
    [Command]
    public void CmdSetPlayerSpell(string placedSpellName, int actionButtonPlace)
    {
        playerSpellsName[actionButtonPlace] = placedSpellName;
        playerSpellCooldowns[actionButtonPlace] = SpellBook.MyInstance.GetSpellCooldown(playerSpellsName[actionButtonPlace]);
        RpcSetPlayerSpell(placedSpellName, actionButtonPlace);
    }

    [ClientRpc]
    public void RpcSetPlayerSpell(string placedSpellName, int actionButtonPlace)
    {
        playerSpellsName[actionButtonPlace] = placedSpellName;
        playerSpellCooldowns[actionButtonPlace] = SpellBook.MyInstance.GetSpellCooldown(playerSpellsName[actionButtonPlace]);
    }

    private void MoveWithMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(m_MousePosition);
        RaycastHit hit;

        if (m_RightMouseClick)
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                m_NavMeshAgent.SetDestination(hit.point);
                newPosition = hit.point;
            }
        }

        if (Input.GetButton("Fire1") && teleportPrepare && teleportCooldown >= timeBetweenTeleports && Time.timeScale != 0)
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                localSlinger.transform.position = hit.point;
                teleportPrepare = false;
            }
            teleportCooldown = 0;
            m_NavMeshAgent.ResetPath();
        }
    }

    // Clears the moving goal of Spellslinger
    void ClearMeshAgent()
    {
        float dist = Vector3.Distance(transform.position, newPosition);
        if (dist < 0.1f)
        {
            m_NavMeshAgent.ResetPath();
        }
    }

    void OnPlayerNameChanged2(string newName)
    {
        Debug.Log("OnPlayerNameChange: OldName:  " + playerName2 + " NewName: " + newName);

        gameObject.name = "Spellslinger [" + newName + "]";

        playerName2 = newName;
    }

    public string GetPlayerName()
    {
        return playerName2;
    }

    void MoveCamera()
    {
        mainCamera.position = transform.position + cameraOffset;
        mainCamera.LookAt(transform);
    }
    
    void CastSpell(int actionButton)
    {       
        if (Input.GetButton("Fire1") && timer[actionButton] >= playerSpellCooldowns[actionButton] && Time.timeScale != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;

            if (Physics.Raycast(ray, out floorHit, 100f, floorMask)) //out = vad som kommer ut ur denna funktion. Sparas i floorhit.s
            {
                Vector3 aimPosition = floorHit.point - transform.position;
                aimPosition.y = 0f;

                if (chosenSpell == null)
                {
                    return;
                }
                
                CmdCastSpell(aimPosition, actionButton);              
            }
        }
    }
    
    public int GetSpellslingerKills()
    {
        return playerKills;
    }

    [Command]
    void CmdCastSpell(Vector3 aimPosition, int actionButton)
    {
        Vector3 yOffset = new Vector3(0, 0.5f, 0); // Makes the fireball not go at floor level
        GameObject spellInstance = Instantiate(chosenSpell, transform.position + yOffset, transform.rotation);

        spellInstance.GetComponent<Fireball>().sourceSlinger = this;
        Rigidbody rb = spellInstance.GetComponent<Rigidbody>();
        //rb.position = transform.position + aimPosition.normalized + yOffset;
        //rb.rotation = transform.rotation;
        rb.velocity = spellSpeed * aimPosition.normalized;

        timer[actionButton] = 0f;
        RpcResetCooldown(actionButton);
        NetworkServer.Spawn(spellInstance);       
        Destroy(spellInstance, 2f);
        chosenSpell = null;
    }

    [ClientRpc]
    void RpcResetCooldown(int actionButton)
    {
        timer[actionButton] = 0f;
    }
    
    [Command]
    void CmdChangePlayerName2(string n)
    {
        Debug.Log("CmdChangePlayerName: " + n);

        // Maybe check for bad names?
        // Do we ignore it in that case? Or do we still call the Rpc? But with the orignal name?

        playerName2 = n;

        // RpcChangePlayerName(n);
    }
}
