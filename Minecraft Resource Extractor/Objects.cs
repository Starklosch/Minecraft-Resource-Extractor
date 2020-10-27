using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Minecraft_Resource_Extractor
{
    class Index
    {
        public Resource[] resources;
    }

    public class Resource
    {
        public string name;

        public string hash;
        public string size;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("name: {0}, hash: {1}, size: {2}", name, hash, size);
        }
    }

    public class ExtractTask
    {

        private static volatile float progress = 0;

        public delegate void ProgressChanged(float value);
        public static event ProgressChanged OnProgressChanged;

        public static float Progress
        {
            set
            {
                progress = value;
                OnProgressChanged?.Invoke(progress);
            }
            get => progress;
        }

        public static async Task<int> Start(string content)
        {
            Resource[] res;

            await Task.Run(async () =>
            {

                res = GetResources(content);

                for (int i = 0; i < res.Length; i++)
                {

                    float newValue = (float)i / res.Length;

                    Progress = newValue;

                    string file = Program.OBJECTS_PATH + @"\" + res[i].hash.Substring(0, 2) + @"\" + res[i].hash;

                    if (File.Exists(file))
                    {
                        await CopyFile(file, Program.EXTRACT_PATH + @"\" + res[i].name, true);
                    } 
                }

                Progress = 1;

            });

            progress = 0;
            return 0;
        }

        public static Resource[] GetResources(string text)
        {
            int start = text.IndexOf(":");
            string content = text.Substring(start + 1);
            //Split resources by "},"
            string[] objects = content.Split(new string[] { "}," }, StringSplitOptions.None);
            objects[0] = objects[0].Trim(new char[] { ' ', '{' });
            objects[objects.Length - 1] = objects[objects.Length - 1].Trim('}');

            Resource[] resources = new Resource[objects.Length];

            for (int i = 0; i < objects.Length; i++)
            {
                string obj = objects[i].Replace("\"", "").Replace(" ", "");
                Resource res = new Resource();

                //Variable initialization
                int bracket = obj.IndexOf('{');
                int separator = obj.IndexOf(':');
                int comma = obj.IndexOf(',');

                //start = bracket < comma ? bracket + 1 : comma + 1;
                start = 0;
                //int end = obj.IndexOf(',', start + 1);

                //Get name from a string using updated variables
                res.name = obj.Substring(start, separator - start);

                //Update variables to catch hash instead name
                separator = obj.IndexOf(':', separator + 1);
                //comma = obj.IndexOf(',', comma + 1);
                start = separator + 1;
                //end = comma - start;

                //Get hash from a string using updated variables
                res.hash = obj.Substring(start, comma - start);

                //Update variables to catch size instead name
                separator = obj.IndexOf(':', separator + 1);
                comma = obj.IndexOf(',', comma + 1);
                start = separator + 1;

                //Get size from a string using updated variables
                res.size = comma < 0 ? obj.Substring(start) : obj.Substring(start, comma - start);

                resources[i] = res;

            }

            return resources;
        }

        public static async Task<int> CopyFile(string sourceFileName, string destFileName, bool overwrite = false)
        {
            destFileName = destFileName.Replace(@"/", @"\").Replace("\n", "");
            bool cond = !Directory.Exists(Path.GetDirectoryName(destFileName));
            
            if (cond)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFileName));
                Console.WriteLine("created path for: " + destFileName);
            }
            File.Copy(sourceFileName, destFileName, overwrite);
            Console.WriteLine("created: " + destFileName);

            return 0;
        }

    }
}
