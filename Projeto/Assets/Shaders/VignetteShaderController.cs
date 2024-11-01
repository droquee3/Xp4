using UnityEngine;

public class VignetteShaderController : MonoBehaviour
{
    public Material vignetteMaterial;     // Referência ao material com o shader de vinheta
    public float fadeSpeed = 5f;          // Velocidade da transição
    private float targetTransparency = 1f; // Transparência desejada (1 = totalmente transparente)
    private bool isDead = false;

    private ProgressBar progressBar; // Referência ao script ProgressBar

    void Start()
    {
        // Obter a referência ao script ProgressBar
        progressBar = FindObjectOfType<ProgressBar>();

        if (progressBar == null)
        {
            Debug.LogError("ProgressBar script not found in the scene.");
        }
    }

    void Update()
    {
        // Verifica o estado de oxigênio do jogador no ProgressBar
        if (progressBar != null)
        {
            isDead = progressBar.isOxygenDepleted; // Acesse a variável diretamente

            // Define a transparência
            targetTransparency = isDead ? 0f : 1f; 

            // Ajuste rápido da transparência
            float currentTransparency = vignetteMaterial.GetFloat("_Transparency");
            currentTransparency = Mathf.MoveTowards(currentTransparency, targetTransparency, fadeSpeed * Time.deltaTime);

            // Atualiza a transparência no shader
            vignetteMaterial.SetFloat("_Transparency", currentTransparency);
        }
    }
}
