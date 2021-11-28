using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuestManager : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questContent;
    [SerializeField] private GameObject questHolder;

    public List<Quest> CurrentQuests;

    private void Awake()
    {
        foreach (var quest in CurrentQuests)
        {
            quest.Initialize();
            quest.QuestCompleted.AddListener(QuestCompleted);
            GameObject questObj = Instantiate(questPrefab, questContent);
            questObj.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.icon;

        }
    }

    public void Kill(string enemyName)
    {
        GameManager.Instance.OnKillEnemy.Invoke(enemyName);
    }
    public void QuestCompleted(Quest quest)
    {
        questContent.GetChild(CurrentQuests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
    }
}
