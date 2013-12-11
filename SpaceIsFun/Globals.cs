using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceIsFun
{
    public static class Globals
    {
        /// <summary>
        /// Global constants
        /// </summary>
        /// 


        #region room types

        public enum roomType
        {
            EMPTY_ROOM,
            CLOAK_ROOM,
            DOOR_ROOM,
            DRONE_ROOM, 
            ENGINE_ROOM, 
            MEDBAY_ROOM, 
            O2_ROOM, 
            PILOT_ROOM, 
            SENSORS_ROOM, 
            SHIELD_ROOM, 
            TELEPORTER_ROOM,
            WEAPONS_ROOM 
        };

        #endregion

        #region skilllevelconsts
        public const int SKILL_NONE = 0;
        public const int SKILL_1 = 1;
        public const int SKILL_2 = 2;
        public const int SKILL_3 = 3;
        #endregion

        #region weapontypeconsts
        #endregion

        #region dronetypeconsts
        #endregion

        #region eventconsts
        #endregion

        #region enumerators
        public enum roomShape 
        { 
            TwoXOne, 
            TwoXTwo, 
            ThreeXOne, 
            ThreeXThree, 
            OneXTwo, 
            OneXThree, 
            RRoom, 
            JRoom 
        };
        #endregion
    }
}
