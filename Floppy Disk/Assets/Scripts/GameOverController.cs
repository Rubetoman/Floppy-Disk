using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverController : MonoBehaviour {

    public Text ScoreText;
    public LeaderBoardController leaderBoard;

    private void OnEnable()
    {
        ScoreText.text = "Score: " + GameManager.Instance.GetScore().ToString();
    }

    public void AddScore(string name)
    {
        leaderBoard.SetScore(name, GameManager.Instance.GetScore());
    }
}
