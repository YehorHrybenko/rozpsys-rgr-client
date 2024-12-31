using UnityEngine;

public class Drone : MonoBehaviour
{
    public Vector3 TargetVelocity = Vector3.zero;

    [SerializeField] private Rigidbody rb;
    public Vector3 Velocity => rb.linearVelocity;

    private PIDController throttlePID;
    private PIDController tiltXPID;
    private PIDController tiltZPID;

    private float throttleF = 1f;
    private float rotationSpeed = 1f;

    private void Awake()
    {
        throttlePID = new PIDController(15f, 0f, 0f);
        tiltXPID = new PIDController(20f, 0f, 0f);
        tiltZPID = new PIDController(20f, 0f, 0f);
    }

    private void FixedUpdate()
    {
        if (TargetVelocity != Vector3.zero)
        {
            Quaternion targetRotationFlat = TargetVelocity.magnitude == 0 ? transform.rotation : Quaternion.LookRotation(Vector3.ProjectOnPlane(TargetVelocity.normalized, Vector3.up));

            var velocityForwardError = TargetVelocity.magnitude - Vector3.Dot(rb.linearVelocity, TargetVelocity.normalized);
            var velocityLateralError = Vector3.Dot(rb.linearVelocity, targetRotationFlat * Vector3.right);

            var tiltX = Mathf.Clamp(-tiltXPID.Compute(0f, velocityForwardError, Time.fixedDeltaTime), -30, 30);
            var tiltZ = Mathf.Clamp(-tiltZPID.Compute(0f, velocityLateralError, Time.fixedDeltaTime), -30, 30);


            var targetRotation = targetRotationFlat * Quaternion.Euler(tiltX, 0f, tiltZ);
            RotateDrone(targetRotation);
        }

        Throttle(TargetVelocity.y);
    }

    private void RotateDrone(Quaternion targetRotation)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }

    private float GetMotorForce(float speed)
    {
        return speed == 0 ? 1 : Mathf.Clamp01(1 / (speed / 5));
    }

    private void Throttle(float targetSpeed)
    {
        float currentSpeed = rb.linearVelocity.y;
        float speedError = targetSpeed - currentSpeed;

        float throttleForce = Mathf.Max(0, -throttlePID.Compute(0f, speedError, Time.fixedDeltaTime)) * throttleF; 
        rb.AddForce(transform.up * throttleForce, ForceMode.Acceleration);
    }

    private class PIDController
    {
        private float kp;
        private float ki;
        private float kd;

        private float integral;
        private float previousError;

        public PIDController(float kp, float ki, float kd)
        {
            this.kp = kp;
            this.ki = ki;
            this.kd = kd;
            integral = 0f;
            previousError = 0f;
        }

        public float Compute(float setpoint, float measuredValue, float deltaTime)
        {
            float error = setpoint - measuredValue;
            integral += error * deltaTime;
            float derivative = (error - previousError) / deltaTime;

            previousError = error;

            return kp * error + ki * integral + kd * derivative;
        }
    }
}