using TMPro;
using UnityEngine;

public class drawBlockDropDown : MonoBehaviour
{
    public TMP_Dropdown dropDown; // Reference to your Dropdown component

    [SerializeField]
    private colorBlock colorBlock;

    [SerializeField]
    private GameObject[] inputs;
    
    void Start()
    {
        dropDown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    // Callback method for the dropdown value change event
    void OnDropdownValueChanged(int index)
    {
        Debug.Log(index);
        switch (index)
        {
            case 0:
                inputs[0].SetActive(true);
                inputs[1].SetActive(false);
                inputs[2].SetActive(false);

                break;
            case 1:
                inputs[0].SetActive(false);
                inputs[1].SetActive(true);
                inputs[2].SetActive(false);
                break;
            case 2:
                inputs[0].SetActive(false);
                inputs[1].SetActive(false);
                inputs[2].SetActive(true);
                break;
            default:
                break;
        };


    }

}
