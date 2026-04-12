using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody bulletRB;

    private void Awake() {
        bulletRB = GetComponent<Rigidbody>();
        bulletRB.AddRelativeForce(Vector3.forward * 1000f);
    }
    private void OnCollisionEnter(Collision collision) {
            Destroy(gameObject);
    }
}
