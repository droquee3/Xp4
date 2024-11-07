using System;
using UnityEngine;

public class VignetteShaderController : MonoBehaviour
{
    public Material vignetteMaterial;      // Referência ao material com o shader de vinheta
    public float changeSpeed = 1.5f;       // Velocidade de alteração do raio do círculo
    public float animationDelay = 2f;      // Tempo até iniciar a animação da vinheta

    private float minCircleRadius = 0f;    // Raio mínimo do círculo
    private float maxCircleRadius = 1f;    // Raio máximo do círculo
    private float targetRadius;            // Raio de destino do círculo
    private float targetBlurWidth = 0.1f;  // Largura do desfoque
    private bool isDead = false;           // Indica se o jogador está morto

    private NovaBarraOxigênio barraOxigenio;  
    private float delayTimer;              // Temporizador para o atraso da animação

    void Start()
    {
        // Referência da NovaBarraOxigênio
        barraOxigenio = FindFirstObjectByType<NovaBarraOxigênio>();

        if (barraOxigenio == null)
        {
            Debug.LogError("NovaBarraOxigênio script not found in the scene.");
        }

        // Inicializa o raio do círculo para o máximo (estado de vida)
        vignetteMaterial.SetFloat("_CircleRadius", maxCircleRadius);
        vignetteMaterial.SetFloat("_BlurWidth", targetBlurWidth);

        // Inicializa o temporizador de atraso
        delayTimer = animationDelay;
    }

    private T FindFirstObjectOfType<T>()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (barraOxigenio != null)
        {
            // Verifica se o oxigênio está esgotado
            isDead = barraOxigenio.isOxygenDepleted;

            if (isDead)
            {
                // Reduz o temporizador de atraso
                delayTimer -= Time.deltaTime;
                if (delayTimer > 0) return; // Se ainda estiver no atraso, não continua

                // Define o raio de destino para a vinheta quando o jogador está morto
                targetRadius = minCircleRadius;
            }
            else
            {
                // Reinicia o temporizador e define o raio de destino para o estado de vida
                delayTimer = animationDelay;
                targetRadius = maxCircleRadius;
            }

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