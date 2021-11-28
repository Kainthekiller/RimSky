using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
   // private float Explosiondespawntime = 1.5f;
    public GameObject ExplosionCircle;
    public float ExplosionRadius= 80f;
    public float ExplosionForce = 200;
    public float ExplosionSpeed = 1f;
    public float downwardforce = 1;
    private float scale = 1f;

    void Start()
    {
        scale = GetScale();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddExplosionForce(ExplosionForce, gameObject.transform.position, ExplosionRadius, 0 , ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (scale < 8)
        {
            scale += Time.deltaTime * ExplosionSpeed;
            ExplosionCircle.transform.localScale = new Vector3(scale, scale, scale);
            transform.position -= new Vector3(0, scale/downwardforce, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    float GetScale()
    {
        return ExplosionCircle.transform.localScale.x;
    }
}
