namespace Assets.Scripts.Driving
{
    using System;
    using System.Collections.Generic;

    using Assets.Scripts.Enums;

    using EnumsNET;

    using UnityEngine;

    public class Car : MonoBehaviour
    {
        private const float AngularVelocityCap = 2f;

        private const float BreakingFactor = -4.5f;

        private const float InertiaDampenerFactor = -.05f;

        private List<string> axis;

        private Dictionary<string, float> currentForces;

        private int maxSpeed;

        public Rigidbody RigidBody { get; private set; }

        public int Speed { get; private set; }

        private void ApplyDirections(float acceleration, bool isBreaking, float turningDirection)
        {
            var velocity = this.RigidBody.velocity.magnitude;

            this.RigidBody.maxAngularVelocity = AngularVelocityCap - (0.03f * velocity);

            if (velocity < 2f)
            {
                this.RigidBody.angularVelocity = new Vector3(0, 0, 0);
                turningDirection = 0;
            }

            if (Math.Abs(acceleration) > .3f)
            {
                this.RigidBody.velocity = isBreaking ? this.RigidBody.velocity * 0.9f : this.RigidBody.velocity;

                this.RigidBody.AddRelativeForce(0, 0, acceleration * 100);
                this.RigidBody.AddRelativeTorque(0, turningDirection * 100, 0);
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
            if (this.RigidBody.velocity.magnitude > this.maxSpeed)
            {
                var breakingForce = new Vector3(
                    this.RigidBody.velocity.x * BreakingFactor,
                    this.RigidBody.velocity.y * BreakingFactor,
                    this.RigidBody.velocity.z * BreakingFactor);
                this.RigidBody.AddForce(breakingForce);
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            this.GatherAxisValues();

            var acceleration = this.GatherAcceleration();
            var turningDirection = this.GatherTurningDirection();
            var isBreaking = this.GatherBreaking();

            this.ApplyDirections(acceleration, isBreaking, turningDirection);
            this.CapAtMaxSpeed();
            this.SetSpeed();
        }

        private float GatherAcceleration()
        {
            var acceleration = 0f;
            if (this.currentForces.ContainsKey(Axis.Vertical.ToString()))
            {
                acceleration = this.currentForces[Axis.Vertical.ToString()];
            }

            return acceleration;
        }

        private void GatherAxisValues()
        {
            this.currentForces.Clear();
            foreach (var entry in this.axis)
            {
                var value = 0f;
                if (Math.Abs(value = Input.GetAxis(entry)) > 0.05f)
                {
                    this.currentForces.Add(entry, value);
                }
            }
        }

        private bool GatherBreaking()
        {
            var isBreaking = this.currentForces.ContainsKey(Axis.Jump.ToString());
            return isBreaking;
        }

        private float GatherTurningDirection()
        {
            var turningDirection = 0f;
            if (this.currentForces.ContainsKey(Axis.Horizontal.ToString()))
            {
                turningDirection = this.currentForces[Axis.Horizontal.ToString()];
            }

            return turningDirection;
        }

        private void SetSpeed()
        {
            this.Speed = (int)(this.RigidBody.velocity.magnitude * 10);
        }

        // Start is called before the first frame update
        private void Start()
        {
            this.RigidBody = this.gameObject.GetComponent<Rigidbody>();

            this.RigidBody.maxAngularVelocity = AngularVelocityCap;
            this.maxSpeed = 30;

            this.currentForces = new Dictionary<string, float>();
            this.axis = new List<string>();
            foreach (var entry in Enum.GetValues(typeof(Axis)))
            {
                this.axis.Add(((Axis)entry).AsString(EnumFormat.Description));
            }
        }
    }
}