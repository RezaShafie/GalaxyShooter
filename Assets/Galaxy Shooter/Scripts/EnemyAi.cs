using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private GameObject _enemyExplosionPrefab;

    private UiManager _uiManager;

    [SerializeField]
    private AudioClip _explosionClip;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.5)
        {
            float randomXPoition = Random.Range(-7.91f, 7.91f);
            transform.position = new Vector3(randomXPoition, 6.62f, transform.position.z);            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Handles enemy hitting the player situation
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();

            DestroyEnemy();//Destroys the enemy
            if (player != null)
            {
                player.Damage();//Reduces player's health or destroys it
            }
        }
    }


    public void DestroyEnemy()
    {        
        AudioSource.PlayClipAtPoint(_explosionClip, Camera.main.transform.position);
        Destroy(gameObject);
        _uiManager.UpdateScore();
        Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);               
    }
}
