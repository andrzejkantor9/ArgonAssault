using UnityEngine;

using UnityEngine.InputSystem;

//todo use some math for movement smothening
//todo use some math for rotation smothening (or input system processors)
//todo enable up down rotation again
//todo ensure controller works
public class PayerController : MonoBehaviour
{
    //CACHE
    [SerializeField] 
    private InputAction movement = null;
    [SerializeField] 
    private InputAction m_movement = null;
    [SerializeField] 
    private InputAction m_fire = null;
    [SerializeField]
    private GameObject[] m_lasers;

    //PARAMETERS
    //position
    [SerializeField] 
    private float m_movementOffset = 30f;
    [SerializeField]
    private float m_positionRangeX = 10f;
    [SerializeField]
    private float m_positionRangeY = 7f;

    //rotation
    [SerializeField]
    private float m_positionPitchFactor = -2f;
    [SerializeField]
    private float m_controlPitchFactor = -15f;
    [SerializeField]
    private float m_positionYawFactor = 2f;
    [SerializeField]
    private float m_controlRollFactor = -20f;

    //STATES
    private float m_horizontalThrow = 0f, m_verticalThrow = 0f;
    //////////////////////////////////////////////////////////////////

    private void OnEnable() {
        movement.Enable();
        m_fire.Enable();
    }

    private void OnDisable() {
        movement.Disable();
        m_fire.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessTranslation()
    {
        m_horizontalThrow = movement.ReadValue<Vector2>().x;
        m_verticalThrow = movement.ReadValue<Vector2>().y;

        Vector3 newLocalPosition = transform.localPosition;
        float rawXPos = transform.localPosition.x + m_horizontalThrow * Time.deltaTime * m_movementOffset;
        newLocalPosition.x = Mathf.Clamp(rawXPos, -m_positionRangeX, m_positionRangeX);        

        float rawYPos = transform.localPosition.y + m_verticalThrow * Time.deltaTime * m_movementOffset;
        newLocalPosition.y = Mathf.Clamp(rawYPos, -m_positionRangeY, m_positionRangeY);

        transform.localPosition = newLocalPosition;
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * m_positionPitchFactor;
        float pitchDueToControlThrow = m_verticalThrow * m_controlPitchFactor;

        // float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float pitch =0 ;
        float yaw = transform.localPosition.x * m_positionYawFactor;
        float roll = m_horizontalThrow * m_controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessFiring()
    {
        if(m_fire.ReadValue<float>() > 0.5f)
        {
            ActivateLasers();
        }
        else
        {
            DeactivateLasers();
        }
    }

    private void ActivateLasers()
    {
        foreach (var laser in m_lasers)
        {
            
        }
    }

    private void DeactivateLasers()
    {

    }
    
    private float m_maxRotationPerSecond = 50f;
    System.Collections.IEnumerator ProcessRotationOverTime(Vector3 endRotation)
    {
        yield return new WaitForEndOfFrame();
        //<quaternion lerp?> delta time * maxrotation * <current rotation and end rotation diff>
    }
}
