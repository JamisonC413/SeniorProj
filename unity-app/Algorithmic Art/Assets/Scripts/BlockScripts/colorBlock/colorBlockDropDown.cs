using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class colorBlockDropDown : MonoBehaviour
{
    public TMP_Dropdown dropDown; // Reference to your Dropdown component

    [SerializeField]
    private colorBlock colorBlock;


    void Start()
    {
        dropDown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    // Callback method for the dropdown value change event
    void OnDropdownValueChanged(int index)
    {

        // Call a method, perform an action, etc.
        colorBlock.color = index;
    }

}
