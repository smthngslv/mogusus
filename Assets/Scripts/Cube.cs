using UnityEngine;

public class Cube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerPiece playerPiece))
        {
            playerPiece.GetComponentInParent<Player>().CreateCube();
            Destroy(gameObject);
        }
    }
}
