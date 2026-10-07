using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork
{
    enum Commands
    {
        QUIT,
        LOOK,
        NORTH,
        SOUTH,
        EAST,
        WEST,
        UNKNOWN
    }

    class Program
    {

        private static bool IsDirection(Commands command) => Directions.Contains(command); // uses expression-bodied method 
        private static readonly Room[,] Rooms =
        {
            { new Room ("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        private static Room CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }
        private static int currentRoom = 1;

        private static (int Row, int Column) Location = (1, 1);

        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private static void InitializeRoomDescriptions()
        {
            var roomMap = new Dictionary<string, Room>();
            foreach (Room room in Rooms)
            {
                roomMap[room.Name] = room;
            }

            roomMap["Rocky Trail"].Description = "You are on a rock-strewn trail."; // Rocky Trail
            roomMap["South of House"].Description = "You are facing the south side of a white house. There is no door here, and all the windows are barred."; // South of House
            roomMap["Canyon View"].Description = "You are at the top of the Great Canyon on its south wall."; // Canyon View

            roomMap["Forest"].Description = "This is a forest, with trees in all directions around you"; // Forest 
            roomMap["West of House"].Description = "This is an open field west of a white house, with a boarded front door."; // West of House
            roomMap["Behind House"].Description = "You are behind the white house. In one corner of the house there is a small window which is slightly ajar."; // Behind House

            roomMap["Dense Woods"].Description = "This is a dimly lit forest, with large trees all around. To the east, there appears to be sunlight."; // Dense Woods
            roomMap["North of House"].Description = "You are facing the north side of a white house. There is no door here, and all the windows are barred."; // North of House
            roomMap["Clearing"].Description = "You are in a clearing, with a forest surrounding you on the west and south."; // Clearing 
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to zork!");
            InitializeRoomDescriptions();


            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                if (previousRoom != null)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }
                Console.WriteLine(CurrentRoom);
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                
                switch(command)
                {
                    case Commands.QUIT:
                        Console.WriteLine("Thank you for playing");
                        break;
                    case Commands.LOOK:
                        Console.WriteLine(CurrentRoom.Description);
                        break;
                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (Move(command) == false)
                        {
                            Console.WriteLine("The way is shut!");
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown Command");
                        break;
                }

            }

        }

        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
        }

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid Direction");

            bool isValidMove = true;
            switch (command)
            {
                case Commands.NORTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;
                case Commands.SOUTH when Location.Row < Rooms.GetLength(0):
                    Location.Row--;
                    break;
                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;
                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    break;
                default:
                    isValidMove = false;
                    break;
            }

            return isValidMove;

        }

    }

    public static class Assert //separate class 
    {
        [Conditional("DEBUG")]
        public static void IsTrue(bool expression, string message = null) // checks to make sure you can go in a certain direction
        {
            if (expression == false)
            {
                throw new Exception(message);
            }
        }


    }

    public class Room
    {
        public string Name { get; }

        public string Description { get; set; }

        public Room(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }


}
