using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Script to manage the game. It contains information that will be accessed by diferent scripts and functions to manage the game.
/// It is instanciated and not destroyed when changing between scenes.
/// </summary>
public class GameManager : MonoBehaviour {

    #region Singleton
    // Declare an instance of GameManager.
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            // If the scene doesn't contain an instance load the prefab.
            if (_instance == null) { Resources.Load("Prefab/GameManager"); }               
            return _instance;
        }
    }
    #endregion

    /// <summary>
    /// Enumum to define the state of the game.
    /// </summary>
    public enum StateType
    {
        Play,           // Player is playing a game level.
        MainMenu,       // Player is on the main menu.
        Options,        // Player is adjusting game options.
        Gameover,       // Player is dead and out of lifes.
        Ranking         // Player has already win the game or is viewing the ranking.
    };
    public GameObject player;           // Player GameObject (updated on each scene).
    public StateType gameState;         // State of the game.

    public GameObject menu;
    public GameObject game_over_menu;
    public GameObject ranking_screen;
    public GameObject options_menu;

    private int score { get; set; }    // Score of the game. 

    void Awake()
    {
        #region instace Code
        if (_instance == null) { _instance = this; }            // If the instance is not set, set this GameManager as it.
        else if (_instance != this) { Destroy(gameObject); }    // If another instance already exist auto destroy.
        DontDestroyOnLoad(gameObject);                          // Avoid destroying the instance when changing scene.
        #endregion

        SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameObject.FindGameObjectWithTag("Player");    // Set player variable.
        score = 0;                                         // Start with score at 0.
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");    // Update Player GameObject on scene loaded.
    }

    /// <summary>
    /// Resets GameManager by seting the TotalScore to 0, Player values to default and resets Player HUD if is on the scene.
    /// </summary>
    public void ResetGameManager()
    {
        ResetTotalScore();
    }

    /// <summary>
    /// Resets current level by reseting the GameManager, reloading the level and seting the state back to Play.
    /// </summary>
    public void ResetLevel()
    {
        SetGameState(StateType.Play);
        ResetGameManager();
    }

    #region Game State Functions

    /// <summary>
    /// Function to set the game state to the one passed as parameter.
    /// </summary>
    /// <param name="state"> State to change to.</param>
    public void SetGameState(StateType state)
    {
        if (gameState != state)
            gameState = state;
        else
            Debug.LogWarning("State already in " + state.ToString());

        switch (state)
        {
            case StateType.Play:
                menu.SetActive(false);
                game_over_menu.SetActive(false);
                ranking_screen.SetActive(false);
                options_menu.SetActive(false);
                break;
            case StateType.Gameover:
                menu.SetActive(false);
                game_over_menu.SetActive(true);
                ranking_screen.SetActive(false);
                options_menu.SetActive(false);
                break;
            case StateType.Ranking:
                menu.SetActive(false);
                game_over_menu.SetActive(false);
                ranking_screen.SetActive(true);
                options_menu.SetActive(false);
                break;
            case StateType.MainMenu:
                menu.SetActive(true);
                game_over_menu.SetActive(false);
                ranking_screen.SetActive(false);
                options_menu.SetActive(false);
                break;
            case StateType.Options:
                menu.SetActive(false);
                game_over_menu.SetActive(false);
                ranking_screen.SetActive(false);
                options_menu.SetActive(true);
                break;

        }
    }

    /// <summary>
    /// Function to get current game state.
    /// </summary>
    public StateType GetGameState()
    {
        return gameState;
    }
    #endregion

    #region ScoreFunctions

    /// <summary>
    /// Returns the Total Score
    /// </summary>
    /// <returns>Int of the total score</returns>
    public int GetTotalScore()
    {
        return score;
    }

    /// <summary>
    /// Adds the amount passed as parameter to the Total Score
    /// </summary>
    /// <param name="amount">Amount of points to add</param>
    public void AddToTotalScore()
    {
        score++;
    }

    /// <summary>
    /// Sets the Total Score back to 0
    /// </summary>
    public void ResetTotalScore()
    {
        score = 0;
    }
    #endregion

}
