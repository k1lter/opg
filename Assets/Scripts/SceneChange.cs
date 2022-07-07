using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeScene(int scene_id)
    {
        SceneManager.LoadScene(scene_id);
    }
}
