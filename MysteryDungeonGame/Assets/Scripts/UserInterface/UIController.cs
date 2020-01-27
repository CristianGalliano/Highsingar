using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private bool CameraZoomed = false;

    public Camera MainCamera;
    private float OrthographicSize;

    public GameObject M_ControlsPanel;

    [SerializeField]private Button cameraZoomButton;

    // Start is called before the first frame update
    void Start()
    {
        //#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR_LINUX
        //   M_ControlsPanel.gameObject.SetActive(false);
        //#endif

        OrthographicSize = CalculateDefaultOrthographicSize(TestPathGeneration.PathGenerator.GridSize);
        MainCamera.orthographicSize = OrthographicSize;

        //StartCoroutine(AdjustCamera(PlayerMovement.Player.transform, 5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cameraZoomButton.interactable)
        {
            OnCameraButtonClicked();
        }
    }

    public void OnRewindButtonClicked()
    {
        if (!PlayerMovement.Player.moving && PlayerMovement.Player.transform.position != PlayerMovement.Player.playerStartPosition)
        {
            PlayerMovement.Player.MoveToPrevious();
        }
    }

    public void OnCameraButtonClicked()
    {
        if (!PlayerMovement.Player.moving)
        {
            if (CameraZoomed)
            {
                StartCoroutine(AdjustCamera(null,OrthographicSize));
            }
            else
            {
                StartCoroutine(AdjustCamera(PlayerMovement.Player.transform.GetChild(0).transform, 5f));
            }
        }
        else
        {
            Debug.LogWarning("Cannot change Adjust Camera whilst the Player is moving!");
        }

    }

    private IEnumerator AdjustCamera(Transform targetParent, float targetCameraSize)
    {
        cameraZoomButton.interactable = false;
        MainCamera.transform.SetParent(targetParent);
        float elapsedTime = 0;
        Vector3 startingPos = MainCamera.transform.localPosition;
        float startingSize = MainCamera.orthographicSize;

        while (elapsedTime < (1f/3f))
        {
            MainCamera.transform.localPosition = Vector3.Lerp(startingPos, new Vector3(0, 0, -10), (elapsedTime / (1f/3f)));

            MainCamera.orthographicSize = Mathf.Lerp(startingSize, targetCameraSize, (elapsedTime / (1f/3f)));

            if (!CameraZoomed)
            {
                cameraZoomButton.GetComponentInChildren<Slider>().value = Mathf.Lerp(0.0f, 1.0f, (elapsedTime / (1f / 3f)));
            }
            else
            {
                cameraZoomButton.GetComponentInChildren<Slider>().value = Mathf.Lerp(1.0f, 0.0f, (elapsedTime / (1f / 3f)));
            }

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        MainCamera.transform.localPosition = new Vector3(0,0,-10);
        MainCamera.orthographicSize = targetCameraSize;
        if (!CameraZoomed)
        {
            cameraZoomButton.GetComponentInChildren<Slider>().value = 1f;
        }
        else
        {
            cameraZoomButton.GetComponentInChildren<Slider>().value = 0f;
        }
        CameraZoomed = !CameraZoomed;
        cameraZoomButton.interactable = true;
        yield return null;
    }

    private float CalculateDefaultOrthographicSize(int GridSize)
    {
        float Size = (GridSize / 2f) * 1.25f;
        return Size;
    }
}
