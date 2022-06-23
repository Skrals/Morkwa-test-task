using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _endGameText;

    private void Start()
    {
        OverText(_endGameText);
    }

    private void OverText(TMP_Text overText)
    {
        overText.text = "end";
    }

    public void ContinueButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void MainMenuButton() => SceneManager.LoadScene(0);
}
