using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{

    public BossController boss;
    public BossUI ui;

    public void StartBossFight()
    {
        boss.StartBoss();
        ui.transform.GetChild(0).gameObject.SetActive(true);
    }
}
