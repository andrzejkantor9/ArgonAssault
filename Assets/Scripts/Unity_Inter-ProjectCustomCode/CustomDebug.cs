using UnityEngine;

//todo custom debug gui log on screen this frame / for time

//todo refactor fps counter code
//todo scale gui according to screen
//todo fix - f11 has to be pressed twice on window minimize
//todo - namescape CustomDebug and separate classes

//todo - on start play change layout and change it back on end play
public class CustomDebug : MonoBehaviour 
{
    GUIStyle textStyle = new GUIStyle();

    ///////////////////////////////////////////////

    private void Awake() 
    {
        SetupFpsCounter();
    }

    void Update()
    {
        UnityEngine.Profiling.Profiler.BeginSample($"{GetType().ToString()}: Update");

        ProcessMaximizeEditorPlayWindow();
        ProcessFpsCounter();      
        ProcessToggleTimescale();  

        UnityEngine.Profiling.Profiler.EndSample();
    }
    
    void OnGUI()
    {
        DisplayFps();
    }

#region MidLevelCode

    public static void Log(string message)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        UnityEngine.Debug.Log(message);
#endif
    } 

#endregion

#region FpsCounter
    private float m_fpsCounterUpdateInterval = 0.5f; 

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    private void DisplayFps()
    {
        //Display the fps and round to 2 decimals
        GUI.Label(new Rect(5, 5, 100, 25), fps.ToString("F2") + "FPS", textStyle);
    }

    private void ProcessFpsCounter()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = (accum / frames);
            timeleft = m_fpsCounterUpdateInterval;
            accum = 0.0f;
            frames = 0;
        }

        m_lastUpdateTime = Time.time;
    }

    private void SetupFpsCounter()
    {
        timeleft = m_fpsCounterUpdateInterval;

        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    float m_lastUpdateTime = 0f;
#endregion

#region MaximizeMinizePlayWindow

    private void ProcessMaximizeEditorPlayWindow()
    {
#if UNITY_EDITOR
        UnityEngine.InputSystem.Keyboard keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null)
        {
            if (UnityEditor.EditorApplication.isPlaying && keyboard.f11Key.wasPressedThisFrame)
            {
                UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
            }
        }
#endif
    }

#endregion

#region ToggleTimeScale
    float[] m_timeScaleValues = {1f, .5f, 2f, 5f};
    int m_currentTimeScaleValue = 0;

    void ProcessToggleTimescale()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        UnityEngine.InputSystem.Keyboard keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null)
        {
            if (UnityEditor.EditorApplication.isPlaying && keyboard.ctrlKey.isPressed && keyboard.tKey.wasReleasedThisFrame)
            {
                ++m_currentTimeScaleValue;
                if(m_currentTimeScaleValue >= m_timeScaleValues.Length)
                    m_currentTimeScaleValue = 0;
                Time.timeScale = m_timeScaleValues[m_currentTimeScaleValue];
            }
        }    
#endif    
    }
#endregion
}