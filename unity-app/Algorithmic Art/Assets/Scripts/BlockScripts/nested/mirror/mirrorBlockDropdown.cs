using TMPro;
using UnityEngine;

public class mirrorBlockDropDown : MonoBehaviour
{
    public TMP_Dropdown dropDown; // Reference to your Dropdown component

    [SerializeField]
    private mirrorBlock mirrorBlock;


    void Start()
    {
        dropDown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    // Callback method for the dropdown value change event
    void OnDropdownValueChanged(int index)
    {

        // Call a method, perform an action, etc.
        mirrorBlock.axis = index;
    }

}
