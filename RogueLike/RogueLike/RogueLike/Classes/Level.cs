using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.AI;
using SharpDX.Direct3D9;

namespace RogueLike.Classes
{
    public class Level
    {
        public Room[,] Rooms { get; set; }

        private Viewport viewport;

        private static Random random = new Random();

        private int roomNumber;

        private bool lastLevel;

        public Level(Viewport viewport, int roomNumber, bool lastLevel)
        {
            this.viewport = viewport;
            Rooms = new Room[4,3];
            this.lastLevel = lastLevel;
            this.roomNumber = roomNumber;
        }

        /*public void generateLevel2()
        {
            int
                width,
                height;

            for (int i = 0; i < roomNumber; i++)
            {
                width = random.Next(9, 15);
                if (width % 2 == 0) width++;
                height = random.Next(15, 29);
                if (height % 2 == 0) height++;
                if (i == 0)
                {
                    Rooms[i] = new Room(viewport, true, false, lastLevel, width, height);
                }
                else if (i == Rooms.Length - 1)
                {
                    Rooms[i] = new Room(viewport, false, true, lastLevel, width, height);
                }
                else
                {
                    Rooms[i] = new Room(viewport, false, false, lastLevel, width, height);
                }
            }
        }*/

        public void generateLevel(ContentManager content)
        {
            int
                width,
            height;

            int i, j;

            int counter = 0;

            Rooms[3, 1] = new Room(viewport, true, false, lastLevel, 11, 11);

            while (counter < roomNumber)
            {
                width = random.Next(9, 15);
                if (width % 2 == 0) width++;
                height = random.Next(15, 29);
                if (height % 2 == 0) height++;
                
                if(counter == roomNumber - 1)
                {
                    j = random.Next(0, 3);
                    if(Rooms[1, j] != null)
                    {
                        Rooms[0, j] = new Room(viewport,false, true, lastLevel, 15, 15);
                        counter++;
                    }
                }
                else
                {
                    i = random.Next(1, 4);
                    j= random.Next(0, 3);
                    if (Rooms[i, j] == null)
                    {
                        if(i == 3)
                        {
                            if(j == 0 && (Rooms[i - 1, j] != null || Rooms[i, j + 1] != null))
                            {
                                Rooms[i, j] = new Room(viewport, false, false, lastLevel, width, height);
                                counter++;
                            }
                            else if(j == 2 && (Rooms[i - 1, j] != null || Rooms[i, j - 1] != null))
                            {
                                Rooms[i, j] = new Room(viewport, false, false, lastLevel, width, height);
                                counter++;
                            }
                            else if(Rooms[i - 1, j] != null || Rooms[i, j - 1] != null || Rooms[i, j + 1] != null)
                            {
                                Rooms[i, j] = new Room(viewport, false, false, lastLevel, width, height);
                                counter++;
                            }               
                        }
                        else if(j == 0)
                        {
                            if (Rooms[i - 1, j] != null || Rooms[i + 1, j] != null || Rooms[i, j + 1] != null)
                            {
                                Rooms[i, j] = new Room(viewport, false, false, lastLevel, width, height);
                                counter++;
                            }
                        }
                        else if(j == 2)
                        {
                            if (Rooms[i - 1, j] != null || Rooms[i + 1, j] != null || Rooms[i, j - 1] != null)
                            {
                                Rooms[i, j] = new Room(viewport, false, false, lastLevel, width, height);
                                counter++;
                            }
                        }
                        else if (Rooms[i - 1, j] != null || Rooms[i + 1, j] != null || Rooms[i, j - 1] != null || Rooms[i, j + 1] != null)
                        {
                            Rooms[i, j] = new Room(viewport, false, false, lastLevel, width, height);
                            counter++;
                        }
                       
                            /*if (i < 3 && i > 0 && j < 2 && j > 0)
                            {
                                if (Rooms[i, j] == null && Rooms[i - 1, j] != null || Rooms[i + 1, j] != null || Rooms[i, j - 1] != null || Rooms[i, j + 1] != null)
                                {
                                    Rooms[i, j] = new Room(viewport, false, false, lastLevel, width, height);

                                    counter++;
                                }
                            }*/
                    }
                }
            }
        }

        public void addDoors(ContentManager content)
        {
            for(int i = 0; i < Rooms.GetLength(0); i++)
            {
                for(int j = 0; j < Rooms.GetLength(1); j++)
                {
                    if (Rooms[i, j] != null)
                    {
                        try
                        {             
                            if (Rooms[i, j - 1] != null) Rooms[i, j].addDoor(Room.DoorType.Left, content);
                        }
                        catch (IndexOutOfRangeException e)
                        {

                        }
                        try { 
                        if (Rooms[i + 1, j] != null) Rooms[i, j].addDoor(Room.DoorType.Bottom, content);
                        }
                        catch (IndexOutOfRangeException e)
                        {

                        }
                        try
                        {
                            if (Rooms[i, j + 1] != null) Rooms[i, j].addDoor(Room.DoorType.Right, content);
                        }
                        catch (IndexOutOfRangeException e)
                        {

                        }
                        try
                        {
                            if (Rooms[i - 1, j] != null) Rooms[i, j].addDoor(Room.DoorType.Top, content);
                        }
                        catch (IndexOutOfRangeException e)
                        {

                        }
                        
                        
                    }
                }
            }
        }
    }
}
