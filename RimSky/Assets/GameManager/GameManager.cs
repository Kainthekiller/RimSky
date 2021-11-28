using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using StarterAssets;
//using UnityEditor.Experimental.GraphView;

public class GameManager : MonoBehaviour
{
    //Gamemanager that keeps reference too all essential references to gameobjects like the player,
    //    HUD, Pause Menu, etc..  
    //    Anything that other scripts needs access to should be found here.
    Keyboard keyboard = Keyboard.current;
    private static GameManager instance; //Use this to grab objects
    public GameObject Player;
    public ThirdPersonController playerScript;
    public HealthBar healthScript;
    public GameObject HUD;
    public GameObject pauseMenu;
    public GameObject healthBar;
    public GameObject manaBar;
    public GameObject compassBar;
    public GameObject SpawnCounter;
    public GameObject KeysHolder;
    public int screamsfemalemax = 2;
    public int screamsfemalecurrent = 0;
    public int screamsmalemax = 2;
    public int screamsmalecurrent = 0;
    public bool gameover = false;
    [HideInInspector]
    public UnityEvent<string> OnKillEnemy;

    private float timerTime = 5f;


    private GameManager()
    {
        // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
        // because the game manager will be created before the objects
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                //Debug.LogError("Game manager is null");
            }

            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
        Player = GameObject.Find("Player");
        playerScript = Player.GetComponent<ThirdPersonController>();
        HUD = GameObject.Find("HUD");
        pauseMenu = GameObject.Find("PauseMenu");
        healthBar = GameObject.Find("HealthBar");
        healthScript = healthBar.GetComponent<HealthBar>();
        manaBar = GameObject.Find("ManaBar");
        compassBar = GameObject.Find("Compass");
        SpawnCounter = GameObject.Find("SpawnCounter");
        KeysHolder = GameObject.Find("KeysHolder");
        OnKillEnemy = new UnityEvent<string>();

        

    }
    // Start is called before the first frame update
    void Start()
    {
       
        ApplicationInteracter.CursorModeLocked();
     
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    void Timer()
    {
        if (timerTime >= 0)
        {
            timerTime -= Time.deltaTime;
        }
        else
        {
            screamsfemalecurrent = 0;
            screamsmalecurrent = 0;
            timerTime = 5f;
        }
    }

}
