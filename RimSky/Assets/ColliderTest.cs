using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ColliderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<ThirdPersonController>().TakeDamage(5);
        }
        else
        {
            Debug.Log(collision.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
