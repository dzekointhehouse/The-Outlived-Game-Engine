using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ZLogger
{
    public class Logger
    {
        private Dictionary<string, Cabinet> FileCabinets { get; } = new Dictionary<string, Cabinet>();

        public Cabinet GetOrCreate(string cabinetLabel)
        {
            return !FileCabinets.ContainsKey(cabinetLabel)
                ? NewCabinet(cabinetLabel) 
                : FileCabinets[cabinetLabel];
        }

        public Dictionary<string, Cabinet> GetLogs()
        {
            return FileCabinets;
        }

        public void ClearAll()
        {
            foreach (var cabinet in FileCabinets)
            {
                cabinet.Value.Clear();
            }
            FileCabinets.Clear();
        }

        private Cabinet NewCabinet(string cabinetLabel)
        {
            if(FileCabinets.ContainsKey(cabinetLabel)) throw new Exception("A cabinet with that label already exists");
            
            var cabinet = new Cabinet(cabinetLabel);
            FileCabinets[cabinetLabel] = cabinet;
            return cabinet;
        }
    }
}