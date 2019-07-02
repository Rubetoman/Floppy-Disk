using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;
using UnityEngine.Video;

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
        Play,           // Player is playing the game.
        MainMenu,       // Player is on the main menu.
        Options,        // Player is adjusting game options.
        Gameover,       // Player is dead.
        Credits,        // Player is loking at the credits.  
        Ranking         // Player is loking at the ranking.
    };

    /// <summary>
    /// Enumum to define the post processing profile.
    /// </summary>
    public enum PostProcessType
    {
        TV,             // Old TV type grain and vignette
        Green,          // Dark green palette
        Black_White,    // Black & White palette
        None       
    };

    /// <summary>
    /// Enumum to define the post processing profile.
    /// </summary>
    public enum FilterType
    {
        VHS,        // VHS like lines and distorsion
        Pixel,      // Pixelated image
        None
    };

    public GameObject player;           // Player GameObject (updated on each scene).
    public GameObject obstacleSpawner;  // The GameObject that spawns the obstacles.
    public Text scoreText;
    public StateType gameState;         // State of the game.

    public GameObject menu;
    public GameObject game_over_menu;
    public GameObject credits_menu;
    public GameObject ranking_screen;
    public GameObject options_menu;

    public GameObject main_camera;
    PostProcessingProfile TVPostProcessingProfile;
    PostProcessingProfile GreenPostProcessingProfile;
    PostProcessingProfile BWPostProcessingProfile;
    public PostProcessType postProcessType = PostProcessType.TV;
    public FilterType filterType = FilterType.VHS;


    public string startSoundName;

    private int score { get; set; }    // Score of the game. 

    void Awake()
    {
        #region instace Code
        if (_instance == null) { _instance = this; }            // If the instance is not set, set this GameManager as it.
        else if (_instance != this) { Destroy(gameObject); }    // If another instance already exist auto destroy.
        DontDestroyOnLoad(gameObject);                          // Avoid destroying the instance when changing scene.
        #endregion

        score = 0;      // Start with score at 0.
    }

    private void Start()
    {
        // Play start sound
        AudioManager.Instance.Play(startSoundName);
    }

    private void Update()
    {
        // If song ended play a new one
        if (gameState == StateType.Play && !AudioManager.Instance.IsClipPlaying())
            AudioManager.Instance.PlayRandomSong();
    }

    /// <summary>
    /// Resets GameManager by seting the TotalScore to 0, Player values to default and resets Player HUD if is on the scene.
    /// </summary>
    public void ResetGameManager()
    {
        ResetScore();
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
                credits_menu.SetActive(false);
                ranking_screen.SetActive(false);
                options_menu.SetActive(false);
                break;
            case StateType.Gameover:
                menu.SetActive(false);
                game_over_menu.SetActive(true);
                credits_menu.SetActive(false);
                ranking_screen.SetActive(false);
                options_menu.SetActive(false);
                scoreText.enabled = false;
                break;
            case StateType.Credits:
                menu.SetActive(false);
                credits_menu.SetActive(true);
                ranking_screen.SetActive(false);
                options_menu.SetActive(false);
                scoreText.enabled = false;
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
                credits_menu.SetActive(false);
                ranking_screen.SetActive(false);
                options_menu.SetActive(false);
                break;
            case StateType.Options:
                menu.SetActive(false);
                game_over_menu.SetActive(false);
                credits_menu.SetActive(false);
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
    /// Returns the Score
    /// </summary>
    /// <returns>Int of the score</returns>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// Adds one point to the Score
    /// </summary>
    public void AddToScore()
    {
        score++;
        UpdateScore();
    }

    /// <summary>
    /// Sets the Score back to 0
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = score.ToString();
    }
    #endregion

    #region PostProcessing Functions

    /// <summary>
    /// Function to set the Post Processing Profile to the one passed as parameter.
    /// </summary>
    /// <param name="type"> PostProcessType to change to.</param>
    public void SetPostProcessType(PostProcessType type)
    {
        if (postProcessType != type)
            postProcessType = type;
        else
            Debug.LogWarning("Post Process Type already in " + type.ToString());

        PostProcessingBehaviour behaviour = main_camera.GetComponent<PostProcessingBehaviour>();
        switch (type)
        {
            case PostProcessType.TV:
                behaviour.profile = TVPostProcessingProfile;
                break;
            case PostProcessType.Green:
                behaviour.profile = GreenPostProcessingProfile;
                break;
            case PostProcessType.Black_White:
                behaviour.profile = BWPostProcessingProfile;
                break;
            default:
            case PostProcessType.None:
                behaviour.profile = null;
                break;
        }
    }

    /// <summary>
    /// Function to set the Post Processing Type to the one passed as parameter.
    /// </summary>
    /// <param name="type"> PostProcessType to change to.</param>
    public void SetPostProcessType(int type)
    {
        SetPostProcessType((PostProcessType)type);
    }

    /// <summary>
    /// Function to get current PostProcessType.
    /// </summary>
    public PostProcessType GetPostProcessType()
    {
        return postProcessType;
    }

    /// <summary>
    /// Function to set the Post Processing Filter to the one passed as parameter.
    /// </summary>
    /// <param name="type"> FilterType to change to.</param>
    public void SetFilterType(FilterType type)
    {
        if (filterType != type)
            filterType = type;
        else
            Debug.LogWarning("Post Process Type already in " + type.ToString());

        PostProcessingBehaviour behaviour = main_camera.GetComponent<PostProcessingBehaviour>();
        switch (type)
        {
            case FilterType.VHS:
                main_camera.GetComponent<PixelatePostProcessEffect>().enabled = false;
                main_camera.GetComponent<VHSPostProcessEffect>().enabled = true;
                main_camera.GetComponent<VideoPlayer>().enabled = true;
                break;
            case FilterType.Pixel:
                main_camera.GetComponent<PixelatePostProcessEffect>().enabled = true;
                main_camera.GetComponent<VHSPostProcessEffect>().enabled = false;
                main_camera.GetComponent<VideoPlayer>().enabled = false;
                break;
            default:
            case FilterType.None:
                behaviour.profile = null;
                break;
        }
    }

    /// <summary>
    /// Function to set the Post Processing Filter to the one passed as parameter.
    /// </summary>
    /// <param name="type"> FilterType to change to.</param>
    public void SetFilterType(int type)
    {
        SetFilterType((FilterType)type);
    }

    /// <summary>
    /// Function to get current FilterType.
    /// </summary>
    public FilterType GetFilterType()
    {
        return filterType;
    }

    #endregion

    public void Play()
    {
        // play sound
        ResetGame();
        SetGameState(StateType.Play);
    }

    public void Options()
    {
        // play sound
        SetGameState(StateType.Options);
    }

    public void Credits()
    {
        // play sound
        SetGameState(StateType.Credits);
    }

    public void Ranking()
    {
        // play sound
        SetGameState(StateType.Ranking);
    }

    public void MainMenu()
    {
        SetGameState(StateType.MainMenu);
    }


    public void ResetGame()
    {
        ResetScore();
        player.GetComponent<PlayerController>().ResetPlayer();
        player.GetComponent<PlayerController>().SetPlaying(true);
        obstacleSpawner.GetComponent<ColumnController>().DisposeAll();
        obstacleSpawner.GetComponent<ColumnController>().enabled = true;
        scoreText.enabled = true;

        // Audio
        AudioManager.Instance.PlayRandomSong();
    }
}
