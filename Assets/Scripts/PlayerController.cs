using UnityEngine;

using UnityEngine.InputSystem;

//todo replay and quit option
public class PlayerController : MonoBehaviour
{
    //CACHE
    [Header("CACHE")]
    [SerializeField]
    private InputAction m_movement = null;
    [SerializeField] 
    private InputAction m_fire = null;
    [SerializeField] 
    private InputAction m_restart = null;
    [SerializeField] 
    private InputAction m_quit = null;
    
    [SerializeField] [Tooltip("Add all player lasers here")]
    private ParticleSystem[] m_lasers;
    [SerializeField]
    private AudioClip m_laserSFX = null;

    private AudioSource m_audioSource  = null;

    //PARAMETERS
    [Space(10)] [Header("PROPERTIES")]
    [Header("General Setup Settings")]
    [SerializeField] [Tooltip("How fast ship moves up and down")] 
    private float m_movementOffset = 30f;
    [SerializeField][Tooltip("How far player moves horizontally")]
    private float m_positionRangeX = 10f;
    [SerializeField][Tooltip("How far  player moves vertically")]
    private float m_positionRangeY = 7f;

    //rotation
    [Header("Screen position based tuning")]
    [SerializeField]
    private float m_positionPitchFactor = -2f;
    [SerializeField]
    private float m_controlPitchFactor = -15f;
    [Header("Player input based tuning")]
    [SerializeField]
    private float m_positionYawFactor = 2f;
    [SerializeField]
    private float m_controlRollFactor = -20f;

    //STATES
    private Vector2 m_InputThrow = Vector2.zero;
    private Vector2 m_DampedPosition = Vector2.zero;
    //////////////////////////////////////////////////////////////////

    private void Awake() 
    {
        m_audioSource = GetComponent<AudioSource>();

        UnityEngine.Assertions.Assert.IsNotNull(m_laserSFX, "m_laserSFX is null");        
        UnityEngine.Assertions.Assert.IsNotNull(m_audioSource, "AudioSource is null");        
    }

    private void OnEnable() {
        m_movement.Enable();
        m_fire.Enable();
        m_restart.Enable();
        m_quit.Enable();
    }

    private void OnDisable() {
        m_movement.Disable();
        m_fire.Disable();
        m_restart.Disable();
        m_quit.Disable(); 

        SetLasersActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();

        if(FindObjectOfType<MasterTimeline>().GetIsEndOfTimeline())
            ProcessEndOfGame();

        ProcessGodMode();
    }

    bool m_endGameMenuShown = false;
    void ProcessEndOfGame()
    {
        if(!m_endGameMenuShown)
        {
            m_endGameMenuShown = true;
            FindObjectOfType<PlayerScore>().ShowEndGameMenu();
            FindObjectOfType<HighestScore>().SetHighestScore(FindObjectOfType<PlayerScore>().GetCurrentScore());
        }

        if(m_restart.ReadValue<float>() > .5f)
        {
            RestartScene();
        }
        else if(m_quit.ReadValue<float>() > .5f)
        {
            QuitGame();
        }
    }

    void RestartScene()
    {
        string currentScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScenePath);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ProcessGodMode()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if(keyboard.ctrlKey.IsPressed() && keyboard.gKey.wasPressedThisFrame)
            GetComponent<BoxCollider>().enabled = !GetComponent<BoxCollider>().enabled;
#endif
    }
    private void ProcessTranslation()
    {

        m_InputThrow = m_movement.ReadValue<Vector2>();

        Vector2 tempv2 = Vector2.zero;
        m_DampedPosition = Vector2.SmoothDamp(m_DampedPosition, m_InputThrow, ref tempv2, .1f);

        Vector3 newLocalPosition = transform.localPosition;
        float rawXPos = transform.localPosition.x + m_DampedPosition.x * Time.deltaTime * m_movementOffset;
        newLocalPosition.x = Mathf.Clamp(rawXPos, -m_positionRangeX, m_positionRangeX);        

        float rawYPos = transform.localPosition.y + m_DampedPosition.y * Time.deltaTime * m_movementOffset;
        newLocalPosition.y = Mathf.Clamp(rawYPos, -m_positionRangeY, m_positionRangeY);

        transform.localPosition = newLocalPosition;       
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * m_positionPitchFactor;
        float pitchDueToControlThrow = m_DampedPosition.y * m_controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        // float pitch =0 ;
        float yaw = transform.localPosition.x * m_positionYawFactor;
        float roll = m_DampedPosition.x * m_controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessFiring()
    {
        if(m_fire.ReadValue<float>() > 0.5f)
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    private void SetLasersActive(bool active)
    {
        foreach (var laser in m_lasers)
        {
            var emission = laser.emission;
            emission.enabled = active;
        }

        if(m_audioSource.clip == m_laserSFX && active == false)
        {
            m_audioSource.Stop();
            m_audioSource.clip = null;
        }
        if(m_audioSource.clip != m_laserSFX && active == true)
        {
            m_audioSource.clip = m_laserSFX;
            m_audioSource.loop = true;
            m_audioSource.volume = .075f;
            m_audioSource.Play();            
        }
        
    }

    private float m_maxRotationPerSecond = 50f;
    System.Collections.IEnumerator ProcessRotationOverTime(Vector3 endRotation)
    {
        yield return new WaitForEndOfFrame();
    }
}
