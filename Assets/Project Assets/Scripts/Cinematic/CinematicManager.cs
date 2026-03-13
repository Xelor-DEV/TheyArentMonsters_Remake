using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CinematicManager : MonoBehaviour
{
    [Header("UI References")]
    public VideoPlayer videoPlayer;
    public Image skipWheel;

    [Header("Settings")]
    public float timeToSkip = 2f;
    public float drainSpeed = 2f;

    [Header("Prueba Local (Solo para test)")]
    public CinematicData testData; // <--- Arrastra aquí tu archivo de la cajita azul

    private float timer = 0f;
    private bool isPressing = false;
    private bool isSceneLoading = false;

    public static CinematicData dataToPlay;

    void Start()
    {
        // Si dataToPlay es null (porque no vinimos de otra escena), usa el testData
        if (dataToPlay == null && testData != null)
        {
            dataToPlay = testData;
        }

        if (dataToPlay != null)
        {
            videoPlayer.clip = dataToPlay.clip;
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnded;
        }
        else
        {
            Debug.LogWarning("No hay datos de cinemática asignados en dataToPlay ni en testData.");
        }

        if (skipWheel != null) skipWheel.fillAmount = 0;
    }

    // ... (el resto del código OnSkip, Update, etc., se queda exactamente igual que lo tienes)

    public void OnSkip(InputAction.CallbackContext context)
    {
        if (context.started || context.performed) isPressing = true;
        else if (context.canceled) isPressing = false;
    }

    void Update()
    {
        if (isSceneLoading) return;
        HandleSkipProgress();
    }

    private void HandleSkipProgress()
    {
        if (isPressing) timer += Time.deltaTime;
        else timer = Mathf.Max(0, timer - Time.deltaTime * drainSpeed);

        if (skipWheel != null) skipWheel.fillAmount = timer / timeToSkip;

        if (timer >= timeToSkip) GoToNextScene();
    }

    private void OnVideoEnded(VideoPlayer vp) => GoToNextScene();

    public void GoToNextScene()
    {
        if (isSceneLoading) return;
        isSceneLoading = true;

        if (dataToPlay != null && !string.IsNullOrEmpty(dataToPlay.SceneNext))
            SceneManager.LoadScene(dataToPlay.SceneNext);
        else
            Debug.LogError("Error: No se encontró el nombre de la escena en el ScriptableObject.");
    }
}