using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class loopBlock : NestedBlock
{
    [SerializeField]
    private TMP_InputField numRepeats;
    public override Block execute()
    {
        StartCoroutine(loopExecute());
        return bottomBlock.nextBlock;
    }

    private IEnumerator loopExecute()
    {

        string inputData = numRepeats.text;
        if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out int parsed))
        {

            for (int i = 0; i < parsed; i++)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = selected;
                yield return new WaitForSeconds(playScript.delay);
                gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;

                Block currentBlock = this.nextBlock;
                while (currentBlock != bottomBlock)
                {
                    Debug.Log(currentBlock + "was run");
                    currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.selected;
                    Block nextBlock = currentBlock.execute();
                    yield return new WaitForSeconds(playScript.delay);
                    currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.defaultSprite;
                    currentBlock = nextBlock;
                }
                currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.selected;
                yield return new WaitForSeconds(playScript.delay);
                currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.defaultSprite;
            }

        }
        else
        {
            Debug.Log("Error parsing number of loop repeats");
        }


    }

}
