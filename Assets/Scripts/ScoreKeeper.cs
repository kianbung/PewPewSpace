using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    private static int score;
    private Text uiScore;

    void Start() {
        // unnecessary cause (I assume) is attached to same gameObject, but leaving this here for reference
        uiScore = GetComponent<Text>();
        uiScore.text = score.ToString();
        ResetScore();
    }

    public void UpdateScore(int points) {
        score += points;
        uiScore.text = score.ToString();
    }

    public void ResetScore() {
        score = 0;
    }

    // for testing
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            UpdateScore(99);
        } else if (Input.GetKeyDown(KeyCode.O)) {
            ResetScore();
            UpdateScore(0);
        }
    }

}
