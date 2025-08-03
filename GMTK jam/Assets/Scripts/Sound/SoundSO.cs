using System;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO")]
    public class SoundSO : ScriptableObject
    {
        public Sounds[] audioList;
    }
    [Serializable]
    public struct Sounds
    {
        public SoundType soundType;
        public AudioClip audioClip;
    }
}