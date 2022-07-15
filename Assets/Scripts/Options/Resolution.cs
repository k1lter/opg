using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    public Dropdown resolution_dropdown;
    public GameObject background;

    public void ChangeRes()
    {
        if(resolution_dropdown.value == 0)
        {
            Screen.SetResolution(1024, 768, true);
        }
        else if(resolution_dropdown.value == 1)
        {
            Screen.SetResolution(1366, 768, true);
        }
        else if (resolution_dropdown.value == 2)
        {
            Screen.SetResolution(1680, 1050, true);
        }
        else if (resolution_dropdown.value == 3)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (resolution_dropdown.value == 4)
        {
            Screen.SetResolution(2560, 1440, true);
        }
    }
}
