using System;
using System.Collections.Generic;
using UnityEngine;

namespace Road{
    public class BlockGenerator : MonoBehaviour{
        public GameObject[] blockGameObjects;
        public float[] heightGameObjects;
        public System.Random random;
        public BlockGenerator(){
            random = new System.Random();
        }

        private float[] seperator = new float[] {-2.5f, 0f, 2.5f};

        public void AddRandomBlocks(GameObject road, float padding, int rows){
            var rowSize = padding/2;
            int max;
            for(int i = 0; i<rows; i++){
                max = random.Next(1,3);
                float zVal;
                if(i < rows/2){
                    zVal = rowSize * (i+1) * -1;
                }else{
                    zVal = rowSize * (i - (rows/2) +1);
                }
                GenerateRandomBlocks(road, max, zVal);
            }
            max = random.Next(1,3);
            GenerateRandomBlocks(road, max, 0);
        }

        private void GenerateRandomBlocks(GameObject road, int blocks, float zVal){
            var floatList = new List<float>();
            for(int i = 0; i < blocks; i++){
                GameObject gameObject;
                int index = random.Next(blockGameObjects.Length);
                float movement = seperator[random.Next(seperator.Length)];
                while(floatList.Contains(movement)){
                    movement = seperator[random.Next(seperator.Length)];
                }
                floatList.Add(movement);
                try
                {
                    gameObject = Instantiate(blockGameObjects[index]) as GameObject;
                }
                catch (Exception e)
                {
                    return;
                }

                try
                {
                    gameObject.tag = "Block";
                    gameObject.transform.SetParent(road.transform);
                    var obstaclePosition = new Vector3(road.transform.position.x + movement, road.transform.position.y + heightGameObjects[index], road.transform.position.z + zVal);
                    gameObject.transform.position = obstaclePosition;
                }
                catch (Exception e)
                {
                    gameObject.SetActive(false);
                }
                
            }
        }
    }
}