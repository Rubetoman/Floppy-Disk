using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script for the Options Menu Cnavas. It contains functions to adjust game options (resolution, invert movement, sound...).
/// Other UI Components actions are managed on the its component.
/// </summary>
public class OptionsMenu : MonoBehaviour {

    //public AudioMixer audioMixer;                   // Audio Mixer for all the sounds in the game.

    private GameManager.StateType previousState;
    private GameObject myEventSystem;
    private GameObject lastSelected;
    void Awake()
    {
        myEventSystem = GameObject.Find("EventSystem"); // Set scene EventSystem.
    }

    private void OnEnable()
    {
        previousState = GameManager.Instance.GetGameState();                // Save previous state (could be MainMenu or PauseMenu).
        GameManager.Instance.SetGameState(GameManager.StateType.Options);   // Set current state to Options.

       // lastSelected = graphicQualityDropdown.gameObject;                   // Use the first button of the options menu for the selection EventSystem.
        if(myEventSystem != null)
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
    }

    private void OnDisable()
    {
        // Once disabled use the first button of the previous menu as selected.
        if (myEventSystem != null)
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject);
    }

    private void Update()
    {
        // If no UI gameObject was selected and horizontal or vertical Input is detected, select the last selected button.
        if (myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject == null)
        {
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
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

    /// <summary>
    /// Sets the game music volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetMusicVolume (float volume)
    {
        //AudioManager.Instance.SetGameMusicVolume(volume);
    }

    /// <summary>
    /// Sets the game sound effects volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetSoundEffectsVolume(float volume)
    {
        //AudioManager.Instance.SetGameSoundEffectsVolume(volume);
    }

    /// <summary>
    /// Sets the game graphic quality.
    /// </summary>
    /// <param name="qualityIndex"> Number of the index on the project settings.</param>
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Function to go back to the previous menu.
    /// </summary>
    public void GoBack()
    {
        GameManager.Instance.SetGameState(previousState);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Plays a audio clip included on Audio Manager.
    /// </summary>
    /// <param name="name"> Name of the audio clip to play.</param>
    public void PlaySoundClip(string name)
    {
        //AudioManager.Instance.Play(name);
    }
}
