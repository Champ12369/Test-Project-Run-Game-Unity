using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    //public GameObject[] Obstacles;
    public void GeneratePrefab()
    {
        List<int> inactiveIndices = new List<int>();
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;
        int minZIndex = -1;

        for (int i = 0; i < prefabs.Length; i++)
        {
            GameObject prefab = prefabs[i];
            if (prefab.activeSelf)
            {
                Debug.Log("Prefab " + i + " is active.");

                float zValue = prefab.transform.position.z;

                // Update the minimum and maximum z values
                if (zValue < minZ)
                {
                    minZ = zValue;
                    minZIndex = i;
                }
                maxZ = Mathf.Max(maxZ, zValue);
            }
            else
            {
                Debug.Log("Prefab " + i + " is not active (false).");
                inactiveIndices.Add(i); // Add the index to the list of inactive indices
            }
        }

        if (minZIndex != -1)
        {
            Debug.Log("Minimum Z Value: " + minZ + " (Prefab Index: " + minZIndex + ")");
            prefabs[minZIndex].SetActive(false); // Set the prefab with minimum z-value to inactive
        }
        else
        {
            Debug.Log("No active prefabs found.");
        }

        // Choose a random index from the list of inactive indices
        if (inactiveIndices.Count > 0)
        {
            int randomInactiveIndex = inactiveIndices[Random.Range(0, inactiveIndices.Count)];
            Debug.Log("Random Inactive Index: " + randomInactiveIndex);
            prefabs[randomInactiveIndex].SetActive(true);
            prefabs[randomInactiveIndex].transform.position = new Vector3(0, 0, maxZ + 61.3f);

            //for (int i = 0; i < 2; i++) 
            //{
            //    int[] values = new int[] { -3, 0, 3 };
            //    int randomValue = values[Random.Range(0, 3)];
            //    maxZ = maxZ + 40;
            //    int randomValue1 = Mathf.RoundToInt(Random.Range(0, Obstacles.Length - 1));
            //    Obstacles[randomValue1].transform.position = new Vector3(randomValue, 0, maxZ);
               

            //}
        }
        else
        {
            Debug.Log("No inactive prefabs found.");
        }
    }
}
