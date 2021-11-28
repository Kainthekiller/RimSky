using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleGateLower : MonoBehaviour
{
    public int DropSlowMultiplier; //Increase to slow the drop speed
    public float ShakeMultiplier = 1;
    void Start()
    {
        if (DropSlowMultiplier <= 1)
        {
            DropSlowMultiplier = 1;
        }
        if (ShakeMultiplier <= 1)
        {
            ShakeMultiplier = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checks for how many spawners
        //lowers gate when spawners are all dead and removes the object

        if (GameManager.Instance.KeysHolder.transform.childCount == 1)
        {
            if (transform.position.y > -11)
            {
                transform.position += (Vector3.down / DropSlowMultiplier);

            }

            if (transform.position.z <= 765)
            {
                transform.position += (Vector3.forward / ShakeMultiplier);
            }
            else
            {
                transform.position += (Vector3.back / ShakeMultiplier);
                //transform.position = new Vector3(transform.position.x, transform.position.y, 764);
            }
        }

        if (transform.position.y <= -11)
        {
            Destroy(gameObject);
        }
    }
}
