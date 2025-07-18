using UnityEngine;

public class DestroyIfDuplicate : MonoBehaviour
{
    // Variabel untuk menyimpan tipe komponen yang ingin kita cek
    // Kita akan gunakan System.Type agar skrip ini bisa dipakai untuk apa saja
    [SerializeField]
    private string componentTypeToCheck; 

    private void Awake()
    {
        // Temukan semua objek dengan tipe komponen yang ditentukan di scene
        Object[] allInstances = FindObjectsOfType(System.Type.GetType(componentTypeToCheck));

        // Jika ada lebih dari satu instance...
        if (allInstances.Length > 1)
        {
            Debug.LogWarning("Duplicate " + componentTypeToCheck + " found. Destroying this instance.");
            // ...maka hancurkan GameObject tempat skrip ini menempel.
            Destroy(gameObject);
        }
    }
}