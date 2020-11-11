using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class RandomMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 min;
    [SerializeField]
    private Vector2 max;
    [SerializeField]
    private Vector2 yRotationRange;
    [SerializeField]
    [Range(0.01f,0.1f)]
    private float lerpSpeed = 0.05f;
    private Vector3 _newPosition;
    private Quaternion _newRotation;

    private void Awake()
    {
        _newPosition = transform.position;
        _newRotation = transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * lerpSpeed);

        if (Vector3.Distance(transform.position, _newPosition) < 1f)
            GetNewPosition();
    }

    void GetNewPosition()
    {
        var xPos = Random.Range(min.x, max.x);
        var zPos = Random.Range(min.y, max.y);
        _newRotation = Quaternion.Euler(0, Random.Range(yRotationRange.x, yRotationRange.y), 0);
        _newPosition = new Vector3(xPos, 0, zPos);
    }
}
