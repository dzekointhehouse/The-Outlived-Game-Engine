using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Penumbra;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    public interface IEntityCreator
    {
        EntityBuilder AddPosition(Vector2 position, int layerDepth = 400);
        EntityBuilder AddSprite(Point position, string spriteName, float scale = 1, float alpha = 1);
        List<IComponent> Build();
        EntityBuilder SetArtificialIntelligence(float followingDistance = 250, bool isWandering = true);
        EntityBuilder SetCollision(Rectangle boundingRectangle, bool isCage = false);
        EntityBuilder SetCreatedComponents(List<IComponent> alreadyCreatedComponents);
        EntityBuilder SetDimensions(int width, int height);
        EntityBuilder SetHealth(int maxhealth = 100, int currentHealth = 100, bool alive = true);
        EntityBuilder SetLight(Light light);
        EntityBuilder SetPlayer(string name);
        EntityBuilder SetTeam(int maxhealth = 100, int currentHealth = 100, bool alive = true);
    }
}