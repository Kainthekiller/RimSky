using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combostufftocopy : MonoBehaviour
{

    private bool isAttacking;
    private bool comboFlag;

    public float timeBetweenAttacks;

    private float timeToAttack;

    private int _countAttack;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;

            if (timeToAttack <= 0)
            {
                Attack();
            }
            else
            {
                timeToAttack -= Time.deltaTime;
            }
        }
    }
    public void Attack()
    {
        if (isAttacking)
        {
            if (!comboFlag)
            {
                // Some attacking code

                _countAttack++;
            }
            else
            {
                //combo attack
                comboFlag = false;
                _countAttack = 0;
            }

            timeToAttack = timeBetweenAttacks;
        }
    }
}
