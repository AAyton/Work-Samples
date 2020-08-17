using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu : MonoBehaviour
{

    [SerializeField]
    private Button[] _btnChoices;

    [SerializeField]
    private Button _btnLeaveCharacter;

    [SerializeField]
    private DialogueManager _dialogueManager;

    [SerializeField]
    private Canvas _buildingCanvas;

    private DialogueContainer _currentContainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void open(DialogueContainer newContainer)
    {
        _currentContainer = newContainer;

        for (int a = 0; a < _btnChoices.Length; a++)
        {
            _btnChoices[a].onClick.RemoveAllListeners();
        }
        _btnLeaveCharacter.onClick.RemoveAllListeners();

        
        _btnLeaveCharacter.onClick.AddListener(() => leaveCharacter());

        if (_btnChoices != null)
        {
            for (int a = 0; a < _btnChoices.Length; a++)
            {
                // passing a directly into choiceInput updates the listener as a increments
                int characterIndex = a;

                _btnChoices[a].onClick.AddListener(() => _dialogueManager.choiceInput(characterIndex));

            }
        }


        Text[] choices = new Text[_btnChoices.Length];

        for (int i = 0; i < choices.Length; i++)
        {
             choices[i] = _btnChoices[i].gameObject.GetComponentInChildren<Text>();
        }

        _dialogueManager.OpenDialogue(_currentContainer,choices);
    }

    void leaveCharacter()
    {
        GetComponent<Canvas>().gameObject.SetActive(false);
        _dialogueManager.clearChoices();
        _buildingCanvas.gameObject.SetActive(true);

    }
}
