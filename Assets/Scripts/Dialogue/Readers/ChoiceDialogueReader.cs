using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChoiceDialogueReader : DialogueReader
{
    private Dictionary<string, ChoiceDialogueDisplay> dialogueDisplays;
    private const string dialogueDisplayerPrefabName = "ChoiceDialogueDisplayer";
    private const string dialogueReaderPrefabName = "ChoiceDialogueReader";

    public static ChoiceDialogueReader Instance
    {
        get
        {
            ChoiceDialogueReader instance = GetInstance<ChoiceDialogueReader>();
            if (instance == null)
            {
                instance = Instantiate(
                    Resources.Load<ChoiceDialogueReader>(dialogueReaderPrefabName)
                );
            }
            return instance;
        }
    }

    public ChoiceDialogueReader()
    {
        dialogueDisplays = new Dictionary<string, ChoiceDialogueDisplay>();
        RegisterInstance(this);
    }

    public override DialogueDisplay CreateDialogueDisplay(string character)
    {
        if (dialogueDisplays.ContainsKey(character))
        {
            return dialogueDisplays[character];
        }

        ChoiceDialogueDisplay newDisplay = Instantiate(
            Resources.Load<ChoiceDialogueDisplay>(dialogueDisplayerPrefabName)
        );
        dialogueDisplays[character] = newDisplay;
        return newDisplay;
    }
}
