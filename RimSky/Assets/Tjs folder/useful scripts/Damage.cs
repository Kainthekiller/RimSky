using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damageAmount = 10;
    public bool destroyOnUse = false;
    public bool sent = false;


    public void SendDamageByCollider(Collider collider)
    {
        Health hit = collider.gameObject.GetComponent<Health>();

        if (hit != null && !sent)
        {
            hit.Damage(damageAmount);
            sent = true;
            if (destroyOnUse)
            {
                DestroyMe();
            }
        }

    }
    public void SendDamageByObject(GameObject obj)
    {
        Health hit = obj.GetComponent<Health>();

        if (hit != null && !sent)
        {
            hit.Damage(damageAmount);
            sent = false;
            if (destroyOnUse)
            {
                DestroyMe();
            }
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

}
