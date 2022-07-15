using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public bool pause = false;

    public GameObject start_menu;
    public GameObject options;
    public GameObject pause_menu;
    public bool two_players = false;

    private void Update()
    {
        if(pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause == false)
            {
                pause_menu.SetActive(true);
                pause = true;
            }
            else
            {
                pause_menu.SetActive(false);
                pause = false;
            }
        }
    }

    public void ChangeScene(int scene_id)
    {
        if (scene_id == 1)
        {
            if (!two_players)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
        else
        {
            SceneManager.LoadScene(scene_id);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SwitchPause()
    {
        pause = false;
        pause_menu.SetActive(false);
    }
    public void SwitchPlayers()
    {
        if (two_players)
        {
            two_players = false;
        }
        else
        {
            two_players = true;
        }
    }
}
