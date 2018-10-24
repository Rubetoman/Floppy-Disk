using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Script to improve Event Sytem selection. This way navigating through the menus with the controller is possible.
/// </summary>
public class SelectOnInput : MonoBehaviour {

    public GameObject firstSelection;   // First element to be selected by the Event System.

    private GameObject myEventSystem;   // Event System of the main_menu scene.
    private GameObject lastSelected;    // Last GameObject selected on the Event System.

    void Awake () {
        myEventSystem = GameObject.Find("EventSystem"); // Find the Event System of the scene.
    }
	
	void Update () {

        // If no UI gameObject was selected and horizontal or vertical Input is detected, select the last selected button.
        if (myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject == null)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
                myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
        }
        else
        {
            // If a UI gameObject is selected and mouse Input is detected, deselect it.
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                lastSelected = myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject;
                myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
            }
        }       
    }

    private void OnEnable()
    {
        lastSelected = firstSelection;                  // Use the GameObject passed as selected by the EventSystem.
        myEventSystem = GameObject.Find("EventSystem"); // Find the Event System of the scene.
        if (myEventSystem != null)
        {
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);          // Start with nothing selected
        }
    }

    private void OnDisable()
    {
    // Once disabled use the first button of the previous menu as selected.
    if (myEventSystem != null)
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject);
    }
}
