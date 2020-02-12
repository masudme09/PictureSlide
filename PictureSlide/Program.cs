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

            //Separate Horizontal and vertical photos
            List<picture> horizonTalPhotos = new List<picture>();
            List<picture> verticalPhotos = new List<picture>();

            foreach(picture picture in pictures)
            {
                if (picture.pictureType == 'H')
                {
                    horizonTalPhotos.Add(picture);
                }
                else
                    verticalPhotos.Add(picture);
            }

            //Sort largest tags to smallest tags in number for both horizontal and vertical
            horizonTalPhotos = horizonTalPhotos.OrderByDescending(x => x.tags.Count).ToList();
            verticalPhotos = verticalPhotos.OrderByDescending(x => x.tags.Count).ToList();

        }

        //Method to calculate points
        public int  points(picture mainPicture, picture targetPic)
        {
            List<string> mainPicTags = new List<string>();
            List<string> targetPicTags = new List<string>();
            mainPicTags =mainPicture.tags;
            targetPicTags = targetPic.tags;
            int matchedCount, unmatchedA, unmatchedB;

            foreach(string tag in mainPicTags)
            {

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
