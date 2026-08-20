using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Настройки чувствительности")]
    public float sensitivity = 0.2f;
    private Vector2 rotation;
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void LateUpdate()
    {
        // Проверяем нажатие (палец на экране или левая кнопка мыши)
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            isDragging = true;
            lastMousePosition = GetCurrentTouchPosition();
        }
        // Проверяем отпускание
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentPosition = GetCurrentTouchPosition();
            Vector3 delta = currentPosition - lastMousePosition;
            lastMousePosition = currentPosition;

            if (delta != Vector3.zero)
            {
                rotation.y += delta.x * sensitivity;
                rotation.x -= delta.y * sensitivity;

                // Ограничиваем угол обзора по вертикали
                rotation.x = Mathf.Clamp(rotation.x, -80f, 80f);

                transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
            }
        }
    }

    // Вспомогательный метод для получения координат как от мыши, так и от пальца
    private Vector3 GetCurrentTouchPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        return Input.mousePosition;
    }
}