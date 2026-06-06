using UnityEngine;
 
[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip clip;
}
 
public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;
 
    public AudioClip GetClipFromName(string name)
    {
        foreach (var soundEffect in soundEffects)
        {
            if (soundEffect.groupID == name)
            {
                return soundEffect.clip;
            }
        }
        return null;
    }
}