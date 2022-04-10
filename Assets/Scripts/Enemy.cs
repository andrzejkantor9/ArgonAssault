using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject m_deathFX = null;
    [SerializeField]
    private GameObject m_hitVFX = null;
    [SerializeField]
    private Transform m_hitVFXTransform = null;

    [SerializeField]
    private int m_scoreIncreaseValue = 15;
    [SerializeField][Tooltip("HP - each hit -1hp")]
    private int m_maxHP = 1;

    private PlayerScore m_scoreBoard = null;
    private GameObject m_runtimeSpawnParent = null;

    private bool m_isDead = false;
    private int m_curerntHP;

    private void Awake()
    {
        AssertCachedComponents();
        InitializeCachedComponents();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessParticleCollision();
    }

    private void AssertCachedComponents()
    {
        UnityEngine.Assertions.Assert.IsNotNull(m_deathFX, "death vfx is null");
        UnityEngine.Assertions.Assert.IsNotNull(m_hitVFX, "death vfx is null");
        // UnityEngine.Assertions.Assert.IsNotNull(m_runtimeSpawnParent, "m_runtimeSpawnParent is null");
    }

    private void InitializeCachedComponents()
    {
        m_scoreBoard = FindObjectOfType<PlayerScore>();
        m_curerntHP = m_maxHP;
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;

        m_runtimeSpawnParent = GameObject.FindWithTag("SpawnAtRuntime");
    }


    private void ProcessParticleCollision()
    {
        if (m_isDead)
            return;
        else
        {
            --m_curerntHP;
            if (m_curerntHP <= 0)
            {
                m_isDead = true;
                CustomDebug.Log($"{this.gameObject.name} destroyed");
                DestroyEnemy();
            }
            else
            {
                Vector3 vfxSpawnPosition = m_hitVFXTransform != null ? m_hitVFXTransform.position : transform.position;
                var vfx = Instantiate(m_hitVFX, vfxSpawnPosition, Quaternion.identity);
                vfx.transform.parent = m_runtimeSpawnParent.transform;
            }
        }
    }

    private void DestroyEnemy()
    {
        m_scoreBoard.IncreaseScore(m_scoreIncreaseValue);

        var fx = Instantiate(m_deathFX, transform.position, Quaternion.identity);
        fx.transform.parent = m_runtimeSpawnParent.transform;
        Destroy(gameObject);
    }
}
