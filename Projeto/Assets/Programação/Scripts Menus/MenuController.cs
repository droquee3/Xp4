using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject menu;
    public GameObject creditos;
    public int hoveredButtonIndex = -1;  
    public int selectedButtonIndex = 0;  
    public bool isControlMode = false;   
    public float inputDelay = 0.2f;

    public Vector3 normalScale = new Vector3(1, 1, 1);
    public Vector3 highlightedScale = new Vector3(1.2f, 1.2f, 1.2f);  // Aumenta 20%

    private float nextInputTime = 0f;

    private bool onCredits = false;
    public AudioSource selection;
    public AudioSource press;

    void Start()
    {
        press.Stop();
        selection.Stop();
        menu.SetActive(true);
        creditos.SetActive(false);
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

        if (Input.GetButtonDown("Cancel"))
        {
            if (onCredits == true)
            {
                OpenMenu();
            }
        }
    }

    void HandleControllerInput()
    {
        if (Time.time < nextInputTime) return;

        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0)  
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttons.Length) % buttons.Length;
            UpdateButtonVisuals();
            nextInputTime = Time.time + inputDelay;
        }
        else if (verticalInput < 0)  
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttons.Length;
            UpdateButtonVisuals();
            nextInputTime = Time.time + inputDelay;
        }

        if (Input.GetButtonDown("Submit"))
        {
            buttons[selectedButtonIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    void UpdateButtonVisuals()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (selection != null)
            {
                Debug.Log("Som tocado");
                selection.Play();
            }
            buttons[i].transform.localScale = (i == selectedButtonIndex) ? highlightedScale : normalScale;
        }
    }

    void ButtonClicked(string buttonName)
    {
        switch (buttonName)
        {
            case "StartButton":
                if (press != null)
                {
                    Debug.Log("Som tocado");
                    press.Play();
                }
                StartGame();
                break;
            case "CreditsButton":
                if (press != null)
                {
                    Debug.Log("Som tocado");
                    press.Play();
                }
                OpenOptions();
                break;
            case "QuitButton":
                if (press != null)
                {
                    Debug.Log("Som tocado");
                    press.Play();
                }
                QuitGame();
                break;
            case "BackButton":
                if (press != null)
                {
                    Debug.Log("Som tocado");
                    press.Play();
                }
                OpenMenu();
                break;
            default:
                Debug.Log("Botão não reconhecido: " + buttonName);
                break;
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    void OpenOptions()
    {
        Debug.Log("Abrindo opções...");
        menu.SetActive(false);
        creditos.SetActive(true);
        onCredits = true;
    }

    void OpenMenu()
    {
        menu.SetActive(true);
        creditos.SetActive(false);
        onCredits = false;
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Jogo fechado!");
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
