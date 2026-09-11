using UniverseSim;

IntPtr world = PhysicsEngine.CreateWorld();
PhysicsEngine.AddBody(world, 0, 0, 1.0f);
PhysicsEngine.Step(world, 0.016f);
Console.WriteLine("Step Complete");
PhysicsEngine.DestroyWorld(world);