using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class loopBlock : NestedBlock
{
    [SerializeField]
    private TMP_InputField numRepeats;
    public override IEnumerator execute()
    {
        string inputData = numRepeats.text;
        if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out int parsed))
        {

            for (int i = 0; i < parsed; i++)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = selected;
                yield return new WaitForSeconds(playScript.delay);
                gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;

                Block currentBlock = nextBlock;
                while (currentBlock != bottomBlock)
                {
                    Debug.Log(currentBlock + "was run");
                    currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.selected;
                    yield return StartCoroutine(currentBlock.execute());

                    if (currentBlock is not NestedBlock)
                    {
                        yield return new WaitForSeconds(playScript.delay);
                    }
                    currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.defaultSprite;
                    currentBlock = currentBlock.getNextPlayBlock();
                    Debug.Log(blockID);
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
