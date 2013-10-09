using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceIsFun
{
    /// <summary>
    /// This contains all the info of a room on a ship 
    /// </summary>
    public partial class Room : Entity
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
        /// Define the shape of the room
        /// </summary>
        protected constants.roomShape rShape;

        public constants.roomShape RShape
        {
            get
            {
                return rShape;
            }
            set
            {
                rShape = value;
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
        public Room(Texture2D texture, Texture2D highlightTexture, int x, int y, constants.roomShape shape, int w, int h)
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

            position = new Vector2(x * 32, y * 32);
            roomTexture = texture;
            roomHighlightTexture = highlightTexture;
            roomPosition = new Vector2(x, y);
            isMannable = new bool();
            isMannable = false;
            roomType = constants.EMPTY_ROOM;
            roomHealth = 200;

            rShape = shape;
            width = w;
            height = h;
            aflame = false;
            hullBreach = false;
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
            maxEnergy = currentAvailableEnergy = 2;
            roomType = constants.ENGINE_ROOM;
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
            maxEnergy = currentAvailableEnergy = 1;
            roomType = constants.PILOT_ROOM;
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
            maxEnergy = currentAvailableEnergy = 2;
            roomType = constants.SHIELD_ROOM;
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
            maxEnergy = currentAvailableEnergy = 3;
            roomType = constants.WEAPONS_ROOM;
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
            roomType = constants.CLOAK_ROOM;
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
            roomType = constants.DOOR_ROOM;
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
            roomType = constants.DRONE_ROOM;
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
            roomType = constants.MEDBAY_ROOM;
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
            roomType = constants.O2_ROOM;
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
            roomType = constants.SENSORS_ROOM;
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
            roomType = constants.TELEPORTER_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }

    #endregion
}
