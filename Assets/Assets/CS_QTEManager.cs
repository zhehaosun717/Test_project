using UnityEngine;
using System.Collections;

public class CS_QTEManager : MonoBehaviour
{
    public GameObject qteButtonPrefab;
    public Canvas parentCanvas;
    public CS_PlayerController playerController;

    public float spawnInterval = 3f;

    [Tooltip("Controls how close to the edge buttons can spawn. 0 = anywhere, 0.2 = 20% border on all sides.")]
    [Range(0f, 0.4f)]
    public float spawnAreaPadding = 0.2f;

    [Header("Audio Settings")]
    public AudioSource sfxSource;
    public AudioClip buttonSpawnSound;
    public AudioClip buttonSuccessSound;

    private GameObject currentQTEButton;

    void Start()
    {
        if (playerController == null)
        {
            // Updated to use the new recommended method to eliminate the obsolete warning.
            playerController = FindFirstObjectByType<CS_PlayerController>();
        }
        StartCoroutine(QTEGenerator());
    }

    private IEnumerator QTEGenerator()
    {
        while (true)
        {
            // Wait for the next spawn time.
            yield return new WaitForSeconds(spawnInterval);

            // Only spawn a button if one isn't already active.
            // The reference becomes "pseudo-null" when the object is destroyed.
            if (currentQTEButton == null)
            {
                ShowNewQTEButton();
            }
        }
    }

    private void ShowNewQTEButton()
    {
        // This is a safety check. If the prefab is not assigned correctly, it will stop and give a clear error.
        if (qteButtonPrefab == null)
        {
            Debug.LogError("QTE Button Prefab is not assigned in the Inspector! Please drag the PREFAB ASSET from the PROJECT WINDOW, not an object from the scene.", this);
            this.enabled = false; // Disable this script to prevent spamming errors.
            return;
        }

        // Play the spawn sound
        if (sfxSource != null && buttonSpawnSound != null)
        {
            sfxSource.PlayOneShot(buttonSpawnSound);
        }

        currentQTEButton = Instantiate(qteButtonPrefab, parentCanvas.transform);
        
        RectTransform buttonRect = currentQTEButton.GetComponent<RectTransform>();
        RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();

        // This is the corrected position calculation logic.
        // It correctly handles the default centered anchor of UI elements.
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // 1. Calculate the total size of the safe area in the middle of the screen.
        float safeAreaWidth = canvasWidth * (1 - spawnAreaPadding * 2);
        float safeAreaHeight = canvasHeight * (1 - spawnAreaPadding * 2);

        // 2. Generate a random position within that safe area.
        // The range is from -half size to +half size because the anchor is in the center.
        float x = Random.Range(-safeAreaWidth / 2, safeAreaWidth / 2);
        float y = Random.Range(-safeAreaHeight / 2, safeAreaHeight / 2);

        buttonRect.anchoredPosition = new Vector2(x, y);

        // This allows the script to be on a child object of the prefab, not just the root.
        CS_QTEButton qteButton = currentQTEButton.GetComponentInChildren<CS_QTEButton>();
        qteButton.manager = this;
    }

    public void OnQTESuccess()
    {
        if (playerController != null)
        {
            playerController.OnQTESuccess();
        }
        
        // Play the success sound
        if (sfxSource != null && buttonSuccessSound != null)
        {
            sfxSource.PlayOneShot(buttonSuccessSound);
        }

        // When successful, destroy the button. The main loop will then be free
        // to create a new one after the next interval.
        if (currentQTEButton != null)
        {
            Destroy(currentQTEButton);
            currentQTEButton = null; // Explicitly set to null for clarity.
        }
    }
}