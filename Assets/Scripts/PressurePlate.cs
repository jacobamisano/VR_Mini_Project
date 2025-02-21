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
        if (other.CompareTag("Weight"))
        {
            objectsOnPlate++;

            if (!isPressed && objectsOnPlate >= requirementCount)
            {
                isPressed = true;
                gameObject.transform.localScale = new Vector3(transform.localScale.x, (float)0.0017, transform.localScale.z);
                onPressed?.Invoke();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weight"))
        {
            objectsOnPlate--;

            if (objectsOnPlate < requirementCount)
            {
                isPressed = false;
                if (transform.localScale.y != 0.035)
                {
                    gameObject.transform.localScale = new Vector3(transform.localScale.x, (float)0.035, transform.localScale.z);
                }
                onReleased?.Invoke();
            }
        }
    }
}
