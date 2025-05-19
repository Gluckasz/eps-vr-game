using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BasicDialogueReader : DialogueReader
{
    private Dictionary<string, BasicDialogueDisplay> dialogueDisplays;
    private const string dialogueDisplayerPrefabName = "BasicDialogueDisplayer";
    private const string dialogueReaderPrefabName = "BasicDialogueReader";

    public static BasicDialogueReader Instance
    {
        get
        {
            BasicDialogueReader instance = GetInstance<BasicDialogueReader>();
            if (instance == null)
            {
                instance = Instantiate(
                    Resources.Load<BasicDialogueReader>(dialogueReaderPrefabName)
                );
            }
            return instance;
        }
    }

    public BasicDialogueReader()
    {
        dialogueDisplays = new Dictionary<string, BasicDialogueDisplay>();
        RegisterInstance(this);
    }

    public override DialogueDisplay CreateDialogueDisplay(string character)
    {
        if (dialogueDisplays.ContainsKey(character))
        {
            return dialogueDisplays[character];
        }

        BasicDialogueDisplay newDisplay = Instantiate(
            Resources.Load<BasicDialogueDisplay>(dialogueDisplayerPrefabName)
        );
        dialogueDisplays[character] = newDisplay;
        return newDisplay;
    }
}
