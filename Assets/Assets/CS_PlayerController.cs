using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the player's cube, including movement, trail generation, and QTE-based growth.
/// </summary>
public class CS_PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The movement speed of the cube.")]
    public float moveSpeed = 5f;

    [Header("Trail Settings")]
    [Tooltip("The prefab for the trail sphere to be spawned.")]
    public GameObject trailPrefab;
    [Tooltip("Time in seconds between each trail sphere spawn.")]
    public float trailSpawnRate = 0.1f;

    [Header("Camera Settings")]
    [Tooltip("A reference to the main camera to enable follow behavior.")]
    public Transform mainCamera;

    [Header("QTE Level Settings")]
    [Tooltip("The sequence of colors the cube will cycle through as it levels up.")]
    public Color[] levelColors = new Color[]
    {
        Color.red,
        new Color(1f, 0.5f, 0f), // Orange
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        new Color(0.5f, 0f, 1f)  // Violet
    };

    private Rigidbody rb;
    private InputSystem_Actions playerInputActions;
    private Vector2 moveInput;
    private Vector3 cameraOffset;
    private float nextSpawnTime;
    private float currentHue = 0f;
    
    // QTE State
    private int qteSuccessCount = 0;
    private int level = 0;
    private Renderer cubeRenderer;
    private Vector3 initialScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInputActions = new InputSystem_Actions();

        cubeRenderer = GetComponent<Renderer>();
        initialScale = transform.localScale;
        UpdateLevelColor();

        if (mainCamera != null)
        {
            cameraOffset = mainCamera.position - transform.position;
        }
        else
        {
            Debug.LogWarning("Main Camera is not assigned in the Inspector. The camera will not follow the player.", this);
        }
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        // Continuously update the hue for the trail's rainbow effect.
        currentHue += Time.deltaTime * 0.2f;
        if (currentHue > 1f)
        {
            currentHue -= 1f;
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        
        // This is the standard and correct way to move a Rigidbody with Input System data.
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Spawn a trail sphere if the player is moving and the spawn cooldown has passed.
        if (moveDirection.sqrMagnitude > 0.1f && Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + trailSpawnRate;
            SpawnTrailSphere();
        }
    }

    private void LateUpdate()
    {
        // The camera is updated in LateUpdate to ensure the player has finished its movement for the frame.
        if (mainCamera != null)
        {
            mainCamera.position = transform.position + cameraOffset;
        }
    }

    /// <summary>
    /// Spawns a single trail sphere behind the player, scaling it based on the player's current size.
    /// </summary>
    private void SpawnTrailSphere()
    {
        if (trailPrefab == null) return;

        GameObject trailSphere = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        
        // Calculate the current size multiplier based on the player's growth from its initial scale.
        if (initialScale.x > 0)
        {
            float scaleFactor = transform.localScale.x / initialScale.x;
            trailSphere.transform.localScale = trailPrefab.transform.localScale * scaleFactor;
        }

        Renderer sphereRenderer = trailSphere.GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            // Assign a unique material to the sphere to give it a color from the rainbow spectrum.
            // This prevents all trail spheres from sharing and changing the same material.
            Color trailColor = Color.HSVToRGB(currentHue, 0.8f, 1f);
            trailColor.a = 0.6f;
            sphereRenderer.material.color = trailColor;
        }
    }

    /// <summary>
    /// Updates the cube's material color based on its current level.
    /// </summary>
    private void UpdateLevelColor()
    {
        if (cubeRenderer != null && level < levelColors.Length)
        {
            cubeRenderer.material.color = levelColors[level];
        }
    }

    /// <summary>
    /// Public method called by the QTE Manager upon a successful QTE click.
    /// This handles the cube's growth and leveling up.
    /// </summary>
    public void OnQTESuccess()
    {
        // Increase the cube's scale by 10%.
        transform.localScale *= 1.1f;

        // Check if the cube should level up.
        qteSuccessCount++;
        if (qteSuccessCount >= 2)
        {
            qteSuccessCount = 0; // Reset counter
            if (level < levelColors.Length - 1)
            {
                level++;
                UpdateLevelColor();
            }
        }
    }
}
