using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum POSITION
    {
        UP,
        DOWN
    }
    [SerializeField] private POSITION position;

    public POSITION Position { get => position; }
}
