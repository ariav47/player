using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int diamonds;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    private static GameManager instance;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();

            return instance;
        }
    }

    public void AddDiamonds(int _diamonds)
    {
        diamonds += _diamonds;
        Debug.Log(diamonds);
    }
}
