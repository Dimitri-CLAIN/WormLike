using UnityEngine;

public class ShowHideController : MonoBehaviour {

    public GameObject objectToToggle;

    public void ToggleObject() {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}