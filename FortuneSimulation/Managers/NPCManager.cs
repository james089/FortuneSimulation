using FortuneSimulation.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FortuneSimulation.Managers
{
    public class NPCManager
    {
        public static List<NPC> mListNPC = new List<NPC>();
        public static void GenerateNPCs(Grid grid, int num_npc)
        {
            Random rand = new Random();
            if (num_npc == 1)
            {
                NPC m = new NPC();
                m.index = 0;
                m.name = nameCreator(rand.Next(0, NPC.LastName.Count()), rand.Next(0, NPC.FirstName.Count()));
                m.consume_rate = 20;
                m.earn_rate = 700;
                m.initial_money = 10000;
                m.initial_loc = new NPC.Location(10, 20);
                m.status = NPC.Status.start;
                m.view_size = 5;
                m.current_money = m.initial_money;
                m.current_loc = m.initial_loc;
                m.lust_rate = 3;
                m.skill_improve_rate = 15;
                m.die_round = 0;

                m.type = "芸芸大众";
                mListNPC.Add(m);
            }
            else
            {
                // generate data
                for (int i = 0; i < num_npc; i++)
                {
                    NPC m = new NPC();
                    m.index = i;
                    m.name = nameCreator(rand.Next(0, NPC.LastName.Count()), rand.Next(0, NPC.FirstName.Count()));
                    m.consume_rate = 10 + rand.Next(0, 10) * 10;
                    m.earn_rate = 10 + rand.Next(0, 10) * 10;
                    m.initial_money = rand.Next(2, 200) * 50;
                    m.initial_loc = new NPC.Location(rand.Next(0, MapManager.MAP_SIZE.width - 1), rand.Next(0, MapManager.MAP_SIZE.height - 1));
                    m.status = NPC.Status.start;
                    m.view_size = rand.Next(1, 6);
                    m.current_money = m.initial_money;
                    m.current_loc = m.initial_loc;
                    m.lust_rate = NPC.LustRateList[rand.Next(0, NPC.LustRateList.Count())];
                    m.skill_improve_rate = NPC.SkillImproveRateList[rand.Next(0, NPC.SkillImproveRateList.Count())];
                    m.die_round = 0;

                    m.type = "";
                    if (m.initial_money > 9000)
                    {
                        m.type += "富二代";
                    }
                    else if (m.initial_money < 1000)
                    {
                        m.type += "草根";
                    }

                    if (m.earn_rate > 70 && m.skill_improve_rate >= NPC.SkillImproveRateList[4])
                    {
                        m.type += "天才";
                    }
                    else if (m.earn_rate > 70 && m.skill_improve_rate < NPC.SkillImproveRateList[1])
                    {
                        m.type += "神童";
                    }
                    else if (m.earn_rate < 20 && m.skill_improve_rate >= NPC.SkillImproveRateList[4])
                    {
                        m.type += "奋斗狂";
                    }
                    else if (m.earn_rate < 20 && m.skill_improve_rate < NPC.SkillImproveRateList[1])
                    {
                        m.type += "傻逼";
                    }

                    if (m.consume_rate > 70 && (m.lust_rate >= NPC.LustRateList[4] ||
                        (float)m.lust_rate / m.skill_improve_rate > 5))
                    {
                        m.type += "败家狂";
                    }
                    else if (m.consume_rate <= 70 && m.lust_rate >= NPC.LustRateList[4] || 
                        (float)m.lust_rate / m.skill_improve_rate > 5)
                    {
                        m.type += "成瘾者";
                    }

                    if ((float)m.lust_rate / m.skill_improve_rate > 0.8 && (float)m.lust_rate / m.skill_improve_rate < 1.3)
                    {
                        m.type += "月光族";
                    }

                    if (m.consume_rate < 20 && m.lust_rate < NPC.LustRateList[1])
                    {
                        m.type += "持家者";
                    }
                    if (m.type == "")
                    {
                        m.type = "芸芸大众";
                    }

                    mListNPC.Add(m);
                }
            }
            

            // generate inital NPCs 
            UpdateNPCUI(grid, mListNPC);
        }

        public static void UpdateNPCAndBankData()
        {
            foreach (NPC npc in mListNPC)
            {
                if (npc.status == NPC.Status.dead)
                    continue;

                // Spend money
                npc.current_money -= npc.consume_rate;
                if (npc.current_money <= 0)
                {
                    npc.status = NPC.Status.dead;
                    npc.die_round = SimulationManager.SimulationRound;
                    continue;
                }
                // Prepare to look for money
                npc.mDecisionList.Clear();
                int max_money = 0;
                // Find max view area to check if there is money, go to the location that has max money
                for (int m = GF.CheckBoundary(npc.current_loc.X - (int)npc.view_size, GF.Axis.x);
                    m < GF.CheckBoundary(npc.current_loc.X + (int)npc.view_size, GF.Axis.y);
                    m++)
                {
                    for (int n = GF.CheckBoundary(npc.current_loc.Y - (int)npc.view_size, GF.Axis.y);
                        n < GF.CheckBoundary(npc.current_loc.Y + (int)npc.view_size, GF.Axis.y);
                        n++)
                    {
                        if (MapManager.Money_abs_data_arr[m, n] > max_money)
                        {
                            max_money = MapManager.Money_abs_data_arr[m, n];
                            npc.mDecisionList.Add(new NPC.Decision(m, n, max_money));
                        }
                    }
                }
                // if he/she found some money...
                if (max_money > 0)
                {
                    npc.status = NPC.Status.working;
                    //After search for max value, the last value in mDecisionList is the max value
                    int max_x = npc.mDecisionList[npc.mDecisionList.Count() - 1].X;
                    int max_y = npc.mDecisionList[npc.mDecisionList.Count() - 1].Y;
                    // NPC move there
                    npc.current_loc = new NPC.Location(max_x, max_y);
                    // Consume bank
                    MapManager.Money_abs_data_arr[max_x, max_y] -= npc.earn_rate;
                    // If this money stack is empty
                    if (MapManager.Money_abs_data_arr[max_x, max_y] < 0)
                    {
                        MapManager.Money_abs_data_arr[max_x, max_y] = 0;
                        MapManager.Money_stack_data_arr[max_x, max_y] = 0;
                    }
                    // If after giving money to npc, it has 1 less stack, update stack data
                    else if (MapManager.Money_abs_data_arr[max_x, max_y] / MapManager.MONEY_PER_STACK
                        < MapManager.Money_stack_data_arr[max_x, max_y] - 1)
                    {
                        MapManager.Money_stack_data_arr[max_x, max_y]--;
                    }
                    // NPC get money
                    npc.current_money += npc.earn_rate;
                    // In next round
                    npc.earn_rate = (int)(npc.earn_rate + npc.skill_improve_rate);
                    npc.consume_rate = (int)(npc.consume_rate + npc.lust_rate);
                }
                // Move to a random location within his/her view field
                else
                {
                    // consume rate drop because of idle
                    npc.consume_rate = (npc.consume_rate - 10 > 0 ? npc.consume_rate - 1 : (10 + npc.lust_rate));
                    npc.status = NPC.Status.moving;
                    // Randomly go to 1 location
                    Random rand = new Random();
                    int new_x = (int)npc.current_loc.X, new_y = (int)npc.current_loc.Y;
                    switch (rand.Next(1, 5))
                    {
                        // go to left column
                        case 1:
                            new_x = GF.CheckBoundary(npc.current_loc.X - (int)npc.view_size, GF.Axis.x);
                            new_y = rand.Next(GF.CheckBoundary(npc.current_loc.Y - (int)npc.view_size, GF.Axis.y),
                                              GF.CheckBoundary(npc.current_loc.Y + (int)npc.view_size, GF.Axis.y));
                            break;

                        // go to right column
                        case 2:
                            new_x = GF.CheckBoundary(npc.current_loc.X + (int)npc.view_size, GF.Axis.x);
                            new_y = rand.Next(GF.CheckBoundary(npc.current_loc.Y - (int)npc.view_size, GF.Axis.y),
                                              GF.CheckBoundary(npc.current_loc.Y + (int)npc.view_size, GF.Axis.y));
                            break;

                        // go to top row
                        case 3:
                            new_x = rand.Next(GF.CheckBoundary(npc.current_loc.X - (int)npc.view_size, GF.Axis.x),
                                              GF.CheckBoundary(npc.current_loc.X + (int)npc.view_size, GF.Axis.x));

                            new_y = GF.CheckBoundary(npc.current_loc.Y - (int)npc.view_size, GF.Axis.y);
                            break;

                        // go to bottom row
                        case 4:
                            new_x = rand.Next(GF.CheckBoundary(npc.current_loc.X - (int)npc.view_size, GF.Axis.x),
                                              GF.CheckBoundary(npc.current_loc.X + (int)npc.view_size, GF.Axis.x));

                            new_y = GF.CheckBoundary(npc.current_loc.Y + (int)npc.view_size, GF.Axis.y);
                            break;
                    }
                    npc.current_loc = new NPC.Location(new_x, new_y);
                }
                // Locate 1 npc
                //if (npc.current_loc.X == i && npc.current_loc.Y == j)
                //{

                //}
                //for (int i = 0; i < MapManager.MAP_SIZE.width; i++)
                //{
                //    for (int j = 0; j < MapManager.MAP_SIZE.height; j++)
                //    {


                //    }
                //}
            }
        }

        public static void UpdateNPCUI(Grid grid, List<NPC> npcList)
        {
            for (int i = 0; i < MapManager.MAP_SIZE.width; i++)
            {
                for (int j = 0; j < MapManager.MAP_SIZE.height; j++)
                {
                    foreach (NPC n in npcList)
                    {
                        if (n.current_loc.X == i && n.current_loc.Y == j)
                        {
                            TextBlock NPCB = new TextBlock();
                            NPCB.Width = 16;
                            NPCB.Height = 16;
                            NPCB.Text = n.index.ToString();
                            NPCB.HorizontalAlignment = HorizontalAlignment.Center;
                            NPCB.VerticalAlignment = VerticalAlignment.Center;
                            switch (n.status)
                            {
                                case NPC.Status.start:
                                    NPCB.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 255));
                                    break;
                                case NPC.Status.moving:
                                    NPCB.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 255));
                                    break;
                                case NPC.Status.working:
                                    NPCB.Background = new SolidColorBrush(Color.FromArgb(200, 0, 255, 0));
                                    break;
                                case NPC.Status.dead:
                                    NPCB.Background = new SolidColorBrush(Color.FromArgb(200, 180, 180, 180));
                                    break;
                            }
                            NPCB.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

                            grid.Children.Add(NPCB);
                            Grid.SetColumn(NPCB, i);
                            Grid.SetRow(NPCB, j);
                        }
                    }
                }
            }
        }

        private static string nameCreator(int lastName, int firstName)
        {
            Random rand = new Random();

            int a = rand.Next(2, 6);

            if (firstName % a == 0)
            {
                return NPC.LastName[lastName] + NPC.FirstName[firstName] + 
                    NPC.FirstName[firstName + a < NPC.FirstName.Count() - 1? a : NPC.FirstName.Count() - 1];
            }
            else
            {
                return NPC.LastName[lastName] + NPC.FirstName[firstName];
            }
        }

    }
}
