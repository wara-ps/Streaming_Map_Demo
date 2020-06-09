using GizmoSDK.GizmoBase;
using Saab.Foundation.Map;
using Saab.Foundation.Unity.MapStreamer;
using Saab.Unity.Extensions;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Time = UnityEngine.Time;

namespace Assets.Combitech.UserControl
{
    public enum CameraMode { Unknown = 0, FreeFlight = 1 }

    public class WorldCameraController : MonoBehaviour, ISceneManagerCamera
    {
        public bool Enabled { get; protected set; }
        public CameraMode Mode { get; set; } = CameraMode.FreeFlight;

        public double X = 0;
        public double Y = 0;
        public double Z = 0;

        protected float InitialSpeed = 5f;
        protected float Acceleration = 50f;
        protected float MaxSpeed = 500f;

        protected float CursorSensitivity = 0.005f;
        protected float RotationSpeed = 2f;

        protected float CurrentSpeed = 0f;
        protected bool Moving = false;

        protected Vec3D HomePosition { get; set; }
        protected Quaternion HomeRotation { get; set; }

        #region ISceneManagerCamera

        public Camera Camera => GetComponent<Camera>();

        public Vec3D GlobalPosition
        {
            get { return new Vec3D(X, Y, Z); }

            set
            {
                X = value.x;
                Y = value.y;
                Z = value.z;
            }
        }

        public Vector3 Up => MapControl.SystemMap.GetLocalOrientation(GlobalPosition).GetCol(2).ToVector3();

        public void PreTraverse()
        {
            // Called before traverser runs
        }

        public void PostTraverse()
        {
            // Called after all nodes have updated their transforms
        }

        public void MapChanged()
        {
            // Called when global map has changed
        }

        #endregion

        protected void Start()
        {
            HomePosition = new Vec3D(X, Y, Z);
            HomeRotation = transform.rotation;
        }

        protected void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                Enabled = !Enabled;
            }

            if (Enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Mode == CameraMode.FreeFlight)
            {
                UpdateFreeFlight();
            }
        }

        protected void UpdateFreeFlight()
        {
            if (!Enabled)
                return;

            if (Input.GetKeyDown(KeyCode.H))
            {
                Moving = false;
                CurrentSpeed = 0;
                X = HomePosition.x;
                Y = HomePosition.y;
                Z = HomePosition.z;
                transform.rotation = HomeRotation;
                return;
            }

            // update movement
            bool lastMoving = Moving;
            Vector3 deltaPosition = Vector3.zero;

            if (Moving)
            {
                CurrentSpeed = Mathf.Min(MaxSpeed, CurrentSpeed + Acceleration * Time.deltaTime);
            }

            Moving = false;

            CheckTranslation(KeyCode.W, ref deltaPosition, Vector3.forward);
            CheckTranslation(KeyCode.S, ref deltaPosition, -Vector3.forward);
            CheckTranslation(KeyCode.D, ref deltaPosition, Vector3.right);
            CheckTranslation(KeyCode.A, ref deltaPosition, -Vector3.right);
            CheckTranslation(KeyCode.E, ref deltaPosition, Vector3.up);
            CheckTranslation(KeyCode.Q, ref deltaPosition, -Vector3.up);

            if (Moving)
            {
                if (Moving != lastMoving)
                    CurrentSpeed = InitialSpeed;

                var speed = Input.GetKey(KeyCode.LeftShift) ? MaxSpeed : CurrentSpeed;
                var rotatedDeltaPosition = transform.localRotation * deltaPosition.normalized * speed * Time.deltaTime;

                X += rotatedDeltaPosition.x;
                Y += rotatedDeltaPosition.y;
                Z -= rotatedDeltaPosition.z;
            }
            else
            {
                CurrentSpeed = 0f;
            }

            // update rotation
            Vector3 angles = transform.eulerAngles;
            angles.x += -Input.GetAxis("Mouse Y") * 359f * CursorSensitivity;
            angles.y += Input.GetAxis("Mouse X") * 359f * CursorSensitivity;

            CheckRotation(KeyCode.LeftArrow, ref angles, -Vector3.up);
            CheckRotation(KeyCode.RightArrow, ref angles, Vector3.up);
            CheckRotation(KeyCode.UpArrow, ref angles, -Vector3.right);
            CheckRotation(KeyCode.DownArrow, ref angles, Vector3.right);

            if (angles.x > 89 && angles.x < 180)
                angles.x = 89;
            else if (angles.x < 271 && angles.x > 180)
                angles.x = 271;

            transform.eulerAngles = angles;
        }

        protected void CheckTranslation(KeyCode keyCode, ref Vector3 deltaPosition, Vector3 directionVector)
        {
            if (Input.GetKey(keyCode))
            {
                Moving = true;
                deltaPosition += directionVector;
            }
        }

        protected void CheckRotation(KeyCode keyCode, ref Vector3 eulerAngles, Vector3 axis)
        {
            if (Input.GetKey(keyCode))
            {
                eulerAngles += RotationSpeed * axis;
            }
        }
    }
}
