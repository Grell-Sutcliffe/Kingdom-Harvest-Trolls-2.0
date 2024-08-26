using UnityEngine;


namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject panel;
        public GameObject trollPrefab;
        public GameObject knightPrefab;
        public float interval = 2f;

        public GameObject trollPointDownLeft;
        public GameObject trollPointUpRight;

        public GameObject knightPointDownLeft;
        public GameObject knightPointUpRight;

        private Vector3 trollAreaMin; // ћинимальна€ позици€ спавна (левый нижний угол)
        private Vector3 trollAreaMax; // ћаксимальна€ позици€ спавна (правый верхний угол)
        private Vector3 knightAreaMin; // ћинимальна€ позици€ спавна (левый нижний угол)
        private Vector3 knightAreaMax; // ћаксимальна€ позици€ спавна (правый верхний угол)

        void Start()
        {
            trollAreaMin = trollPointDownLeft.transform.position;
            trollAreaMax = trollPointUpRight.transform.position;
            knightAreaMin = knightPointDownLeft.transform.position;
            knightAreaMax = knightPointUpRight.transform.position;

            InvokeRepeating("TrollSpawn", interval, interval);
        }

        private void TrollSpawn()
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(trollAreaMin.x, trollAreaMax.x),
                Random.Range(trollAreaMin.y, trollAreaMax.y),
                0
            );

            GameObject new_spawn = Instantiate(trollPrefab, panel.transform);
            new_spawn.transform.position = spawnPosition;
        }

        public void KnightSpawn()
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(knightAreaMin.x, knightAreaMax.x),
                Random.Range(knightAreaMin.y, knightAreaMax.y),
                0
            );

            GameObject new_spawn = Instantiate(knightPrefab, panel.transform);
            new_spawn.transform.position = spawnPosition;
        }
    }
}

