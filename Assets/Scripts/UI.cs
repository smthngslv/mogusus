using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    public Player player;
    public string nextScene;

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject coinA;
    public GameObject coinB;
    
    public TextMeshProUGUI scoreLabel;
    public TextMeshProUGUI multiplierLabel;
    public Button nextButton;
    public Button retryButton;

    public MultiplierCounter multiplier;

    private bool _isFinished = false;
    private int _score = 0;

    private void Awake()
    {
        _score = PlayerPrefs.GetInt("score", 0);
    }

    private void Start () {
        winUI.SetActive(false);
        loseUI.SetActive(false);
        nextButton.onClick.AddListener(NextButtonOnClick);
        retryButton.onClick.AddListener(RetryButtonOnClick);
    }

    private void NextButtonOnClick(){
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
    
    private void RetryButtonOnClick(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    private void AddCoinsAndSave()
    {
        _score += 10 * multiplier.Multiplier;
        PlayerPrefs.SetInt("score", _score);
        PlayerPrefs.Save();
        
        // Display.
        scoreLabel.text = _score.ToString();
    }
    
    private void Update()
    {
        if (player.Score != 0 || _isFinished)
        {
            return;
        }
        
        if (multiplier.Multiplier == 0)
        {
            _isFinished = true;
            loseUI.gameObject.SetActive(true);
            return;
        }
        
        _isFinished = true;
        winUI.SetActive(true);
        scoreLabel.text = PlayerPrefs.GetInt("score").ToString();
        multiplierLabel.text = multiplier.Multiplier.ToString();
        
        // Animate.
        Instantiate(
            coinA, coinA.transform.position, coinA.transform.rotation, coinA.transform.parent
        ).transform.DOMove(
            coinB.transform.position, 2
        ).onComplete = AddCoinsAndSave;
    }
}
