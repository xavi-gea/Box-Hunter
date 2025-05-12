using System;
using UnityEngine;

/// <summary>
/// Start a <see cref="Dialogue"/> when the <see cref="Interact"/> function is triggered
/// </summary>
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
