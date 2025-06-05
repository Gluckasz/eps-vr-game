using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class DialogueReader : MonoBehaviour
{
    private const string textsFolder = "Scene1Texts";
    private static Dictionary<Type, DialogueReader> instances =
        new Dictionary<Type, DialogueReader>();

    public abstract DialogueDisplay CreateDialogueDisplay(string character);

    protected static T GetInstance<T>()
        where T : DialogueReader
    {
        Type type = typeof(T);
        return instances.ContainsKey(type) ? (T)instances[type] : null;
    }

    protected static void RegisterInstance(DialogueReader instance)
    {
        Type type = instance.GetType();
        if (!instances.ContainsKey(type))
        {
            instances[type] = instance;
        }
    }

    public DialogueData ReadJsonDialogueData(string fileName)
    {
        string resourcePath = Path.Combine(textsFolder, Path.GetFileNameWithoutExtension(fileName));

        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);

        if (textAsset != null)
        {
            string jsonContent = textAsset.text;
            DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(jsonContent);

            Debug.Log("Dialogue from Resource: " + resourcePath + " loaded successfully!");

            return dialogueData;
        }
        else
        {
            Debug.LogError("Could not find JSON resource at path: " + resourcePath);
            return null;
        }
    }
}
