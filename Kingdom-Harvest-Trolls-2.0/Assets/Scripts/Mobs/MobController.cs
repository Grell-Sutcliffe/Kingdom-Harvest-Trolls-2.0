using UnityEngine;
using UnityEngine.UI;


namespace Game
{
    public class MobController : MonoBehaviour
    {
        public RectTransform target;
        public float speed = 100f;
        public float attackDistance = 10f;
        public float attackRate = 1f;

        private RectTransform rectTransform;
        private float lastAttackTime;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            lastAttackTime = -attackRate;
        }

        void Update()
        {
            if (target != null)
            {
                MoveTowardsTarget();
            }
        }

        public void SetTarget(RectTransform newTarget)
        {
            target = newTarget;
        }

        private void MoveTowardsTarget()
        {
            Vector2 direction = (target.anchoredPosition - rectTransform.anchoredPosition).normalized;
            float distance = Vector2.Distance(rectTransform.anchoredPosition, target.anchoredPosition);

            if (distance > attackDistance)
            {
                rectTransform.anchoredPosition += direction * speed * Time.deltaTime;
            }
            else
            {
                if (Time.time - lastAttackTime >= attackRate)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }

        private void Attack()
        {
            Debug.Log("Attacking target!");
            // Здесь можно добавить логику атаки
        }
    }
}

