using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed = 1f; // Скорость поворота игрока

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Получаем позицию курсора в экранных координатах
        Vector3 screenPos = Input.mousePosition;

        // Получаем позицию курсора в мировых координатах на плоскости Z=0
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCamera.transform.position.y));

        // Определяем вектор направления от игрока к позиции курсора
        Vector3 direction = worldPos - transform.position;
        direction.y = 0f; // Устанавливаем Y-компоненту вектора равной 0, чтобы игрок не наклонялся в сторону курсора по вертикали

        // Интерполируем текущее направление игрока к направлению курсора с помощью сферической интерполяции
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Плавно поворачиваем игрока в заданное направление
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}