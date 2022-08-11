using UnityEngine;
using UnityEngine.UI;

public class ColorSwatch : MonoBehaviour
{
    public void SetColor(Image image)
    {
        GetComponent<Image>().color = image.color;
    }
}
