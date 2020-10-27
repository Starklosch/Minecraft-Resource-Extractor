using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Resource_Extractor
{
    public struct IndexReader
    {
        public string text;

        public IndexReader(string text)
        {
            this.text = text;
        }

        public Resource GetResources()
        {
            int start = text.IndexOf(":");
            string content = text.Substring(start + 1);
            string[] objects = content.Split(new string[] { "}," }, StringSplitOptions.None);

            foreach (string obj in objects)
            {
                Console.WriteLine(obj);
            }

            return new Resource();
        }

        public static Resource GetResources(string text)
        {
            IndexReader ir;
            ir.text = text;
            return ir.GetResources();
        }
    }
}
