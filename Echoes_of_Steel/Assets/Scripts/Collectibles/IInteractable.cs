using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string name { get; set; }
    public void Interact();
}
