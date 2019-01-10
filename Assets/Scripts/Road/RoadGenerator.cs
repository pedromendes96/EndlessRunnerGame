using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Road
{
    public class RoadGenerator : MonoBehaviour
    {
		public BlockGenerator blockGenerator;
        public GameObject[] prefabs;
        public GameObject firstRoad;
        private float totalZ;

        private List<GameObject> roadsList;
        private Transform playerTransform;
        private Random random;

        private float[] xVals = new float[]{
        0f,2.5f,-2.5f,5f,-5f
    };

        private float[] yVals = new float[]{
        0f,2f,-2f
    };

        private float[] zVals = new float[]{
        0f,2f,4f
    };
        private List<float[]> HeightAndSpaceValues;

        // Use this for initialization
        void Start()
        {
            HeightAndSpaceValues = new List<float[]>();
            foreach (var x in xVals)
            {
                foreach (var y in yVals)
                {
                    foreach (var z in zVals)
                    {
                        HeightAndSpaceValues.Add(new float[]{
                        x,y,z
                    });
                    }
                }
            }
            random = new Random();
            playerTransform = GameObject.FindGameObjectWithTag("Runner").transform;
            roadsList = new List<GameObject>();
            roadsList.Add(firstRoad);
            totalZ = getZSize(firstRoad);
        }

        private float getZSize(GameObject gameObject)
        {
            float total = 0f;
            foreach (Transform child in gameObject.transform)
            {
                try
                {
                    total += child.GetComponent<Renderer>().bounds.size.z;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            return total;
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log("TotalZ: " + totalZ);
            // Debug.Log("playerTransform.position.z: " + playerTransform.position.z);
            if (roadsList.Count < 6 || Math.Abs(totalZ - playerTransform.position.z) < 180)
            {
            // if(roadsList.Count == 1){
                int mIndex = random.Next(roadsList.Count);
                var road = SpawnRoad(mIndex);
				blockGenerator.AddRandomBlocks(road, 15, 4);
            }
        }

        private GameObject SpawnRoad(int prefabIndex)
        {
            GameObject go;

            try
            {
                go = Instantiate(prefabs[prefabIndex]) as GameObject;
            }
            catch (Exception e)
            {
                return null;
            }

            go.transform.SetParent(transform);
            foreach (Transform child in go.transform)
            {
                child.tag = "Ground";
            }
            float size = getZSize(go);
            var x = GetRandomHeightAndSpace();
            var lastRoad = roadsList[roadsList.Count - 1];
            go.transform.Translate(lastRoad.transform.position.x + x[0], lastRoad.transform.position.y + x[1], (totalZ - 3 * roadsList.Count) + x[2]);
            totalZ += size + x[2];
            roadsList.Add(go);
			return go;
        }

        private float[] GetRandomHeightAndSpace()
        {
            return HeightAndSpaceValues[random.Next(HeightAndSpaceValues.Count)];
        }
    }
}