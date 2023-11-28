using UnityEngine;

public class functionStart : Block
{
    [SerializeField]
    private string startName;

    // Top snap point
    [SerializeField]
    private GameObject snap1;

    public Play playScript;

    void Awake()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;
        //startName = null;

        snapPositions = new Vector2[1];
        snapPositions[0] = snap1.transform.position;

        playScript = GameObject.FindGameObjectWithTag("playHandler").GetComponent<Play>();

        // Ask about this?!
        if (startName == null)
        {
            Debug.Log("Add a name to the functionStart prefab");
        }
        playScript.functions.Add(startName, this);
    }

    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = snap1.transform.position;
    }

    public void OnDestroy()
    {
        if (playScript.functions.ContainsKey(startName))
        {
            playScript.functions.Remove(startName);
        }
    }
}
