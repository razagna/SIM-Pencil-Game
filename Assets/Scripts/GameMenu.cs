using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject clearButton, gameOverMenu, youWin, youLose, playerScores;

    void Awake() => GameManager.OnGameStateChanged += OnGameStateChanged;
    void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
            StartCoroutine(DisplayGameOverMenu(1.7f));
    }

    IEnumerator DisplayGameOverMenu(float time)
    {
        yield return new WaitForSeconds(time);

        clearButton.SetActive(false);
        gameOverMenu.SetActive(true);
        DisplayMessage();
    }

    void DisplayMessage()
    {
        Player loser = GameManager.Instance.GetLoser();
        youWin.SetActive(loser.GetType().Equals(typeof(AI)));
        youLose.SetActive(loser.GetType().Equals(typeof(Player)));
        //playerScores.SetActive(true);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

}
