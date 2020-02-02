namespace Assets.Scripts.Driving
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Assets.Scripts.Enums;

    using EnumsNET;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    using Random = System.Random;

    public class Car : MonoBehaviour
    {
        private const float AngularVelocityCap = 2f;

        private const float BrakingFactor = -4.5f;

        private const float InertiaDampenerFactor = -.05f;

        private List<string> axis;

        [SerializeField]
        private int carEffectDuration;

        private Timer carEffectTimer;
        private Timer sceneTransitionTimer;

        public int sceneTransitionDelay;
        private bool sceneTransitionTrigger = false;

        private Dictionary<string, float> currentForces;

        [SerializeField]
        private int maxSpeed;

        [SerializeField]
        private float slowingFactor;

        [SerializeField]
        private float turningSpeed;

        [SerializeField]
        private float acceleration;

        public float Acceleration
        {
            get => this.acceleration;
            private set => this.acceleration = value;
        }

        public int CarEffectDuration
        {
            get => this.carEffectDuration;
            private set => this.carEffectDuration = value;
        }

        public int CurrentSpeed { get; private set; }

        public int MaxSpeed
        {
            get => this.maxSpeed;
            private set => this.maxSpeed = value;
        }

        public Rigidbody RigidBody { get; private set; }

        public float SlowingFactor
        {
            get => this.slowingFactor;
            private set => this.slowingFactor = value;
        }

        public CarState State { get; private set; }

        public float TurningSpeed
        {
            get => this.turningSpeed;
            private set => this.turningSpeed = value;
        }

        private void ApplyBrakingForce(bool isBraking)
        {
            this.RigidBody.velocity = isBraking ? this.RigidBody.velocity * 0.9f : this.RigidBody.velocity;
        }

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

        private bool CheckForBraking()
        {
            var isbraking = this.currentForces.ContainsKey(Axis.Jump.ToString());
            return isbraking;
        }

        private int DetermineCurrentCarDirection()
        {
            var direction = this.transform.InverseTransformVector(this.RigidBody.velocity).z > 0 ? 1 : -1;
            return direction;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (this.sceneTransitionTrigger){
                this.WinOrLose();
            }

            if (this.State == CarState.Dead)
            {
                this.SetCurrentSpeed();
                this.sceneTransitionTimer = new Timer(this.SetTransitionTrigger, null, this.sceneTransitionDelay * 1000, 0);
                return;
            }

            this.GetAxisValues();

            float currentAcceleration;
            float currentTurningDirection;
            var isBraking = false;


            if (this.State != CarState.Wobbly)
            {
                currentAcceleration = this.GetAcceleration();
                currentTurningDirection = this.GetTurningDirection();
                isBraking = this.CheckForBraking();
            }
            else
            {
                var random = new Random();
                currentAcceleration = (float)random.Next(-100, 100) / 100f;
                currentTurningDirection = (float)random.Next(-100, 100) / 100f;
            }

            this.ApplyDirections(currentAcceleration, isBraking, currentTurningDirection);
            this.CapAtMaxSpeed();
            this.SetCurrentSpeed();
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

        private float GetTurningDirection()
        {
            var turningDirection = 0f;
            if (this.currentForces.ContainsKey(Axis.Horizontal.ToString()))
            {
                turningDirection = this.currentForces[Axis.Horizontal.ToString()];
            }

            return turningDirection;
        }

        private void LimitTurningSpeed(ref float turningDirection, float velocity)
        {
            this.RigidBody.maxAngularVelocity = AngularVelocityCap - (0.03f * velocity);

            if (velocity < 3f)
            {
                this.RigidBody.angularVelocity = new Vector3(0, 0, 0);
                turningDirection = 0;
            }
        }

        public void Triggered(Obstacle obstacle)
        {
            if (this.State == CarState.Dead)
            {
                return;
            }

            if (obstacle != null)
            {
                var obstacleType = obstacle.Type;
                this.ResetTimerDuration();

                if (this.State != CarState.Immune)
                {
                    switch (obstacleType)
                    {
                        case ObstacleType.Powerup:
                            this.State = CarState.Immune;
                            break;
                        case ObstacleType.Drift:
                            this.State = CarState.Wobbly;
                            break;
                        case ObstacleType.Slowdown:
                            this.State = CarState.Slowed;
                            break;
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            var obstacle = collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                var obstacleType = obstacle.Type;
                this.ResetTimerDuration();

                if (this.State != CarState.Immune)
                {
                    switch (obstacleType)
                    {
                        case ObstacleType.Powerup:
                            this.State = CarState.Immune;
                            break;
                        case ObstacleType.Drift:
                            this.State = CarState.Wobbly;
                            break;
                        case ObstacleType.Slowdown:
                            this.State = CarState.Slowed;
                            break;
                    }
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var obstacle = collision.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                var obstacleType = obstacle.Type;
                this.ResetTimerDuration();

                if (this.State != CarState.Immune && obstacleType == ObstacleType.Crash)
                {
                    this.State = CarState.Dead;
                }
            }
        }

        private void ResetTimerDuration()
        {
            this.carEffectTimer.Change(this.CarEffectDuration * 1000, Timeout.Infinite);
        }

        private void ResetCarState(object state)
        {
            if (this.State != CarState.Dead)
            {
                this.State = CarState.Normal;
            }
        }

        private void SetCurrentSpeed()
        {
            this.CurrentSpeed = (int)(this.RigidBody.velocity.magnitude * 10);
        }

        // Start is called before the first frame update
        private void Start()
        {
            this.RigidBody = this.gameObject.GetComponent<Rigidbody>();

            this.RigidBody.maxAngularVelocity = AngularVelocityCap;

            this.State = CarState.Normal;
            this.carEffectTimer = new Timer(this.ResetCarState);

            this.currentForces = new Dictionary<string, float>();
            this.axis = new List<string>();
            foreach (var entry in Enum.GetValues(typeof(Axis)))
            {
                this.axis.Add(((Axis)entry).AsString(EnumFormat.Description));
            }
        }

        private void SetTransitionTrigger(object state){
            this.sceneTransitionTrigger = true;
        }

        private void WinOrLose(){
        
            if (this.State != CarState.Dead){
                SceneManager.LoadScene(2);
            }
            if (this.State == CarState.Dead){
                SceneManager.LoadScene(3);
            }
            else {
                return;
            }

        }
    }
}