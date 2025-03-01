using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Extensions
{
    public static class AnimationExtensions
    {
        public static async UniTask PlayClipAsync(this Animation animation, AnimationClip clip)
        {
            animation.clip = clip;
            animation.Play();
            var lengthMilliseconds = (int)(animation.clip.length * 1000);
            await UniTask.Delay(lengthMilliseconds);
        }
    }
}