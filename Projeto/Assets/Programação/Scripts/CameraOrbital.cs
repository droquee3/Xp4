using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbital : MonoBehaviour
{
    public Transform target;      
    public float distance = 5.0f; 
    public float xSpeed = 150.0f; 
    public float ySpeed = 150.0f; 

    public float yMinLimit = 1f;  
    public float yMaxLimit = 80f; 
    public float minHeight = 1.0f;

    private float x = 0.0f; 
    private float y = 0.0f; 

    private bool isPaused = false; // Variável para controlar o estado de pausa

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        // Verifica se o jogo está pausado
        if (isPaused || target == null)
        {
            return; // Se estiver pausado, não permite a movimentação da câmera
        }

        // Calcula os inputs do mouse e do joystick
        float mouseX = Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        float mouseY = Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        float rightStickX = Input.GetAxis("Right Stick X") * xSpeed * 0.1f;
        float rightStickY = Input.GetAxis("Right Stick Y") * ySpeed * 0.1f;

        // Atualiza os ângulos de rotação da câmera
        x += mouseX + rightStickX;
        y -= mouseY + rightStickY;

        // Limita a rotação vertical
        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        // Calcula a nova rotação e posição da câmera
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

        // Aplica a rotação e a posição à câmera
        transform.rotation = rotation;
        transform.position = position;
    }

    // Função para definir o estado de pausa
    public void SetPauseState(bool paused)
    {
        isPaused = paused;
    }
}
