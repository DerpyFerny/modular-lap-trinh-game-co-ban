using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    //check if in range
    //check key press
    //perform action

    public bool isInRange;
    public bool canInteract = true;
    public KeyCode interactKey;
    public UnityEvent interactionAction;

    //all 3 step
    void Update()
    {
        if (isInRange && canInteract)
        {
            if (Input.GetKeyDown(interactKey))
            {
                canInteract = false;
                interactionAction.Invoke();
            }
        }
    }

    public void enable_canInteract()
    {
        canInteract = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Enter interact zone");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("exit interact zone");
        }
    }
}
