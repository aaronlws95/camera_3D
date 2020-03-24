using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace camera_3D
{
    class Calc
    {
        public static void QuaternionToEuler(Quaternion q, out float roll, out float pitch, out float yaw)
        {
            // roll (x-axis rotation)
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            roll = MathHelper.ToDegrees((float)Math.Atan2(sinr_cosp, cosr_cosp));

            // pitch (y-axis rotation)
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                pitch = Math.Sign(sinp) * 90f;
            else
            pitch = MathHelper.ToDegrees((float)Math.Asin(sinp));

            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            yaw = MathHelper.ToDegrees((float)Math.Atan2(siny_cosp, cosy_cosp));
        }
    }
}
