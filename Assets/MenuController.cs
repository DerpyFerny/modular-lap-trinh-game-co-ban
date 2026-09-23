using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public void OpenMenu(Button firstMenuButton)
    {   
        // 2. Clear current focus to prevent glitches
        EventSystem.current.SetSelectedGameObject(null); 
        
        // 3. Assign the new button to keyboard focus
        EventSystem.current.SetSelectedGameObject(firstMenuButton.gameObject); 
    }
}
