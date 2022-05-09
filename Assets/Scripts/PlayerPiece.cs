using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rigidbody;
    private bool _isInLava = false;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Lava _))
        {
            _isInLava = true;
            return;
        }
        
        if (other.transform.TryGetComponent(out Obstacle _))
        {
            --_player.Score;
            transform.parent = null;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out Lava _))
        {
            _isInLava = false;
            --_player.Score;
            Destroy(gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        // Can only fall down.
        if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        
        // Avoid too fast falling.
        if (_isInLava && _rigidbody.velocity.y < -1.0f)
        {
            _rigidbody.velocity = new Vector3(0.0f, -1.0f, 0.0f);
        }
    }
}
