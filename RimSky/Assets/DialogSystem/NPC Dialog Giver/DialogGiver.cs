using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogGiver : MonoBehaviour
{
    Animator anim;
    public GameObject panel;
    public AudioSource audioSource;
    public AudioClip hi;
    public AudioClip bye;
    bool goodByeSaid = true;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        anim = this.gameObject.GetComponent<Animator>();
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FireBall")
        {
            return;
        }

        else if (panel != null)
        {
            audioSource.PlayOneShot(hi);
            panel.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "FireBall")
        {
            return;
        }

        else if (panel != null)
        {     
            panel.SetActive(false);
           

        }
        if (other.transform.tag == "GoodBye" && goodByeSaid)
        {
            audioSource.PlayOneShot(bye);
            anim.SetTrigger("Idle1");
            goodByeSaid = false;
        }
        
        
    }

    void idleOne()
    {
        anim.SetTrigger("Idle1");
    }
    void idleTwo()
    {
        anim.SetTrigger("Idle2");
       
    }
    void idleThree()
    {
        anim.SetTrigger("Idle3");
    }
}
