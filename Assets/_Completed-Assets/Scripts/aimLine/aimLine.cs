using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Complete
{
    public class aimLine : MonoBehaviour
    {

        private LineRenderer m_aimLine;
        public Material lineMat;
        public float startWidth = 0.21f;
        public float endWidth = 0.21f;
        public Color lineColor = Color.green;
        public Transform m_FireTransform;
        //public Rigidbody m_Shell;                   // Prefab of the shell.
        public int m_lineVertex = 100;
        // A child of the tank where the shells are spawned.
        //public Transform pointB;     // 终点
        public static bool m_Enabled = true;
        public static bool m_Aiming = false;
        public static Vector3 m_Velocity = new Vector3(0, 0, 0);
        private float g = -9.8f;        // 重力加速度

        public float m_MinFireAngle = 310f;        // The force given to the shell if the fire button is not held.
        public float m_MaxFireAngle = 350f;        // The force given to the shell if the fire button is held for the max charge time.

        //public float m_LeftFireAngle = -30f;        // The force given to the shell if the fire button is not held.
        //public float m_RightFireAngle = 30f;        // The force given to the shell if the fire button is held for the max charge time.

        public float UDAngleSpeed = 0f;
        //public float LRAngleSpeed = 0f;





        // Start is called before the first frame update
        void Start()
        {
            //transform.position = m_FireTransform.position;
            //transform.rotation = m_FireTransform.rotation;

            initLineRenderer();
        }

        // Update is called once per frame
        void Update()
        {
            m_aimLine.enabled = m_Enabled;
            if (m_Enabled == false)
                return;
            generateLineRenderer();
        }

        private void FixedUpdate()
        {
            UpdateFireToward();
        }



        void initLineRenderer()
        {
            if (GetComponent<LineRenderer>())
            {
                m_aimLine = GetComponent<LineRenderer>();
            }
            else
            {
                m_aimLine = gameObject.AddComponent<LineRenderer>();
            }
            m_aimLine.enabled = m_Enabled;
            m_aimLine.alignment = LineAlignment.TransformZ;
            m_aimLine.positionCount = m_lineVertex;
            m_aimLine.startWidth = startWidth;
            m_aimLine.endWidth = endWidth;
            m_aimLine.material = lineMat;
            m_aimLine.startColor = lineColor;
            m_aimLine.endColor = lineColor;

        }

        private void generateLineRenderer()
        {
            m_aimLine.positionCount = m_lineVertex;
            Vector3[] linePoints = new Vector3[m_lineVertex + 1];


            //// Create an instance of the shell and store a reference to it's rigidbody.
            //Rigidbody ghostShell =
            //    Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            //// Set the shell's velocity to the launch force in the fire position's forward direction.
            //ghostShell.velocity = m_Velocity;
            //ghostShell.GetComponent<Renderer>().enabled = false;

            for (int i = 0; i < m_lineVertex; i++)
            {
                float t = i * 0.01f;
                Vector3 tempPoint = m_FireTransform.position + m_Velocity * t;
                //Vector3 tempPoint = m_FireTransform.transform.TransformPoint(m_FireTransform.position + m_Velocity * t);

                tempPoint.y += 0.5f*g * t * t;
                //tempPoint += Vector3.forward *i;

                //tempPoint.y += g * i;
                //tempPoint = transform.InverseTransformPoint(tempPoint);
                //tempPoint = m_FireTransform.transform.InverseTransformPoint(tempPoint);
                linePoints[i] = tempPoint;

                //float x = m_FireTransform.position.x + m_Velocity.x * t;
                //float y = m_FireTransform.position.y + m_Velocity.y * t;
  
            //float z = m_FireTransform.position.z + m_Velocity.z * t;

                //linePoints[i] = new Vector3(x, y, z);

            }

            m_aimLine.SetPositions(linePoints);
        }

        public void SetFireAngleInput(Vector2 input)
        {
            UDAngleSpeed = input.y * 100;
            //LRAngleSpeed = input.x * 100;
            if (Mathf.Abs(UDAngleSpeed) >1)
            {
                m_Enabled = true;
                m_Velocity = TankShooting.m_MaxLaunchForce / 15 * m_FireTransform.forward;
            }
            //else if (Mathf.Abs(LRAngleSpeed) > 1)
            //{
            //    m_Enabled = true;
            //    m_Velocity = TankShooting.m_MaxLaunchForce / 15 * m_FireTransform.forward;
            //}
            else
            {
                m_Enabled = false;
            }

            if (m_Aiming)
            {
                m_Enabled = true;
            }
        }


        public virtual void UpdateFireToward()
        {
            //float torqueInput = EngineOn ? MotorInput : 0;

            // Add torque / rotate wheels
            // Steering

            if (Mathf.Abs(UDAngleSpeed) > 50)
            {
                //m_FireTransform.Rotate(Vector3.right, AngleSpeed * Time.deltaTime);

                if (m_FireTransform.rotation.eulerAngles.x < m_MaxFireAngle &&
                m_FireTransform.rotation.eulerAngles.x > m_MinFireAngle)
                {
                    m_FireTransform.Rotate(Vector3.right, UDAngleSpeed * Time.deltaTime);
                }
                if (m_FireTransform.rotation.eulerAngles.x < m_MinFireAngle)
                {
                    m_FireTransform.Rotate(Vector3.right, -1 * UDAngleSpeed * Time.deltaTime);
                }
                if (m_FireTransform.rotation.eulerAngles.x > m_MaxFireAngle)
                {
                    m_FireTransform.Rotate(Vector3.right, -1 * UDAngleSpeed * Time.deltaTime);
                }
            }

            //if (Mathf.Abs(LRAngleSpeed) < 0.01)
            //{
            //    continue;
            //}
            //if (Mathf.Abs(LRAngleSpeed) > 50)
            //{

            //    if (m_FireTransform.rotation.eulerAngles.y < m_RightFireAngle &&
            //    m_FireTransform.rotation.eulerAngles.y > m_LeftFireAngle)
            //    {
            //        m_FireTransform.Rotate(Vector3.up, LRAngleSpeed * Time.deltaTime);
            //    }
            //    if (m_FireTransform.rotation.eulerAngles.y < m_LeftFireAngle)
            //    {
            //        m_FireTransform.Rotate(Vector3.up, -1 * LRAngleSpeed * Time.deltaTime);
            //    }
            //    if (m_FireTransform.rotation.eulerAngles.y > m_RightFireAngle)
            //    {
            //        m_FireTransform.Rotate(Vector3.up, -1 * LRAngleSpeed * Time.deltaTime);
            //    }
            //}


        }
        //public virtual void SetVelocityInput(Vector3 input)
        //{
        //    velocity = input;
        //}

        //public virtual void SetEnabledInput(bool input)
        //{
        //    m_aimLine.enabled = input;
        //}
    }

}

