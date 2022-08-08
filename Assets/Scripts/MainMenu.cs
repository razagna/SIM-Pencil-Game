using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Image player;
    public Board preview;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnDropDownValueChanged(TMP_Dropdown dropDown)
    {
        PlayerPrefs.SetInt("shape", dropDown.value + 5);
        Debug.Log(PlayerPrefs.GetInt("shape"));
        preview.DrawPreview();
    }

    public void SetPlayer(Image player)
    {
        this.player = player;
    }

    public void SetPlayerColor(Image swatch) //, Image swatch
    {
        player.color = swatch.color;
    }

}
