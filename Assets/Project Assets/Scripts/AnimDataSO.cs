using UnityEngine;
using Animancer;

[CreateAssetMenu(fileName = "NewAnimData", menuName = "BeatEmUp/Animation Data")]
public class AnimDataSO : ScriptableObject
{
    [Tooltip("El nombre del estado, ej: 'Idle', 'Walk', 'Punch'")]
    public string stateName;

    [Tooltip("El clip y sus configuraciones de transición")]
    public ClipTransition clipTransition;
}