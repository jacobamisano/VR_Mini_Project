using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent onPressed;
    [SerializeField] private UnityEvent onReleased;
    private bool isPressed = false;
    private int objectsOnPlate = 0;
    public int requirementCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Weight"))
        {
            objectsOnPlate++;

            if (!isPressed && objectsOnPlate >= requirementCount)
            {
                isPressed = true;
                gameObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y/2, transform.localScale.z);
                onPressed?.Invoke();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Weight"))
        {
            objectsOnPlate--;

            if (objectsOnPlate < requirementCount)
            {
                isPressed = false;
                gameObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
                onReleased?.Invoke();
            }
        }
    }
}
