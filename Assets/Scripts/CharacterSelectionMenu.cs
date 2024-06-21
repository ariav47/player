using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionMenu : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    private int currentAnime;

    private void Awake()
    {
        SelectAnime(0);
    }

    private void SelectAnime(int _index)
    {
        nextButton.interactable = (_index != transform.childCount-1);
        previousButton.interactable = (_index != 0); 
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }

    public void ChangeAnime(int _change)
    {
        currentAnime += _change;
        SelectAnime(currentAnime);
    }
}
