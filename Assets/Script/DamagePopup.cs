using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float lifeTime = 1f;
    public float moveSpeed = 1.5f;

    public Color normalHitColor = Color.white;
    public Color criticalHitColor = Color.yellow;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        // === DETEKTIF UNTUK AWAKE ===
        if (textMesh == null)
        {
            Debug.LogError("DamagePopup Error: Komponen TextMeshProUGUI tidak ditemukan pada prefab!");
        }
    }

    public void Setup(int damageAmount, bool isCritical)
    {
        // === DETEKTIF UNTUK SETUP ===
        Debug.Log("DamagePopup.Setup dipanggil. Damage: " + damageAmount + ", Critical: " + isCritical);

        if (textMesh != null)
        {
            textMesh.text = damageAmount.ToString();
            if (isCritical)
            {
                textMesh.color = criticalHitColor;
                textMesh.fontSize *= 1.5f;
            }
            else
            {
                textMesh.color = normalHitColor;
            }
        }
        else
        {
            Debug.LogError("DamagePopup Error: Gagal mengatur teks karena textMesh adalah null.");
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }
}