using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Spelkonstruktionsprojekt.ZEngine.Managers
{
    /*
* The AssetManager centralises storage of game content.
* Though in most cases content would be loaded at startup only and should be quick enough not to warrant multithreading,
* the AssetManager has still been made thread safe to enable concurrency in instances where content would be modified by multiple threads.
*/
    public sealed class AssetManager
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _contentDictionary;

        #region Thread-safe singleton - use "AssetManager.Instance" to access
        private static readonly Lazy<AssetManager> Lazy = new Lazy<AssetManager>(() => new AssetManager(), true);

        private AssetManager()
        {
            _contentDictionary = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();
        }

        public static AssetManager Instance => Lazy.Value;

        #endregion

        public void Add<T>(ContentManager content, String contentName)
        {
            if (!_contentDictionary.ContainsKey(typeof(T)))
            {
                _contentDictionary.TryAdd(typeof(T), new ConcurrentDictionary<string, object>());
            }
            _contentDictionary[typeof(T)].TryAdd(contentName, content.Load<T>(contentName));
        }

        public void Add<T>(ContentManager content, String sourceName, String contentName)
        {
            if (!_contentDictionary.ContainsKey(typeof(T)))
            {
                _contentDictionary.TryAdd(typeof(T), new ConcurrentDictionary<string, object>());
            }
            _contentDictionary[typeof(T)].TryAdd(contentName, content.Load<T>(sourceName));
        }

        public T Get<T>(String contentName) where T : class
        {
            return _contentDictionary[typeof(T)][contentName] as T;
        }
    }
}
