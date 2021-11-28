using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocation : MonoBehaviour
{
    //This code goes on the gameobject
    //Task to be performed by the player at the location
    //This code can contain any logic as long
    //as when the task is completed it injects the three statuses
    //back into the quest system (as per in the oncollision Enter)
    //Currently Here
    public QuestManager qManager;
    public QuestEvent qEvent;
    public QuestButton qButton;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") return;

        //Prevents registration as its not the current quest
        if (qEvent.status != QuestEvent.EventStatus.CURRENT) return;


        //INject these back
        qEvent.UpdateQuestEvent(QuestEvent.EventStatus.DONE);
        qButton.UpdateButton(QuestEvent.EventStatus.DONE);
        qManager.UpdateQuestsOnCompletion(qEvent);
        Destroy(this.gameObject);
    }
    public void Setup(QuestManager qm, QuestEvent qe, QuestButton qb)
    {
        qManager = qm;
        qEvent = qe;
        qButton = qb;
        //Setup Link Between Events and buttons
        qe.button = qButton;
    }

}
