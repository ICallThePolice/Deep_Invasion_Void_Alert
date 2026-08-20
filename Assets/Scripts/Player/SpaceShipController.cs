using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShipController : MonoBehaviour
{
    public Transform cameraTransform;
    public float moveSpeed = 15f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void FixedUpdate()
    {
        if (cameraTransform == null) return;

        Keyboard keyboard = Keyboard.current;
        float moveInput = 0f;
        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed) moveInput += 1f;
            if (keyboard.sKey.isPressed) moveInput -= 1f;
        }

        // 1. Движение строго вперед/назад по вектору камеры
        Vector3 moveDirection = cameraTransform.forward;
        rb.AddForce(moveDirection * moveInput * moveSpeed, ForceMode.Acceleration);

        // 2. Плавное гашение инерции при отпускании клавиш
        if (Mathf.Abs(moveInput) < 0.01f)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 2f);
        }

        // 3. Плавный поворот корабля лицом туда, куда смотрит камера
        // Используем LookRotation с учетом верхнего вектора камеры
        Quaternion targetRotation = Quaternion.LookRotation(cameraTransform.forward, cameraTransform.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
    }
}