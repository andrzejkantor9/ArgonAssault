using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MasterTimeline : MonoBehaviour
{
    [SerializeField][HideInInspector]
    private PlayableDirector m_playableDirector = null;

    bool m_isEndOfTimeline = false;

    void OnValidate() {
        m_playableDirector = GetComponent<PlayableDirector>(); 
    }

    private void Update() {
        // CustomDebug.Log($"play state: {this.m_playableDirector.state.ToString()}");
        if(!m_isEndOfTimeline && Mathf.Approximately((float)m_playableDirector.time, (float)m_playableDirector.duration))
            m_isEndOfTimeline = true;
    }

    void ReplayTimeline()
    {            
        m_playableDirector.time = 5.5f;
    }

    public bool GetIsEndOfTimeline()
    {
        return m_isEndOfTimeline;
    }
}
