using UnityEngine;

// Nama class diubah menjadi Collectible (dengan i) agar konsisten dengan saran sebelumnya, 
// tapi Anda bisa tetap menggunakan Collectable (dengan a) jika mau.
public class Collectable : MonoBehaviour 
{
    // Kita tambahkan SpriteRenderer untuk efek visual saat diambil
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Kirim informasi GameObject player ke method Collected
            Collected(collision.gameObject);
        }
    }

    // Method ini sekarang menerima GameObject yang mengambilnya
    protected virtual void Collected(GameObject playerObject)
    {
        // Logika dasar setelah item diambil adalah menghancurkan dirinya sendiri.
        // Skrip turunan bisa menambahkan logikanya sebelum memanggil ini.
        Destroy(gameObject);
    }
}