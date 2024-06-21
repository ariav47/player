using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    public System.Action<Collider2D> onPlayerAttackDetected;

    private void Awake()
    {
        // Initialize list of detected colliders
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);
        if (collision.CompareTag("PlayerAttack"))
        {
            onPlayerAttackDetected?.Invoke(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
    }
}
