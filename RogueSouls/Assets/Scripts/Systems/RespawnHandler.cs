using UnityEngine;

public class RespawnHandler : MonoBehaviour
{
    [SerializeField]
    Transform _respawnPoint;

    PlayerStats _playerStats;

    [SerializeField]
    PolygonCollider2D _newCameraBounds;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            _playerStats = other.gameObject.GetComponent<PlayerStats>();
            _playerStats.ChangeRespawnPoint(_respawnPoint);
            _playerStats.SetRespawnCameraBounds(_newCameraBounds);
        }
    }
}
