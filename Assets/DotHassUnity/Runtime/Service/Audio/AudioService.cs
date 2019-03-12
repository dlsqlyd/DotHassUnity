using Hellmade.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotHass.Unity
{

    /// <summary>
    /// 适配器 模式
    /// </summary>
    public class AudioService : IAudioService
    {
        public float globalVolume
        {
            get
            {
                return EazySoundManager.GlobalVolume;
            }
            set
            {
                EazySoundManager.GlobalVolume = value;
            }
        }

        /// <summary>
        /// Global music volume
        /// </summary>
        public float globalMusicVolume
        {
            get
            {
                return EazySoundManager.GlobalMusicVolume;
            }
            set
            {
                EazySoundManager.GlobalMusicVolume = value;
            }
        }

        /// <summary>
        /// Global sounds volume
        /// </summary>
        public float globalSoundsVolume
        {
            get
            {
                return EazySoundManager.GlobalSoundsVolume;
            }
            set
            {
                EazySoundManager.GlobalSoundsVolume = value;
            }
        }

        /// <summary>
        /// Global UI sounds volume
        /// </summary>
        public float globalUISoundsVolume
        {
            get
            {
                return EazySoundManager.GlobalUISoundsVolume;
            }
            set
            {
                EazySoundManager.GlobalUISoundsVolume = value;
            }
        }


        public AudioService()
        {
            EazySoundManager.IgnoreDuplicateMusic = false;
            EazySoundManager.IgnoreDuplicateSounds = false;
            EazySoundManager.IgnoreDuplicateUISounds = false;
        }
        public Audio GetAudio(int audioID)
        {
            return EazySoundManager.GetAudio(audioID);
        }

        public int PlayMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            return EazySoundManager.PlayMusic(clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        public int PlaySound(AudioClip clip, float volume, bool loop, Transform sourceTransform)
        {
            return EazySoundManager.PlaySound(clip, volume, loop, sourceTransform);
        }

        public int PlayUISound(AudioClip clip, float volume)
        {
            return EazySoundManager.PlayUISound(clip, volume);
        }
    }


    public static class SoundServiceExtensions
    {
        public static int PlayMusic(this IAudioService service, AudioClip clip)
        {
            return service.PlayMusic(clip, 1f, false, false, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Play background music
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public static int PlayMusic(this IAudioService service, AudioClip clip, float volume)
        {
            return service.PlayMusic(clip, volume, false, false, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Play background music
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name = "persist" > Whether the audio persists in between scene changes</param>
        /// <returns>The ID of the created Audio object</returns>
        public static int PlayMusic(this IAudioService service, AudioClip clip, float volume, bool loop, bool persist)
        {
            return service.PlayMusic(clip, volume, loop, persist, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Play background music
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name="persist"> Whether the audio persists in between scene changes</param>
        /// <param name="fadeInValue">How many seconds it needs for the audio to fade in/ reach target volume (if higher than current)</param>
        /// <param name="fadeOutValue"> How many seconds it needs for the audio to fade out/ reach target volume (if lower than current)</param>
        /// <returns>The ID of the created Audio object</returns>
        public static int PlayMusic(this IAudioService service, AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds)
        {
            return service.PlayMusic(clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, -1f, null);
        }


        /// <summary>
        /// Play a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public static int PlaySound(this IAudioService service, AudioClip clip)
        {
            return service.PlaySound(clip, 1f, false, null);
        }

        /// <summary>
        /// Play a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public static int PlaySound(this IAudioService service, AudioClip clip, float volume)
        {
            return service.PlaySound(clip, volume, false, null);
        }

        /// <summary>
        /// Play a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="loop">Wether the sound is looped</param>
        /// <returns>The ID of the created Audio object</returns>
        public static int PlaySound(this IAudioService service, AudioClip clip, bool loop)
        {
            return service.PlaySound(clip, 1f, loop, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="clip"></param>
        /// <returns></returns>
        public static int PlayUISound(this IAudioService service, AudioClip clip)
        {
            return service.PlayUISound(clip, 1f);
        }
    }
}
