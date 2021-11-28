using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

public class BossSwordCollider : MonoBehaviour
{

    public int damage = 10;
    public UnityEvent onHit;
    public UnityEvent onHeavyHit;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _boss;
    private BossController _bossController;

    private void Start()
    {
        _animator = _boss.GetComponent<Animator>();
        _bossController = _boss.GetComponent<BossController>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                SetHit(_bossController.lightDamage);
                collision.gameObject.GetComponent<ThirdPersonController>().TakeDamage(damage);
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                SetHeavyHit(_bossController.heavyDamage);
                collision.gameObject.GetComponent<ThirdPersonController>().TakeDamage(damage);
            }
        }

        //Debug.Log("Boss Sword Hit Player For " + damage.ToString());
    }

    public void SetHit(int amount)
    {
        damage = amount;
    }
    public void SetHeavyHit(int amount)
    {
        damage = amount;
    }
}
