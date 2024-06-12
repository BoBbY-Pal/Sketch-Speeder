using Sketch_Speeder.Managers;
using UnityEngine;

public class UIToggleButton : MonoBehaviour
{
    [SerializeField] private GameObject toggleObject;
    [SerializeField] private GameObject[] toggleObjects;

    public void ToggleObject()
    {
        if (toggleObject == null) return;
        SoundManager.Instance.Play(SoundTypes.ButtonClick);
        toggleObject.SetActive(!toggleObject.activeSelf);
    }

    public void ToggleObjects()
    {
        if (toggleObjects == null) return;
        SoundManager.Instance.Play(SoundTypes.ButtonClick);
        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    public void ToggleOffObejcts()
    {
        if (toggleObjects == null) return;

        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ToggleOnObejcts()
    {
        if (toggleObjects == null) return;

        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(true);
        }
    }
}
