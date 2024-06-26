using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Periksa nama adegan saat ini
            if (SceneManager.GetActiveScene().name == "Ending")
            {
                // Panggil metode LoadHomeScene jika adegan saat ini adalah "Ending"
                GameManager.MyInstance.LoadHomeScene();
            }
            else
            {
                GameManager.MyInstance.Finish();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIManager.MyInstance.HideWinCondition();
        }
    }
}
