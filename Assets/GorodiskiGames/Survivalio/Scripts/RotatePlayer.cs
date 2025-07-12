using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class RotatePlayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    public Transform RotationNode;
    [SerializeField] private Animator _animator;
    private float targetAngle = -155f;
    public float rotationSpeed = 0.2f;     // Скорость вращения
    public float inertiaDamping = 5f;      // Коэффициент затухания инерции
    public float returnDelay = 3f;         // Задержка перед возвратом (сек)
    public float returnSpeed = 2f;         // Скорость возврата к исходной ротации

    private bool isDragging = false;
    private bool dragOccurred = false;       // Флаг, был ли драг
    private Vector2 lastPointerPosition;

    private Quaternion initialRotation;    // Исходная ротация модели
    private float idleTimer = 0f;           // Таймер бездействия

    private Vector2 currentVelocity;        // Текущая угловая 

    private const float dragThreshold = 5f;  // Минимальное расстояние для определения драг (в пикселях)


    void Start() {
        if (RotationNode != null)
            initialRotation = RotationNode.rotation;
    }

    public void OnPointerDown(PointerEventData eventData) {
        isDragging = true;
        dragOccurred = false;
        lastPointerPosition = eventData.position;
        idleTimer = 0f;
        currentVelocity = Vector2.zero; // Сбрасываем инерцию при начале вращения
    }

    private void OnClick() {
        var nameHash = Animator.StringToHash("Fall");
        _animator.PlayInFixedTime(nameHash, 0, float.NegativeInfinity);
    }

    public void OnPointerUp(PointerEventData eventData) {
        isDragging = false;
        idleTimer = 0f;

        // Если драг не был — считаем это кликом и вызываем метод
        if (!dragOccurred) {
            OnClick();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (isDragging && RotationNode != null) {
            Vector2 delta = eventData.position - lastPointerPosition;

            if (!dragOccurred && delta.magnitude > dragThreshold) {
                dragOccurred = true;
            }

            if (dragOccurred) {
                // Обновляем текущую скорость вращения (инерцию)
                currentVelocity = delta * rotationSpeed;

                // Вращаем модель по X и Y с учётом дельты
                RotationNode.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);
                //targetModel.Rotate(Vector3.right, delta.y * rotationSpeed, Space.World);

                lastPointerPosition = eventData.position;
                idleTimer = 0f;
            }
        }
    }

    void Update() {

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Die") && stateInfo.normalizedTime >= 1f) {
            var nameHash = Animator.StringToHash("Idle");
            _animator.PlayInFixedTime(nameHash, 0, float.NegativeInfinity);
        }

        if (!isDragging && RotationNode != null) {
            idleTimer += Time.deltaTime;

            // Применяем инерцию — продолжаем вращать модель с уменьшающейся скоростью
            if (currentVelocity.magnitude > 0.01f) {
                RotationNode.Rotate(Vector3.up, -currentVelocity.x, Space.World);
                //RotationNode.Rotate(Vector3.right, currentVelocity.y, Space.World);

                // Затухание инерции
                currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, inertiaDamping * Time.deltaTime);
                idleTimer = 0f; // Пока есть инерция — не считаем бездействие
            } else if (idleTimer >= returnDelay) {
                // Плавно возвращаем модель к исходной ротации
                Vector3 currentEuler = RotationNode.rotation.eulerAngles;
                float y = Mathf.LerpAngle(currentEuler.y, targetAngle, Time.deltaTime * returnSpeed);
                Vector3 newEuler = new Vector3(0, y, 0);
                RotationNode.rotation = Quaternion.Euler(newEuler);
                //RotationNode.rotation = Quaternion.Slerp(RotationNode.rotation, initialRotation, Time.deltaTime * returnSpeed);
            }
        }
    }
}
