using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Play,
        SwitchPlayer,
        EvaluateBoard,
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

    Player user = new Player();
    AI enemy = new AI();
    Player[] players;

    int activePlayer = 1;
    int losingPlayer = 0;

    void Awake() => Instance = this;

    void Start()
    {
        PlayerPrefs.SetInt("PlayerType", 0);
        enemyType = PlayerPrefs.GetInt("EnemyType") == 0 ? EnemyType.AI : EnemyType.Human;
        InitPlayers();
        UpdateGameState(GameState.SwitchPlayer);  
    }

    public void InitPlayers()
    {
        user.color = GetPlayerColor("Color_A");
        enemy.color = GetPlayerColor("Color_B");
        players = new Player[2] { user, enemy };
    }

    public static Color GetPlayerColor(string player)
    {
        var strings = PlayerPrefs.GetString(player).Split(", "[0]);

        Color color = Color.black;
        for (var i = 0; i < 3; i++)
            color[i] = float.Parse(strings[i]);

        return color;
    }

    public void UpdateGameState(GameState newState)
    {
        if (newState == state) return;

        state = newState;

        switch (newState)
        {
            case GameState.Play:
                HandlePlay();
                break;
            case GameState.SwitchPlayer:
                HandleSwitchPlayer();
                break;
            case GameState.EvaluateBoard:
                HandleEvaluateBoard();
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

        OnGameStateChanged?.Invoke(newState);
    }

    void HandlePlay()
    {
        players[activePlayer].SelectMove();
    }

    void HandleSwitchPlayer()
    {
        activePlayer = 1 - activePlayer;
        UpdateGameState(GameState.Play);
    }

    void HandleEvaluateBoard()
    {
        if (!players[activePlayer].HasCreatedTriangle())
            UpdateGameState(GameState.SwitchPlayer);
        else
        {
            losingPlayer = activePlayer;
            UpdateGameState(GameState.GameOver);
        }
    }

    void HandleGameOver()
    {
        string message = user.HasLost() ? "You Have Lost! :(" : "You Have Won! :)";
        Debug.Log(message);
    }

    void HandleReset()
    {
        Debug.Log("Game is being reset");
        foreach (Player player in players) player.ResetValues();
        UpdateGameState(GameState.SwitchPlayer);
    }

    public Player GetActivePlayer()
    {
        return players[activePlayer];
    }

    public Player GetLoser()
    {
        return players[losingPlayer];
    }

}