using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject ExplosionGameObject;
    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log(gameObject.transform.position.x + " " + gameObject.transform.position.y + " " + gameObject.transform.position.z + " ");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 0)
        {
            Boom();
        }
    }



    void Boom()
    {
        //On Collide Stops fireball 
        transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);


        //creates explosion


        Instantiate(ExplosionGameObject, transform.position, transform.rotation);

        //known bugs - explosion is making itself to big.  disabled currently 

        //deletes itself
        Destroy(gameObject);
    }
}
