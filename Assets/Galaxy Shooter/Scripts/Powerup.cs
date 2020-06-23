using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    enum Powerups
    {
        TripleShot,
        Speed,
        Shield
    }

    [SerializeField]
    private int _powerupId;
    private float _powerupSpeed = 5;

    [SerializeField]
    private AudioClip _pickupSound;
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        if(transform.position.y < -7)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();

            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_pickupSound, Camera.main.transform.position);
                switch (_powerupId)
                {                    
                    //if _powerupId equals to 0 calls triple shot powerup
                    case (int)Powerups.TripleShot:
                        player.TripleShotPowerupOn();
                        break;

                    //if _powerupId equals to 1 calls speed powerup
                    case (int)Powerups.Speed:
                        player.SpeedPowerupOn();
                        break;

                    //if _powerupId equals to 2 calls shield powerup
                    case (int)Powerups.Shield:
                        player.ShieldPowerupOn();
                        break;
                }
                
            }

            Destroy(gameObject);
        }
    }
}
