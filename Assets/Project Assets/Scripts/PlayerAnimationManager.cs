using UnityEngine;
using Animancer;
using System.Collections.Generic;

[RequireComponent(typeof(AnimancerComponent))]
public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private AnimancerComponent animancer;
    [SerializeField] private AnimDataSO[] animationStates;

    private Dictionary<string, ClipTransition> animDictionary;
    private string currentState = string.Empty;

    private void Awake()
    {
        if (animancer == null) animancer = GetComponent<AnimancerComponent>();

        // Inicializamos el diccionario para búsquedas rápidas (O(1))
        animDictionary = new Dictionary<string, ClipTransition>();

        foreach (var animData in animationStates)
        {
            if (!animDictionary.ContainsKey(animData.stateName))
            {
                animDictionary.Add(animData.stateName, animData.clipTransition);
            }
        }
    }

    public void ChangeState(string newStateName)
    {
        // Evitamos reiniciar la animación si ya estamos en ese estado
        if (currentState == newStateName) return;

        if (animDictionary.TryGetValue(newStateName, out ClipTransition transition))
        {
            animancer.Play(transition);
            currentState = newStateName;
        }
        else
        {
            Debug.LogWarning($"[AnimManager] No se encontró el estado de animación: {newStateName}");
        }
    }
}