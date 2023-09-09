using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
}