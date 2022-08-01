using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject winScreen, loseScreen, clearButton;

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
        //Debug.Log("new state is " + state);
        clearButton.SetActive(state == GameManager.GameState.PlayerTurn || state == GameManager.GameState.EnemyTurn);
        winScreen.SetActive(state == GameManager.GameState.Victory);
        loseScreen.SetActive(state == GameManager.GameState.Lose);
    }
}