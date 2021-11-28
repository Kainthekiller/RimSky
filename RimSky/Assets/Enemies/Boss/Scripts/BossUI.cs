using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public BossController boss;
    public GameObject bossUIPanel;
    public GameObject bossDefeatedUIPanel;
    public Slider health;
    public GameObject spawnerUI;


    void Start()
    {
        spawnerUI.SetActive(false);
    }

    public void StartUI()
    {

    }
    public void OpenHealthUI()
    {
        bossUIPanel.SetActive(true);
    }
    public void CloseHealthUI()
    {
        bossUIPanel.SetActive(false);
    }
    public void OpenBossDefeatedUI()
    {
        bossDefeatedUIPanel.SetActive(true);
    }
    public void CloseBossDefeatedUI()
    {
        bossDefeatedUIPanel.SetActive(false);
    }
    private void Update()
    {
        if (boss.bossStart)
        {
            health.value = boss.health;
            OpenHealthUI();
            spawnerUI.SetActive(true);
        }
    }
}
