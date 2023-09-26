using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class drawBlock : Block
{
    [SerializeField]
    private TMP_InputField XInput;
    [SerializeField]
    public TMP_InputField YInput;
    [SerializeField]
    private float snapOffset = 1f;

    public int X;
    public int Y;
    void Awake()
    {
        this.blockID = Block.nextID;
        this.topSnapped = false;
        this.botSnapped = false;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;

        snapPositions = new Vector2[2];
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y - snapOffset);
        snapPositions[1] = new Vector2(transform.position.x, transform.position.y + snapOffset);

        //Debug.Log(snapPositions[0]);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y - snapOffset);
        snapPositions[1] = new Vector2(transform.position.x, transform.position.y + snapOffset);

        string inputData = XInput.text;
        if (!string.IsNullOrEmpty(inputData))
        {
            X = int.Parse(inputData);
        }
        else
        {
            X = 1;
        }

        inputData = YInput.text;
        if (!string.IsNullOrEmpty(inputData))
        {
            Y = int.Parse(inputData);
        }
        else
        {
            Y = 1;
        }
    }

    public override void execute()
    {
        
    }
}
