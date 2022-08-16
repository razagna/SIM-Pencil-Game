using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitOptions : MonoBehaviour
{
    public Image playerA, playerB;
    public TMP_Dropdown dropDown;

    void Start()
    {
        InitColor(playerA);
        InitColor(playerB);
        InitDropdown();
    }

    void InitColor(Image player)
    {
        player.color = GameManager.GetPlayerColor(player.name);
    }

    void InitDropdown()
    {
        Debug.Log(PlayerPrefs.GetInt("shape"));
        dropDown.value = PlayerPrefs.GetInt("shape");
    }
}
