using UnityEngine;
using TMPro; // Penting untuk mengakses komponen TextMeshPro

[RequireComponent(typeof(TextMeshProUGUI))] // Otomatis menambah komponen jika belum ada
public class HoverTextColor : MonoBehaviour
{
    // Variabel untuk menyimpan referensi ke komponen teks
    private TextMeshProUGUI textMesh;

    // Atur warna-warnanya di Inspector
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private void Awake()
    {
        // Ambil komponen TextMeshProUGUI saat game dimulai
        textMesh = GetComponent<TextMeshProUGUI>();
        // Set warna awal
        SetNormalColor();
    }

    // Method PUBLIC ini yang akan kita panggil dari Event Trigger
    public void SetHighlightColor()
    {
        if (textMesh != null)
        {
            textMesh.color = highlightColor;
        }
    }

    // Method PUBLIC ini juga akan kita panggil dari Event Trigger
    public void SetNormalColor()
    {
        if (textMesh != null)
        {
            textMesh.color = normalColor;
        }
    }
}