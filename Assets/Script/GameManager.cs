using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int collectedDiamonds, winCondition = 3;
    
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

    private void Start()
    {
        UIManager.MyInstance.UpdateDiamondUI(collectedDiamonds, winCondition);
    }

    public void AddDiamonds(int _diamonds)
    {
        collectedDiamonds += _diamonds;
        UIManager.MyInstance.UpdateDiamondUI(collectedDiamonds, winCondition);
    }
}
