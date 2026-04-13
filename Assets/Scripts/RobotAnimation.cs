using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    Animator robotAnimator;
    RobotMovement robotMovement;

    void Start()
    {
        robotAnimator = GetComponentInChildren<Animator>();
        robotMovement = GetComponent<RobotMovement>();
    }

    void Update()
    {
        if(robotMovement.moveInput.magnitude > 0.01f) {
            robotAnimator.SetBool("IsWalking", true);
        }
        else {
            robotAnimator.SetBool("IsWalking", false);
        }

        if (robotMovement.IsJumping) {
            robotAnimator.SetTrigger("IsJumping");
        }

    }
}
