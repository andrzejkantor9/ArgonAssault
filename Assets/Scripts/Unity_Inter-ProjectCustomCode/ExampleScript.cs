using UnityEngine;

//todo introduce delegates here
public class ExampleScript : MonoBehaviour
{
    //CACHE
    [Header("CACHE")]
    [SerializeField][HideInInspector]
    BoxCollider m_boxCollider = null;
    
    //PROPERTIES
    [Space(10)] [Header("PROPERTIES")]
    [SerializeField] [Range(0,1)] [Tooltip("to display in inspector")]
    float m_speed = 1f; 
    
    //STATES
    bool m_isDead = false;
    const string FRIENDLY_TAG = "Friendly";

    ///////////////////////////////////////////////
    //only engine methods without regions
    //only methods inside engine methods
    //methods called must be below methods calling them
    //#if DEVELOPMENT_BUILD || UNITY_EDITOR

    void OnValidate()
    {
        SetupComponents();
        AssertComponents();
    }

    void Awake() 
    {
        AssertComponents();
        Initialize();  
    }

    void Update() 
    {
        UnityEngine.Profiling.Profiler.BeginSample($"{GetType().ToString()}: Update");

        //update code here

        UnityEngine.Profiling.Profiler.EndSample();
    }

#region MidLevelCode

    void SetupComponents()
    {
        m_boxCollider = GetComponent<BoxCollider>();
    }

    void AssertComponents()
    {
        UnityEngine.Assertions.Assert.IsNotNull(m_boxCollider, $"Script: {GetType().ToString()} variable m_boxCollider is null");
    }

    /// <summary>
    /// Setup on awake
    /// </summary>
    /// <param name="other">No params here</param>
    bool Initialize()
    {
        string exampleName = string.Empty;
        return true;
    }
    /// ?return? <>

#endregion

#region LowLevelCode
#endregion

#region RegionsForSpecificPartOfFunctionality_QuestionMark
#endregion

}