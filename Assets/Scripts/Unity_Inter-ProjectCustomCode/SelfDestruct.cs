using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]
    private float m_destroyDelay = 1f;

    private void Start() {
        Destroy(this.gameObject, m_destroyDelay);
    }
}
