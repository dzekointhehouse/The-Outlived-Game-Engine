using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using ZEngine.Components;
using ZEngine.Managers;
using static Spelkonstruktionsprojekt.ZEngine.GameTest.TestChaseGame;

namespace Spelkonstruktionsprojekt.Zenu
{

    public interface ButtonLink
    {
        void Click();
    }

    public class Menu
    {
        public List<Button> buttons = new List<Button>();

        public static int Create()
        {
            var entityId = EntityManager.GetEntityManager().NewEntity();
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.W, "") //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FireWeapon)
                .SetAction(Keys.LeftShift, EventConstants.Running)
                .Build();


            var renderComponent = new RenderComponentBuilder()
                .Position(500, 500, 20)
                .Dimensions(300, 60)
                .Fixed(true)
                .Build();
            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "dot"
            };
            var light = new LightComponent()
            {
                Light = new PointLight()
                {
                    Position = new Vector2(150, 150),
                    Scale = new Vector2(500f),
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }
            };

            var sound = new SoundComponent()
            {
                SoundEffectName = "zombiewalking",
                Volume = 1f

            };
            ComponentManager.Instance.AddComponentToEntity(sound, entityId);

            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(actionBindings, entityId);
            ComponentManager.Instance.AddComponentToEntity(light, entityId);

            return entityId;
        }

        public void AddButton(string text, ButtonLink link)
        {
            buttons.Add(new Button());
        }

        public void AddButton()

        public void Show()
        {

        }

        public void Hide()
        {

        }

    }
    public class Button
    {
        public Button(string text, ButtonLink link)
        {
            var entityId = EntityManager.GetEntityManager().NewEntity();
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.W, "") //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FireWeapon)
                .SetAction(Keys.LeftShift, EventConstants.Running)
                .Build();


            var renderComponent = new RenderComponentBuilder()
                .Position(500, 500, 20)
                .Dimensions(300, 60)
                .Fixed(true)
                .Build();
            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "dot"
            };
            var light = new LightComponent()
            {
                Light = new PointLight()
                {
                    Position = new Vector2(150, 150),
                    Scale = new Vector2(500f),
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }
            };

            var sound = new SoundComponent()
            {
                SoundEffectName = "zombiewalking",
                Volume = 1f

            };
            ComponentManager.Instance.AddComponentToEntity(sound, entityId);

            var moveComponent = new MoveComponent()
            {
                MaxVelocitySpeed = 205,
                AccelerationSpeed = 5,
                RotationSpeed = 4,
                Direction = new Random(DateTime.Now.Millisecond).Next(0, 40) / 10
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(actionBindings, entityId);
            ComponentManager.Instance.AddComponentToEntity(light, entityId);
        }
    }
}