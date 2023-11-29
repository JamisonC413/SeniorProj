using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CanvasTest : MonoBehaviour
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scene2");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestCameraChange()
    {

        yield return new WaitForEndOfFrame();

        GameObject mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");

        GameObject maxCanvas = GameObject.FindGameObjectWithTag("MaxCanvas");

        UnityEngine.Assertions.Assert.IsTrue(mainCanvas.activeSelf);
        //UnityEngine.Assertions.Assert.IsFalse(maxCanvas.activeSelf);

        


        //Debug.Log("type: " + instantiatedBlock.GetType().ToString());



    }
}
