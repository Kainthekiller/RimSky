using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public Quests quest = new Quests();
    public GameObject questPrintBox;
    public GameObject buttonPrefab;
    public GameObject victoryPopup;
    public GameObject compassNeedle;
    public GameObject compassBar;
    QuestEvent final;

    //Locations
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
   // public GameObject E;

    // Start is called before the first frame update
    void Start()
    {
        //Creating the even itself
        QuestEvent a = quest.AddQuestEvent("Quest1", "Village 1 Key", A);
        QuestEvent b = quest.AddQuestEvent("Quest2", "Village 2 Key", B);
        QuestEvent c = quest.AddQuestEvent("Quest3", "Village 3 Key", C);
        //QuestEvent d = quest.AddQuestEvent("Quest4", "Kill The Boss", D);
        //QuestEvent e = quest.AddQuestEvent("Quest5", "Smile", E);

        //Creating the Paths
        quest.AddPath(a.GetId(), b.GetId());
        quest.AddPath(b.GetId(), c.GetId());
        //quest.AddPath(c.GetId(), d.GetId());
        //quest.AddPath(c.GetId(), e.GetId());
        //quest.AddPath(d.GetId(), e.GetId());


        quest.BFS(a.GetId());

        QuestButton button = CreateButton(a).GetComponent<QuestButton>();
        A.GetComponent<QuestLocation>().Setup(this, a, button);
        button = CreateButton(b).GetComponent<QuestButton>();
        B.GetComponent<QuestLocation>().Setup(this, b, button);
        button = CreateButton(c).GetComponent<QuestButton>();
        C.GetComponent<QuestLocation>().Setup(this, c, button);
        //button = CreateButton(d).GetComponent<QuestButton>();
        //D.GetComponent<QuestLocation>().Setup(this, d, button);
        //button = CreateButton(e).GetComponent<QuestButton>();
        //E.GetComponent<QuestLocation>().Setup(this, e, button);

        final = c;

        quest.PrintPath();
    }
    IEnumerator turnOffVictory()
    {
        yield return new WaitForSeconds(4);
        victoryPopup.SetActive(false);
    }

    GameObject CreateButton(QuestEvent e)
    {
        GameObject b = Instantiate(buttonPrefab);
        b.GetComponent<QuestButton>().Setup(e, questPrintBox);
        if (e.order == 1)
        {
            b.GetComponent<QuestButton>().UpdateButton(QuestEvent.EventStatus.CURRENT);
            e.status = QuestEvent.EventStatus.CURRENT;
        }
        return b;
    }

    public void UpdateQuestsOnCompletion(QuestEvent e)
    {
        if (e == final)
        {
            victoryPopup.SetActive(true);
            StartCoroutine(turnOffVictory());
            Destroy(compassBar);
            Destroy(compassNeedle);
            return;
        }

        foreach (QuestEvent n in quest.questEvents)
        {
            //If this event is the next in order
            if (n.order == (e.order + 1))
            {
                //Make the next in line available for completion
                n.UpdateQuestEvent(QuestEvent.EventStatus.CURRENT);
            }
        }
    }
}
