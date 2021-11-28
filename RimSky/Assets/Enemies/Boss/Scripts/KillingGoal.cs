using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingGoal : Quest.QuestGoal
{
    public string enemy;
    public int amount = 1;
    public override string GetDescription()
    {
        return $"Kill {amount} {enemy}";
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.Instance.OnKillEnemy.AddListener(OnKillEnemy);
    }

    public void OnKillEnemy(string enemyKilled)
    {
        if (enemyKilled == enemy)
        {
            currentAmount++;
            Evaluate();
        }
    }
}
