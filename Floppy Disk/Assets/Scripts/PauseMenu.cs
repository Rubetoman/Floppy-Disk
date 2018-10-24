using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

/// <summary>
/// Script for the Pause Menu Canvas. It contains functions for navigating through the pause menu and functions for some buttons.
/// Other button actions are managed on the button component.
/// </summary>
public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuUI;          // Canvas that contains the Pause Menu buttons.
    public GameObject mainMenuChooser;      // Canvas that contains the dialogue displayed before loading Main Menu.
    public AudioMixerSnapshot paused;       // Audio Mixer Snapshot to use when the game is on the Pause Menu.
    public AudioMixerSnapshot unpaused;     // Audio Mixer default Snapshot.

    private GameObject myEventSystem;       // Event System of the scene. 
    private GameObject lastSelected;        // Last GameObject selected on the Event System.

    private void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");     // Find the Event System.

        // Set lastSelected variable.
        if (myEventSystem != null)
            lastSelected = myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject;
    }

    void Update ()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (GameManager.Instance.GetGameState() == GameManager.StateType.PauseMenu) // Start pressed while on the PauseMenu -> Resume game.
            {
                Resume();
            }
            else if(GameManager.Instance.GetGameState() == GameManager.StateType.Play)  // Start pressed while on Play mode -> Pause game.
            {
                Pause();
            }
        }

        if (Input.GetButtonDown("Back"))
        {
            switch (GameManager.Instance.GetGameState())
            {
                case GameManager.StateType.PauseMenu:                   // Back pressed while on the PauseMenu -> Resume game.
                    Resume();
                break;

                case GameManager.StateType.Options:                     // Back pressed while on the Options -> Hide options and show Pause Menu.
                    GetComponentInChildren< OptionsMenu > ().GoBack();
                    pauseMenuUI.SetActive(true);
                    myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
                break;

                case GameManager.StateType.Play:                        // Back pressed while on Play mode -> Pause game.
                    Pause();
                break;
            }
        }

        if (GameManager.Instance.GetGameState() == GameManager.StateType.PauseMenu && !mainMenuChooser.activeInHierarchy)
        {
            // If no UI gameObject was selected and horizontal or vertical Input is detected, select the last selected button.
            if (myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject == null)
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
                    myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
            }
            else
            {
                // If a UI gameObject is selected and mouse Input is detected, deselect it.
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    lastSelected = myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject;
                    myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
                }
            }
        }
    }

    /// <summary>
    /// Funtion to resume gameplay.
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);                                   // Hide Pause Menu.
        unpaused.TransitionTo(0);                                       // Use the default Snapshot.
        Time.timeScale = 1f;                                            // Resume time.
        GameManager.Instance.SetGameState(GameManager.StateType.Play);  // Set game state to Play.
    }

    /// <summary>
    /// Funtion to pause gameplay.
    /// </summary>
    void Pause()
    {
        pauseMenuUI.SetActive(true);                                            // Show Pause Menu.
        paused.TransitionTo(0);                                                 // Use the Snapshot for paused game.
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);  // Start with nothing selected.
        Time.timeScale = 0f;                                                    // Pause time.
        GameManager.Instance.SetGameState(GameManager.StateType.PauseMenu);     // Set the game state to PauseMenu.
    }

    /// <summary>
    /// Funtion to load the Main Menu.
    /// </summary>
    public void LoadMainMenu()
    {
        unpaused.TransitionTo(0);           // Use the default Snapshot. 
        GameManager.Instance.LoadScene(0);  // Load Main Menu.
    }

    /// <summary>
    /// Plays a audio clip included on Audio Manager.
    /// </summary>
    /// <param name="name"> Name of the audio clip to play.</param>
    public void PlaySoundClip(string name)
    {
        AudioManager.Instance.Play(name);
    }
}
