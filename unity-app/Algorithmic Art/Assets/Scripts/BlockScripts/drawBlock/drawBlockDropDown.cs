using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class drawBlockDropDown : MonoBehaviour
{
    public TMP_Dropdown dropDown; // Reference to your Dropdown component

    public drawBlock drawBlock;

    [SerializeField]
    private GameObject[] inputs;

    [SerializeField]
    private Sprite[] sprites;

    // Default for x and y coordinates
    [SerializeField]
    private int defaultCoords = 0;

    public SpriteRenderer spriteRenderer;

    public BoxCollider2D boxCollider;



    void Start()
    {
        dropDown.onValueChanged.AddListener(OnDropdownValueChanged);
        drawBlock.data = new float[3];
    }

    private void Update()
    {
        TMP_InputField xInput;
        TMP_InputField yInput;
        TMP_InputField lengthInput;
        TMP_InputField radiusInput;
        Toggle toggle;
        string inputData;
        switch (drawBlock.mode)
        {
            case 0:
                xInput = (TMP_InputField)(inputs[0].transform.Find("X").GetComponent("TMP_InputField"));
                yInput = (TMP_InputField)(inputs[0].transform.Find("Y").GetComponent("TMP_InputField"));

                inputData = xInput.text;
                if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out int parsedX))
                {
                    drawBlock.data[0] = parsedX;
                }
                else
                {
                    drawBlock.data[0] = defaultCoords;
                }

                inputData = yInput.text;
                if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out int parsedY))
                {
                    drawBlock.data[1] = parsedY;
                }
                else
                {
                    drawBlock.data[1]= defaultCoords;
                }
                break;  

            case 1:
                xInput = (TMP_InputField)(inputs[1].transform.Find("X").GetComponent("TMP_InputField"));
                yInput = (TMP_InputField)(inputs[1].transform.Find("Y").GetComponent("TMP_InputField"));
                toggle = (Toggle)(inputs[1].transform.Find("Toggle").GetComponent("Toggle"));

                inputData = xInput.text;
                if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out parsedX))
                {
                    drawBlock.data[0] = parsedX;
                }
                else
                {
                    drawBlock.data[0] = defaultCoords;
                }

                inputData = yInput.text;
                if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out parsedY))
                {
                    drawBlock.data[1] = parsedY;
                }
                else
                {
                    drawBlock.data[1] = defaultCoords;
                }

                if(toggle.isOn)
                {
                    drawBlock.data[2] = 1;
                }
                else
                {
                    drawBlock.data[2] = 0;
                }

                break;
            case 2:
                lengthInput = (TMP_InputField)(inputs[2].transform.Find("Length").GetComponent("TMP_InputField"));
                toggle = (Toggle)(inputs[2].transform.Find("Toggle").GetComponent("Toggle"));

                inputData = lengthInput.text;
                if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out parsedY))
                {
                    drawBlock.data[1] = Math.Abs(parsedY);
                }
                else
                {
                    drawBlock.data[1] = defaultCoords;
                }

                if (toggle.isOn)
                {
                    drawBlock.data[2] = 1;
                }
                else
                {
                    drawBlock.data[2] = 0;
                }
                break;
            case 3:
                radiusInput = (TMP_InputField)(inputs[3].transform.Find("Radius").GetComponent("TMP_InputField"));
                toggle = (Toggle)(inputs[3].transform.Find("Toggle").GetComponent("Toggle"));

                inputData = radiusInput.text;
                if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out parsedY))
                {
                    drawBlock.data[1] = parsedY;
                }
                else
                {
                    drawBlock.data[1] = defaultCoords;
                }

                if (toggle.isOn)
                {
                    drawBlock.data[2] = 1;
                }
                else
                {
                    drawBlock.data[2] = 0;
                }
                break;
            default:
                break;
        };
    }

    // Callback method for the dropdown value change event
    void OnDropdownValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                inputs[0].SetActive(true);
                inputs[1].SetActive(false);
                inputs[2].SetActive(false);
                inputs[3].SetActive(false);
                spriteRenderer.sprite = sprites[0];

                // Resize the collider
                boxCollider.size = new Vector2(3.9f, 2.5f);

                // Change the offset of the collider
                boxCollider.offset = new Vector2(0f, 0f);

                drawBlock.mode = 0;
                drawBlock.defaultSprite = sprites[0];
                drawBlock.selected = sprites[2];
                break;
            case 1:
                inputs[0].SetActive(false);
                inputs[1].SetActive(true);
                inputs[2].SetActive(false);
                inputs[3].SetActive(false);
                spriteRenderer.sprite = sprites[1];
                drawBlock.defaultSprite = sprites[1];
                drawBlock.selected = sprites[3];

                // Resize the collider
                boxCollider.size = new Vector2(5.37f, 2.4f);

                // Change the offset of the collider
                boxCollider.offset = new Vector2(0.75f, 0f);

                drawBlock.mode = 1;
                break;
            case 2:
                inputs[0].SetActive(false);
                inputs[1].SetActive(false);
                inputs[2].SetActive(true);
                inputs[3].SetActive(false);
                spriteRenderer.sprite = sprites[0];
                drawBlock.defaultSprite = sprites[0];
                drawBlock.selected = sprites[2];

                // Resize the collider
                boxCollider.size = new Vector2(3.9f, 2.5f);

                // Change the offset of the collider
                boxCollider.offset = new Vector2(0f, 0f);

                drawBlock.mode = 2;
                break;
            case 3:
                inputs[0].SetActive(false);
                inputs[1].SetActive(false);
                inputs[2].SetActive(false);
                inputs[3].SetActive(true);
                spriteRenderer.sprite = sprites[0];
                drawBlock.defaultSprite = sprites[0];
                drawBlock.selected = sprites[2];

                // Resize the collider
                boxCollider.size = new Vector2(3.9f, 2.5f);

                // Change the offset of the collider
                boxCollider.offset = new Vector2(0f, 0f);

                drawBlock.mode = 3;
                break;
            default:
                break;
        };


    }

}
