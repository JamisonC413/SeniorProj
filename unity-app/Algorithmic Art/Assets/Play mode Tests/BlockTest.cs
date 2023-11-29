using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
public class BlockIDTest : MonoBehaviour
{

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scene2");
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestBlockTag()
    {

        yield return new WaitForEndOfFrame();

        //GameObject spawnedObject = Instantiate(prefabToSpawn, new Vector3(-5, 4, 0), Quaternion.identity);

        GameObject blockPrefab = Resources.Load<GameObject>("drawBlock");

        GameObject instantiatedBlock = Object.Instantiate(blockPrefab);

        //instantiatedBlock.GetComponent<drawBlock>().BlockID;

       

        //GameObject block = GameObject.FindGameObjectWithTag("block");

        Debug.Log("type: " + instantiatedBlock.GetType().ToString());
        UnityEngine.Assertions.Assert.AreEqual("UnityEngine.GameObject", instantiatedBlock.GetType().ToString());
        UnityEngine.Assertions.Assert.AreEqual("block", instantiatedBlock.tag);



    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestDrawBlockInitialize()
    {

        yield return new WaitForEndOfFrame();


        drawBlock db = new drawBlock();
        db.initialize();

        //BlockID is 1 here due to using Object.Instantiate instead of regular Instantiate
        //UnityEngine.Assertions.Assert.IsFalse(db.topSnapped);
        //UnityEngine.Assertions.Assert.IsFalse(db.botSnapped);
        UnityEngine.Assertions.Assert.IsNull(db.prevBlock);
        UnityEngine.Assertions.Assert.IsNull(db.nextBlock);

    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestMoveBrushBlockInitialize()
    {

        yield return new WaitForEndOfFrame();


        moveBrush mb = new moveBrush();
        mb.initialize();

        //BlockID is 1 here due to using Object.Instantiate instead of regular Instantiate
        //UnityEngine.Assertions.Assert.IsFalse(mb.topSnapped);
        //UnityEngine.Assertions.Assert.IsFalse(mb.botSnapped);
        UnityEngine.Assertions.Assert.IsNull(mb.prevBlock);
        UnityEngine.Assertions.Assert.IsNull(mb.nextBlock);

    }


}