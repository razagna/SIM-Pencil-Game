using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        PlayerTurn,
        EnemyTurn,
        GameOver,
        Reset
    }

    public GameState state;

    public enum EnemyType
    {
        AI,
        Human
    }

    public EnemyType enemyType;

    public static event Action<GameState> OnGameStateChanged;

    public Player user = new Player();
    public Player enemy = new Player();

    void Awake() => Instance = this;

    private void Start()
    {
        InitPlayers();
        UpdateGameState(GameState.PlayerTurn);
    }

    private void InitPlayers()
    {
        user.playerType = Player.PlayerType.User;
        user.color = Color.green;
        enemy.playerType = Player.PlayerType.Enemy;
        enemy.color = Color.red;
    }

    public void UpdateGameState(GameState newState)
    {
        if (newState == state) return;

        state = newState;

        switch (newState)
        {
            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case GameState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            case GameState.Reset:
                HandleReset();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        // notify all scripts that care about the state change
        OnGameStateChanged?.Invoke(newState);
    }

    void HandlePlayerTurn()
    {
        Debug.Log("It's your turn!");
    }

    async void HandleEnemyTurn()
    {
        List<LineSegment> lineSegments = Board.lineSegments;
        for (int current = 0; current < lineSegments.Count; current++)
        {
            if (!lineSegments[current].selected)
            {
                Debug.Log("AI is picking");
                await Task.Delay(300);
                lineSegments[current].AssignTo(enemy);
                break;
            }
        }

        if (enemy.HasCreatedTriangle())
            UpdateGameState(GameState.GameOver);
        else
            UpdateGameState(GameState.PlayerTurn);
    }

    void HandleGameOver()
    {
        string message = user.HasLost() ? "You Have Lost! :(" : "You Have Won! :)";
        Debug.Log(message);
    }

    void HandleReset()
    {
        Debug.Log("Game is being reset");
        user.ResetValues();
        enemy.ResetValues();
        UpdateGameState(GameState.PlayerTurn);
    }

}