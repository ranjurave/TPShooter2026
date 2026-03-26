using UnityEditor.Animations;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    Animator animator;
    RobotMovement robotMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        robotMovement = GetComponent<RobotMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(robotMovement == null || animator == null) {
            return;
        }
        if(robotMovement.moveInput.magnitude > 0.01f) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }
    }
}
