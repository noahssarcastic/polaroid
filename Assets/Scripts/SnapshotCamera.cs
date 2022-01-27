using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class SnapshotCamera : MonoBehaviour
{
    [SerializeField] private Image snapshotViewArea;
    [SerializeField] private GameObject polaroid;

    private PlayerInput actions;
    private Camera snapshotCamera;
    private Texture2D snapshot;
    private bool isTakingSnapshot;

    public void Awake()
    {
        actions = new PlayerInput();
        actions.Player.Enable();
        snapshotCamera = GetComponent<Camera>();
        snapshotCamera.enabled = false;
        snapshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        isTakingSnapshot = false;
    }

    public void OnEnable()
    {
        actions.Player.Snap.performed += Snap;
    }

    public void OnDisable()
    {
        actions.Player.Snap.performed -= Snap;
    }

    public void Update()
    {
        if (isTakingSnapshot)
        {
            isTakingSnapshot = false;
            StartCoroutine(CaptureSnapshot());
        }
    }

    private IEnumerator CaptureSnapshot()
    {
        yield return new WaitForEndOfFrame();

        // Copy the pixels from the Camera's render target to the texture
        Rect region = new Rect(0, 0, Screen.width, Screen.height);
        snapshot.ReadPixels(region, 0, 0, false);

        // Upload texture data to the GPU, so the GPU renders the updated texture
        snapshot.Apply();

        snapshotCamera.enabled = false;
        ShowSnapshot();
    }

    private void ShowSnapshot()
    {
        Rect rect = new Rect(0f, 0f, snapshot.width, snapshot.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite snapshotSprite = Sprite.Create(snapshot, rect, pivot, 100f);

        snapshotViewArea.sprite = snapshotSprite;
        polaroid.SetActive(true);
    }

    public void Snap(InputAction.CallbackContext context)
    {
        if (!isTakingSnapshot)
        {
            isTakingSnapshot = true;
            snapshotCamera.enabled = true;
        }
    }
}
