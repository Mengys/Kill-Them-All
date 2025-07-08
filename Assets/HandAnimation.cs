using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    public float speed = 2f;
    public float scale = 1f;
    private float _time;
    private Vector3 startPosition;

    private void Awake() {
        startPosition = transform.position;
    }

    void Update() {
        _time += Time.deltaTime * speed;

        // Уравнения для лемнискаты
        float x = startPosition.x + Mathf.Cos(_time) * scale;
        float y = startPosition.y + Mathf.Sin(2 * _time) * scale * 0.5f;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
