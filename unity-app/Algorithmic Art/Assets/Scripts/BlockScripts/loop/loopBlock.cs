using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class loopBlock : NestedBlock
{
    [SerializeField]
    private TMP_InputField numRepeats;

    public int repeat;

    public void Start()
    {
        repeat = 0;
    }

    public override void execute()
    {
        if(repeat == 0)
        {
            string inputData = numRepeats.text;
            if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out int parsed))
            {
                repeat = parsed;
            }
            else
            {
                repeat = 1;
            }
        }
        Debug.Log(repeat);

        repeat--;
    }


}
