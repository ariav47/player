using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image potraitImage;

    [Header("Dialogue Content")]
    [SerializeField] private string[] speaker;
    [SerializeField] [TextArea] private string[] dialogueWords;
    [SerializeField] private Sprite[] potrait;

    private bool isDialogueActive;
    private int step;
    private bool playerInRange; // Untuk melacak apakah player ada di dalam jangkauan

    private void Update()
    {
        Debug.Log("Dialogue Update - PlayerInRange: " + playerInRange + " | Input 'Interact' ditekan: " + Input.GetButtonDown("Interact"));

        // Pengecekan input sekarang juga memeriksa apakah player ada di jangkauan
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            Debug.Log("!!! KONDISI INTERAKSI TERPENUHI !!!");

            // Jika dialog belum aktif, mulai. Jika sudah, lanjutkan.
            if (!isDialogueActive)
            {
                StartDialogue();
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Princess disentuh oleh objek: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            // Saat player masuk, tandai bahwa ia ada dalam jangkauan
            Debug.Log("Player terdeteksi! playerInRange sekarang diatur ke true.");
            playerInRange = true;
            // Di sini kita bisa menampilkan sebuah prompt/ikon "!" di atas NPC
            // untuk menandakan bisa diajak bicara.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Saat player keluar, nonaktifkan semuanya
            playerInRange = false;
            EndDialogue();
        }
    }

    private void StartDialogue()
    {
        // === DETEKTIF UNTUK START DIALOGUE ===
        Debug.Log("StartDialogue() dipanggil.");

        isDialogueActive = true;
        step = 0;

        if (dialogueCanvas == null)
        {
            Debug.LogError("GAGAL: Referensi 'dialogueCanvas' di Inspector kosong (null)!");
            return; // Hentikan eksekusi jika canvas null
        }

        dialogueCanvas.SetActive(true);
        Debug.Log("dialogueCanvas.SetActive(true) sudah dipanggil.");
        ShowNextLine(); // Tampilkan baris pertama
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
    }

    private void ShowNextLine()
    {
        // === DETEKTIF UNTUK SHOW NEXT LINE ===
        Debug.Log("ShowNextLine() dipanggil pada step: " + step);
    
        if (dialogueWords.Length == 0)
        {
            Debug.LogError("GAGAL: Array 'dialogueWords' kosong! Tidak ada dialog untuk ditampilkan.");
            EndDialogue();
            return;
        }
        
        if (step >= dialogueWords.Length)
        {
            Debug.Log("Dialog selesai. Memanggil EndDialogue().");
            EndDialogue();
            return;
        }
    
        // Pengecekan null untuk setiap elemen UI sebelum digunakan
        if (speakerText == null || dialogueText == null || potraitImage == null)
        {
            Debug.LogError("GAGAL: Salah satu referensi UI (speakerText, dialogueText, potraitImage) kosong!");
            return;
        }
    
        Debug.Log("Menampilkan dialog baris ke-" + (step + 1) + ": '" + dialogueWords[step] + "'");
        speakerText.text = speaker[step];
        dialogueText.text = dialogueWords[step];
        potraitImage.sprite = potrait[step];
        
        step++;
    }
}