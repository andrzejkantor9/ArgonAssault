using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private void Awake() 
    {
        var allSingletons = FindObjectsOfType<Singleton>(true);
        int thisSingletonCount = 0;

        foreach (var singleton in allSingletons)
        {
            if(singleton.gameObject.name == this.gameObject.name)
                ++thisSingletonCount;
        }

        if(thisSingletonCount > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
}
