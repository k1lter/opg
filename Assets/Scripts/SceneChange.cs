using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public bool pause = false;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause == false)
            {
                Time.timeScale = 0;
                pause = true;
            }
            else
            {
                Time.timeScale = 1;
                pause = false;
            }
        }
    }
    public void ChangeScene(int scene_id)
    {
        SceneManager.LoadScene(scene_id);
    }
}
