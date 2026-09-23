using UnityEngine;
using UnityEngine.InputSystem;

public class RotateModel : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 0.2f;
    [SerializeField] float inercia = 5f;

    private AudioSource audioSource;

    private Vector2 lastTouchPosition;
    private float rotationVelocity = 0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                lastTouchPosition = touch.position.ReadValue();

                Ray ray = Camera.main.ScreenPointToRay(touch.position.ReadValue());

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == transform)
                    {
                        audioSource.Play();
                    }
                }
            }

            if (touch.press.isPressed)
            {
                Vector2 currentTouchPosition = touch.position.ReadValue();
                Vector2 delta = currentTouchPosition - lastTouchPosition;

                rotationVelocity = -delta.x * rotationSpeed;

                transform.Rotate(rotationVelocity, 0f, 0f);

                lastTouchPosition = currentTouchPosition;
            }
        }

        if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.isPressed)
        {
            transform.Rotate(rotationVelocity, 0f, 0f);

            rotationVelocity = Mathf.Lerp(rotationVelocity, 0f, inercia * Time.deltaTime);
        }
    }
}
