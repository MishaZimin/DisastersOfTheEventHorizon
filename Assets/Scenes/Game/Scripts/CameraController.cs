using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;

    public float panSpeed = 30f;
    public float scrollSpeed = 5f;
    public float mousePanSpeed = 0.35f; // увеличим скорость
    public float minY = 10f;
    public float maxY = 80f;

    private Vector3 lastMousePosition;
    private float lastMouseUpdateTime = 0f;
    private float mouseUpdateInterval = 0.01f; // 10 мс

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            doMovement = !doMovement;

        if (!doMovement)
            return;

        // Движение по WASD
        if (Input.GetKey("w"))
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey("s"))
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey("d"))
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey("a"))
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);

        // Зум колесиком
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;

        // Панорамирование мышью (ПКМ)
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
            lastMouseUpdateTime = Time.time;
        }

        if (Input.GetMouseButton(1) && Time.time - lastMouseUpdateTime >= mouseUpdateInterval)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, 0, -delta.y) * mousePanSpeed;
            transform.Translate(move, Space.World);

            lastMousePosition = Input.mousePosition;
            lastMouseUpdateTime = Time.time;
        }
    }
}