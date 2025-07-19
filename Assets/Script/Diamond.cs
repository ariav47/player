using UnityEngine;

public class Diamond : Collectable
{
    [SerializeField] int diamondValue = 1;

    protected override void Collected(GameObject playerObject)
    {
        // 1. Tambahkan nilai diamond ke GameManager
        if (GameManager.MyInstance != null)
        {
            GameManager.MyInstance.AddDiamonds(diamondValue);
        }

        // 2. SURUH AudioManager untuk memutar suara
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDiamondCollectSound();
        }

        // 3. Panggil method dasar untuk menghancurkan objek
        base.Collected(playerObject);
    }
}