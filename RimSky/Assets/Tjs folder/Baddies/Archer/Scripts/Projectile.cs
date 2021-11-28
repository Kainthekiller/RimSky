using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public BaddieController parent;
    public float timeToDeath;
    
    public bool alive;

    public int damageToPass;

    public Rigidbody rb;
    private float timeAlive;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (alive)
        {
            timeAlive += Time.deltaTime;
            if (timeAlive > timeToDeath)
            {
                parent.DespawnArrow(this);
            }
        }
    }

    public void ResetLife()
    {
        timeAlive = 0f;
    }

    public void SetDammage(int ammount)
    {
        damageToPass = ammount;
    }
    public void SetParent(BaddieController _parent)
    {
        parent = _parent;
    }
}
