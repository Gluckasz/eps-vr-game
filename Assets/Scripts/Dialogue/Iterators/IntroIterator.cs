using System;
using UnityEngine;

public class IntroIterator : DialogueIterator
{
    private readonly Intro intro_;

    public IntroIterator(Intro intro)
    {
        intro_ = intro;
    }

    public DialogueNode GetNode(string nodeId)
    {
        if (HasMore(nodeId))
        {
            return intro_.dialogueNodes_[nodeId];
        }
        else
        {
            throw new IndexOutOfRangeException("There are no more dialogue nodes to process");
        }
    }

    public bool HasMore(string nodeId)
    {
        return intro_.dialogueNodes_.ContainsKey(nodeId);
    }
}
