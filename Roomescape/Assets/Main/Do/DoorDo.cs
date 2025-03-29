using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// Controls a door: click to open/close. 
/// When fully opened, it shows a puzzle UI.
/// Also allows passing a reference to disable or enable camera/player controls.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DoorDo : MonoBehaviour
{
    [Header("Door State")]
    public bool IsOpen = false;

    [Header("Rotation Pivot (Optional)")]
    public Transform Target;
    // If the actual pivot is a different transform, drag it here.
    // Otherwise, leave empty to rotate this object itself.

    [Header("Door Angles")]
    public float closeAngle = 0f;
    public float openAngle = 90f;

    [Header("Animation Duration")]
    public float durTime = 1f;

    [Header("Puzzle Question & Answer")]
    public string puzzleQuestion = "1 + 1 = ?";
    public string puzzleAnswer = "2";

    [Header("Optional Player/Camera Script")]
    public MonoBehaviour cameraOrPlayerScript;
    // If you have a camera or player controller script that locks the mouse,
    // drag it here so we can disable/enable it. If not, leave it empty.

    private void OnMouseDown()
    {
        // Prevent clicks when pointer is over UI
        if (IsPointerOverUI()) return;

        // Toggle door state
        IsOpen = !IsOpen;

        // Decide which transform to rotate
        var rotateObj = (Target == null) ? transform : Target;

        if (IsOpen)
        {
            // Rotate from closeAngle to openAngle
            DOTween.To(
                value => rotateObj.localEulerAngles = new Vector3(0, value, 0),
                closeAngle,
                openAngle,
                durTime
            )
            .OnComplete(() =>
            {
                // After fully opened, show puzzle UI
                PuzzleUI puzzle = FindObjectOfType<PuzzleUI>();
                if (puzzle != null)
                {
                    // Tell the puzzle which camera script to disable (optional)
                    puzzle.cameraOrPlayerScript = cameraOrPlayerScript;
                    puzzle.ShowPuzzle(puzzleQuestion, puzzleAnswer);
                }
                else
                {
                    Debug.LogWarning("PuzzleUI not found in the scene. Cannot show puzzle.");
                }
            });
        }
        else
        {
            // Rotate from openAngle back to closeAngle
            DOTween.To(
                value => rotateObj.localEulerAngles = new Vector3(0, value, 0),
                openAngle,
                closeAngle,
                durTime
            );
        }
    }

    /// <summary>
    /// Checks if the current mouse/touch is over any UI element
    /// to avoid door interaction when clicking UI.
    /// </summary>
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
        return EventSystem.current.IsPointerOverGameObject();
    }
}
