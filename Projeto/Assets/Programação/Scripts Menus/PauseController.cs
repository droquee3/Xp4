using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    public EventSystem eventSystem;
    public GameObject firstSelected;
    public GameObject[] buttons;
    
    public CameraOrbital customCamera;

    private bool isPaused = false; 
    private int currentIndex = 0;
    private float moveThreshold = 0.2f; 
    private bool isNavigating = false; 

    void Start()
    {
        pauseMenuUI.SetActive(false);

        if (firstSelected != null)
        {
            eventSystem.SetSelectedGameObject(firstSelected);
            currentIndex = System.Array.IndexOf(buttons, firstSelected);
            UpdateButtonVisuals();
        }
        SetupButtonActions();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (isPaused)
        {
            float vertical = Input.GetAxis("Vertical");

            if (vertical > moveThreshold && !isNavigating)
            {
                currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
                UpdateButtonVisuals();
                isNavigating = true;
            }
            else if (vertical < -moveThreshold && !isNavigating)
            {
                currentIndex = (currentIndex + 1) % buttons.Length;
                UpdateButtonVisuals();
                isNavigating = true;
            }
            else if (Mathf.Abs(vertical) < moveThreshold)
            {
                isNavigating = false;
            }

            if (Input.GetButtonDown("Submit"))
            {
                if (eventSystem.currentSelectedGameObject != null)
                {
                    ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, new PointerEventData(eventSystem), ExecuteEvents.submitHandler);
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f; 
        isPaused = false;
        eventSystem.SetSelectedGameObject(null);

        ResetPlayerInputs();

        if (customCamera != null)
        {
            customCamera.SetPauseState(false);
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); 
        Time.timeScale = 0f; 
        isPaused = true;
        currentIndex = 0;
        eventSystem.SetSelectedGameObject(firstSelected);
        UpdateButtonVisuals();

        if (customCamera != null)
        {
            customCamera.SetPauseState(true);
        }
    }

    private void ResetPlayerInputs()
    {
        Input.ResetInputAxes();
    }

    private void SetupButtonActions()
    {
        Button continueButton = buttons[0].GetComponent<Button>(); 
        continueButton.onClick.AddListener(() => {
            Resume(); 
        });

        Button configButton = buttons[1].GetComponent<Button>(); 
        configButton.onClick.AddListener(() => {
            Debug.Log("Config button clicked"); // Verificação
        });

        Button quitButton = buttons[2].GetComponent<Button>(); 
        quitButton.onClick.AddListener(() => {
            Debug.Log("Quit to Menu button clicked"); // Verificação
            QuitToMenu();
        });
    }

    private void QuitToMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void UpdateButtonVisuals()
    {
        foreach (GameObject button in buttons)
        {
            button.transform.localScale = Vector3.one; 
        }

        buttons[currentIndex].transform.localScale = Vector3.one * 1.1f;
        eventSystem.SetSelectedGameObject(buttons[currentIndex]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Transform buttonTransform = eventData.pointerEnter.transform;
        buttonTransform.localScale = Vector3.one * 1.1f; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Transform buttonTransform = eventData.pointerEnter.transform;
        buttonTransform.localScale = Vector3.one; 
    }
}
