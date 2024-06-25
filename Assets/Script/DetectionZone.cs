using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    public System.Action<Collider2D> onPlayerAttackDetected;

    private void Awake()
    {
        detectedColliders = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectedColliders.Add(collision);
            onPlayerAttackDetected?.Invoke(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectedColliders.Remove(collision);
        }
    }
}
