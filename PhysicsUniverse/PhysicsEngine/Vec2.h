#pragma once

struct Vec2
{
	float x, y;

	Vec2()
	{
		x = 0;
		y = 0;
	}

	Vec2(float x, float y)
	{
		this->x = x;
		this->y = y;
	}

	Vec2 operator+(const Vec2& o) const { return { x + o.x, y + o.y }; }
	Vec2 operator-(const Vec2& o) const { return { x - o.x, y - o.y }; }
	Vec2 operator*(float s) const { return { x * s, y * s }; }
	Vec2& operator+=(const Vec2& o) { x += o.x; y += o.y; return *this; };

	float Dot(Vec2& v)
	{
		return x * v.x + y * v.y;
	}

	float LengthSq()
	{
		return x * x + y * y;
	}
};