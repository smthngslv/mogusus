using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIMenu : MonoBehaviour
{
    public Button startButton;

    private void Start () {
        startButton.onClick.AddListener(StartButtonOnClick);
    }

    private void StartButtonOnClick(){
        SceneManager.LoadScene(PlayerPrefs.GetString("level", "Level0"), LoadSceneMode.Single);
    }
}
