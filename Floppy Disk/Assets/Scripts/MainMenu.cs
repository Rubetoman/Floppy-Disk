using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for the Main Menu Canvas. It contains functions for navigating through the main menu and functions for some buttons.
/// Other button actions are managed on the button component.
/// </summary>
public class MainMenu : MonoBehaviour {

    public GameObject titleScreenUI;        // Canvas that contains the title of the game.
    public GameObject mainMenuUI;           // Canvas that contains the Main Menu buttons.
    //public AudioClip loopClip;              // Music clip that is going to loop on the menu.

    private GameObject myEventSystem;       // Event System of the main_menu scene.
    private GameObject lastSelected;        // Last GameObject selected on the Event System.
    //private AudioSource audioSource;        // Audio Source that plays the Main Menu music clip.

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();      // Set audioSource variable with the Audio Source of teh gameObject.
        myEventSystem = GameObject.Find("EventSystem"); // Find the Event System.

        // Set lastSelected variable.
        if (myEventSystem != null)
            lastSelected = myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject;

        // Play the music to loop on the main menu once the one beoing played ends.
        //StartCoroutine(AudioManager.Instance.ChangeAudioSourceClip(audioSource, loopClip, true, true));
    }

    void Update()
    {
        if (Input.GetButtonDown("Start") && titleScreenUI.activeSelf)           // Title screen -> hide title and show main menu buttons.
        {
            //AudioManager.Instance.Play("Start");                                    // Play sound effect.
            titleScreenUI.SetActive(false);
            mainMenuUI.SetActive(true);
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);  // Start with no button selected selected.
        }

        if (Input.GetButtonDown("Back")) 
        {
            if (GameManager.Instance.GetGameState() == GameManager.StateType.Options)               // Options menu -> hide options and show main menu. 
            {
                GetComponentInChildren<OptionsMenu>().GoBack();
                mainMenuUI.SetActive(true);
                lastSelected = myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject;   // Change selected button for the first of the main menu.
                myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
            }
            else if(GameManager.Instance.GetGameState() == GameManager.StateType.MainMenu && mainMenuUI.activeSelf) // Main menu buttons -> hide Main Menu and show tittle screen.
            {
                mainMenuUI.SetActive(false);
                titleScreenUI.SetActive(true);
            }
        }

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

    #region Button Functions
    /// <summary>
    /// Function for the Play button. It launches the first level of the game.
    /// </summary>
    public void PlayGame()
    {
        //GetComponent<AudioSource>().Stop(); // Stop every sound.
        GameManager.Instance.NextScene();
    }

    /// <summary>
    /// Function for the Credits button. It launches the credits of the game.
    /// </summary>
    public void Credits()
    {
        //GetComponent<AudioSource>().Stop();
        GameManager.Instance.LoadScene("credits");
    }

    public void Ranking()
    {
        //GetComponent<AudioSource>().Stop();
        GameManager.Instance.LoadScene("ranking");
    }

    /// <summary>
    /// Function for the Quit button. It closes the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region SoundEffects

    /// <summary>
    /// Plays a audio clip included on Audio Manager.
    /// </summary>
    /// <param name="name"> Name of the audio clip to play.</param>
    public void PlaySoundClip(string name)
    {
        AudioManager.Instance.Play(name);
    }

    #endregion
}
