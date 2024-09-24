using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject firstSelected;
    public GameObject[] buttons;

    private int currentIndex = 0;
    private float moveThreshold = 0.2f; // Limite para movimento do joystick
    private bool isNavigating = false; // Para verificar se estamos navegando

    void Start()
    {
        if (firstSelected != null)
        {
            eventSystem.SetSelectedGameObject(firstSelected);
            currentIndex = System.Array.IndexOf(buttons, firstSelected);
            UpdateButtonVisuals();
        }
    }

    void Update()
    {
        // Captura a entrada do joystick
        float vertical = Input.GetAxis("Vertical");

        // Verifica se a entrada está acima do limite
        if (vertical > moveThreshold && !isNavigating)
        {
            currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
            UpdateButtonVisuals();
            isNavigating = true; // Indica que estamos navegando
        }
        else if (vertical < -moveThreshold && !isNavigating)
        {
            currentIndex = (currentIndex + 1) % buttons.Length;
            UpdateButtonVisuals();
            isNavigating = true; // Indica que estamos navegando
        }
        else if (Mathf.Abs(vertical) < moveThreshold)
        {
            isNavigating = false; // Reseta a navegação ao soltar o joystick
        }

        if (Input.GetButtonDown("Submit"))
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, new PointerEventData(eventSystem), ExecuteEvents.submitHandler);
            }
        }
    }

    private void UpdateButtonVisuals()
    {
        // Reverte a escala de todos os botões
        foreach (GameObject button in buttons)
        {
            button.transform.localScale = Vector3.one; // Escala original
        }

        // Aumenta a escala do botão atualmente selecionado
        buttons[currentIndex].transform.localScale = Vector3.one * 1.1f; // Aumenta a escala
        eventSystem.SetSelectedGameObject(buttons[currentIndex]);
    }

    // Métodos para o feedback visual com o mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Aumenta a escala do botão quando o mouse entra
        Transform buttonTransform = eventData.pointerEnter.transform;
        buttonTransform.localScale = Vector3.one * 1.1f; // Aumenta a escala
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaura a escala do botão quando o mouse sai
        Transform buttonTransform = eventData.pointerEnter.transform;
        buttonTransform.localScale = Vector3.one; // Restaura a escala original
    }

    public void OnButtonClick()
    {
        // Este método é chamado quando o botão é clicado
        if (eventSystem.currentSelectedGameObject != null)
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, new PointerEventData(eventSystem), ExecuteEvents.submitHandler);
        }
    }
}
