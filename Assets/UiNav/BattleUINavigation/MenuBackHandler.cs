using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuBackHandler : MonoBehaviour
{
    public Button fightButton; // to reselect when going back
    public UnityEvent goBack;

    void Update()
    {
        // "Cancel" is already mapped to Escape by default in Unity's Input Manager
        if (Input.GetButtonDown("Cancel"))
        {
            goBack.Invoke();
        }
    }

    public void GoBack()
    {
        if (gameObject.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(fightButton.gameObject);
            gameObject.SetActive(false);
        }
    }
}