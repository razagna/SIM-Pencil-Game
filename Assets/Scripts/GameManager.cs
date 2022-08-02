using System;
using System.Collections;
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
        Victory,
        Loss,
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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitPlayers();
        UpdateGameState(GameState.PlayerTurn);
    }

    private void InitPlayers()
    {
        user.playerType = Player.PlayerType.User;
        user.SetColor(Color.green);
        enemy.playerType = Player.PlayerType.Enemy;
        enemy.SetColor(Color.red);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case GameState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case GameState.Victory:
                HandleVictory();
                break;
            case GameState.Loss:
                HandleLoss();
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
        Debug.Log("It's your turn");
    }

    async void HandleEnemyTurn()
    {
        List<LineSegment> lineSegments = Board.lineSegments;
        for (int current = 0; current < lineSegments.Count; current++)
        {
            if (!lineSegments[current].selected)
            {
                Debug.Log("Launching enemy attack");
                await Task.Delay(300);
                lineSegments[current].AssignTo(enemy);
                break;
            }
        }

        if (enemy.HasCreatedTriangle())
            UpdateGameState(GameState.Victory);
        else
            UpdateGameState(GameState.PlayerTurn);
    }

    void HandleVictory()
    {
        Debug.Log("You Have Won! :)");
    }

    void HandleLoss()
    {
        Debug.Log("You Have Lost! :(");
    }

    void HandleReset()
    {
        Debug.Log("Game is being Reset");
    }

}