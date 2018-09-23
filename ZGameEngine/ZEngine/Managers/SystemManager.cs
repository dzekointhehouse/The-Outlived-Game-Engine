using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreLinq;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Managers
{
    public class SystemManager
    {
        // _____________________________________________________________________________________________________________________ //

        // Singleton pattern strikes again! we contain an instance
        // of SystemManager which we can access by using Instance.
        public static SystemManager Instance => LazyInitializer.Value;
        private static readonly Lazy<SystemManager> LazyInitializer = new Lazy<SystemManager>(() => new SystemManager());

        // We immediately put instances of systems in this
        // dictionary, now they can easily be accessed with it's
        // type used as the key.
        public Dictionary<Type, IUpdateables> UpdateableSystems { get; set; }
        public Dictionary<Type, IDrawables> DrawableSystems { get; set; }

        // _____________________________________________________________________________________________________________________ //


        // Checks the systems dictionary if it contains the 
        // system that is specified as (T) type parameter when
        // this method is called, No big deal.
        private bool ContainsSystem<T>()
        {
            return UpdateableSystems.Count(system => system.Value.GetType() == typeof(T)) == 1;
        }

        // Gets the instance of system that is specified
        // as a type parameter. The method checks by using ContainsSystem
        // if the systemtype is a valid system and then returns the 
        // instance from the dictionary above. https://msdn.microsoft.com/en-us/library/d5x73970.aspx  
        public T Get<T>() where T : class, ISystem
        {
            //return Instance.GetSystem(typeof(T)) as T;
            if (UpdateableSystems.ContainsKey(typeof(T)))
            {
                return UpdateableSystems[typeof(T)] as T;
            }
            else if (DrawableSystems.ContainsKey(typeof(T)))
            {
                return DrawableSystems[typeof(T)] as T;
            }
            throw new Exception("No such system exist.");
        }


        public async void Update(GameTime gt)
        {
            UpdateableSystems.ForEach(async system =>
            {

                if (system.GetType() == typeof(CollisionSystem))
                {
                    await Instance.Get<CollisionSystem>().Update();
                }
                else
                {
                    system.Value.Update(gt);
                }

            });
            await Instance.Get<CollisionSystem>().Update();
        }

        public void Draw(SpriteBatch sb)
        {

        }


        public void Reset()
        {
            UpdateableSystems.Clear();
            DrawableSystems.Clear();
        }
    }
}