using UnityEngine;

public class HealthCollectible : Collectable // Mewarisi dari Collectible
{
    [SerializeField] private int healAmount = 20;

    // Override method Collected untuk memberikan logika spesifik
    protected override void Collected(GameObject playerObject)
    {
        // Ambil komponen HealthManager dari player yang mengambil item
        HealthManager healthManager = playerObject.GetComponent<HealthManager>();
        
        if (healthManager != null)
        {
            // Panggil method Heal
            healthManager.Heal(healAmount);
            Debug.Log("Player healed by " + healAmount);
        }

        // Panggil method dasar untuk menghancurkan objek item ini
        base.Collected(playerObject);
    }
}