using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerSingleton : MonoBehaviour
{
    private void Awake() 
    {
        int musicPlayersCount = FindObjectsOfType<MusicPlayerSingleton>().Length;
        if(musicPlayersCount > 1)
        {
            Destroy(gameObject);
        }            
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
