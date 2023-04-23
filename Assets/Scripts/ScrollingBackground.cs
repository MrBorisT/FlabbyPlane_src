using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    float currentScrollSpeed;
    SpriteRenderer sr;

    public float ScrollSpeed { set => currentScrollSpeed = value; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentScrollSpeed = scrollSpeed;
    }

    void Update()
    {
        sr.material.mainTextureOffset = new Vector2(sr.material.mainTextureOffset.x - Time.deltaTime * currentScrollSpeed, 0);
    }

    public void ResetBG()
    {
        currentScrollSpeed = scrollSpeed;
        sr.material.mainTextureOffset = new Vector2(0, 0);
    }
}
