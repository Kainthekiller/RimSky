using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    private Rigidbody fireballRigidbody;

    private void Awake()
    {
        fireballRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 10f;
        fireballRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
