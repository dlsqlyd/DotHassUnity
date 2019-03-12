using UnityEngine;

namespace HFramework
{
    public interface IAudioService
    {
        int PlayMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform);

        int PlaySound(AudioClip clip, float volume, bool loop, Transform sourceTransform);

        int PlayUISound(AudioClip clip, float volume);

    }
}