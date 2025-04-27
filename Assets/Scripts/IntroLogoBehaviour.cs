using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLogoBehaviour : MonoBehaviour
{
    public float Off_Delay;
    public List<GameObject> Logos;
    private AsyncOperation SceneLoad;
    // Start is called before the first frame update
    void Start()
    {
        // check all game objects and turn them on to play their respective animations
        foreach(GameObject Logo in Logos)
        {
            Logo.SetActive(true);
        }

        // start loading the scene on the very next index in background so that by the time animation has played out it is fully loaded  
        SceneLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        // donot allow the scene to interrupt the animation by activating prematurely
        SceneLoad.allowSceneActivation = false;
        // Turn off the game objects when their animation time runs out
        Invoke("Turn_Off",Off_Delay);
    }

    private void Turn_Off()
    {
        // Turn off all the game objects in the list to ensure they dont stay active on canvas when not in use
        foreach(GameObject Logo in Logos)
        {
            Logo.SetActive(false);
        }
        // allow the scene to load up as by this time all animations have played and we can move onto next scene
        SceneLoad.allowSceneActivation = true;
    }
}
