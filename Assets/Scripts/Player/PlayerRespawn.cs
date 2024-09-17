using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;

    private void Awake() {
        playerHealth = GetComponent<Health>();
    }

    public void RespwanPlayer()
    {
        print($"RespwanPlayer - currentCheckpoint.position = {currentCheckpoint.position}");
        transform.position = currentCheckpoint.position;
        playerHealth.RespwanHealth();

        // Move camera
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Constants.TAG_CHECKPOINT)) {
            currentCheckpoint = other.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            other.GetComponent<Collider2D>().enabled = false;
            other.GetComponent<Animator>().SetTrigger(Constants.TRIGGER_CHECKPOINT_APPEAR);
        }
    }
    
}
