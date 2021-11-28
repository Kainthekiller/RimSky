using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public int healAmount = 20;
    public bool destroyOnUse = false;
    public bool sent = false;
    public void SendHealthByCollider(Collider collider)
    {
        Health hit = collider.gameObject.GetComponent<Health>();

        if (hit != null && !sent)
        {
            hit.Heal(healAmount);
            sent = true;
            if (destroyOnUse)
            {
                DestroyMe();
            }
        }

    }
    public void SendHealthByObject(GameObject obj)
    {
        Health hit = obj.GetComponent<Health>();

        if (hit != null && !sent)
        {
            hit.Heal(healAmount);
            sent = true;
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
