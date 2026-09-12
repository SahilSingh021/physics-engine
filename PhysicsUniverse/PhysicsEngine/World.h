#pragma once
#include <vector>

#include "Vec2.h"
#include "Body.h"

struct World
{
private:
	std::vector<Body> bodies;

public:
	void AddBody(Body b);
	void Step(float deltaTime);
	int GetBodyCount();
	Body& GetBody(int index);
};