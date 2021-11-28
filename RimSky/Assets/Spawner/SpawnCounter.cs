using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCounter : MonoBehaviour
{
    public Text SpawnerCount;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       SpawnerCount.text = "Spawners Remaining: " + transform.childCount.ToString();


        if (this.transform.childCount == 0)
        {
            GameManager.Instance.gameover = true;
            //Debug.Log("GameOver");
        }
    }
}
