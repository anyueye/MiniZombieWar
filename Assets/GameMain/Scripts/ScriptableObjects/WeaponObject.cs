using UnityEngine;


    [CreateAssetMenu(menuName = "Weapon", order = 0)]
    public class WeaponObject : ScriptableObject
    {
        public Sprite icon;
        public float attackRange;
        public float interval;
        public float impactPower;
        public float dynamicSector;
        public float staticSector;
        public Color rangeColor;
        public Color hitRateColor;
    }
