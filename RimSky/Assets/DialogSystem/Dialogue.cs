using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public GameObject mainPlayer;
    private int index;


    // Start is called before the first frame update
    void Start()
    {
        mainPlayer = GameObject.Find("Player");
        textComponent.text = string.Empty;
        mainPlayer.GetComponent<Animator>().SetTrigger("isDialog");
        mainPlayer.GetComponent<StarterAssetsInputs>().move.x = 0f;
        mainPlayer.GetComponent<StarterAssetsInputs>().move.y = 0f;

        mainPlayer.SetActive(false);
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            mainPlayer.GetComponent<Animator>().SetTrigger("isNotDialog");
            mainPlayer.SetActive(true);
            Destroy(this.gameObject);
        }
    }


}
