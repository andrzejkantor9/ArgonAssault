using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int m_score = 0;
    [SerializeField]
    private TMPro.TMP_Text m_scoreText = null;

    [SerializeField]
    private GameObject m_endGameMenu = null;

    private void Awake() {
        m_scoreText = GetComponent<TMPro.TMP_Text>();
        m_scoreText.text = "Score";
    }

    public void IncreaseScore(int amountToIncrease)
    {
        m_score += amountToIncrease;
        m_scoreText.text = m_score.ToString();
        // Debug.Log($"current score: {m_score.ToString()}");
    }

    public int GetCurrentScore()
    {
        return m_score;
    }

    public void ShowEndGameMenu()
    {
        m_endGameMenu.SetActive(true);
    }
}
