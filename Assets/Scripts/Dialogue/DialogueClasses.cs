using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueData
{
    public List<DialogueNode> dialogue;
}

[Serializable]
public class DialogueNode
{
    public string id;
    public string character;
    public string text;
    public string audio;
    public string nextId;
    public List<DialogueChoiceNode> choices;
}

[Serializable]
public class DialogueChoiceNode
{
    public string shortText;
    public string text;
    public string nextId;
    public string item;
}
