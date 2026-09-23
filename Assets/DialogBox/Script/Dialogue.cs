using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public UnityEvent DialogueEnd;
    private int index;
    public KeyCode interactKey;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void getDialogueFromObject(GameObject obj)
    {
        DialogList dList = obj.GetComponent<DialogList>();
        if (dList)
        {
            lines = (string[])dList.lines.Clone();
        }
    }

    public void StartDialogue()
    {
        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = String.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            DialogueEnd.Invoke();
            gameObject.SetActive(false);
        }
    }
}
