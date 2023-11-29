using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class BrushTest : MonoBehaviour
{

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scene2");
    }



    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    //Tests the starting position of the brush
    public IEnumerator TestBrushObj()
    {

        yield return new WaitForEndOfFrame();

        //GameObject spawnedObject = Instantiate(prefabToSpawn, new Vector3(-5, 4, 0), Quaternion.identity);

        //instantiatedBlock.GetComponent<drawBlock>().BlockID;



        GameObject brush = GameObject.FindGameObjectWithTag("brush");

        Debug.Log("type: " + brush.GetType().ToString());
        UnityEngine.Assertions.Assert.AreEqual("UnityEngine.GameObject", brush.GetType().ToString());
        UnityEngine.Assertions.Assert.AreEqual("brush", brush.tag);

        Destroy(brush);
        

    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    //Tests the starting position of the brush
    public IEnumerator TestBrushStartPos()
    {

        yield return new WaitForEndOfFrame();

        //GameObject spawnedObject = Instantiate(prefabToSpawn, new Vector3(-5, 4, 0), Quaternion.identity);

        //instantiatedBlock.GetComponent<drawBlock>().BlockID;



        GameObject brush = GameObject.FindGameObjectWithTag("brush");

        Transform brushTransform = brush.GetComponent<Transform>();

        Vector3 starPos = brushTransform.position;

        UnityEngine.Assertions.Assert.AreEqual(new Vector3(3.91f, -0.87f, 90.62f), starPos);

        Destroy(brush);


    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    //Tests the starting position of the brush
    public IEnumerator TestMoveBrush()
    {

        yield return new WaitForEndOfFrame();

        GameObject brush = GameObject.FindGameObjectWithTag("brush");

        Transform brushTransform = brush.GetComponent<Transform>();
        Brush brushComponent = brush.GetComponent<Brush>();

        Vector3 starPos = brushTransform.position;

        UnityEngine.Assertions.Assert.AreEqual(new Vector3(3.91f, -0.87f, 90.62f), starPos);

        moveBrush mb = new moveBrush();

        //Tests moving brush 3, 3
        mb.brush = brushComponent;
        mb.X = 3;
        mb.Y = 3;

        mb.execute();
        brushTransform = brush.GetComponent<Transform>();
        Vector3 newPos = brushTransform.position;


        //Unable to compare vectors directly due to floating point precision errors. Instead testing equality using distance <= 0.01 
        //UnityEngine.Assertions.Assert.IsTrue(Vector3.Distance(new Vector3(7.6f, -5.4f, 0), newPos) <= 0.01);
        //Debug.Log("dist: " + Vector3.Distance(new Vector3(7.6f, -5.4f, 0), newPos));

        //mb.X = -10;
        //mb.Y = -10;
        //mb.execute();
        //brushTransform = brush.GetComponent<Transform>();
        //newPos = brushTransform.position;

        ////Brush should not be able to go lower value than its initial start position
        //UnityEngine.Assertions.Assert.IsTrue(Vector3.Distance(new Vector3(4.6f, -8.4f, 0), newPos) <= 0.01);
        //Debug.Log("pos: " + newPos);


        //Destroy(brush);


    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
