using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class HighScoreSystem : ISystem
    {

        private void readFile(HighScoreComponent highScore)
        {
            highScore.path = Directory.GetCurrentDirectory() + "\\saveScore";
            highScore.score = File.ReadAllLines(highScore.path); 
        }

        private void writeFile(string path, string[] score)
        {
            File.WriteAllLines(path, score);
        }
        

        public void SubmitScore(int newScore)
        {
            var HighScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(HighScoreComponent));
            if (HighScoreList.Count <= 0) return;
            var highScore = (HighScoreComponent)HighScoreList.First().Value;

            readFile(highScore);

            if (newScore > Convert.ToInt32(highScore.score[9]))
            { 
                for (int i = 0; i < highScore.score.Length; i++)
                {
                    int temp = Convert.ToInt32(highScore.score[i]);
                    if (temp <= newScore) {
                        highScore.score[i] = Convert.ToString(newScore);
                        newScore = temp;
                    }
                }
                writeFile(highScore.path, highScore.score);
            }
        }
        /*
        public void submitScore(int newScore)
        {
            for (int i = 0; i < score.Length; i++)
            {
                if (newScore > Convert.ToInt32(score[i])) {
                    replaceScore(i);
                    score[i] = Convert.ToString(newScore);
                    break;
                }
            }
            writeFile();
        }
        private void replaceScore(int position)
        {
            for (int i = score.Length - 1; i > position; i--)
            {
                score[i] = score[i - 1];
            }
        }
        [System.Obsolete("Lost the contest")]
        public void NickScore(int newScore)
        {
            bool replaced = false;
            string old = "";
            for (int i = 0; i < score.Length; i++)
            {
                if (!replaced)
                { 
                    if (newScore > Convert.ToInt32(score[i]))
                    {
                        replaced = true;
                        old = score[i];
                        score[i] = Convert.ToString(newScore);
                    }
                }
                else
                {
                    string temp = old;
                    old = score[i];
                    score[i] = temp;
                }
            }
            writeFile();
        }

        */
    }
}
