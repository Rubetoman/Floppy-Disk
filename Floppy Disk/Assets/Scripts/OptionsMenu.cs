using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script for the Options Menu Cnavas. It contains functions to adjust game volume.
/// </summary>
public class OptionsMenu : MonoBehaviour {

    //public AudioMixer audioMixer;                   // Audio Mixer for all the sounds in the game.

    /// <summary>
    /// Sets the game music volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetMusicVolume (float volume)
    {
        AudioManager.Instance.SetGameMusicVolume(volume);
    }

    /// <summary>
    /// Sets the game sound effects volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetSoundEffectsVolume(float volume)
    {
        AudioManager.Instance.SetGameSoundEffectsVolume(volume);
    }


    /// <summary>
    /// Function to go back to the main menu.
    /// </summary>
    public void GoBack()
    {
        GameManager.Instance.SetGameState(GameManager.StateType.MainMenu);
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
