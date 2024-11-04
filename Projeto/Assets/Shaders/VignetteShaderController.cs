using UnityEngine;

public class VignetteShaderController : MonoBehaviour
{
    public Material vignetteMaterial;      // Referência ao material com o shader de vinheta
    public float changeSpeed = 1.5f;       // Velocidade de alteração do raio do círculo
    private float minCircleRadius = 0f; // Raio mínimo do círculo
    private float maxCircleRadius = 1f;    // Raio máximo do círculo
    private float targetRadius;             // Raio de destino do círculo
    private float targetBlurWidth = 0.1f;  // Largura do desfoque
    private bool isDead = false;            // Indica se o jogador está morto

    private ProgressBar progressBar;        // Referência ao script ProgressBar

    void Start()
    {
        // Obter a referência ao script ProgressBar
        progressBar = FindObjectOfType<ProgressBar>();

        if (progressBar == null)
        {
            Debug.LogError("ProgressBar script not found in the scene.");
        }

        // Inicializa o raio do círculo para o máximo (estado de vida)
        vignetteMaterial.SetFloat("_CircleRadius", maxCircleRadius);
        vignetteMaterial.SetFloat("_BlurWidth", targetBlurWidth);
    }

    void Update()
    {
        if (progressBar != null)
        {
            // Verifica se o jogador está morto
            isDead = progressBar.isOxygenDepleted;

            // Define o raio de destino dependendo do estado do jogador
            targetRadius = isDead ? minCircleRadius : maxCircleRadius;

            // Obtém o raio atual do círculo
            float currentRadius = vignetteMaterial.GetFloat("_CircleRadius");

            // Interpola o raio atual em direção ao raio de destino
            currentRadius = Mathf.MoveTowards(currentRadius, targetRadius, changeSpeed * Time.deltaTime);

            // Atualiza o raio do círculo no shader
            vignetteMaterial.SetFloat("_CircleRadius", currentRadius);

            // Atualiza a largura do desfoque baseado no raio do círculo
            if (isDead && Mathf.Approximately(currentRadius, minCircleRadius))
            {
                targetBlurWidth = 0f; // Começa a diminuir a largura do desfoque
            }
            else if (!isDead)
            {
                targetBlurWidth = 0.1f; // Retorna para a largura máxima
            }

            // Interpola a largura do desfoque
            float currentBlurWidth = vignetteMaterial.GetFloat("_BlurWidth");
            currentBlurWidth = Mathf.MoveTowards(currentBlurWidth, targetBlurWidth, changeSpeed * Time.deltaTime);

            // Atualiza a largura do desfoque no shader
            vignetteMaterial.SetFloat("_BlurWidth", currentBlurWidth);
        }
    }
}
