using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using Sprity;

    public class ImportOsb
    {
        public string OsbPath = "/Users/josepuma/Documents/storyboards/358685 keeno - alternate/keeno - alternate (mintong89).osb";

        private Dictionary<string, string> variables = new Dictionary<string, string>();
        private List<Sprite> sprites = new List<Sprite>();
        public List<Sprite> Generate()
        {
            var lines = File.ReadAllLines(OsbPath);
            parseEventsSection(lines);
            return sprites;
        }

        public void parseEventsSection(string[] lines)
        {
            Sprite osbSprite = null;
            var inCommandGroup = false;
            foreach(var line in lines){
                //Console.WriteLine(line);
                if (line.StartsWith("//")) continue;
                if (line.StartsWith("[")) continue;

                var depth = 0;
                while (line.Substring(depth).StartsWith(" "))
                    ++depth;

                var trimmedLine = applyVariables(line.Trim());
                var values = trimmedLine.Split(',');

                if (inCommandGroup && depth < 2)
                {
                    //osbSprite.EndGroup();
                    osbSprite = null;
                    inCommandGroup = false;
                }

                switch (values[0])
                {
                    case "Sprite":
                        {
                            var layerName = values[1];
                            //var origin = (OsbOrigin)Enum.Parse(typeof(OsbOrigin), values[2]);
                            var path = removePathQuotes(values[3]).ToLower();
                            var cleanPath = path.Replace("\\", "/");
                            var x = float.Parse(values[4], CultureInfo.InvariantCulture);
                            var y = float.Parse(values[5], CultureInfo.InvariantCulture);
                            if(osbSprite != null){
                                sprites.Add(osbSprite);
                                osbSprite = null;
                            } 
                            //Console.WriteLine("sprite:" + cleanPath);
                            osbSprite = new Sprite(cleanPath){ Position = new Point((int)x,(int)y) };
                            //osbSprite = GetLayer(layerName).CreateSprite(path, origin, new Vector2(x, y));
                        }
                        break;
                    case "Animation":
                        {
                            var layerName = values[1];
                            //var origin = (OsbOrigin)Enum.Parse(typeof(OsbOrigin), values[2]);
                            var path = removePathQuotes(values[3]);
                            var x = float.Parse(values[4], CultureInfo.InvariantCulture);
                            var y = float.Parse(values[5], CultureInfo.InvariantCulture);
                            var frameCount = int.Parse(values[6]);
                            var frameDelay = double.Parse(values[7], CultureInfo.InvariantCulture);
                            //var loopType = (OsbLoopType)Enum.Parse(typeof(OsbLoopType), values[8]);
                            //osbSprite = GetLayer(layerName).CreateAnimation(path, frameCount, frameDelay, loopType, origin, new Vector2(x, y));
                        }
                        break;
                    case "Sample":
                        {
                            var time = double.Parse(values[1], CultureInfo.InvariantCulture);
                            var layerName = values[2];
                            var path = removePathQuotes(values[3]);
                            var volume = float.Parse(values[4], CultureInfo.InvariantCulture);
                            //GetLayer(layerName).CreateSample(path, time, volume);
                        }
                        break;
                    case "T":
                        {
                            var triggerName = values[1];
                            var startTime = double.Parse(values[2], CultureInfo.InvariantCulture);
                            var endTime = double.Parse(values[3], CultureInfo.InvariantCulture);
                            var groupNumber = values.Length > 4 ? int.Parse(values[4]) : 0;
                            //osbSprite.StartTriggerGroup(triggerName, startTime, endTime, groupNumber);
                            //inCommandGroup = true;
                        }
                        break;
                    case "L":
                        {
                            var startTime = double.Parse(values[1], CultureInfo.InvariantCulture);
                            var loopCount = int.Parse(values[2]);
                            //osbSprite.StartLoopGroup(startTime, loopCount);
                            //inCommandGroup = true;
                        }
                        break;
                    default:
                        {
                            if (string.IsNullOrEmpty(values[3]))
                                values[3] = values[2];

                            var commandType = values[0];
                            //var easing = (OsbEasing)int.Parse(values[1]);
                            var startTime = double.Parse(values[2], CultureInfo.InvariantCulture);
                            var endTime = double.Parse(values[3], CultureInfo.InvariantCulture);

                            switch (commandType)
                            {
                                case "F":
                                    {
                                        var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                        osbSprite.Fade((int)startTime,(int)endTime, startValue, endValue);
                                    }
                                    break;
                                case "S":
                                    {
                                        var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                        osbSprite.Scale((int)startTime, (int)endTime, startValue, endValue);
                                    }
                                    break;
                                case "V":
                                    {
                                        var startX = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var startY = double.Parse(values[5], CultureInfo.InvariantCulture);
                                        var endX = values.Length > 6 ? double.Parse(values[6], CultureInfo.InvariantCulture) : startX;
                                        var endY = values.Length > 7 ? double.Parse(values[7], CultureInfo.InvariantCulture) : startY;
                                        osbSprite.ScaleVec((int)startTime, (int)endTime, startX, startY, endX, endY);
                                    }
                                    break;
                                case "R":
                                    {
                                        var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                        osbSprite.Rotate((int)startTime, (int)endTime, startValue, endValue);
                                    }
                                    break;
                                case "M":
                                    {
                                        var startX = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var startY = double.Parse(values[5], CultureInfo.InvariantCulture);
                                        var endX = values.Length > 6 ? double.Parse(values[6], CultureInfo.InvariantCulture) : startX;
                                        var endY = values.Length > 7 ? double.Parse(values[7], CultureInfo.InvariantCulture) : startY;
                                        osbSprite.Move((int)startTime, (int)endTime, startX, startY, endX, endY);
                                    }
                                    break;
                                case "MX":
                                    {
                                        var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                        osbSprite.MoveX((int)startTime, (int)endTime, startValue, endValue);
                                    }
                                    break;
                                case "MY":
                                    {
                                        var startValue = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var endValue = values.Length > 5 ? double.Parse(values[5], CultureInfo.InvariantCulture) : startValue;
                                        osbSprite.MoveY((int)startTime, (int)endTime, startValue, endValue);
                                    }
                                    break;
                                case "C":
                                    {
                                        var startX = double.Parse(values[4], CultureInfo.InvariantCulture);
                                        var startY = double.Parse(values[5], CultureInfo.InvariantCulture);
                                        var startZ = double.Parse(values[6], CultureInfo.InvariantCulture);
                                        var endX = values.Length > 7 ? double.Parse(values[7], CultureInfo.InvariantCulture) : startX;
                                        var endY = values.Length > 8 ? double.Parse(values[8], CultureInfo.InvariantCulture) : startY;
                                        var endZ = values.Length > 9 ? double.Parse(values[9], CultureInfo.InvariantCulture) : startZ;
                                        osbSprite.Tint(startX, startY, startZ);
                                        //osbSprite.Color(easing, startTime, endTime, startX / 255f, startY / 255f, startZ / 255f, endX / 255f, endY / 255f, endZ / 255f);
                                    }
                                    break;
                                case "P":
                                    {
                                        var type = values[4];
                                        switch (type)
                                        {
                                            case "A": osbSprite.IsAdditive(); break;
                                            //case "H": osbSprite.FlipH(startTime, endTime); break;
                                            //case "V": osbSprite.FlipV(startTime, endTime); break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }

            if (inCommandGroup)
            {
                //osbSprite.EndGroup();
                inCommandGroup = false;
            }
        }

        private static string removePathQuotes(string path)
        {
            return path.StartsWith("\"") && path.EndsWith("\"") ? path.Substring(1, path.Length - 2) : path;
        }

        private string applyVariables(string line)
        {
            if (!line.Contains("$"))
                return line;

            foreach (var entry in variables)
                line = line.Replace(entry.Key, entry.Value);

            return line;
        }
    }
