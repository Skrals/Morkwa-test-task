using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _endGameText;

    public void OverText(bool finishFlag)
    {
        string text = finishFlag ? "You won" : "You Lose";
        _endGameText.text = text;
    }

    public void ContinueButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void MainMenuButton() => SceneManager.LoadScene(0);
}
