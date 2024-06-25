using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueCanvas;
    [SerializeField]
    private TMP_Text speakerText;
    [SerializeField]
    private TMP_Text dialogueText;
    [SerializeField]
    private Image potraitImage;
    [SerializeField]
    private string[] speaker;
    [SerializeField]
    [TextArea]
    private string[] dialogueWords;
    [SerializeField]
    private Sprite[] potrait;

    private bool dialogueActivated;
    private int step;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && dialogueActivated == true)
        {
            if (step >= speaker.Length)
            {
                dialogueCanvas.SetActive(false);
                step = 0;
            }
            else
            {
            dialogueCanvas.SetActive(true);
            speakerText.text = speaker[step];
            dialogueText.text = dialogueWords[step];
            potraitImage.sprite = potrait[step];
            step += 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueActivated = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueActivated = false;
        dialogueCanvas.SetActive(false);
    }
}
