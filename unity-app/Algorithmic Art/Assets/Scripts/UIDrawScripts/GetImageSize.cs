using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetImageSize : MonoBehaviour
{

    private Image image;
    private Rect rect;

    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
        getImageSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getImageSize()
    {
        rect = image.rectTransform.rect;
        float width = rect.width;
        float height = rect.height;

        Debug.Log("width = " + width + "height = " + height);
    }
}
