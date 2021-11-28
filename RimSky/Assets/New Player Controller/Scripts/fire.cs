using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour
{
    public GameObject FireballGameObject;
    public Transform FireballSpawnPoint;

    public void Shoot()
    {
        Instantiate(FireballGameObject, FireballSpawnPoint.position + (transform.forward / 2), transform.rotation);
    }
}
