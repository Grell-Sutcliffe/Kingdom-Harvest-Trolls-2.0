using UnityEngine;


namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject panel;
        public GameObject trollPrefab;
        public GameObject knightPrefab;
        public GameObject villagerPrefab;
        public GameObject kingPrefab;
        public float interval = 2f;

        public GameObject trollPointDownLeft;
        public GameObject trollPointUpRight;

        public GameObject knightPointDownLeft;
        public GameObject knightPointUpRight;

        private Vector3 trollAreaMin; // ћинимальна€ позици€ спавна (левый нижний угол)
        private Vector3 trollAreaMax; // ћаксимальна€ позици€ спавна (правый верхний угол)
        private Vector3 knightAreaMin; // ћинимальна€ позици€ спавна (левый нижний угол)
        private Vector3 knightAreaMax; // ћаксимальна€ позици€ спавна (правый верхний угол)

        public bool need_to_spawn_troll = false;

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
            if (need_to_spawn_troll)
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(trollAreaMin.x, trollAreaMax.x),
                    Random.Range(trollAreaMin.y, trollAreaMax.y),
                    0
                );

                GameObject new_spawn = Instantiate(trollPrefab, panel.transform);
                new_spawn.transform.position = spawnPosition;
            }
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

        /*public void VillagerSpawn(float x, float y, int index_i, int index_j)
        {
            Vector3 spawnPosition = new Vector3(
                x,
                y,
                0f
            );

            GameObject new_spawn = Instantiate(villagerPrefab, panel.transform);
            new_spawn.transform.position = spawnPosition;

            new_spawn.GetComponent<VillagerController>().index_i = index_i;
            new_spawn.GetComponent<VillagerController>().index_j = index_j;
        }*/

        public void VillagerSpawn(Vector3 spawnPosition, int index_i, int index_j)
        {
            GameObject new_spawn = Instantiate(villagerPrefab, panel.transform);
            new_spawn.transform.position = spawnPosition;

            new_spawn.GetComponent<VillagerController>().index_i = index_i;
            new_spawn.GetComponent<VillagerController>().index_j = index_j;
        }

        public void KingSpawn(Vector3 spawnPosition, int index_i, int index_j)
        {
            GameObject new_spawn = Instantiate(kingPrefab, panel.transform);
            new_spawn.transform.position = spawnPosition;

            new_spawn.GetComponent<KingController>().index_i = index_i;
            new_spawn.GetComponent<KingController>().index_j = index_j;
        }
    }
}

