using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Penumbra;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    public interface IEntityCreator
    {
        EntityBuilder SetPosition(Vector2 position, int layerDepth = 400);
        EntityBuilder SetSprite(string spriteName, float scale = 1, float alpha = 1);
        List<IComponent> Build();
        EntityBuilder SetArtificialIntelligence(float followingDistance = 250, bool isWandering = true);
        EntityBuilder SetCollision(Rectangle boundingRectangle = default(Rectangle), bool isCage = false);
        EntityBuilder SetCreatedComponents(List<IComponent> alreadyCreatedComponents);
        EntityBuilder SetDimensions(int width, int height);
        EntityBuilder SetHealth(int maxhealth = 100, int currentHealth = 100, bool alive = true);
        EntityBuilder SetLight(Light light);
        EntityBuilder SetPlayer(string name);
        EntityBuilder SetTeam(int maxhealth = 100, int currentHealth = 100, bool alive = true);
        EntityBuilder SetSound(string soundname, float volume = 1f);
        EntityBuilder SetRendering(int width, int height);
        EntityBuilder SetMovement(float maxVelocity, float acceleration, float rotationSpeed, float direction);




    }

}