using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceIsFun
{
    static class constants
    {
        /// <summary>
        /// Global constants
        /// </summary>
        /// 


        #region roomtypeconsts

        public const int EMPTY_ROOM = 0;
        public const int CLOAK_ROOM = 1;
        public const int DOOR_ROOM = 2;
        public const int DRONE_ROOM = 3;
        public const int ENGINE_ROOM = 4;
        public const int MEDBAY_ROOM = 5;
        public const int O2_ROOM = 6;
        public const int PILOT_ROOM = 7;
        public const int SENSORS_ROOM = 8;
        public const int SHIELD_ROOM = 9;
        public const int TELEPORTER_ROOM = 10;
        public const int WEAPONS_ROOM = 11;

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
        public enum roomShape { TwoXOne, TwoXTwo, ThreeXOne, ThreeXThree, OneXTwo, OneXThree, RRoom, JRoom };
        #endregion
    }
}
