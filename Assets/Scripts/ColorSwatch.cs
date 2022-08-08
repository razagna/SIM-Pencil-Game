using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSwatch : MonoBehaviour
{
    public void SetColor(Image image)
    {
        gameObject.GetComponent<Image>().color = image.color;
    }
}
