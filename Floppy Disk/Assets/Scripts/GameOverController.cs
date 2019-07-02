using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverController : MonoBehaviour
{

    public Text ScoreText;
    public LeaderBoardController leaderBoard;

    private void OnEnable()
    {
        ScoreText.text = "Score: " + GameManager.Instance.GetScore().ToString();
    }

    /// <summary>
    /// Adds score to Leaderboard
    /// </summary>
    /// <param name="name"> Name of the player given by input</param>
    public void AddScore(string name)
    {
        leaderBoard.SetScore(name, GameManager.Instance.GetScore());
    }
}
