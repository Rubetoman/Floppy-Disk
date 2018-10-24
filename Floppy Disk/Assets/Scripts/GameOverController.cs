using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverController : MonoBehaviour {

    public Text ScoreText;

    private void OnEnable()
    {
        ScoreText.text = "Score: " + GameManager.Instance.GetScore().ToString();
    }
}
