using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int collectedDiamonds;
    [SerializeField] private int winCondition = 3;

    private static GameManager instance;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).Name);
                    instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton); // Optional: Jika ingin tetap ada di antara pergantian scene
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Jika ingin tetap ada di antara pergantian scene
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
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

    public void Finish()
    {
        Debug.Log("Finish method called");
        if (collectedDiamonds >= winCondition)
        {
            Debug.Log("Collected Diamonds: " + collectedDiamonds + ". Loading Level 2.");
            SceneManager.LoadScene("Level 2");
        }
        else
        {
            Debug.Log("Not enough diamonds. Collected: " + collectedDiamonds + " / " + winCondition);
            UIManager.MyInstance.ShowWinCondition(collectedDiamonds, winCondition);
        }
    }
}
