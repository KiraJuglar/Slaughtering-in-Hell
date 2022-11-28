using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gameState
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public gameState currentGameState = gameState.menu;
    public static GameManager sharedInstance;
    [SerializeField] int score = 0;
    [Range(1, 10)] public int difficulty = 1;


    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SetGameState(gameState.inGame);
    }

    public void GameOver()
    {
        SetGameState(gameState.gameOver);
    }

    public void GameMenu()
    {
        SetGameState(gameState.menu);
    }

    void SetGameState(gameState newGameState)
    {
        switch (newGameState)
        {
            case gameState.menu:
                //Mostrar el menu
                break;
            case gameState.inGame:
                //Iniciar el juego
                break;
            case gameState.gameOver:
                //Finalizar el juego
                break;
        }
        currentGameState = newGameState;
    }
}
