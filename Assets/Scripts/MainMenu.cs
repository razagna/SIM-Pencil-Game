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
        int shape = dropDown.value == 0 ? 6 : dropDown.value + 4;
        PlayerPrefs.SetInt("shape", shape);

        if (preview.transform.GetChild(0).childCount > 0) preview.DestroyBoard();
        preview.Init(1.3f, PlayerPrefs.GetInt("shape"));
        preview.Draw(0.004f, 0.15f, true);

        GameManager.Instance.InitPlayers();
    }

    public void SetPlayer(Image player)
    {
        this.player = player;
    }

    public void SetPlayerColor(Image swatch)
    {
        player.color = swatch.color;
        string combinedColorString = swatch.color.r.ToString() + ", " + swatch.color.g.ToString() + ", " + swatch.color.b.ToString();
        PlayerPrefs.SetString(player.name, combinedColorString);

        preview.ResetBoard();
        GameManager.Instance.InitPlayers();
    }

}
