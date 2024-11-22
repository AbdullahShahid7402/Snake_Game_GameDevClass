using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Snake_Mechanics : MonoBehaviour
{
    bool paused;
    private int score;
    public TextMeshProUGUI score_text,score_text_final;
    private bool starting;
    public Slider volume, speed;
    public TextMeshProUGUI Mute_Text;
    private float buttontime;
    private Transform snakehead_Transform;
    public Portal_Mechanics portal_Mechanics;
    private Vector2 direction;
    private bool Teleported;
    private float gamespeed;
    public BoxCollider2D food_spawn_region;
    private List<GameObject> SnakeBody;
    public GameObject snakeBody_prefab;
    public AudioSource Button_Sound,BGM,Collect_Sound;
    public Animator SettingsPannel,GameoverPannel;
    public Animator Mute,DeleteHighScore,SettingBack;
    public FoodCollisions food;


    private int Starting_Size;

    // Start is called before the first frame update
    void Start()
    {
        SettingsPannel.gameObject.SetActive(false);
        GameoverPannel.gameObject.SetActive(false);
        score = 0;
        score_text.text = score.ToString();
        volume.value = PlayerPrefs.GetFloat("volume");
        speed.value = PlayerPrefs.GetFloat("speed");
        paused = false;
        if(get_mute())
        {
            Mute_Text.text = "UnMute";
        }
        else
        {
            Mute_Text.text = "Mute";
        }
        buttontime = 20f/60f;
        Teleported = false;
        Starting_Size = 3;
        SnakeBody = new List<GameObject>();
        snakehead_Transform = this.transform;
        snakehead_Transform.position = new Vector2(0f,0f);
        SnakeBody.Add(this.gameObject);
        gamespeed = 0.075f;
        start_growth();
    }
    private bool get_mute()
    {
        return PlayerPrefs.GetInt("mute") == 1;
    }
    void start_growth()
    {
        direction = Vector2.right * snakehead_Transform.lossyScale.x;
        for(int i = 0;i < Starting_Size; i++)
        {
            body_growth();
            snake_move();
        }
        direction = new Vector2(0f,0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Sync Values from Sliders
        if(paused)
        {
            PlayerPrefs.SetFloat("volume",volume.value);
            PlayerPrefs.SetFloat("speed",speed.value);
            PlayerPrefs.Save();
            // Background Music Functionality sync
            BGM.mute = get_mute();
            BGM.pitch = (speed.value + 0.5f);
            BGM.volume = PlayerPrefs.GetFloat("volume");
            // Button Sounds Functionality sync
            Button_Sound.volume = PlayerPrefs.GetFloat("volume");
            return;
        }
        score_text.text = score.ToString();
        // Game working
        Time.fixedDeltaTime = gamespeed/((speed.value+1)*(speed.value+1));
        input_manager();
    }

    /* This Function Basically Handles the Game Functionality based on the Input of user */
    private void input_manager()
    {
        if(direction != Vector2.down * snakehead_Transform.lossyScale.x && Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up * snakehead_Transform.lossyScale.x;
            this.transform.eulerAngles = new Vector3(0f,0f,270f);
        }
        else if(direction != Vector2.up * snakehead_Transform.lossyScale.x && Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down * snakehead_Transform.lossyScale.x;
            this.transform.eulerAngles = new Vector3(0f,0f,90f);
        }
        else if(direction != Vector2.right * snakehead_Transform.lossyScale.x && Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left * snakehead_Transform.lossyScale.x;
            this.transform.eulerAngles = new Vector3(0f,0f,0f);
        }
        else if(direction != Vector2.left * snakehead_Transform.lossyScale.x && Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right * snakehead_Transform.lossyScale.x;
            this.transform.eulerAngles = new Vector3(0f,0f,180f);
        }

        if(!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            paused = true;
            Button_Sound.Play();
            SettingsPannel.gameObject.SetActive(true);
            SettingsPannel.SetTrigger("Slidein");
        }
        
    }
    public void Retry_click()
    {
        score = 0;
        Button_Sound.Play();
        SettingBack.SetTrigger("Pop");
        Invoke("Retry_button",buttontime);
    }
    private void Retry_button()
    {
        // SettingsBack Button Functionality Here
        GameoverPannel.SetTrigger("Slideout");
        Invoke("ResetTime",1f);
    }
    public void MainMenu_click()
    {
        score = 0;
        Button_Sound.Play();
        SettingBack.SetTrigger("Pop");
        Invoke("MainMenu_button",buttontime);
    }
    private void MainMenu_button()
    {
        // MainMenu Button Functionality Here
        SceneManager.LoadScene("MainMenu");
    }
    public void SettingsBack_click()
    {
        Button_Sound.Play();
        SettingBack.SetTrigger("Pop");
        Invoke("SettingsBack_button",buttontime);
    }
    private void SettingsBack_button()
    {
        // SettingsBack Button Functionality Here
        SettingsPannel.SetTrigger("Slideout");
        Invoke("ResetTime",1f);
    }
    private void ResetTime()
    {
        paused = false;
        SettingsPannel.gameObject.SetActive(false);
        GameoverPannel.gameObject.SetActive(false);
    }
    public void Mute_click()
    {
        Button_Sound.Play();
        Mute.SetTrigger("Pop");
        Invoke("Mute_button",buttontime);
    }
    private void Mute_button()
    {
        // Mute Button Functionality Here
        if(get_mute())
        {
            PlayerPrefs.SetInt("mute",0);
            Mute_Text.text = "Mute";
        }
        else
        {
            PlayerPrefs.SetInt("mute",1);
            Mute_Text.text = "UnMute";
        }
        PlayerPrefs.Save();
    }
    public void DeleteHighScore_click()
    {
        Button_Sound.Play();
        DeleteHighScore.SetTrigger("Pop");
        Invoke("DeleteHighScore_button",buttontime);
    }
    private void DeleteHighScore_button()
    {
        // DeleteHighScore Button Functionality Here
        PlayerPrefs.SetInt("highscore",0);
    }

    private void FixedUpdate()
    {
        if(paused)
            return;
        snake_move();
    }
    private void snake_move()
    {
        if(direction != new Vector2(0f,0f))
        {
            for(int i = SnakeBody.Count - 1; i > 0; i--)
            {
                if(i%2!=0)
                {
                    SnakeBody[i].transform.localScale = SnakeBody[0].transform.localScale * 0.75f;
                }
                else
                {
                    SnakeBody[i].transform.localScale = SnakeBody[0].transform.localScale * 0.82f;
                }
                SnakeBody[i].transform.position = SnakeBody[i - 1].transform.position;
                SnakeBody[i].transform.eulerAngles = SnakeBody[i - 1].transform.eulerAngles;
            }
            SnakeBody[SnakeBody.Count-1].transform.localScale = SnakeBody[0].transform.localScale * 0.5f;
            // move the snake based on the last known direction in fixed intervals
            snakehead_Transform.position = new Vector2(snakehead_Transform.position.x + direction.x,snakehead_Transform.position.y + direction.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if his by a wall
        if(collision.tag == "Wall" || collision.tag == "Snake Body")
        {
            snakehead_Transform.position = new Vector2(0f,0f);
            kill_snake();
            paused = true;
            if(score > PlayerPrefs.GetInt("highscore"))
            {
                score_text_final.text = "New HighScore = "+score.ToString();
                PlayerPrefs.SetInt("highscore",score);
                PlayerPrefs.Save();
            }
            else
            {
                score_text_final.text = "Score = "+score.ToString();
            }
            GameoverPannel.gameObject.SetActive(true);
            GameoverPannel.SetTrigger("Slidein");
        }
        // if the snake catches food
        if(collision.tag == "Food")
        {
            score += (int)((speed.value + 1)*(speed.value + 1)*50);
            body_growth();
            Collect_Sound.Play();
        }
        // if the snake lands on a portal
        if(collision.tag == "Portal" && !Teleported)
        {
            this.transform.position = portal_Mechanics.Teleport(collision.transform.position);
            Teleported = true;
            Invoke("Reset_Teleport",1f * gamespeed);
        }
    }

    private void Reset_Teleport()
    {
        Teleported = false;
    }

    
    private void body_growth()
    {
        var temp = Instantiate(snakeBody_prefab);
        SnakeBody.Add(temp.gameObject);
        var n = SnakeBody.Count;
        SnakeBody[n-1].transform.position = SnakeBody[n-2].transform.position;
    }
    // this function is responsible for resetting the snake to all the initial conditions
    private void kill_snake()
    {
        for(int i = 1; i < SnakeBody.Count; i++)
        {
            Destroy(SnakeBody[i]);
        }
        SnakeBody = new List<GameObject>();
        SnakeBody.Add(this.gameObject);
        start_growth();
    }
}
