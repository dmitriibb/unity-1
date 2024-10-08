using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == Constants.TAG_PLAYER)
        {
            if (collider.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(false);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
            }
        }
    }
}
