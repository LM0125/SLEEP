using UnityEngine;
public class MouseManager : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

public class SimpleFPSController : MonoBehaviour
{
    public float moveSpeed = 5f;        // 移动速度
    public float mouseSensitivity = 2f; // 鼠标灵敏度
    public Transform cameraTransform;   // 绑定你的主摄像机

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        Vector3 move = transform.right * x + transform.forward * z;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 控制上下视角（只旋转摄像机，不旋转身体）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 控制左右转向（旋转整个Player）
        transform.Rotate(Vector3.up * mouseX);
    }
}
