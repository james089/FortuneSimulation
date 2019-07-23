using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FortuneSimulation.Modules
{
    public class NPC
    {
        public static List<string> LastName = new List<string>()
        {
            "曹",
            "皮",
            "姑苏",
            "幽蓝",
            "母",
            "楼",
            "小",
            "大",
            "妩媚",
            "老",
            "射",
            "粉",
            "慕容",
            "李",
            "凤舞",
            "羊",
            "独孤",
            "含笑",
        };

        public static List<string> FirstName = new List<string>()
        {
            "清",
            "兰",
            "贱",
            "渡",
            "娇",
            "若梦",
            "鱼",
            "毛",
            "三",
            "煞",
            "珊",
            "甜",
            "苑",
            "路",
            "溪",
            "兰",
            "玉",
            "坨",
            "肥",
            "水",
            "便",
            "猪",
            "伞",
            "辣",
            "子",
            "鸡",
            "土",
            "汤",
            "媛",
        };

        public static List<int> LustRateList = new List<int>()
        {
            1,
            3,
            6,
            9,
            15
        };

        public static List<int> SkillImproveRateList = new List<int>()
        {
            2,
            4,
            7,
            10,
            16
        };

        public static List<int> ViewSizeList = new List<int>()
        {
            1,
            3,
            5,
        };
        
        public enum Status
        {
            start,
            moving,
            working,
            dead
        }

        public class  Location
        {
            public int X, Y;
            public Location(int _x, int _y)
            {
                X = _x;
                Y = _y;
            }
        }
        /// <summary>
        /// Decide where to go
        /// </summary>
        public class Decision
        {
            public int X, Y;
            public int value;
            public Decision(int _x, int _y, int _value)
            {
                X = _x;
                Y = _y;
                value = _value;
            }
        }

        public int index { get; set; }
        public string name { get; set; }
        public string type { get; set; }            // What kind of person is this guy
        public int initial_money { get; set; }   // start with this amount from his/her parents
        public Location initial_loc { get; set; }
        public int consume_rate { get; set; }    // how much $ consumed per day
        public int earn_rate { get; set; }        // how much $ earned per day
        public int view_size { get; set; }
        public Status status { get; set; }
        /// <summary>
        /// Defines how much more money a npc will spend when he find some money
        /// </summary>
        public int lust_rate { get; set; }
        /// <summary>
        /// Defines how much more money a npc will earn when he find some money
        /// </summary>
        public int skill_improve_rate { get; set; }

        public int current_money { get; set; }
        public Location current_loc { get; set; }
        public int die_round { get; set; }

        public List<Decision> mDecisionList = new List<Decision>();


    }
}
