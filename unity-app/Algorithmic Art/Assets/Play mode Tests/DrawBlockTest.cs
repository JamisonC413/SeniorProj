using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class DrawBlockTest : MonoBehaviour
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scene2");
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestDrawBlock()
    {

        yield return new WaitForEndOfFrame();

        //GameObject spawnedObject = Instantiate(prefabToSpawn, new Vector3(-5, 4, 0), Quaternion.identity);

        GameObject blockPrefab = Resources.Load<GameObject>("drawBlock");
        GameObject lineRenderPrefab = Resources.Load<GameObject>("LineRenderer");
        GameObject meshRenderPrefab = Resources.Load<GameObject>("MeshRenderer");

        GameObject instantiatedBlock = Object.Instantiate(blockPrefab);

        //instantiatedBlock.GetComponent<drawBlock>().BlockID;

        GameObject brushObject = GameObject.FindGameObjectWithTag("brush");
        GameObject playObject = GameObject.FindGameObjectWithTag("playHandler");

        Brush brush = brushObject.GetComponent<Brush>();
        Play play = playObject.GetComponent<Play>();
        float[] data = { 1f, 1f, 1f };
        brush.LineRendererPrefab = lineRenderPrefab;

        //GameObject block = GameObject.FindGameObjectWithTag("block");

        Debug.Log("type: " + instantiatedBlock.GetType().ToString());
        UnityEngine.Assertions.Assert.AreEqual("UnityEngine.GameObject", instantiatedBlock.GetType().ToString());
        UnityEngine.Assertions.Assert.AreEqual("block", instantiatedBlock.tag);

        drawBlock db = blockPrefab.GetComponent<drawBlock>();


        db.brush = brush;
        db.play = play;
        db.data = data;

        db.execute();

        db.mode = 1;
        db.execute();

        
        db.mode = 2;
        db.execute();

        db.mode = 3;
        db.execute();


    }

    [UnityTest]
    public IEnumerator TestDrawBlockDropDown()
    {

        yield return new WaitForEndOfFrame();

        //GameObject spawnedObject = Instantiate(prefabToSpawn, new Vector3(-5, 4, 0), Quaternion.identity);

        GameObject blockPrefab = Resources.Load<GameObject>("drawBlock");
        GameObject lineRenderPrefab = Resources.Load<GameObject>("LineRenderer");
        GameObject meshRenderPrefab = Resources.Load<GameObject>("MeshRenderer");

        GameObject instantiatedBlock = Object.Instantiate(blockPrefab);

        //instantiatedBlock.GetComponent<drawBlock>().BlockID;

        GameObject brushObject = GameObject.FindGameObjectWithTag("brush");
        GameObject playObject = GameObject.FindGameObjectWithTag("playHandler");

        Brush brush = brushObject.GetComponent<Brush>();
        Play play = playObject.GetComponent<Play>();
        float[] data = { 1f, 1f, 1f };
        brush.LineRendererPrefab = lineRenderPrefab;

        //GameObject block = GameObject.FindGameObjectWithTag("block");

        Debug.Log("type: " + instantiatedBlock.GetType().ToString());
        UnityEngine.Assertions.Assert.AreEqual("UnityEngine.GameObject", instantiatedBlock.GetType().ToString());
        UnityEngine.Assertions.Assert.AreEqual("block", instantiatedBlock.tag);

        drawBlock db = blockPrefab.GetComponent<drawBlock>();
        //drawBlockDropDown dbd = blockPrefab.GetComponent<drawBlockDropDown>();


        db.brush = brush;
        db.play = play;
        db.data = data;

        db.execute();


    }
}
