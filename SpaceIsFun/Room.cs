using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;

namespace SpaceIsFun
{
    /// <summary>
    /// This contains all the info of a room on a ship 
    /// </summary>
    [Serializable] public partial class Room : Entity, ISerializable
    {
        #region fields
        /// <summary>
        /// grid position of the room's top-left grid (if the room's top-left grid's grid position is (2,2), then this will be (2,2)
        /// </summary>
        protected Vector2 roomPosition;

        /// <summary>
        /// parameter for roomPosition
        /// </summary>
        public Vector2 RoomPosition
        {
            get
            {
                return roomPosition;
            }

            set
            {
                RoomPosition = value;
            }
        }

        /// <summary>
        /// position of this room's 1,1 in screenspace
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// parameter for position
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }


        /// <summary>
        /// drawable for the room (this is probably never gonna move)
        /// </summary>
        protected Drawable sprite;

        public Drawable Sprite
        {
            get
            {
                return sprite;
            }

            set
            {
                sprite = value;
            }
        }

        /// <summary>
        /// texture for the room
        /// </summary>
        protected Texture2D roomTexture;

        /// <summary>
        /// texture for the room while highlighted
        /// </summary>
        protected Texture2D roomHighlightTexture;

        /// <summary>
        /// parameter for roomTexture
        /// </summary>
        public Texture2D RoomTexture
        {
            get
            {
                return roomTexture;
            }

            set
            {
                roomTexture = value;
            }
        }

        /// <summary>
        /// paramter for roomHighlightTexture
        /// </summary>
        public Texture2D RoomHighlightTexture
        {
            get
            {
                return roomHighlightTexture;
            }

            set
            {
                roomHighlightTexture = value;
            }
        }

        /// <summary>
        /// whether or not the room is highlighted
        /// </summary>
        protected bool highlighted;

        /// <summary>
        /// parameter for highlighted
        /// </summary>
        public bool Highlighted
        {
            get
            {
                return highlighted;
            }

            set
            {
                highlighted = value;
            }
        }

        /// <summary>
        /// parameter for is mannable
        /// </summary>

        protected bool isMannable;

        public bool IsMannable
        {
            get
            {
                return isMannable;
            }

        }

        /// <summary>
        /// parameter for room type
        /// </summary>

        protected int roomType;

        public int RoomType
        {
            get
            {
                return roomType;
            }

        }

        /// <summary>
        /// parameter for max energy
        /// </summary>
        protected int maxEnergy;

        public int MaxEnergy
        {
            get
            {
                return maxEnergy;
            }
            set
            {
                maxEnergy = value;
            }
        }

        /// <summary>
        /// parameter for current available energy
        /// </summary>

        protected int currentAvailableEnergy;

        public int CurrentAvailableEnergy
        {
            get
            {
                return currentAvailableEnergy;
            }
            set
            {
                currentAvailableEnergy = value;
            }
        }


        /// <summary>
        /// parameter for max energy
        /// </summary>
        protected int isManned;

        public int IsManned
        {
            set
            {
                isManned = value;
            }
        }

        /// <summary>
        /// parameter for room health
        /// </summary>

        protected int roomHealth;

        public int RoomHealth
        {
            get
            {
                return roomHealth;
            }
            set
            {
                roomHealth = value;
            }
        }

        protected bool aflame;

        public bool Aflame
        {
            get
            {
                return aflame;
            }
            set
            {
                aflame = value;
            }
        }


        /// <summary>
        /// Parameter to determine if hull is breached
        /// </summary>
        protected bool hullBreach;

        public bool HullBreach
        {
            get
            {
                return hullBreach;
            }
            set
            {
                hullBreach = value;
            }
        }

