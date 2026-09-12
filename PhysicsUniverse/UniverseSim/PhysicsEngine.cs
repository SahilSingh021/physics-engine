using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace UniverseSim
{
    internal class PhysicsEngine
    {
        [DllImport("PhysicsEngine.dll")]
        public static extern IntPtr CreateWorld();

        [DllImport("PhysicsEngine.dll")]
        public static extern void DestroyWorld(IntPtr world);

        [DllImport("PhysicsEngine.dll")]
        public static extern void AddBody(IntPtr world, float x, float y, float mass);

        [DllImport("PhysicsEngine.dll")]
        public static extern void Step(IntPtr world, float deltaTime);

        [DllImport("PhysicsEngine.dll")]
        public static extern int GetBodyCount(IntPtr world);

        [DllImport("PhysicsEngine.dll")]
        public static extern void GetBodyPosition(IntPtr world, int index, out float x, out float y);
    }
}
