using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _lives;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private GameObject _titleScreen;

    private GameManager _gameManager;

    [SerializeField]
    private Text _scoreText;

    private int _score;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void UpdateLives(int remainingLives)
    {
        _livesImage.sprite = _lives[remainingLives];
    }

    
    public void UpdateScore()
    {
        _score += 10;

        _scoreText.text = ("Score: " + _score);
    }

    public void ShowTitleScreen()
    {
        _titleScreen.SetActive(true);
        _gameManager.IsGameOver(true);
        
    }
    public void HideTitleScreen()
    {
        _titleScreen.SetActive(false);
        _gameManager.IsGameOver(false);

        //resetting score
        _score = 0;
        _scoreText.text = "Score: 0";
    }
}
