using UnityEngine;

public class HighestScore : MonoBehaviour
{
    int m_highestScore = 0;
    [SerializeField][HideInInspector]
    TMPro.TMP_Text m_scoreText = null;

    // public static HighestScore s_Instance = null;

    void OnValidate()
    {
        SetupComponents();
        AssertComponents();
    }

    void Awake()
    {
        AssertComponents();

        // s_Instance = this;
        m_scoreText.text = "High";
    }

    void SetupComponents()
    {
        m_scoreText = GetComponent<TMPro.TMP_Text>();
    }

    void AssertComponents()
    {
        UnityEngine.Assertions.Assert.IsNotNull(m_scoreText, "m_scoreText is null");
    }

    public void SetHighestScore(int currentScore)
    {
        m_highestScore = m_highestScore < currentScore ? currentScore : m_highestScore;
        m_scoreText.text = m_highestScore.ToString();
    }
}
