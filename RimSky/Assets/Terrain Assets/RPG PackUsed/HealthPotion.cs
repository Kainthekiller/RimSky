using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public HealthBar health;
    public float PickupDistance = 0.81f;

    private void Start()
    {
        health = FindObjectOfType<HealthBar>();
    }
    // Start is called before the first frame update
    


    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= PickupDistance && health.GetHealth() < 100)
        {
            int number = health.GetHealth();
            if (number >= 71)
            {
                health.SetHealth(100);
                health.PlayHealthSound();
                Destroy(this.gameObject);
            }
            else
            {
                health.SetHealth(number + 30);
                health.PlayHealthSound();
                Destroy(this.gameObject);
            }


        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, PickupDistance);
    }
}
