using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    private GameObject MainPlayer;
    private int _damage = 5;
    private float TickTimer = 1.5f;
    public float DamageRadius = 0.90f;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            MainPlayer = GameManager.Instance.Player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MainPlayer == null)
        {
            return;
        }

        if (Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= DamageRadius && TickTimer <= 0)
        {
            MainPlayer.gameObject.GetComponent<ThirdPersonController>().TakeDamage(_damage);
            TickTimer = 1.5f;
        }

        if (TickTimer >= 0)
        {
            TickTimer -= Time.deltaTime;
        }

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, DamageRadius);
    }

}
