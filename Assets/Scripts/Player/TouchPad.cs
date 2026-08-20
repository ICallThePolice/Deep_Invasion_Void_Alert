using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Настройки чувствительности и плавности")]
    [Range(0.0001f, 0.01f)]
    public float sensitivity = 0.001f; // Сверхмалый базовый множитель
    public float smoothing = 10f;      // Скорость инерции/догоняния (чем меньше, тем мягче)

    public Transform cameraPivot;

    private Vector2 targetRotation;
    private Vector2 currentRotation;
    private bool isDragging = false;

    void Start()
    {
        if (cameraPivot != null)
        {
            currentRotation = cameraPivot.localEulerAngles;
            targetRotation = currentRotation;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (cameraPivot == null) return;

        // Накапливаем целевой угол только когда тянем палец
        if (isDragging)
        {
            float deltaX = eventData.delta.x * sensitivity * 100f;
            float deltaY = eventData.delta.y * sensitivity * 100f;

            targetRotation.y += deltaX;
            targetRotation.x -= deltaY;

            targetRotation.x = Mathf.Clamp(targetRotation.x, -80f, 80f);
        }
    }

    void Update()
    {
        if (cameraPivot == null) return;

        // Плавно интерполируем текущий поворот к целевому (создает эффект инерции и мягкости)
        currentRotation.x = Mathf.Lerp(currentRotation.x, targetRotation.x, Time.deltaTime * smoothing);
        currentRotation.y = Mathf.Lerp(currentRotation.y, targetRotation.y, Time.deltaTime * smoothing);

        cameraPivot.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0f);
    }
}