using TMPro;
using UnityEngine;

public class WidthBlockDropDown : MonoBehaviour
{
    public TMP_Dropdown dropDown; // Reference to your Dropdown component

    [SerializeField]
    private widthBlock widthBlock;


    void Start()
    {
        dropDown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    // Callback method for the dropdown value change event
    void OnDropdownValueChanged(int index)
    {

        // Call a method, perform an action, etc.
        switch (index)
        {
            case 1:
                widthBlock.width = 2f;
                break;
            case 2:
                widthBlock.width = 3f;
                break;
            default:
                widthBlock.width = 1f;
                break;
        }
    }

}
