using UnityEngine;

public class MultiplierCounter : MonoBehaviour
{
    internal int Multiplier { get; private set; } = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerPiece playerPiece))
        {
            Multiplier = Mathf.Min(Mathf.CeilToInt(playerPiece.GetComponentInParent<Player>().Score / 2.0f), 5);
        }
    }
}
