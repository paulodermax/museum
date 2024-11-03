using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;           // Geschwindigkeit für die Bewegung
    public float lookSensitivity = 2f;     // Empfindlichkeit der Maussteuerung

    private Rigidbody rb;
    private float xRotation = 0f;          // Speichert die vertikale Rotation für die Kamera

    public Transform playerCamera;         // Verweise auf die Kamera, die als Kindobjekt der Kapsel sein sollte

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Verhindert, dass die Kapsel durch Kollisionen kippt

        // Stellt sicher, dass der Cursor im Spiel fixiert ist und unsichtbar wird
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        LookAround();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Bewegung in Richtung der Blickrichtung
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 newPosition = transform.position + move * moveSpeed * Time.deltaTime;

        rb.MovePosition(newPosition); // Physik-basiertes Verschieben
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Horizontale Rotation des gesamten Körpers (Dreht den Spieler um die Y-Achse)
        transform.Rotate(Vector3.up * mouseX);

        // Vertikale Rotation der Kamera (Blick nach oben/unten), begrenzt auf -90° bis 90°
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Anwenden der Rotation auf die Kamera
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
