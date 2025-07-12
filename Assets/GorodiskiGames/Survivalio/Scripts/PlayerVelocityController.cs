using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVelocityController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private void Update() {
        _rigidbody.velocity = Vector3.zero;
    }
}
