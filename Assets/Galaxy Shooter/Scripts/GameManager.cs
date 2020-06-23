using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    private bool _gameOver = true;

    private UiManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_gameOver)
            {
                Instantiate(_playerPrefab, new Vector3(0, -6, 0), Quaternion.identity);
                _gameOver = false;
                _uiManager.HideTitleScreen();
            }
            else
            {

            }
        }
    }
    public void IsGameOver(bool isGameOver)
    {
        _gameOver = isGameOver;
    }
    public bool GetGameOverStatus()
    {
        return _gameOver;
    }
}
