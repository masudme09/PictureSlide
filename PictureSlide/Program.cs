using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureSlide
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create Picture objects from input
            string[] allLines = File.ReadAllLines(Console.ReadLine());
            List<picture> pictures = new List<picture>();

            for(int i=1;i<allLines.Length;i++)
            {
                pictures.Add(new picture(allLines[i], i-1));
            }


        }
    }

    class picture
    {
        public char pictureType { get; set; }
        public int numberOfTags  { get; set; }
        public List<string> tags = new List<string>();
        public int id { get; set; }
        public picture(string lineContainPictureDef,int id)
        {
            this.id = id;
            string[] splited = lineContainPictureDef.Split(' ');
            pictureType = Convert.ToChar(splited[0]);
            numberOfTags = Convert.ToInt32(splited[1]);
            for (int i=2;i<splited.Length;i++)
            {
                tags.Add(splited[i]);
            }
        }
    }
}
