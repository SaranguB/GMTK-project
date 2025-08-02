using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName ="Level", menuName = "ScriptableObjects/Level")]
    public class LevelSO : ScriptableObject
    {
        [SerializeField] private List<LevelData> levelData;
        
        public List<LevelData> LevelData => levelData;
    }
    
    [Serializable]
    public class LevelData
    {
        public int levelID;
        public Levels level;
        public List<Transform> checkPoints;
    }


}