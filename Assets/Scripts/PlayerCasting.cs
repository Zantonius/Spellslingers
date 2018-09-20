using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCasting : NetworkBehaviour {

    public float timeBetweenBullets = 0.15f;
    public float range = 20f;
    public GameObject fireball;
    public float fireballVelocity = 20f;

    private float timer;
    private RaycastHit shootHit;

    private float effectsDisplayTime = 0.2f;
    private Vector3 mouseClickPosition;
    private int floorMask;
    private Vector3 yOffset = new Vector3(0, 1, 0);
    private Vector3 aimPos;


    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        if (!hasAuthority)
        {
            return;
        }

        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            timer = 0f;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;

            if (Physics.Raycast(ray, out floorHit, 100f, floorMask)) //out = vad som kommer ut ur denna funktion. Sparas i floorhit.s
            {
                aimPos = floorHit.point - transform.position;
                aimPos.y = 0f;

                Debug.Log("I'm shooting at: " + aimPos);                

                CmdShootFireball();            
            }            
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {

        }
    }

    [Command]
    void CmdShootFireball()
    {
        RpcShootFireball();
    }

    [ClientRpc]
    void RpcShootFireball()
    {
        GameObject fireballInstance = Instantiate(fireball, transform.position + aimPos.normalized + yOffset, transform.rotation);

        NetworkServer.Spawn(fireballInstance);
        Rigidbody newFireball = fireballInstance.GetComponent<Rigidbody>();
        newFireball.velocity = fireballVelocity * aimPos.normalized;
    }
}
