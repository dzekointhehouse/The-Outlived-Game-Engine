using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Game.Components;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Game.Systems
{
    public class CarSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public CarSystem Start()
        {
            EventBus.Subscribe<InputEvent>("MountCar", Mount);
            EventBus.Subscribe<InputEvent>("UnmountCar", Unmount);
            return this;
        }

        public CarSystem Stop()
        {
            return this;
        }

        public void Mount(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var cars = ComponentManager.GetEntitiesWithComponent(typeof(CarComponent));
                if (cars.Count == 0) return;
                var car = cars.First();
                var carComponent = car.Value as CarComponent;
                if (carComponent.Driver != null) return;
                carComponent.Driver = inputEvent.EntityId;
                
                var driverRenderComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(inputEvent.EntityId);
                if (driverRenderComponent != null)
                {
                    driverRenderComponent.IsVisible = false;
                }

                ComponentManager.AddComponentToEntity(new DriverComponent {Car = car.Key}, inputEvent.EntityId);
            }
        }

        private void TurnOnCarLights()
        {
            
        }

        private void Unmount(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var driverComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<DriverComponent>(inputEvent.EntityId);
                if (driverComponent == null) return;
                var driverRenderComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(inputEvent.EntityId);
                if (driverRenderComponent != null)
                {
                    driverRenderComponent.IsVisible = true;
                }
                var carComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<CarComponent>(driverComponent.Car);
                carComponent.Driver = null;
                ComponentManager.Instance.RemoveComponentFromEntity<DriverComponent>(inputEvent.EntityId);
            }
        }

        public void Update()
        {
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(CarComponent)))
            {
                var carComponent = entity.Value as CarComponent;
                if (carComponent.Driver == null){ continue;}

                var driverPosition =
                    ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(carComponent.Driver.Value);
                if (driverPosition == null) continue;
                var driverMoveComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(carComponent.Driver.Value);
                if (driverMoveComponent == null) continue;
                
                var carPosition =
                    ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                if (carPosition == null) continue;
                var carMoveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                if (carMoveComponent == null) return;
                
                driverPosition.Position = new Vector2(carPosition.Position.X, carPosition.Position.Y);

                var driverActiomBindings =
                    ComponentManager.GetEntityComponentOrDefault<ActionBindings>(carComponent.Driver.Value);
                
//                carPosition.Position = new Vector2(driverPosition.Position.X, driverPosition.Position.Y);
//                carMoveComponent.Direction = driverMoveComponent.Direction;
            }
        }

        public struct UnmountEvent
        {
            public uint Driver;
        }
    }
}