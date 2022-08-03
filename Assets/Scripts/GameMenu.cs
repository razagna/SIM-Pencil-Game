using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject clearButton, gameOverMenu, youWin, youLose;

    void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

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
        youWin.SetActive(!GameManager.Instance.user.HasLost());
        youLose.SetActive(GameManager.Instance.user.HasLost());
    }

    public void Restart()
    {
        Debug.Log("restarting game");
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
