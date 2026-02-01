using System.Collections;
using UnityEngine;

public class LockedDoorUI : MonoBehaviour
{
    public static LockedDoorUI I;

    [SerializeField] private GameObject textObject;
    [SerializeField] private float displayTime = 1.5f;

    private Coroutine routine;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;

        if (textObject != null)
            textObject.SetActive(false);
    }

    public void Show()
    {
        if (textObject == null) return;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        textObject.SetActive(true);
        yield return new WaitForSecondsRealtime(displayTime);
        textObject.SetActive(false);
    }
}
