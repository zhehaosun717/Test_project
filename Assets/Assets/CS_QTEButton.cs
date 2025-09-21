using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to the QTE button prefab's foreground object.
/// Manages the button's countdown visual, click detection, and self-destruction.
/// </summary>
[RequireComponent(typeof(Button), typeof(Image))]
public class CS_QTEButton : MonoBehaviour
{
    /// <summary>
    /// A reference to the QTE Manager that spawned this button.
    /// This is set by the manager upon instantiation.
    /// </summary>
    [HideInInspector]
    public CS_QTEManager manager;
    
    [Tooltip("The lifetime of this button in seconds.")]
    public float lifetime = 2f;

    private float spawnTime;
    private Button button;
    private Image progressImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        
        progressImage = GetComponent<Image>();
        if (progressImage.type != Image.Type.Filled)
        {
            Debug.LogWarning("The Image component on the QTE Button is not set to 'Filled'. The countdown visual will not work.", this);
        }
    }

    private void Start()
    {
        // Record the time this button was created to calculate its age.
        spawnTime = Time.time;
    }

    private void Update()
    {
        float age = Time.time - spawnTime;
        if (age >= lifetime)
        {
            // Time is up. Destroy the parent object, which is the root of the entire button prefab
            // (the container holding both the background and this foreground).
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            return;
        }

        // Update the fill amount of the image to represent the countdown timer, going from 1 (full) to 0 (empty).
        progressImage.fillAmount = 1f - (age / lifetime);
    }

    /// <summary>
    /// Called when the button component is clicked by the user.
    /// </summary>
    private void OnClick()
    {
        if (manager != null)
        {
            // Notify the manager that this button was successfully clicked.
            manager.OnQTESuccess();
        }
        // The manager is now responsible for destroying this button.
    }
}