using SharpDX.Windows;
using System.Diagnostics;
using UniverseSim;

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

SimWindow simWindow = new SimWindow("Universe Sim", 1280, 720);

IntPtr world = PhysicsEngine.CreateWorld();
PhysicsEngine.AddBody(world, 0, 0, 1.0f);

simWindow.Show();

var stopwatch = Stopwatch.StartNew();
float lastTime = 0f;
RenderLoop.Run(simWindow, () =>
{
    float currentTime = (float)stopwatch.Elapsed.TotalSeconds;
    float deltaTime = currentTime - lastTime;
    lastTime = currentTime;

    PhysicsEngine.Step(world, deltaTime);
    simWindow.RenderFrame();
});

PhysicsEngine.DestroyWorld(world);
