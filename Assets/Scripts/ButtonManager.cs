using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void ButtonPlay()
    {
        SceneManager.LoadScene("Map");
    }
}
