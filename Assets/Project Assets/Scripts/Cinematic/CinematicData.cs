using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "CinematicData", menuName = "Scriptable Objects/CinematicData")]
public class CinematicData : ScriptableObject
{
    public VideoClip clip;
    public string SceneNext;
}
