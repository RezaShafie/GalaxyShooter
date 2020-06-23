using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    #region Fields

    //Variables for indicating whether a powerup is collected or not
    private bool _isTripleShotCollected = false;
    private bool _isSpeedCollected = false;
    public bool _isShieldCollected = false;

    [SerializeField]
    private float _speed = 15.0f;//Spaceship traversing speed

    private float _speedMultiplier = 1.5f;//Speed is multiplied by this value when Speed powerup is collected

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private GameObject _shieldPrefab;

    [SerializeField]
    private GameObject[] _engineFailures;

    [SerializeField]
    private float _fireRate = 0.15f;//Spaceship fire rate

    private float _nextFire = 0.0f;

    [SerializeField]
    private int _health = 3;//Spaceship healths count

    [SerializeField]
    private AudioClip _explosionSound;

    private UiManager _uiManager;
    private SpawnManager _spawnManager;

    private AudioSource _laserSound;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _laserSound = GetComponent<AudioSource>();
        if (_uiManager != null)
        {
            _uiManager.UpdateLives(_health);
        }

        if (_spawnManager != null)
        {
            _spawnManager.StartGameRoutines();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    //Instantiates lasers
    private void Shoot()
    {
        if (Time.time >= _nextFire)
        {
            
            if (!_isTripleShotCollected)
            {
                _laserSound.pitch = 1;
                _laserSound.Play();
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.89f, 0), Quaternion.identity);
            }
            else
            {
                _laserSound.pitch = 5;
                _laserSound.Play();
                
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }

            _nextFire = Time.time + _fireRate;
        }
    }

    //Handles movement logic of spaceship
    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Moving the player alongside with checking for Speed powerup
        switch (_isSpeedCollected)
        {
            case false://if not collected
                transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
                transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
                break;

            case true://if collected
                transform.Translate(Vector3.right * horizontalInput * _speed * _speedMultiplier * Time.deltaTime);
                transform.Translate(Vector3.up * verticalInput * _speed * _speedMultiplier * Time.deltaTime);
                break;

        }


        //y axis restriction
        if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, transform.position.z);
        }
        else if (transform.position.y > 4.2f)
        {
            transform.position = new Vector3(transform.position.x, 4.2f, transform.position.z);
        }

        //x axis warpping
        if (transform.position.x >= 9.2f)
        {
            transform.position = new Vector3(-9.2f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -9.2f)
        {
            transform.position = new Vector3(9.2f, transform.position.y, transform.position.z);
        }
    }

    //Handles TripleShot powerup logic
    public void TripleShotPowerupOn()
    {
        _isTripleShotCollected = true;
        StartCoroutine(TripleShotPowerdownRoutine());
    }

    //Disables TripleShot powerup after 5 seconds
    private IEnumerator TripleShotPowerdownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotCollected = false;
    }



    //Handles Speed powerup logic
    public void SpeedPowerupOn()
    {
        _isSpeedCollected = true;
        StartCoroutine(SpeedPowerdownRoutine());
    }

    //Disables Speed powerup after 5 seconds
    private IEnumerator SpeedPowerdownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedCollected = false;
    }

    public void ShieldPowerupOn()
    {
        _isShieldCollected = true;
        _shieldPrefab.SetActive(true);
    }

    //Handles spaceship health count or destruction(end of game).
    public void Damage()
    {
        if (!_isShieldCollected)
        {
            switch (_health)//if spaceship's health is zero destory it, otherwise reduce remaining healths
            {
                case 0:
                    AudioSource.PlayClipAtPoint(_explosionSound, Camera.main.transform.position);
                    Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                    _uiManager.ShowTitleScreen();
                    Destroy(gameObject);
                    break;                

                default://this case hits more often than the other one
                    _health--;
                    _engineFailures[_health].SetActive(true);
                    _uiManager.UpdateLives(_health);
                    break;               
            }            
        }
        else
        {
            _isShieldCollected = false;
            _shieldPrefab.SetActive(false);
        }
    }

}
