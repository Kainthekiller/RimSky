using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwordDamage : MonoBehaviour
{
    public GameObject _swordCollider;
    public BoxCollider _Collider;
    private void Start()
    {
        _swordCollider = GameObject.Find("BossSwordCollider");
        _Collider = _swordCollider.GetComponent<BoxCollider>();
        _Collider.enabled = false;
    }
    public void SwordBoxColliderEnabled()
    {
        _Collider.enabled = true;
    }

    public void SwordBoxColliderDisabled()
    {
        _Collider.enabled = false;
    }
    public void SetHit(int amount)
    {
        _Collider.GetComponent<BossSwordCollider>().SetHit(amount);
    }
    public void SetHeavyHit(int amount)
    {
        _Collider.GetComponent<BossSwordCollider>().SetHeavyHit(amount);
    }
}
