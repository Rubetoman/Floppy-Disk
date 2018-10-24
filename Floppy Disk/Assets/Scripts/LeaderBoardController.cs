﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class LeaderBoardController : MonoBehaviour {

    Dictionary<string, int> playerScores;
    public GameObject playerScoreEntryPrefab;
    public GameObject playerScoreData;

    private void Awake()
    {
        SetScore("AAA", 10);
        SetScore("ABB", 0);
        SetScore("Ruben", 999);
        SetScore("adsad", 112);
        SetScore("NAS", 8);
        SetScore("BBB", 232);
        SetScore("ADS", 1);
        SetScore("AAA", 10);
        SetScore("AAA", 22);
    }

    void Init () {
        if (playerScores != null)
            return;

        playerScores = new Dictionary<string, int>();
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
        Init();
        if (playerScores.ContainsKey(username))
        {
            return playerScores[username];
        }
        else
        {
            Debug.LogWarning("No score with that username");
            return 0;
        }
    }

    public void SetScore(string username, int score)
    {
        Init();
        playerScores[username] = score;
    }

    public void OrderByScore()
    {
        var items = from entry in playerScores
                    orderby entry.Value descending
                    select entry;

        Dictionary<string, int> auxDictionary = new Dictionary<string, int>();

        foreach (KeyValuePair<string,int> entry in items)
        {
            auxDictionary.Add(entry.Key, entry.Value);
        }

        playerScores = auxDictionary;
        DeleteScores();
        ShowScores();
    }

    public void OrderByName()
    {
        var list = playerScores.Keys.ToList();
        list.Sort();

        Dictionary<string, int> auxDictionary = new Dictionary<string, int>();

        foreach (var entry in list)
        {
            auxDictionary.Add(entry, playerScores[entry]);
        }

        playerScores = auxDictionary;
        DeleteScores();
        ShowScores();
    }

    public void ShowScores()
    {
        foreach(var entry in playerScores)
        {
            GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
            go.transform.SetParent(playerScoreData.transform);
            go.transform.Find("Username").GetComponent<Text>().text = entry.Key;
            go.transform.Find("Score").GetComponent<Text>().text = entry.Value.ToString();
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
