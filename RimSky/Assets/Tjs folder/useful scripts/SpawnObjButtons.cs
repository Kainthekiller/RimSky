using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjButtons : MonoBehaviour
{

    public GameObject Heal;
    public GameObject Damage;

    public void SpawnHeal()
    {
        GameObject.Instantiate(Heal, transform.position, transform.rotation, null);
    }
     public void SpawnDamage()
    {
        GameObject.Instantiate(Damage, transform.position, transform.rotation, null);
    }
}
