using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator _animator;

    //switch to running
    public void PlayRunningAnimation() {
        _animator = GetComponent<Animator>(); //get the animator reference
        _animator.SetBool("isFlying", false);
        _animator.SetBool("isRunning", true); //update the boolean
    }

    //switch to death animation
    public void PlayDeathAnimation() {
        _animator.SetBool("hasDied", true);
    }

    //switch to flying
    public void PlayJumpingAnimation() {
        _animator.SetBool("isRunning", false); //update the boolean
        _animator.SetBool("isJumping", true);
    }
}
