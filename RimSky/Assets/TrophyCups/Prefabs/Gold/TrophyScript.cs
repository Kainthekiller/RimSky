using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrophyScript : MonoBehaviour
{

    //private bool Collected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= 1.5)
        {
            //Debug.Log("Roll Credits");
            SceneManager.LoadScene("Credits");
        }
        
    }
}
