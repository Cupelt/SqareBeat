using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.cupelt.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


namespace com.cupelt.sqarebeat
{
    [System.Serializable]
    public class BeatMap
    {
        public BeatMap(string path)
        {
            this.path = path;
            
            using (StreamReader file = File.OpenText(path + @"\beatmap.sbm"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                this.json = (JObject)JToken.ReadFrom(reader);
            }
    
            JObject general = (JObject)this.json["General"].ToObject(typeof(JObject));
            this.Clip = Util.LoadMp3(path + @"\" + general["audio"]);
    
            this.Artist = general["artist"].ToString();
            this.Title = general["title"].ToString();
        }
        
        private JObject json;
        private string path;
    
        public string Artist;
        public string Title;
    
        public AudioClip Clip;
    
        public Stack<Note> getNotes()
        {
            JToken notes = this.json.SelectToken("Notes");

            List<Note> notelist = new List<Note>();
            foreach (JToken note in notes)
            {
                string data = note.ToString();

                try
                {
                    switch (data.Split(',')[0])
                    {
                        case "n":
                            notelist.Add(new Note.normal(data));
                            break;
                        case "l":
                            notelist.Add(new Note.laser(data));
                            break;
                    }
                }
                catch (Exception e)
                {
                    //beatmap something is wrong
                }
            }

            return new Stack<Note>(notelist);
        }
    }
    
    public class TimeLine
    {
        public long time;
        
        public TimeLine(long time)
        {
            this.time = time;
        }
    }
    
    public class Note : TimeLine
    {
        public Note(long time) : base(time) {}
    
        public class normal : Note
        {
            public enum Direction { Up, Down, Left, Right }

            public int index;
            public Direction direction;
            
            public normal(long time, Direction direction, int index) : base(time)
            {
                this.direction = direction;
                this.index = index;
            }

            public normal(string format) : base(long.Parse(format.Split(',')[1]))
            {
                string[] data = format.Split(',');
                
                this.direction = (Direction)int.Parse(data[2]);
                this.index = int.Parse(data[3]);
            }
        }
        
        public class laser : Note
        {
            public int index;
            public long duration;
            public bool isHorizontal;

            public laser(long time, bool isHorizontal, int index, long duration) : base(time)
            {
                this.isHorizontal = isHorizontal;
                this.index = index;
                this.duration = duration;
            }
            
            public laser(string format) : base(long.Parse(format.Split(',')[1]))
            {
                string[] data = format.Split(',');
                
                this.isHorizontal = bool.Parse(data[2]);
                this.index = int.Parse(data[3]);
                this.duration = long.Parse(data[4]);
            }
        }
    }
}