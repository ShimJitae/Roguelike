using System;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] SFXType sfxType;
    [SerializeField] bool playOnAwake;

    void OnEnable()
    {
        if (playOnAwake)
        {
            PlaySFX();
        }
    }

    public void PlaySFX(float tmp = 0f)
    {
        SoundManager.Instance.PlaySFX(sfxType);
    }
}
