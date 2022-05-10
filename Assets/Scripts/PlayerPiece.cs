using System.Linq;
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

            var character = GetComponentInChildren<Animator>();
            if (character != null && _player.Score > 0)
            {
                // Move character to the latest.
                character.transform.parent = transform.parent.GetComponentsInChildren<PlayerPiece>().Last().transform;
                character.transform.localPosition = new Vector3(0.25f, 0.5f, -0.25f);
            }
            
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
        if (_isInLava && _rigidbody.velocity.y < -0.5f)
        {
            _rigidbody.velocity = new Vector3(0.0f, -0.5f, 0.0f);
        }
    }
}
