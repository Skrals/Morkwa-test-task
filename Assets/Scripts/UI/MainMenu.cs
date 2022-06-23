using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton() => SceneManager.LoadScene("Game");
    public void ExitButton() => Application.Quit();
}
