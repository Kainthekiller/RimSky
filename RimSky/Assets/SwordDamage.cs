using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public GameObject _swordCollider;
    BoxCollider _Collider;
    private void Start()
    {

        if (_swordCollider != null)
        {
            _Collider = _swordCollider.GetComponent<BoxCollider>();
            _Collider.enabled = false;
        }
       
    }
    public void SwordBoxColliderEnabled()
    {
        _Collider.enabled = true;
    }

    public void SwordBoxColliderDisabled()
    {
        _Collider.enabled = false;
    }
}
