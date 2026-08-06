using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] SFXType sfxType;

    public void PlaySFX()
    {
        SoundManager.Instance.PlaySFX(sfxType);
    }
}
