using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    const string PLAYER = "Player";
    [SerializeField] AudioClip collectedClip;
    [SerializeField] int points;
    [SerializeField] float volume = 0.4f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == PLAYER)
        {
            FindObjectOfType<GameManager>().AddPoint(points);
            AudioSource.PlayClipAtPoint(collectedClip, Camera.main.transform.position, volume);
            Destroy(gameObject);
        }
    }

}
