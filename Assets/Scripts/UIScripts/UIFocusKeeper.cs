using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocusKeeper : MonoBehaviour
{
    public GameObject firstSelected;

    void Start()
    {
        if (firstSelected != null)
            EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null && firstSelected != null)
            EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
