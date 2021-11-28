using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destory : MonoBehaviour
{
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
    }
public void DestoryHelper() {
        Destroy(transform.parent.gameObject);
    }
}
