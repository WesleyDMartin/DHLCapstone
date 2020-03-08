using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float r = 0.5f;
    public bool rotating = false;
    public float speed = 3.0f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var moveDelta = new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            Input.GetAxis("Vertical") * speed * Time.deltaTime, 0);

        transform.Translate(moveDelta, Space.World);

        var rotationAxis = Vector3.Cross(moveDelta.normalized, Vector3.forward);

        transform.RotateAround(transform.position, rotationAxis,
            Mathf.Sin(moveDelta.magnitude * r * 2 * Mathf.PI) * Mathf.Rad2Deg);
    }
}