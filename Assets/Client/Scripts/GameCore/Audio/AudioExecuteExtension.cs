using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public static class AudioExecuteExtension
    {
        public static void PlayOneShoot(this IAudioPlayable target, AudioData data)
        {
            if (ReferenceEquals(target.AudioData, null))
            {
                Debug.LogError("Null Reference of Audio Source");
                return;
            }
            
            if (ReferenceEquals(target.AudioData.First(a => a.Type == data.Type), data))
                target.Source.PlayOneShot(data.Clip);
        }

        public static AudioData GetData(this IEnumerable<AudioData> data, AudioType audioType)
        {
            return data.FirstOrDefault(d => d.Type == audioType);
        }
    }
}
