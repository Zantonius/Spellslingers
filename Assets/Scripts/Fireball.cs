using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Fireball : NetworkBehaviour
{
    public float radius = 3f;
    public float damage = 10f;

    public LayerMask slingerMask;
    public LayerMask spellMask;

    Rigidbody targetRigidbody;

    public SpellslingerScript sourceSlinger; // The slinger that cast the spell

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (isServer == false)
        {
            return;
        }

        // Is this our Spellslinger? Don't hit it please
        if (sourceSlinger.GetComponent<Rigidbody>() == collider.attachedRigidbody)
        {
            return;
        }

        Collider[] spellCols = Physics.OverlapSphere(this.transform.position, radius, spellMask);

        foreach (Collider col in spellCols)
        {
            if (col.attachedRigidbody == null)
            {
                continue;
            }

            Debug.Log("Wazzaaah");

            NetworkInstanceId objectsNetworkId = col.attachedRigidbody.GetComponent<NetworkIdentity>().netId;
            CmdRemoveSpellObject(objectsNetworkId);
        }

        Collider[] cols = Physics.OverlapSphere(this.transform.position, radius, slingerMask); //.OverlapCircleAll(this.transform.position, radius);

        foreach (Collider col in cols)
        {
            if (col.attachedRigidbody == null)
            {
                continue;
            }

            NetworkInstanceId objectsNetworkId = col.attachedRigidbody.GetComponent<NetworkIdentity>().netId;
            CmdAddForce(objectsNetworkId);

            Health h = col.attachedRigidbody.GetComponent<Health>();
            if (h != null)
            {
                h.CmdChangeHealthAndCheckKill(damage, sourceSlinger.GetComponent<NetworkIdentity>().netId);
                               
            }
        }
    }

    [Command]
    void CmdAddForce(NetworkInstanceId targetSlingerId)
    {
        GameObject targetSlinger = ClientScene.FindLocalObject(targetSlingerId);
        Rigidbody targetRigidbody = targetSlinger.GetComponent<Rigidbody>();
        targetRigidbody.freezeRotation = true;
        targetRigidbody.freezeRotation = false;
        RpcAddForce(targetSlingerId);
    }

    [ClientRpc]
    void RpcAddForce(NetworkInstanceId targetSlingerId)
    {
        GameObject targetSlinger = ClientScene.FindLocalObject(targetSlingerId);
        if (targetSlinger != null)
        {
            Rigidbody targetRigidbody = targetSlinger.GetComponent<Rigidbody>();
            targetRigidbody.freezeRotation = true;
            //Health currentHealth = targetRigidbody.GetComponent<Health>();
            //float health = currentHealth.GetHitPoints();
            //float explosionForce = 700f * 100f / health;
            //Debug.Log(explosionForce);
            //targetRigidbody.velocity = new Vector3(20f, 0, 20f);
            targetRigidbody.AddExplosionForce(700f, this.transform.position, radius);
            targetRigidbody.freezeRotation = false;
            Destroy(gameObject);
        }
    }

    [Command]
    void CmdRemoveSpellObject(NetworkInstanceId targetSpellId)
    {
        GameObject targetSpell = ClientScene.FindLocalObject(targetSpellId);
        RpcRemoveSpellObject(targetSpell);
    }

    [ClientRpc]
    void RpcRemoveSpellObject(GameObject targetSpell)
    {
        Destroy(targetSpell);
    }
}