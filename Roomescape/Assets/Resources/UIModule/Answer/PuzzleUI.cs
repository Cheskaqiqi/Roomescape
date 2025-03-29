using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;         // The puzzle panel (UI)
    public Text questionText;        // Text component for the puzzle question
    public InputField answerInput;   // InputField for player's answer

    [Header("Optional Camera/Player Script")]
    public MonoBehaviour cameraOrPlayerScript;
    // We can disable this while the puzzle is active,
    // so you can move the mouse freely.

    private string correctAnswer = ""; // Stores the correct answer

    void Start()
    {
        // Hide the panel at start
        if (panel != null)
        {
            panel.SetActive(false);
        }

        // Make sure the cursor is initially locked/hidden if your game uses that
        // (Optional - depends on your game setup)
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    /// <summary>
    /// Called by DoorDo after the door is fully opened. 
    /// Displays the puzzle question and allows free mouse cursor.
    /// </summary>
    /// <param name="question">Puzzle question text</param>
    /// <param name="answer">Correct answer</param>
    public void ShowPuzzle(string question, string answer)
    {
        if (panel == null)
        {
            Debug.LogWarning("Puzzle panel reference is missing.");
            return;
        }

        // Activate puzzle panel
        panel.SetActive(true);

        // Unlock and show cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable camera/player script if assigned
        if (cameraOrPlayerScript != null)
        {
            cameraOrPlayerScript.enabled = false;
        }

        // Update puzzle question and answer
        questionText.text = question;
        correctAnswer = answer;

        // Clear previous input
        answerInput.text = "";
    }

    /// <summary>
    /// Called by the "Confirm" button (OnClick event).
    /// Checks the player's input.
    /// </summary>
    public void OnConfirmAnswer()
    {
        if (answerInput.text == correctAnswer)
        {
            Debug.Log("Correct answer!");

            // Hide puzzle panel
            panel.SetActive(false);

            // Lock and hide cursor again if that's your gameplay style
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Re-enable camera/player controls
            if (cameraOrPlayerScript != null)
            {
                cameraOrPlayerScript.enabled = true;
            }
        }
        else
        {
            Debug.Log("Wrong answer. Try again.");
            // You could shake the UI or highlight the input field, etc.
        }
    }
}
