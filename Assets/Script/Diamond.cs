using UnityEngine;

// Tidak perlu menyertakan System.Collections atau System.Collections.Generic jika tidak dipakai
public class Diamond : Collectable // Pastikan nama base class sudah benar (Collectible/Collectable)
{
    [SerializeField] int diamondValue = 1;

    // PERBAIKAN: Tambahkan parameter 'GameObject playerObject' agar cocok dengan base class
    protected override void Collected(GameObject playerObject)
    {
        // 1. Jalankan logika spesifik untuk diamond
        // Pastikan Anda punya GameManager yang bisa diakses seperti ini
        if (GameManager.MyInstance != null)
        {
            GameManager.MyInstance.AddDiamonds(diamondValue);
        }

        Debug.Log("Diamond collected! Value: " + diamondValue);

        // 2. Panggil method Collected() dari base class untuk menjalankan logika umum (seperti Destroy)
        base.Collected(playerObject);
    }
}