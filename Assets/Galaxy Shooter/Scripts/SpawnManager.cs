using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerupsPrefab;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();        
    }

    public void StartGameRoutines()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerups());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemy()
    {
        while (!_gameManager.GetGameOverStatus())
        {
            var randomxPosition = Random.Range(-7.91f, 7.91f);
            Instantiate(_enemyPrefab, new Vector3(randomxPosition,6.62f), Quaternion.identity);
            yield return new WaitForSeconds(2.0f);
        }
    }
    private IEnumerator SpawnPowerups()
    {
        while (!_gameManager.GetGameOverStatus())
        {
            var randomPowerup = Random.Range(0, 3);
            var randomxPosition = Random.Range(-7.91f, 7.91f);

            Instantiate(_powerupsPrefab[randomPowerup], new Vector3(randomxPosition, 6.62f), Quaternion.identity);

            yield return new WaitForSeconds(5.0f);
        }
    }
}
