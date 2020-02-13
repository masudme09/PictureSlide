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
            while (Console.ReadLine()!="E")
            {
                //Create Picture objects from input
                Console.WriteLine("FilePAth: ");
                string[] allLines = File.ReadAllLines(Console.ReadLine());
                List<picture> pictures = new List<picture>();

                for (int i = 1; i < allLines.Length; i++)
                {
                    pictures.Add(new picture(allLines[i], i - 1));
                }

                //Separate Horizontal and vertical photos
                List<picture> horizonTalPhotos = new List<picture>();
                List<picture> verticalPhotos = new List<picture>();

                foreach (picture picture in pictures)
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

                //Add vertical photos to make slide for max points
                //Delete old one and modify list
                //slide of vertical photos
                picture selectedVpic = new picture();
                List<Slide> Slides = new List<Slide>();
                for (int i = 0; i < verticalPhotos.Count; i++)
                {
                    picture Vpic = new picture();
                    Vpic = verticalPhotos[i];
                    int maxPoint = 0;
                    int loopCount = 0;
                    for (int l=i;l< verticalPhotos.Count;l++)
                    {
                        picture vPic = new picture();
                        vPic = verticalPhotos[l];
                        //if (loopCount > 1000)
                        //    break;
                        if (Vpic.id != vPic.id)
                        {
                            //if (maxPoint < points(Vpic, vPic))
                            //{
                            //    maxPoint = points(Vpic, vPic);
                            //    selectedVpic = vPic;
                            //}
                            selectedVpic = vPic;
                            break;
                        }
                        loopCount++;
                    }

                    //if (maxPoint != 0)
                    //{
                    //    Slides.Add(new Slide(Vpic, selectedVpic));
                    //    verticalPhotos.Remove(selectedVpic);
                    //    verticalPhotos.Remove(Vpic);
                    //}
                    
                    Slides.Add(new Slide(Vpic, selectedVpic));
                    verticalPhotos.Remove(selectedVpic);
                    verticalPhotos.Remove(Vpic);
                    
                    if (verticalPhotos.Count == 1)
                        break;
                }
                //add the remaing vertical pics to make slides as it is
                int j = 0;
                while (j + 1 < verticalPhotos.Count)
                {
                    Slides.Add(new Slide(verticalPhotos[j], verticalPhotos[j + 1]));
                    verticalPhotos.Remove(verticalPhotos[j]);
                    verticalPhotos.Remove(verticalPhotos[j]);
                }

                //Add horizontal photos to the Slides
                foreach (picture pic in horizonTalPhotos)
                {
                    Slides.Add(pic.ToSlide());
                }

                //Sort Slides by tag number
                Slides = Slides.OrderByDescending(x => x.tags.Count).ToList();

                //Now go for each slide and match it with other slide for maximum possible point
                //For maximum point or 500 compare whichever least select a photo for the next slide and move on
                List<Slide> finalSlidesInOrder = new List<Slide>();

                for (int i = 0; i < Slides.Count; i++)
                {
                    Slide slide = new Slide();
                    slide = Slides[i];

                    int mPoint = 0;
                    int loopCount = 0;
                    Slide selectedSLide = new Slide();
                    for (int k = i; k < Slides.Count; k++)
                    {
                        Slide slide1 = Slides[k];
                        if (mPoint > 2)
                            break;

                        if (slide.id != slide1.id)
                        {
                            if (mPoint < points(slide, slide1))
                            {
                                mPoint = points(slide, slide1);
                                selectedSLide = slide1;
                            }
                        }
                        loopCount++;
                    }

                    if (mPoint != 0)
                    {
                        int indexOfCurrentSlide = Slides.IndexOf(slide);
                        int indexOfSelectedSlide = Slides.IndexOf(selectedSLide);
                        Slides.Remove(selectedSLide);
                        Slides.Insert(indexOfCurrentSlide + 1, selectedSLide);
                        //finalSlidesInOrder.Add(slide);
                        //finalSlidesInOrder.Add(selectedSLide);
                    }
                }

                //Writing output and overall point
                int overAllPoints = 0;
                for (int i = 0; i < Slides.Count; i++)
                {
                    if (i + 1 < Slides.Count)
                        overAllPoints = overAllPoints + points(Slides[i], Slides[i + 1]);
                }

                Console.WriteLine("OVERALL POINTS EARNED: " + overAllPoints);
            }
        }

        //Method to calculate points
        public static int  points(picture mainPicture, picture targetPic)
        {
            List<string> mainPicTags = new List<string>();
            List<string> targetPicTags = new List<string>();
            mainPicTags =mainPicture.tags;
            targetPicTags = targetPic.tags;
            int matchedCount, unmatchedA, unmatchedB;

            List<string> matchedItems = mainPicTags.Intersect(targetPicTags).ToList();
            matchedCount = matchedItems.Count;
            unmatchedA = mainPicTags.Count - matchedCount;
            unmatchedB = targetPicTags.Count - matchedCount;


            return Math.Min(Math.Min(matchedCount, unmatchedA),unmatchedB);
            
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
        public picture()
        {

        }

        public Slide ToSlide()
        {
            Slide slide = new Slide(this);
            return slide;
        }
    }

    class Slide:picture
    {
        public string id { get; set; } //id consists of two picture id with space seperator
        //public int numberOfTags { get; set; }
        //public List<string> tags = new List<string>();
                
        public Slide(picture vPicture1, picture vPicture2)
        {
            id = vPicture1.id + " " + vPicture2.id;
            numberOfTags = vPicture1.numberOfTags + vPicture2.numberOfTags;
            tags = vPicture1.tags;
            tags.Concat(vPicture2.tags);
            pictureType = 'V';
        }

        /// <summary>
        /// For horizontal pictures
        /// </summary>
        /// <param name="picture"></param>
        public Slide(picture picture)
        {
            id = picture.id.ToString();
            numberOfTags = picture.numberOfTags;
            tags = picture.tags;
            pictureType = picture.pictureType;
        }

        public Slide()
        {
        }
    }

}
