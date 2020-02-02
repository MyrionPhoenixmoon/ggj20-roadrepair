using System.Threading;

namespace Assets.Scripts.Driving
{
    using System;
    using System.Collections.Generic;

    using Assets.Scripts.Enums;

    using EnumsNET;

    using UnityEngine;

    using Random = System.Random;

    public class Car : MonoBehaviour
    {
        private const float AngularVelocityCap = 2f;

        private const float BrakingFactor = -4.5f;

        private const float InertiaDampenerFactor = -.05f;

        private List<string> axis;

        private Dictionary<string, float> currentForces;

        [SerializeField]
        public int MaxSpeed { get; set; }

        private Timer carEffectTimer;

        public Rigidbody RigidBody { get; private set; }

        public int CurrentSpeed { get; private set; }

        public CarState State { get; private set; }

        private void ApplyDirections(float acceleration, bool isBraking, float turningDirection)
        {
            var direction = this.DetermineCurrentCarDirection();
            turningDirection *= direction;

            var velocity = this.RigidBody.velocity.magnitude;

            this.LimitTurningSpeed(ref turningDirection, velocity);
            this.ApplyBrakingForce(isBraking);
            this.ApplyMovementToCar(acceleration, turningDirection);
        }

        private void ApplyMovementToCar(float acceleration, float turningDirection)
        {
            if (this.State == CarState.Slowed)
            {
                this.RigidBody.velocity *= this.SlowingFactor;
            }
            else if (this.State == CarState.Wobbly)
            {
                var random = new Random();
                acceleration *= random.Next(-1, 1);
                turningDirection *= random.Next(-1, 1);
            }

            if (Math.Abs(acceleration) > .3f)
            {
                this.RigidBody.AddRelativeForce(0, 0, acceleration * this.Acceleration);
                this.RigidBody.AddRelativeTorque(0, turningDirection * this.TurningSpeed, 0);
            }
            else
            {
                if (Math.Abs(turningDirection) > .1f)
                {
                    var inertiaDampener = new Vector3(
                        this.RigidBody.velocity.x * InertiaDampenerFactor,
                        this.RigidBody.velocity.y * InertiaDampenerFactor,
                        this.RigidBody.velocity.z * InertiaDampenerFactor);
                    this.RigidBody.AddForce(inertiaDampener);

                    this.RigidBody.AddRelativeTorque(0, turningDirection * 100, 0);
                    this.RigidBody.AddRelativeForce(
                        new Vector3(turningDirection * this.RigidBody.velocity.magnitude, 0, 0));
                }
            }
        }

        public float SlowingFactor { get; set; }

        [SerializeField]
        public float TurningSpeed { get; set; }

        [SerializeField]
        public float Acceleration { get; set; }

        private void ApplyBrakingForce(bool isBraking)
        {
            this.RigidBody.velocity = isBraking ? this.RigidBody.velocity * 0.9f : this.RigidBody.velocity;
        }

        private void LimitTurningSpeed(ref float turningDirection, float velocity)
        {
            this.RigidBody.maxAngularVelocity = AngularVelocityCap - (0.03f * velocity);

            if (velocity < 2f)
            {
                this.RigidBody.angularVelocity = new Vector3(0, 0, 0);
                turningDirection = 0;
            }
        }

        private int DetermineCurrentCarDirection()
        {
            var direction = this.transform.InverseTransformVector(this.RigidBody.velocity).z > 0 ? 1 : -1;
            return direction;
        }

        private void CapAtMaxSpeed()
        {
            if (this.RigidBody.velocity.magnitude > this.MaxSpeed)
            {
                var brakingForce = new Vector3(
                    this.RigidBody.velocity.x * BrakingFactor,
                    this.RigidBody.velocity.y * BrakingFactor,
                    this.RigidBody.velocity.z * BrakingFactor);
                this.RigidBody.AddForce(brakingForce);
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (this.State == CarState.Dead)
            {
                this.SetCurrentSpeed();
                return;
            }
            this.GetAxisValues();

            var acceleration = this.GetAcceleration();
            var turningDirection = this.GetTurningDirection();
            var isBraking = this.CheckForBraking();

            this.ApplyDirections(acceleration, isBraking, turningDirection);
            this.CapAtMaxSpeed();
            this.SetCurrentSpeed();
        }

        private void SetCurrentSpeed()
        {
            this.CurrentSpeed = (int)(this.RigidBody.velocity.magnitude * 10);
        }

        private float GetAcceleration()
        {
            var acceleration = 0f;
            if (this.currentForces.ContainsKey(Axis.Vertical.ToString()))
            {
                acceleration = this.currentForces[Axis.Vertical.ToString()];
            }

            return acceleration;
        }

        private void GetAxisValues()
        {
            this.currentForces.Clear();
            foreach (var entry in this.axis)
            {
                float value;
                if (Math.Abs(value = Input.GetAxis(entry)) > 0.05f)
                {
                    this.currentForces.Add(entry, value);
                }
            }
        }

        private bool CheckForBraking()
        {
            var isbraking = this.currentForces.ContainsKey(Axis.Jump.ToString());
            return isbraking;
        }

        private float GetTurningDirection()
        {
            var turningDirection = 0f;
            if (this.currentForces.ContainsKey(Axis.Horizontal.ToString()))
            {
                turningDirection = this.currentForces[Axis.Horizontal.ToString()];
            }

            return turningDirection;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var obstacle = collision.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                var obstacleType = obstacle.Type;
                this.carEffectTimer.Change(this.CarEffectDuration * 1000, 0);

                if (this.State == CarState.Normal)
                {
                    switch (obstacleType)
                    {
                        case ObstacleType.Powerup:
                            this.State = CarState.Immune;
                            break;
                        case ObstacleType.Crash:
                            this.State = CarState.Dead;
                            break;
                        case ObstacleType.Drift:
                            this.State = CarState.Wobbly;
                            break;
                        case ObstacleType.Slowdown:
                            this.State = CarState.Slowed;
                            break;
                        default:
                            this.State = CarState.Normal;
                            break;
                    }
                }
                else if (this.State != CarState.Dead)
                {
                    switch (obstacleType)
                    {
                        case ObstacleType.Powerup:
                            this.State = CarState.Immune;
                            break;
                        case ObstacleType.Crash:
                            if (this.State != CarState.Immune)
                            {
                                this.State = CarState.Dead;
                            }

                            break;
                        case ObstacleType.Slowdown:
                            this.State = CarState.Slowed;
                            break;
                    }
                }
            }

            //TODO: Car goes booooom
        }

        public int CarEffectDuration { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            this.RigidBody = this.gameObject.GetComponent<Rigidbody>();

            this.RigidBody.maxAngularVelocity = AngularVelocityCap;
            this.MaxSpeed = 30;
            this.Acceleration = 100;
            this.TurningSpeed = 100;
            this.SlowingFactor = .95f;
            this.CarEffectDuration = 10;

            this.State = CarState.Normal;
            this.carEffectTimer = new Timer(this.ResetCarState);

            this.currentForces = new Dictionary<string, float>();
            this.axis = new List<string>();
            foreach (var entry in Enum.GetValues(typeof(Axis)))
            {
                this.axis.Add(((Axis)entry).AsString(EnumFormat.Description));
            }
        }

        private void ResetCarState(object state)
        {
            if (this.State != CarState.Dead)
            {
                this.State = CarState.Normal;
            }
        }
    }
}