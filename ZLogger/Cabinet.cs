using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security;
using System.Security.Permissions;

namespace ZLogger
{
    public class Cabinet
    {
        public string Label { get;}
        
        private Dictionary<string, List<IFile>> Folders { get; } = new Dictionary<string, List<IFile>>();

        public Cabinet(string label)
        {
            Label = label;
        }
        
        public void StoreFile(string type, IFile file)
        {
            Folders[GetOrCreateKey(type)].Add(file);
        }

        public IFile[] GetFolder(string type, IFile file)
        {
            if (!Folders.ContainsKey(type)) throw new Exception("No such file type in the Logs");

            return Folders[type].ToArray();
        }

        public Dictionary<string, List<IFile>> GetAllFolders()
        {
            return Folders;
        }

        public void Clear()
        {
            foreach (var list in Folders)
            {
                list.Value.Clear();
            }
            Folders.Clear();
        }

        private string GetOrCreateKey(string key)
        {
            if (!Folders.ContainsKey(key)) Folders[key] = new List<IFile>();
            return key;
        }
    }
}