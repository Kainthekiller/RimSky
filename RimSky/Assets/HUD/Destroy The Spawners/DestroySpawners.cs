using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestroySpawners : MonoBehaviour
{
    Keyboard keyboard = Keyboard.current;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard.anyKey.isPressed)
        {
            Destroy(gameObject);
        }
    }
}
