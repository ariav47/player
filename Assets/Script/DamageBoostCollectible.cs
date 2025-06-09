using UnityEngine;

public class DamageBoostCollectible : Collectable // Mewarisi dari Collectible
{
    [SerializeField] private float damageBonus = 15f;
    [SerializeField] private float duration = 10f;

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

        // Panggil method dasar untuk menghancurkan objek item ini
        base.Collected(playerObject);
    }
}