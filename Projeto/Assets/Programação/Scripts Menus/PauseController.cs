using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject OptionsUI;
    public GameObject[] buttons;          
    public int hoveredButtonIndex = -1;   
    public int selectedButtonIndex = 0;   
    public bool isControlMode = false;   

    public Vector3 normalScale = new Vector3(1, 1, 1);
    public Vector3 highlightedScale = new Vector3(1.2f, 1.2f, 1.2f); // Aumenta 20%

    private bool isPaused = false;

    public float inputDelay = 0.2f;  // Atraso entre as entradas do controle
    private float nextInputTime = 0f; // Tempo para a próxima entrada

    void Start()
    {
        pauseMenuUI.SetActive(false);
        OptionsUI.SetActive(false);

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            GameObject buttonObj = buttons[i];
            Button button = buttonObj.GetComponent<Button>();

            if (button != null)
            {
                string buttonName = buttonObj.name;

                button.onClick.AddListener(() => ButtonClicked(buttonName));

                buttonObj.transform.localScale = normalScale;

                EventTrigger trigger = buttonObj.AddComponent<EventTrigger>();

                EventTrigger.Entry entryEnter = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                entryEnter.callback.AddListener((eventData) => { OnMouseHover(index); });
                trigger.triggers.Add(entryEnter);

                EventTrigger.Entry entryExit = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerExit
                };
                entryExit.callback.AddListener((eventData) => { OnMouseExit(); });
                trigger.triggers.Add(entryExit);
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (isPaused)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Submit"))
            {
                if (!isControlMode)
                {
                    isControlMode = true;
                    Debug.Log("Modo controle ativado");
                }
            }

            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                if (isControlMode)
                {
                    isControlMode = false;
                    Debug.Log("Modo mouse ativado");
                }
            }

            if (isControlMode)
            {
                HandleControllerInput();
            }
        }
    }

     private void HandleControllerInput()
    {
        // Verifica se o controle está ativo e se o tempo atual é maior ou igual ao tempo de entrada permitido
        if (isControlMode)
        {
            float verticalInput = Input.GetAxis("Vertical");

            // Verifica se há entrada vertical significativa
            if (Mathf.Abs(verticalInput) > 0.1f && Time.time >= nextInputTime)
            {
                if (verticalInput > 0) // Cima
                {
                    selectedButtonIndex = (selectedButtonIndex - 1 + buttons.Length) % buttons.Length;
                    UpdateButtonVisuals();
                    nextInputTime = Time.time + inputDelay; // Atualiza o tempo para a próxima entrada
                }
                else if (verticalInput < 0) // Baixo
                {
                    selectedButtonIndex = (selectedButtonIndex + 1) % buttons.Length;
                    UpdateButtonVisuals();
                    nextInputTime = Time.time + inputDelay; // Atualiza o tempo para a próxima entrada
                }
            }

            // Aciona o botão selecionado
            if (Input.GetButtonDown("Submit"))
            {
                buttons[selectedButtonIndex].GetComponent<Button>().onClick.Invoke();
            }

            // Reseta o tempo de entrada se não houver movimento
            if (Mathf.Abs(verticalInput) < 0.1f)
            {
                nextInputTime = Time.time; // Permite mudança imediata se não houver movimento
            }
        }
    }




    void UpdateButtonVisuals()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.localScale = (i == selectedButtonIndex) ? highlightedScale : normalScale;
        }
    }

    void ButtonClicked(string buttonName)
    {
        switch (buttonName)
        {
            case "ContinueButton":
                ResumeGame();
                break;
            case "SettingsButton":
                OpenSettings();
                break;
            case "ReturnToMenuButton":
                ReturnToMainMenu();
                break;
            default:
                Debug.Log("Botão não reconhecido: " + buttonName);
                break;
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Desabilitar a movimentação da câmera ou outros scripts que não devem funcionar enquanto o jogo está pausado
        // Aqui você pode desabilitar scripts que não devem ser executados
        // Exemplo: FindObjectOfType<CameraController>().enabled = false; 
    }

    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        OptionsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Habilitar novamente a movimentação da câmera ou outros scripts
        // Exemplo: FindObjectOfType<CameraController>().enabled = true; 
    }

    void OpenSettings()
    {
        Debug.Log("Abrir configurações");
        pauseMenuUI.SetActive(false);
        OptionsUI.SetActive(true);
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void OnMouseHover(int index)
    {
        if (!isControlMode)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.localScale = (i == index) ? highlightedScale : normalScale;
            }

            hoveredButtonIndex = index;
        }
    }

    public void OnMouseExit()
    {
        if (!isControlMode)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.localScale = normalScale;
            }

            hoveredButtonIndex = -1;
        }
    }
}
