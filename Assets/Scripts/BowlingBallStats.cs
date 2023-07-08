using UnityEngine;

[CreateAssetMenu(fileName = "BowlingBallStats", menuName = "Bowling/BowlingBallStats")]
public class BowlingBallStats : ScriptableObject
{
        public float speed = 1f;
        public int damage = 1;
        public int health = 1;
}