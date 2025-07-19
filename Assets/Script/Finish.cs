using UnityEngine;
using UnityEngine.SceneManagement; // Kita tetap butuh ini untuk contoh di bawah

public class Finish : MonoBehaviour
{
    [Header("Level Configuration")]
    [Tooltip("Jumlah item yang dibutuhkan untuk menyelesaikan level ini.")]
    public int winCondition = 3;

    [Tooltip("Nama scene yang akan dimuat setelah level ini selesai.")]
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached the finish point for level: " + gameObject.scene.name);

            // Cek apakah GameManager ada
            if (GameManager.MyInstance != null)
            {
                // Berikan data dari level ini (yang diatur di Inspector) ke GameManager
                GameManager.MyInstance.CheckLevelCompletion(winCondition, nextSceneName);
            }
            else
            {
                Debug.LogError("GameManager instance not found!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Panggil UIManager untuk menyembunyikan pesan (jika ada)
            if (UIManager.MyInstance != null)
            {
                UIManager.MyInstance.HideWinCondition();
            }
        }
    }
}