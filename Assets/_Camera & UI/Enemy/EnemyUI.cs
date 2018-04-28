using UnityEngine;

// Add a UI Socket transform to your enemy
// Attack this script to the socket
// Link to a canvas prefab that contains NPC UI
namespace RPG.Characters
{
    public class EnemyUI : MonoBehaviour 
    {
        // Works around Unity 5.5's lack of nested prefabs
        [Tooltip("The UI canvas prefab")]
        [SerializeField]
        GameObject enemyCanvasPrefab = null;

        Camera cameraToLookAt;
        bool exists = false;

        // Use this for initialization 
        void Start()
        {
            cameraToLookAt = Camera.main;
        }

        // Update is called once per frame 
        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
            transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
        }

        public void DisplayHealthbar(bool display)
        {
            if (display && !exists)
            {
                Instantiate(enemyCanvasPrefab, transform.position, transform.rotation, transform);
                exists = true;
            }
            else if (!display && exists)
            {
                DestroyImmediate(enemyCanvasPrefab);
                exists = false;
            }

        }
    }
}