using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stairs
{
    public class BallMovement : MonoBehaviour
    {
        public float speedTranslate = 10f;
        public float speedRotate = 75f;
        public int currentStep = 0;
        public Vector3[] posPoint = new Vector3[15];

        //private Vector3 dirA;
        //private Vector3 dirB;
        //private Vector3 dirC;
        //private Vector3 dirD;
        private float length = 0.77292f;
        private int timerStart = 0;
        private int timerEnd = 0;
        private bool isResetTimer = false;


        private void Awake()
        {
            //dirA = new Vector3(Mathf.Cos(3.14159f / 3.0f), 0, -Mathf.Sin(3.14159f / 3.0f));
            //dirB = new Vector3(Mathf.Sin(3.14159f / 3.0f), 0, Mathf.Cos(3.14159f / 3.0f));
            //dirC = new Vector3(-Mathf.Cos(3.14159f / 3.0f), 0, Mathf.Sin(3.14159f / 3.0f));
            //dirD = new Vector3(-Mathf.Sin(3.14159f / 3.0f), 0, -Mathf.Cos(3.14159f / 3.0f));
            //dirA.Normalize();
            //dirB.Normalize();
            //dirC.Normalize();
            //dirD.Normalize();
            timerStart = Time.frameCount;
            speedTranslate *= 0.001f;
            posPoint[14] = posPoint[0];
        }

        private void Update()
        {
            timerEnd = Time.frameCount;
            float step = length / 2.0f / speedTranslate;
            if (!isResetTimer)
            {
                if (timerEnd >= (timerStart + (int)step))
                {
                    currentStep++;
                    isResetTimer = !isResetTimer;
                }
            }
            if (isResetTimer)
            {
                if (timerEnd >= (timerStart + (int)step * 2))
                {
                    currentStep++;
                    if (currentStep >= 28)
                    {
                        currentStep = 0;
                    }
                    timerStart = timerEnd;
                    isResetTimer = !isResetTimer;
                }
            }

            transform.position = Vector3.Lerp(posPoint[currentStep / 2], posPoint[currentStep / 2 + 1], ((float)(timerEnd - timerStart) / ((int)step * 2)));
            int offsetId;
            if (currentStep == 0 || currentStep == 27)
            {
                offsetId = 0;
            }
            else
            {
                offsetId = currentStep % 2 + currentStep / 2;
                offsetId = offsetId + (14 - offsetId * 2);
            }
            Vector3 vec = new Vector3(0, 0, 0.06f * Mathf.Sin(0.15f * (float)Time.frameCount + (float)offsetId * 0.41887f));
            transform.position += vec;

            if (currentStep >= 0 && currentStep < 14)
            {
                //transform.position += dirB * speedTranslate;
                transform.Rotate(Vector3.up * speedRotate * Time.deltaTime, Space.World);
            }
            else if(currentStep >= 14 && currentStep < 28)
            {
                //transform.position += dirC * speedTranslate;
                transform.Rotate(Vector3.down * speedRotate * Time.deltaTime, Space.World);
            }
        }
    }
}
