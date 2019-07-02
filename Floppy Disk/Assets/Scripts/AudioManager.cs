using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Script that manages Audio of the scene, every sound clip to be played from a script can be managed using an instance of the AudioManager.
/// </summary>
public class AudioManager : MonoBehaviour {

    #region Singleton
    // Declare an instance of AudioManager.
    private static AudioManager _instance = null;
    public static AudioManager Instance {
        get {
            // If the scene doesn't contain an instance load the prefab.
            if (_instance == null) { Resources.Load("Prefab/GameManagement/AudioManager"); }    
            return _instance; }
    }
    #endregion

    /// <summary>
    /// Enum to divide audio clips in two groups.
    /// </summary>
    public enum SoundType
    {
        SoundEffect,
        Music,
    }
    /// <summary>
    /// Sound class that will contain information about each audio clip.
    /// Some of this variables will be passed to the AudioSource created for each clip.
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        public string name;         // Name for this sound.
        public AudioClip clip;      // AudioClip of the sound.
        public SoundType soundType; // Type of clip (music or sound effect).
        [Range(0f, 1f)]             
        public float volume = 1f;   // Volume at which the sound will be played.
        [Range(0f, 3f)]     
        public float pitch = 1f;    // Pitch at which the sound will be played.
        public bool loop = false;   // If it will be looped when the playing ends.
        [HideInInspector]
        public AudioSource source;  // The AudioSource that will play the sound. (This is automatically assigned)
    }
    
    public AudioMixer audioMixer;               // Mixer wich will reproduce the audio clips.
    public AudioMixerGroup musicGroup;          // Group of the mixer where music clips are played.
    public AudioMixerGroup soundEffectsGroup;   // Group of the mixer where sound effects clips are played.
    public string nextSongSoundEffect;          // Name of the sound effect that will play before any song
    public Sound[] sounds;                      // Array containing all the sounds added to the AudioManager.
    private Sound currentMusicClip;             // Music clip that is currently being played (only one can be played at the same time).
    public List<Sound> songs;                   // List with all songs, it is auto-filled on awake.

    private void Awake()
    {
        #region instance Code
        // If another instance already exist auto destroy and stop execution.
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else { _instance = this; }  // If the instance is not set, set this AudioManager as it.
        #endregion

        //Asign an AudioSource to each sound with the defined values.
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();  // Add AudioSource component
            s.source.clip = s.clip;                             // Asign the clip to the AudioSource
            if (s.soundType == SoundType.Music)                 // Asign an output AudioMixerGroup
            {
                s.source.outputAudioMixerGroup = musicGroup;
                songs.Add(s);
            }
            else
                s.source.outputAudioMixerGroup = soundEffectsGroup;
            s.source.loop = s.loop;                             // Set the clip to loop or not.
            s.source.volume = s.volume;                         // Set clip volume.
            s.source.pitch = s.pitch;                           // Set clip pitch.
        }
    }

    /// <summary>
    /// Function to play an audio clip added to the AudioSource, it is found by the name given in the Sound class.
    /// </summary>
    /// <param name="name"> Name of the sound to be played. </param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }
        if(s.soundType == SoundType.Music)                                      // See if the sound is a music clip.
        {
            // If there was a previous music clip playing stop it.
            if (currentMusicClip != null && currentMusicClip.source.isPlaying)
                Stop(currentMusicClip.name);
            currentMusicClip = s;
        }
        s.source.Play();
    }

    /// <summary>
    /// Function to play any audio song added to the AudioSource.
    /// Before playing the song an audio effect will be played
    /// </summary>
    public void PlayRandomSong()
    {
        int index = UnityEngine.Random.Range(0, songs.Count);
        Sound s = songs[index];
        if (s == null)
        {
            Debug.LogWarning("Song: " + s.name + " not found.");
            return;
        }
        if (s.soundType != SoundType.Music)                                      // See if the sound is a music clip.
            return;

        // If there was a previous music clip playing stop it.
        if (currentMusicClip != null && currentMusicClip.source.isPlaying)
        {
            //Stop(currentMusicClip.name);
            currentMusicClip.source.Stop();
        }

        // Play sound effect
        Play(nextSongSoundEffect);
        currentMusicClip = s;

        //ChangeAudioSourceClip(s.source, s.clip, true, true);
        StartCoroutine(ChangeAudioSourceClip(currentMusicClip.source, currentMusicClip.clip, false, false));
    }

    /// <summary>
    /// Function to play an audio song added to the AudioSource, it is found by the name given in the Sound class.
    /// Before playing the song an audio effect will be played
    /// </summary>
    /// <param name="name"> Name of the sound to be played. </param>
    public void PlaySong(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Soung: " + name + " not found.");
            return;
        }
        if (s.soundType != SoundType.Music)                                      // See if the sound is a music clip.
            return;

        // If there was a previous music clip playing stop it.
        if (currentMusicClip != null && currentMusicClip.source.isPlaying)
        {
            Stop(currentMusicClip.name);
        }

        // Play sound effect
        Play(nextSongSoundEffect);
        currentMusicClip = s;

        //ChangeAudioSourceClip(s.source, s.clip, true, true);
        StartCoroutine(ChangeAudioSourceClip(s.source, s.clip, true, false));
    }

    /// <summary>
    /// Function to stop an audio clip added to the AudioSource, it is found by the name given in the Sound class.
    /// </summary>
    /// <param name="name"> Name of the sound to be stoped. </param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }
        s.source.Stop();
    }

    /// <summary>
    /// Function to stop every audio clip added to the AudioSource.
    /// WARNING: It stops only the Sounds added to the AudioSource, any other clip that could be playing will not stop.
    /// </summary>
    public void StopEverySound()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
                s.source.Stop();
        }
    }

    /// <summary>
    /// Function to stop every audio clip of type SoundEffect added to the AudioSource.
    /// WARNING: It stops only the Sounds added to the AudioSource, any other sound effect clip that could be playing will not stop.
    /// </summary>
    public void StopSoundEffects()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying && s.soundType == SoundType.SoundEffect)
                s.source.Stop();
        }
    }

    /// <summary>
    /// Function to stop every audio clip of type Music added to the AudioSource.
    /// WARNING: It stops only the Sounds added to the AudioSource, any other music clip that could be playing will not stop.
    /// </summary>
    public void StopMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying && s.soundType == SoundType.Music)
                s.source.Stop();
        }
    }

    #region AudioSettingsFunctions
    /// <summary>
    /// Sets the game music volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetGameMusicVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", volume);
        else
            Debug.LogError("Missing audio mixer in Game Manager");
    }

    /// <summary>
    /// Sets the game sound effects volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetGameSoundEffectsVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("SoundEffectsVolume", volume);
        else
            Debug.LogError("Missing audio mixer in Game Manager");
    }
    #endregion

    #region AudioSourceFunctions

    /// <summary>
    /// Funtion to change the AudioClip of an existing AudioSource. You can make the new one to loop and decide if wait for the previous song to end before changing.
    /// </summary>
    /// <param name="source"> The existing AudioSource. </param>
    /// <param name="newClip"> The new AudioClip to play. </param>
    /// <param name="loop"> If the new AudioClip will loop. </param>
    /// <param name="wait"> If the function needs to wait for the AudioClip currently on the AudioSource to end before playing the new song. </param>
    /// <returns></returns>
    public IEnumerator ChangeAudioSourceClip(AudioSource source, AudioClip newClip, bool loop, bool wait)
    {
        if (wait)
            yield return new WaitForSeconds(source.clip.length - source.time);
        source.clip = newClip;
        source.loop = loop;
        if (!source.isPlaying)
            source.Play();
    }

    /// <summary>
    /// Function that returns true if an AudioClip is being played by the AudioManager.
    /// The AudioClip is found by the name given on the Sound class.
    /// </summary>
    /// <param name="name"> Name variable on the Sound class.</param>
    /// <returns> True if it is being played, false otherwise.</returns>
    public bool IsClipPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return false;
        }
        if (s.source.isPlaying)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Function that returns true if an AudioClip is being played by the AudioManager.
    /// </summary>
    /// <returns> True if a clip is being played, false otherwise.</returns>
    public bool IsClipPlaying()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
                return true;
        }
        return false;
    }
    #endregion
}
