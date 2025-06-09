using UnityEngine;

public class DamageBoostCollectible : Collectable // Mewarisi dari Collectible
{
    [SerializeField] private float damageBonus = 15f;
    [SerializeField] private float duration = 10f;
    [SerializeField] private AudioClip pickupSound;

    protected override void Collected(GameObject playerObject)
    {
        // Ambil komponen PlayerController (atau skrip utama player Anda)
        PlayerController playerController = playerObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // Panggil method untuk menerapkan buff damage
            playerController.ApplyDamageBuff(damageBonus, duration);
            Debug.Log("Player received a damage buff of " + damageBonus);
        }

        AudioSource playerAudio = playerObject.GetComponent<AudioSource>();

        // 3. Lakukan pengecekan untuk memastikan semuanya ada
        if (playerAudio != null && pickupSound != null)
        {
            // 4. Mainkan suara satu kali melalui AudioSource milik player
            playerAudio.PlayOneShot(pickupSound);
        }

        // Panggil method dasar untuk menghancurkan objek item ini
        base.Collected(playerObject);
    }
}