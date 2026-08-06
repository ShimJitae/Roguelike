using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip bgm_Title, bgm_Normal, bgm_Boss;
    [SerializeField] private AudioClip sfx_OnHit, sfx_Guen, sfx_One, sfx_Ma, sfx_Portal;

    private Dictionary<BGMType, AudioClip> bgmDic;
    private Dictionary<SFXType, AudioClip> sfxDic;

    [Header("Audio Sources")]
    private AudioSource audioSource;

    private void Awake()
    {
        // 싱글톤 중복 생성 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        bgmDic = new Dictionary<BGMType, AudioClip>()
        {
            { BGMType.Title, bgm_Title},
            { BGMType.Normal, bgm_Normal},
            { BGMType.Boss, bgm_Boss}
        };

        sfxDic = new Dictionary<SFXType, AudioClip>()
        {
            { SFXType.OnHit, sfx_OnHit},
            { SFXType.Guen, sfx_Guen},
            { SFXType.One, sfx_One},
            { SFXType.Ma, sfx_Ma},
            { SFXType.Portal, sfx_Portal},
        };
    }

    /// <summary>
    /// BGM을 재생합니다.
    /// 동일한 BGM이 이미 재생 중이면 다시 시작하지 않습니다.
    /// </summary>
    public void PlayBGM(BGMType bgmType)
    {
        if (audioSource == null)
        {
            Debug.LogError("[SoundManager] BGM AudioSource가 연결되지 않았습니다.");
            return;
        }

        if (audioSource.clip == bgmDic[bgmType] && audioSource.isPlaying)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = bgmDic[bgmType];
        audioSource.Play();
    }

    /// <summary>
    /// 현재 재생 중인 BGM을 중지합니다.
    /// </summary>
    public void StopBGM()
    {
        if (audioSource == null)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = null;
    }

    /// <summary>
    /// 효과음을 한 번 재생합니다.
    /// </summary>
    public void PlaySFX(SFXType sfxType)
    {
        if (audioSource == null)
        {
            Debug.LogError("[SoundManager] SFX AudioSource가 연결되지 않았습니다.");
            return;
        }

        audioSource.PlayOneShot(sfxDic[sfxType]);
    }
}
