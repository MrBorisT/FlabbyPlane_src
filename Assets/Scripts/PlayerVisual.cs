using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    const string EXPLODE_TRIGGER = "Explosion";
    [SerializeField] Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player.OnStateChange += Player_OnStateChange;
    }

    private void Player_OnStateChange(object sender, Player.OnStateChangeEventArgs e)
    {
        if (e.state == Player.State.GameOver)
        {
            animator.SetBool(EXPLODE_TRIGGER, true);
        } else if (e.state == Player.State.GameReset)
        {
            animator.SetBool(EXPLODE_TRIGGER, false);
        }
    }
}
