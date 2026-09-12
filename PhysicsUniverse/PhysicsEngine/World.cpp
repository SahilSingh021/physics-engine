#include "World.h"

void World::AddBody(Body b)
{
	bodies.push_back(b);
}

void World::Step(float deltaTime)
{
	for (Body& b : bodies)
	{
		b.velocity += b.acceleration * deltaTime;
		b.position += b.velocity * deltaTime;
	}
}

int World::GetBodyCount()
{
	return (int)bodies.size();
}

Body& World::GetBody(int index)
{
	return bodies[index];
}