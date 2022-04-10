using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_explosionEffect = null;
    [SerializeField]
    private AudioClip m_explosionClip = null;

    private AudioSource m_audioSource  = null;

    [SerializeField]
    PlayerScore m_playerScore = null;
    HighestScore m_highestScore = null;

    private void Awake() 
    {
        m_audioSource = GetComponent<AudioSource>();
        m_highestScore = FindObjectOfType<HighestScore>();

        UnityEngine.Assertions.Assert.IsNotNull(m_explosionEffect, "explosion visual effect is null");
        UnityEngine.Assertions.Assert.IsNotNull(m_audioSource, "explosion sound effect is null");
        UnityEngine.Assertions.Assert.IsNotNull(m_explosionClip, "m_explosionClip is null");
        UnityEngine.Assertions.Assert.IsNotNull(m_playerScore, "m_playerScore is null");
        UnityEngine.Assertions.Assert.IsNotNull(m_highestScore, "m_highestScore is null");
    }

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerDeath();
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy)
        {
            // enemy.DestroyEnemy();
        }       
    }

    private void OnPlayerDeath()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        m_explosionEffect.Play();
        m_audioSource.volume = .15f;
        m_audioSource.clip = m_explosionClip;
        m_audioSource.loop = false;
        m_audioSource.Play();

        // CustomDebug.Log($"highest score: {FindObjectOfType<HighestScore>()?.ToString()}, playerscore: {FindObjectOfType<HighestScore>()?.ToString()}");
        m_highestScore.SetHighestScore(m_playerScore.GetCurrentScore());
        // Debug.Log($"clip: {this.m_audioSource.clip.ToString()}, volume: {this.m_audioSource.volume.ToString()}");
        StartCoroutine(DelayReload(1f));
    }

    System.Collections.IEnumerator DelayReload(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        string currentScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScenePath);
    }
}
