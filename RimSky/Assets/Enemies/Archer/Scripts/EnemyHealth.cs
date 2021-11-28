using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int Maxhealth = Mathf.Clamp(100, 0, 100);
    int CurrentEnemyHealth;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyHealth = Maxhealth;
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamages(int damage)
    {
        
        CurrentEnemyHealth -= damage;
        CurrentEnemyHealth = Mathf.Clamp(CurrentEnemyHealth, 0, 100);
        if (CurrentEnemyHealth == 0)
        {
            animator.SetTrigger("isDead");
        }
        else
        {
            animator.SetTrigger("takeDamage");
        }
    }
    public void DestroyEnemy()
    {
            Destroy(this.gameObject);

    }


}
