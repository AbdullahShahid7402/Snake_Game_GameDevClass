using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_Game_Setup : MonoBehaviour
{
    public GameObject intro,mainmenu;
    public TextMeshProUGUI Mute_Text;
    public Slider volume, speed;
    private float buttontime;
    public Animator SettingsPannel,LevelSelectPannel;
    public Animator Play,Settings,Exit;
    public Animator Mute,DeleteHighScore,SettingBack;
    public AudioSource BGM;
    public AudioSource Button_Sound;

    // Start is called before the first frame update
    void Start()
    {
        mainmenu.SetActive(true);
        intro.SetActive(true);
        hidepannels();
        buttontime = 20f/60f;
        initialzePlayerPrefs();
        volume.value = PlayerPrefs.GetFloat("volume");
        speed.value = PlayerPrefs.GetFloat("speed");
        if(get_mute())
        {
            Mute_Text.text = "UnMute";
        }
        else
        {
            Mute_Text.text = "Mute";
        }
        Invoke("TurnOffIntro",9.5f);
    }
    private void hidepannels()
    {
        SettingsPannel.gameObject.SetActive(false);
        LevelSelectPannel.gameObject.SetActive(false);
    }
    private void flipmenu()
    {
        mainmenu.SetActive(!mainmenu.activeSelf);
    }
    private void TurnOffIntro()
    {
        intro.SetActive(false);
    }
    private bool get_mute()
    {
        return PlayerPrefs.GetInt("mute") == 1;
    }
    private void initialzePlayerPrefs()
    {
        if(!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume",1f);
        }
        if(!PlayerPrefs.HasKey("speed"))
        {
            PlayerPrefs.SetFloat("speed",0f);
        }
        if(!PlayerPrefs.HasKey("mute"))
        {
            PlayerPrefs.SetInt("mute",0);
        }
        if(!PlayerPrefs.HasKey("highscore"))
        {
            PlayerPrefs.SetInt("highscore",0);
        }
        PlayerPrefs.Save();
    }
    public void Start_click()
    {
        Button_Sound.Play();
        Play.SetTrigger("Pop");
        Invoke("Start_button",buttontime);
    }
    private void Start_button()
    {
        // Start Button Functionality Here
        LevelSelectPannel.gameObject.SetActive(true);
        LevelSelectPannel.SetTrigger("Slidein");
        Invoke("flipmenu",1f);
    }
    public void StartBack_click()
    {
        Button_Sound.Play();
        Play.SetTrigger("Pop");
        Invoke("StartBack_button",buttontime);
    }
    private void StartBack_button()
    {
        // StartBack Button Functionality Here
        flipmenu();
        LevelSelectPannel.SetTrigger("Slideout");
        Invoke("hidepannels",1f);
    }
    
    public void Settings_click()
    {
        Button_Sound.Play();
        Settings.SetTrigger("Pop");
        Invoke("Settings_button",buttontime);
    }
    private void Settings_button()
    {
        // Settings Button Functionality Here
        SettingsPannel.gameObject.SetActive(true);
        SettingsPannel.SetTrigger("Slidein");
        Invoke("flipmenu",1f);
    }

    public void Exit_click()
    {
        Button_Sound.Play();
        Exit.SetTrigger("Pop");
        Invoke("Exit_button",buttontime);
    }
    private void Exit_button()
    {
        // Exit Button Functionality Here
        Application.Quit();
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
        flipmenu();
        SettingsPannel.SetTrigger("Slideout");
        Invoke("hidepannels",1f);
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

    // Update is called once per framePlay
    void Update()
    {
        // Sync Values from Sliders
        PlayerPrefs.SetFloat("volume",volume.value);
        PlayerPrefs.SetFloat("speed",speed.value);
        PlayerPrefs.Save();
        // Background Music Functionality sync
        BGM.mute = get_mute();
        BGM.volume = PlayerPrefs.GetFloat("volume");
        // Button Sounds Functionality sync
        Button_Sound.volume = PlayerPrefs.GetFloat("volume");
    }
}
