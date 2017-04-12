using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class LightSystem : ISystem
    {
        public static string SystemName = "LightSystem";
        private GameDependencies _gameDependencies;
        private PenumbraComponent _penumbra;

        public void Initialize(GameDependencies gameDependencies)
        {
            this._gameDependencies = gameDependencies;
            _penumbra = new PenumbraComponent(gameDependencies.Game);

            var lights = ComponentManager.Instance.GetEntitiesWithComponent<LightComponent>();

            foreach (var instance in lights)
            {
                _penumbra.Lights.Add(instance.Value.Light);
            }

            _penumbra.Initialize();
        }

        public void DrawLights()
        {
            _penumbra.BeginDraw();
        }

        public void EndDraw()
        {
            _penumbra.Draw(_gameDependencies.GameTime);
        }
    }
}