        /// <summary>
        /// Parameter for width of the room
        /// </summary>
        protected int width;

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }


        /// <summary>
        /// Parameter for height of the room
        /// </summary>
        protected int height;

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Parameter for O2 of the room
        /// </summary>
        protected int roomO2;

        public int RoomO2
        {
            get
            {
                return roomO2;
            }
            set
            {
                roomO2 = value;
            }
        }

        /// <summary>
        /// Define the shape of the room
        /// </summary>
        protected Globals.roomShape roomShape;

        public Globals.roomShape RoomShape
        {
            get
            {
                return roomShape;
            }
            set
            {
                roomShape = value;
            }
        }

        /// <summary>
        /// define the type of the room
        /// </summary>
        protected Globals.roomType typeOfRoom;

        public Globals.roomType TypeOfRoom
        {
            get
            {
                return typeOfRoom;
            }

            set
            {
                typeOfRoom = value;
            }
        }

        /// <summary>
        /// the amount of grid spaces this room takes up
        /// </summary>
        protected int roomSize;

        /// <summary>
        /// getter for roomSize
        /// </summary>
        public int RoomSize
        {
            get
            {
                return roomSize;
            }

            set
            {
                roomSize = value;
            }
        }

        /// <summary>
        /// the grids contained in the room
        /// </summary>
        private List<int> roomGrids;
        

        /// <summary>
        /// getter for roomGrids
        /// </summary>
        public List<int> RoomGrids
        {
            get
            {
                return roomGrids;
            }
            set
            {
                roomGrids = value;
            }
        }



        /// <summary>
        /// Declarations for state machines of individual room
        /// </summary>
        StateMachine roomStateMachine;

        State normal, damaged, inoperable, disabled;

        #endregion

        #region constructors / destructors
        public Room() { }

        /// <summary>
        /// constructor for a room
        /// </summary>
        /// <param name="texture">texture for the room</param>
        /// <param name="highlightTexture">texture for the room when its highlighted</param>
        /// <param name="x">x-position of the top-left grid position</param>
        /// <param name="y">y-position of the top-left grid position</param>
        public Room(Texture2D texture, Texture2D highlightTexture, int x, int y, Vector2 shipOffset, Globals.roomShape shape, Globals.roomType type, int w, int h)
        {
            #region room state machine setup
            roomStateMachine = new StateMachine();

            normal = new State { Name = "normal" };
            damaged = new State { Name = "damaged" };
            inoperable = new State { Name = "inoperable" };
            disabled = new State { Name = "disabled" };

            roomStateMachine.Start(normal);

            normal.Transitions.Add(damaged.Name, damaged);
            normal.Transitions.Add(disabled.Name, disabled);
            normal.Transitions.Add(inoperable.Name, inoperable);

            damaged.Transitions.Add(normal.Name, normal);
            damaged.Transitions.Add(inoperable.Name, inoperable);
            damaged.Transitions.Add(disabled.Name, disabled);

            inoperable.Transitions.Add(normal.Name, normal);
            inoperable.Transitions.Add(damaged.Name, damaged);
            inoperable.Transitions.Add(disabled.Name, disabled);

            disabled.Transitions.Add(normal.Name, normal);
            disabled.Transitions.Add(damaged.Name, damaged);
            disabled.Transitions.Add(inoperable.Name, inoperable);

            #endregion

            setupNormal();
            setupDamaged();
            setupInoperable();
            setupDisabled();

            position = new Vector2((x * 32) + shipOffset.X, (y * 32) + shipOffset.Y);
            roomTexture = texture;
            roomHighlightTexture = highlightTexture;
            sprite = new Drawable(highlightTexture, position);
            roomPosition = new Vector2(x, y);
            isMannable = new bool();
            isMannable = false;
            roomType = (int)type;
            roomHealth = 200;

            roomShape = shape;
            width = w;
            height = h;
            aflame = false;
            hullBreach = false;

            roomGrids = new List<int>();
        }

        public Room(SerializationInfo si, StreamingContext sc)
        {
            roomPosition = (Vector2)si.GetValue("roomPosition", typeof(Vector2));
            position = (Vector2)si.GetValue("position", typeof(Vector2));
            sprite = (Drawable)si.GetValue("sprite", typeof(Drawable));
            roomTexture = (Texture2D)si.GetValue("roomTexture", typeof(Texture2D));
            roomHighlightTexture = (Texture2D)si.GetValue("roomHighlightTexture", typeof(Texture2D));
            highlighted = si.GetBoolean("highlighted");
            isMannable = si.GetBoolean("isMannable");
            roomType = si.GetInt32("roomType");
            maxEnergy = si.GetInt32("maxEnergy");
            currentAvailableEnergy = si.GetInt32("currentAvailableEnergy");
            isManned = si.GetInt32("isManned");
            roomHealth = si.GetInt32("roomHealth");
            aflame = si.GetBoolean("aflame");
            hullBreach = si.GetBoolean("hullBreach");
            width = si.GetInt32("width");
            height = si.GetInt32("height");
            roomO2 = si.GetInt32("roomO2");
            roomShape = (Globals.roomShape)si.GetValue("roomShape", typeof(Globals.roomShape));
            typeOfRoom = (Globals.roomType)si.GetValue("typeOfRoom", typeof(Globals.roomType));
            roomSize = si.GetInt32("roomSize");
            roomGrids = (List<int>)si.GetValue("roomGrids", typeof(List<int>));
            roomStateMachine = (StateMachine)si.GetValue("roomStateMachine", typeof(StateMachine));
            normal = (State)si.GetValue("normal", typeof(State));
            damaged = (State)si.GetValue("damaged", typeof(State));
            inoperable = (State)si.GetValue("inoperable", typeof(State));
            disabled = (State)si.GetValue("disabled", typeof(State));

        }


        #endregion

        #region methods

        /// <summary>
        /// update the room
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

            Sprite.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// draw the room
        /// </summary>
        /// <param name="spriteBatch">main spriteBatch object</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (highlighted == true)
            {
                Sprite.SpriteTexture = roomHighlightTexture;
            }

            if (highlighted == false)
            {
                Sprite.SpriteTexture = roomTexture;
            }

            Sprite.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// highlight the room if its unhighlighted, unhighlight it if its highlighted
        /// </summary>
        public void Highlight()
        {
            if (highlighted == true)
            {
                highlighted = false;
            }

            else
            {
                highlighted = true;
            }
        }
        
        /// <summary>
        /// set up the normal game state
        ///  </summary>
        void setupNormal()
        {
            normal.enter += () =>
            {
            };

            normal.update += (GameTime gameTime) =>
            {
                if (currentAvailableEnergy < maxEnergy)
                {
                    roomStateMachine.Transition(damaged.Name);
                }

            };

            normal.leave += () =>
            {
            };
        }

        /// <summary>
        /// set up damaged game state
        /// </summary>
        void setupDamaged()
        {
            damaged.enter += () =>
            {
            };

            damaged.update += (GameTime gameTime) =>
            {
                if (currentAvailableEnergy == 0)
                {
                    roomStateMachine.Transition(inoperable.Name);
                }
                else if (currentAvailableEnergy == maxEnergy)
                {
                    roomStateMachine.Transition(normal.Name);
                }
            };

            damaged.leave += () =>
            {
            };
        }

        /// <summary>
        /// set up inoperable game state
        /// </summary>
        void setupInoperable()
        {
            inoperable.enter += () =>
            {
            };

            inoperable.update += (GameTime gameTime) =>
            {
                if (currentAvailableEnergy > 0)
                {
                    roomStateMachine.Transition(damaged.Name);
                }
            };

            inoperable.leave += () =>
            {
            };
        }

        /// <summary>
        /// set up disabled game state
        /// </summary>
        void setupDisabled()
        {
            disabled.enter += () =>
            {
            };

            disabled.update += (GameTime gameTime) =>
            {
                //decrement ion counter
                
                //check for exit
            };

            disabled.leave += () =>
            {
            };
        }

        public string getStatus()
        {
            return roomStateMachine.CurrentState.Name;
        }

        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("roomPosition", roomPosition, typeof(Vector2));
            si.AddValue("position", position, typeof(Vector2));
            si.AddValue("sprite", sprite, typeof(Drawable));
            si.AddValue("roomTexture", roomTexture, typeof(Texture2D));
            si.AddValue("roomHighlightTexture", roomHighlightTexture, typeof(Texture2D));
            si.AddValue("highlighted", highlighted);
            si.AddValue("isMannable", isMannable);
            si.AddValue("roomType", roomType);
            si.AddValue("maxEnergy", maxEnergy);
            si.AddValue("currentAvailableEnergy", currentAvailableEnergy);
            si.AddValue("isManned", isManned);
            si.AddValue("roomHealth", roomHealth);
            si.AddValue("aflame", aflame);
            si.AddValue("hullBreach", hullBreach);
            si.AddValue("width", width);
            si.AddValue("height", height);
            si.AddValue("roomO2", roomO2);
            si.AddValue("roomShape", roomShape, typeof(Globals.roomShape));
            si.AddValue("typeOfRoom", typeOfRoom, typeof(Globals.roomType));
            si.AddValue("roomSize", roomSize);
            si.AddValue("roomGrids", roomGrids, typeof(List<int>));
            si.AddValue("roomStateMachine", roomStateMachine, typeof(StateMachine));
            si.AddValue("normal", normal, typeof(State));
            si.AddValue("damaged", damaged, typeof(State));
            si.AddValue("inoperable", inoperable, typeof(State));
            si.AddValue("disabled", disabled, typeof(State));
        }


        #endregion



    }

    /// <summary>
    /// This contains info on system rooms that can be manned
    /// </summary>
    #region mannedRooms

    class engineRoom : Room
    {
        #region fields
        #endregion

        #region constructors / destructors
        public engineRoom()
        {
        }

        public engineRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            isMannable = new bool();
            isMannable = true;
            isManned = new int();
            isManned = 0;
            maxEnergy = currentAvailableEnergy = 2;
            roomType = (int)Globals.roomType.ENGINE_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class pilotRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public pilotRoom()
        {
        }

        public pilotRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            isMannable = new bool();
            isMannable = true;
            isManned = new int();
            isManned = 0;
            maxEnergy = currentAvailableEnergy = 1;
            roomType = (int)Globals.roomType.PILOT_ROOM;
        }
        #endregion

        #region methods
        
        #endregion
    }

    class shieldRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public shieldRoom()
        {
        }

        public shieldRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            isMannable = new bool();
            isMannable = true;
            isManned = new int();
            isManned = 0;
            maxEnergy = currentAvailableEnergy = 2;
            roomType = (int)Globals.roomType.SHIELD_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class weaponRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public weaponRoom()
        {
        }

        public weaponRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            isMannable = new bool();
            isMannable = true;
            isManned = new int();
            isManned = 0;
            maxEnergy = currentAvailableEnergy = 3;
            roomType = (int)Globals.roomType.WEAPONS_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }
    #endregion

    /// <summary>
    /// This contains info on system rooms that are unmanned
    /// </summary>
    #region unmannedRooms

    class cloakRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public cloakRoom()
        {
        }

        public cloakRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            maxEnergy = currentAvailableEnergy = 1;
            roomType = (int)Globals.roomType.CLOAK_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class doorRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public doorRoom()
        {
        }

        public doorRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            maxEnergy = currentAvailableEnergy = 1;
            roomType = (int)Globals.roomType.DOOR_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class droneRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public droneRoom()
        {
        }
        public droneRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            maxEnergy = currentAvailableEnergy = 1;
            roomType = (int)Globals.roomType.DRONE_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class medbayRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public medbayRoom()
        {
        }
        public medbayRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            maxEnergy = currentAvailableEnergy = 1;
            roomType = (int)Globals.roomType.MEDBAY_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class O2Room : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public O2Room()
        {
        }
        public O2Room(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            maxEnergy = currentAvailableEnergy = 1;
            roomType = (int)Globals.roomType.O2_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class sensorRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public sensorRoom()
        {
        }
        public sensorRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            maxEnergy = currentAvailableEnergy = 1;
            roomType = (int)Globals.roomType.SENSORS_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class teleporterRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public teleporterRoom()
        {
        }
        public teleporterRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            maxEnergy = currentAvailableEnergy = 1;
            roomType = (int)Globals.roomType.TELEPORTER_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    #endregion
}
