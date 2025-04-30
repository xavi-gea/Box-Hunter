using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;

    /// <summary>
    /// Start a dialogue with the player
    /// </summary>
    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
