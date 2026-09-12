#include "Windows.h"
#include "World.h"

extern "C" __declspec(dllexport) World* CreateWorld();
extern "C" __declspec(dllexport) void DestroyWorld(World* world);
extern "C" __declspec(dllexport) void AddBody(World* world, float x, float y, float mass);
extern "C" __declspec(dllexport) void Step(World* world, float deltaTime);
extern "C" __declspec(dllexport) int GetBodyCount(World* world);
extern "C" __declspec(dllexport) void GetBodyPosition(World* world, int index, float* outX, float* outY);

World* CreateWorld()
{
    return new World();
}

void DestroyWorld(World* world)
{
    delete world;
}

void AddBody(World* world, float x, float y, float mass)
{
    Body body;
    body.position = { x, y };
    body.mass = mass;

    world->AddBody(body);
}

void Step(World* world, float deltaTime)
{
    world->Step(deltaTime);
}

int GetBodyCount(World* world)
{
    return world->GetBodyCount();
}

void GetBodyPosition(World* world, int index, float* outX, float* outY)
{
    Body& body = world->GetBody(index);
    *outX = body.position.x;
    *outY = body.position.y;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

