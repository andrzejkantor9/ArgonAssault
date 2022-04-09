using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_explosionEffect = null;
    [SerializeField]
    private AudioSource m_audioSource  = null;

    private void Awake() 
    {
        UnityEngine.Assertions.Assert.IsNotNull(m_explosionEffect, "explosion visual effect is null");
        UnityEngine.Assertions.Assert.IsNotNull(m_audioSource, "explosion sound effect is null");
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
        m_audioSource.Play();
        StartCoroutine(DelayReload(1f));
    }

    System.Collections.IEnumerator DelayReload(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        string currentScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScenePath);
    }
}
