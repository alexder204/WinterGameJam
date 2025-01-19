using UnityEngine;
using TMPro;  // Include TextMeshPro namespace

public class ScoreManager : MonoBehaviour
{
    public int score = 0;  // The score
    public TextMeshProUGUI scoreText;  // Reference to the TextMeshProUGUI component

    // Method to update the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();  // Update the score UI
    }

    // Method to update the score text
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();  // Update score display
        }
    }
}
