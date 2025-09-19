using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] 
    private float sensitivity = 1f;

    private GameObject cameraHolder;
    private InputManager inputManager;
    private float maxAngle = 85f; // muss > 0 und < 90 sein

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraHolder = transform.Find("CameraHolder")?.gameObject;
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 delta = inputManager.GetMouseDelta() * sensitivity;
        transform.eulerAngles += new Vector3(0, delta.x, 0);

        // verhindert das sich die Kamera �berschl�gt
        float newAngle = cameraHolder.transform.eulerAngles.x - delta.y;

        if (newAngle > maxAngle && newAngle < 180)
        {
            cameraHolder.transform.eulerAngles = new Vector3(maxAngle, cameraHolder.transform.eulerAngles.y, cameraHolder.transform.eulerAngles.z);
        }
        else if (newAngle < 360 - maxAngle && newAngle > 180)
        {
            cameraHolder.transform.eulerAngles = new Vector3(360 - maxAngle, cameraHolder.transform.eulerAngles.y, cameraHolder.transform.eulerAngles.z);
        }
        else
        {
            cameraHolder.transform.eulerAngles -= new Vector3(delta.y, 0, 0);
        }
        
    }
}
