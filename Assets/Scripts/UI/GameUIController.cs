using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Map1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
