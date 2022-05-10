using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocity = 1.0f;
    public GameObject tower;
    
    private GameObject _camera;
    private GameObject _particles;
    private bool _isRotating = false;
    private bool _isFinishing = false;
    private Vector3 _rotatingAxis = Vector3.positiveInfinity;
    private Vector3 _rotatingPoint = Vector3.positiveInfinity;

    internal int Score { get; set; } = 1;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>().gameObject;
        _particles = GetComponentInChildren<ParticleSystem>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Turn _))
        {
            _isRotating = true;
            _rotatingAxis = -other.transform.parent.up;
            _rotatingPoint = other.transform.parent.position;
            
            // If turn to right.
            if (Vector3.Dot(transform.forward, other.transform.parent.forward) <= 0.5f)
            {
                _rotatingAxis *= -1;
            }
            
            return;
        }
        
        if (other.transform.TryGetComponent(out Straight _))
        {
            _isRotating = false;
            // Align.
            transform.rotation = other.transform.parent.rotation;
            
            return;
        }

        if (other.transform.TryGetComponent(out Finish _))
        {
            _isRotating = false;
            _isFinishing = true;
            // Align.
            transform.rotation = other.transform.parent.rotation;
        }
    }
    
    private void Update()
    {
        if (Score == 0)
        {
            _particles.SetActive(false);
            return;
        }

        if (Input.touchCount > 0)
        {
            // Calculate position between 0 and 1. Use 75% of screen's width.
            float positionX = Input.GetTouch(0).deltaPosition.x / (Screen.width * 0.75f);
            
            // Update.
            Vector3 position = tower.transform.localPosition;
            position.x = Mathf.Clamp(position.x + positionX * 2.5f, 0.0f, 2.5f);
            tower.transform.localPosition = position;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 position = tower.transform.localPosition;
            position.x = Mathf.Clamp(position.x - velocity * Time.deltaTime, 0.0f, 2.5f);
            tower.transform.localPosition = position;
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 position = tower.transform.localPosition;
            position.x = Mathf.Clamp(position.x + velocity * Time.deltaTime, 0.0f, 2.5f);
            tower.transform.localPosition = position;
        }
        
        if (_isRotating)
        {
            float angularVelocity = velocity / Vector3.Magnitude(transform.position - _rotatingPoint);
            transform.RotateAround(_rotatingPoint, _rotatingAxis, Time.deltaTime * angularVelocity * Mathf.Rad2Deg);
        }
        else
        {
            transform.position += transform.forward * (velocity * Time.deltaTime);
        }

        if (_isFinishing)
        {
            _camera.transform.position += transform.up * (velocity / Mathf.Sqrt(2.0f) * Time.deltaTime);
        }
    }

    internal void CreateCube()
    {
        // Top cube.
        GameObject cube = GetComponentInChildren<Animator>().transform.parent.gameObject;
        
        // Create at the same position.
        GameObject newCube = Instantiate(cube, cube.transform.position, cube.transform.rotation, tower.transform);
        // Remove character.
        Destroy(newCube.GetComponentInChildren<Animator>().gameObject);
        
        Score++;
        // Move a cube a bit higher.
        Vector3 position = cube.transform.position;
        position.y += 0.75f;
        cube.transform.position = position;
    }
}
