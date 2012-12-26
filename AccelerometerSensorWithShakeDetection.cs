namespace MagicBall
{
    using System;

    using Microsoft.Devices.Sensors;
    using Microsoft.Xna.Framework;

    public class AccelerometerSensorWithShakeDetection : IDisposable
    {
        private const double ShakeThreshold = 0.6;
        private readonly Accelerometer _sensor = new Accelerometer();
        private Vector3 _lastReading;
        private int _shakeCount;
        private bool _shaking;

        public AccelerometerSensorWithShakeDetection()
        {
            var sensor = new Accelerometer();
            if (sensor.State == SensorState.NotSupported)
                throw new NotSupportedException("Accelerometer not supported on this device");
            this._sensor = sensor;
        }

        public SensorState State
        {
            get { return this._sensor.State; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this._sensor != null)
                this._sensor.Dispose();
        }

        #endregion

        private event EventHandler ShakeDetectedHandler;

        public event EventHandler ShakeDetected
        {
            add
            {
                this.ShakeDetectedHandler += value;
                this._sensor.CurrentValueChanged += this.ReadingChanged;
            }
            remove
            {
                this.ShakeDetectedHandler -= value;
                this._sensor.CurrentValueChanged -= this.ReadingChanged;
            }
        }

        public void Start()
        {
            if (this._sensor != null)
                this._sensor.Start();
        }

        public void Stop()
        {
            if (this._sensor != null)
                this._sensor.Stop();
        }

        private void ReadingChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            //Code for checking shake detection
            if (this._sensor.State == SensorState.Ready)
            {
                var reading = e.SensorReading.Acceleration;
                try
                {

                    if (!this._shaking && CheckForShake(this._lastReading, reading, ShakeThreshold) && this._shakeCount >= 1)
                    {
                        //We are shaking
                        this._shaking = true;
                        this._shakeCount = 0;
                        this.OnShakeDetected();
                    }
                    else if (CheckForShake(this._lastReading, reading, ShakeThreshold))
                    {
                        this._shakeCount++;
                    }
                    else if (!CheckForShake(this._lastReading, reading, 0.2))
                    {
                        this._shakeCount = 0;
                        this._shaking = false;
                    }

                    this._lastReading = reading;
                }
                catch
                {
                    /* ignore errors */
                }
            }
        }

        private void OnShakeDetected()
        {
            if (this.ShakeDetectedHandler != null)
                this.ShakeDetectedHandler(this, EventArgs.Empty);
        }

        private static bool CheckForShake(Vector3 last, Vector3 current,
                                          double threshold)
        {
            double deltaX = Math.Abs((last.X - current.X));
            double deltaY = Math.Abs((last.Y - current.Y));
            double deltaZ = Math.Abs((last.Z - current.Z));

            return (deltaX > threshold && deltaY > threshold) ||
                   (deltaX > threshold && deltaZ > threshold) ||
                   (deltaY > threshold && deltaZ > threshold);
        }
    }
}