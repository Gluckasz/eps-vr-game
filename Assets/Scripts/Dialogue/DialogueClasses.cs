using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueData
{
    public List<DialogueNode> intro;
    public List<DialogueNode> scene;
}

[Serializable]
public class DialogueNode
{
    public string id;
    public string character;
    public string text;
    public string audio;
    public string nextId;
    public List<DialogueChoice> choices;
}

[Serializable]
public class DialogueChoice
{
    public string shortText;
    public string text;
    public string nextId;
    public string item;
}
