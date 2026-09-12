using System.Runtime.InteropServices;
using Vector3 = SharpDX.Vector3;
using Vector4 = SharpDX.Vector4;

namespace UniverseSim.structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vector3 Position;
        public Vector4 Color;

        public Vertex(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }

        public const int SizeInBytes = (3 + 4) * 4;
    }
}