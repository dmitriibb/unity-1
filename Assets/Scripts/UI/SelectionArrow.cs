using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    private RectTransform rect;
    private int currentPosition;

    private void Awake() {
        rect = GetComponent<RectTransform>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            ChangePosition(-1);
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            ChangePosition(1);
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            Interract();
        }
    }

    private void ChangePosition(int position) {
        currentPosition += position;
        currentPosition = Mathf.Abs(currentPosition % options.Length);
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, rect.position.z);
    }

    private void Interract() {
        print($"Interract - {currentPosition}");
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
