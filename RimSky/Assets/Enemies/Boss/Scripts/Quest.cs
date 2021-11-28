using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest : ScriptableObject
{
    [System.Serializable]
    public struct Info
    {
        public string name;
        public Sprite icon;
        public string description;
    }
    [Header("Info")] public Info Information;

    [Header("Reward")]
    public UnityEvent RewardAction;

    public bool completed { get; protected set; }
    public QuestCompletedEvent QuestCompleted;

    public abstract class QuestGoal:ScriptableObject
    {
        protected string description;
        public int currentAmount { get; protected set; }
        public int requiredAmount = 1;
        public bool completed { get; protected set; }
        public UnityEvent GoalCompleted;

        public virtual string GetDescription()
        {
            return description;
        }
        public virtual void Initialize()
        {
            completed = false;
            GoalCompleted = new UnityEvent();

        }
        protected void Evaluate()
        {
            if (currentAmount >= requiredAmount)
            {
                Complete();
            }
        }
        private void Complete()
        {
            completed = true;
            GoalCompleted.Invoke();
            GoalCompleted.RemoveAllListeners();
        }
    }

    public List<QuestGoal> questGoals;
    public void Initialize()
    {
        completed = false;
        QuestCompleted = new QuestCompletedEvent();
        foreach (QuestGoal goal in questGoals)
        {
            goal.Initialize();
            goal.GoalCompleted.AddListener(delegate { CheckGoals(); });
        }
    }
    private void CheckGoals()
    {
        completed = questGoals.TrueForAll(g => g.completed);
        if (completed)
        {
            QuestCompleted.Invoke(this);
            QuestCompleted.RemoveAllListeners();
        }
    }
}
public class QuestCompletedEvent : UnityEvent<Quest>{

}


