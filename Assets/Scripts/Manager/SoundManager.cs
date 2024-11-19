using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField][Range(0f, 1f)] float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] float musicVolume;

    AudioSource musicAudioSource;
    public AudioClip musicClip;

    void Awake()
    {
        instance = this;

        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;
    }

    private void Start() 
    {
        ChangeBackgroundMusic(musicClip);
    }

    void ChangeBackgroundMusic(AudioClip clip)
    {
        // 실행 전 미리 한번 꺼주면 안전한 사용이 가능함
        instance.musicAudioSource.Stop();
        instance.musicAudioSource.clip = clip;
        instance.musicAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = GameManager.Instance.ObjectPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        
        // 한 가지 사운드를 여러가지 방식으로 사용
        soundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance);
    }
}
