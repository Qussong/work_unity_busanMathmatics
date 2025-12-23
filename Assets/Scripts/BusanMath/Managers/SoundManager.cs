using BusanMath.Core;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [Header("=== Sound Resource ===")]
    [SerializeField] private AudioClip correctEffectSound;
    [SerializeField] private AudioClip disCorrectEffectSound;
    [SerializeField] private AudioClip buttonEffectSound;

    public void PlayCorrectSound()
    {
        if (null == correctEffectSound) return;
        PlaySound(correctEffectSound);
    }

    public void PlayDisCorrectSound()
    {
        if (null == disCorrectEffectSound) return;
        PlaySound(disCorrectEffectSound);
    }

    public void PlayButtonSound()
    {
        if (null == buttonEffectSound) return;
        PlaySound(buttonEffectSound);
    }

    /// <summary>
    /// UI 효과음 한 번 재생
    /// </summary>
    /// <param name="clip">재생할 AudioClip</param>
    /// <param name="volume">볼륨 (0.0 ~ 1.0)</param>
    private void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip이 null입니다.");
            return;
        }

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

}
