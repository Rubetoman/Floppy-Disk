using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class LeaderBoardController : MonoBehaviour
{

    [System.Serializable]
    private class PlayerScore
    {
        public PlayerScore(string username, int userScore)
        {
            name = username;
            score = userScore;
        }
        public string name;
        public int score;
    }

    private class Scores
    {
        public List<PlayerScore> playerScoresList;
    }

    private List<PlayerScore> playerScores;
    public GameObject playerScoreEntryPrefab;
    public GameObject playerScoreData;

    private void Awake()
    {
        // Init playerScores
        playerScores = new List<PlayerScore>();

        // Load previous saved scores
        string jsonString = PlayerPrefs.GetString("scores");
        Scores scores = JsonUtility.FromJson<Scores>(jsonString);
        if(scores == null)
        {
            scores = new Scores();
        }
        playerScores = scores.playerScoresList;
        if(playerScores == null)
        {
            playerScores = new List<PlayerScore>();
        }
    }

    private void OnEnable()
    {
        ShowScores();
    }

    private void OnDisable()
    {
        DeleteScores();
    }

    public int GetScore(string username)
    {
        if (playerScores == null) return 0;

        foreach (PlayerScore playerScore in playerScores)
        {
            if (playerScore.name.Equals(username))
                return playerScore.score;
        }

        //Debug.LogWarning("No score with that username");
        return 0;
    }

    /// <summary>
    /// Adds a new score to scores
    /// </summary>
    public void SetScore(string username, int score)
    {
        if (playerScores == null) return;

        // Load saved scores
        string jsonString = PlayerPrefs.GetString("scores");
        Scores scores = JsonUtility.FromJson<Scores>(jsonString);
        if (scores == null)
        {
            scores = new Scores();
            scores.playerScoresList = new List<PlayerScore>();
        }

        // Add new score to scores
        scores.playerScoresList.Add(new PlayerScore(username, score));

        // Save updated scores
        string json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString("scores", json);
        PlayerPrefs.Save();

        // Update local variable
        playerScores = scores.playerScoresList;
    }

    /// <summary>
    /// Sorts the scores List from higher to lower score.
    /// Deletes all score gameobjects and creates them again sorted.
    /// </summary>
    public void SortByScore()
    {
        for(int i = 0; i < playerScores.Count; i++)
        {
            for (int j = i + 1; j < playerScores.Count; j++)
            {
                if(playerScores[j].score > playerScores[i].score)
                {
                    // Swap
                    PlayerScore aux = playerScores[i];
                    playerScores[i] = playerScores[j];
                    playerScores[j] = aux;
                }
            }
        }

        DeleteScores();
        ShowScores();
    }

    // [WIP]
    /// <summary>
    /// Sorts the scores List by username alphabetical order.
    /// Deletes all score gameobjects and creates them again sorted.
    /// </summary>
    public void SortByName()
    {
        DeleteScores();
        ShowScores();
    }

    /// <summary>
    /// Creates a UI element for every score.
    /// The UI element is taken from a prefab and displays the player name and score.
    /// </summary>
    public void ShowScores()
    {
        foreach(var entry in playerScores)
        {
            GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
            go.transform.SetParent(playerScoreData.transform);
            go.transform.Find("Username").GetComponent<Text>().text = entry.name;
            go.transform.Find("Score").GetComponent<Text>().text = entry.score.ToString();
        }
    }

    public void DeleteScores()
    {
        foreach (Transform child in playerScoreData.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
