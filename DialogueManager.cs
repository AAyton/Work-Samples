using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private Text _dialogue;
    
    private Text[] _choiceText;
    private DialogueContainer _currentContainer;
    private List<NodeLinkData> _link;
    private List<ExposedProperty> _conditions;
    private List<DialogueNodeData> _textData;
    private List<NodeLinkData> _currentChoices;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   public void OpenDialogue(DialogueContainer dialogueContainer, Text[] choices)
    {
        _currentContainer = dialogueContainer;
        _link = _currentContainer.NodeLinks;
        _conditions = _currentContainer.ExposedProperties;
        _textData = _currentContainer.DialogueNodeData;
        _choiceText = choices;
       

        _currentChoices = new List<NodeLinkData>();

        int listSize = _textData.Count;
        // Dialogue.text = textData[listSize - 1].DialogueText;
        _dialogue.text = _textData[0].DialogueText;
        // updateChoices(textData[listSize - 1].NodeGUID);
        updateChoices(_textData[0].NodeGUID);
    }

     void updateChoices(string baseNodeGuid)
    {
        // choices should be cleared before adding new chocies
        clearChoices();
        bool validChoice = true;
        //Update choice text to show new available choices
        for (int i = 0; i < _link.Count; i++)
        {

            //for all local link nodes 
            if (_link[i].BaseNodeGUID == baseNodeGuid)
            {
                NodeLinkData targetLink = _link[i];
                //if it has lock check
                if (targetLink.lockChecks != null)
                {
                    //for all lock checks
                    for (int a = 0; a < targetLink.lockChecks.Length; a++)
                    {
                        //for all nodes in container targeted by lock check
                        for (int b = 0; b < targetLink.lockChecks[a].targetContainer.NodeLinks.Count; b++)
                        {

                            //check if node is the required node
                            if (targetLink.lockChecks[a].targetContainer.NodeLinks[b].PortName == targetLink.lockChecks[a].CheckNodePortName)
                            {
                                //check that required node have been used and no forbidden nodes have been used
                                if (targetLink.lockChecks[a].targetContainer.NodeLinks[b].used != targetLink.lockChecks[a].used)
                                {
                                    validChoice = false;
                                }
                            }
                        }
                    }
                }




                if (validChoice)
                {
                    _currentChoices.Add(_link[i]);
                }
            }
        }


        for (int i = 0; i < _currentChoices.Count; i++)
        {

            _choiceText[i].text = _currentChoices[i].PortName;
        }
    }

    public void clearChoices()
    {
        _currentChoices.Clear();
        for (int i = 0; i < _choiceText.Length; i++)
        {
            _choiceText[i].text = "";
        }
    }

    public void choiceInput(int tempChoice)
    {

        NodeLinkData choice = _currentChoices[tempChoice];

        //Go through lock targets and update lock and disable status
        choice.used = true;


        //Update text display to show new target node
        for (int i = 0; i < _textData.Count; i++)
        {

            if (_textData[i].NodeGUID == choice.TargetNodeGuid)
            {
                _dialogue.text = _textData[i].DialogueText;
                Debug.Log(_dialogue.text);
               
                updateChoices(_textData[i].NodeGUID);
            }
        }


    }
}
