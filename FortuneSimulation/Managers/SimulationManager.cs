using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static FortuneSimulation.MainWindow;
using static FortuneSimulation.Managers.MapManager;
using static FortuneSimulation.Managers.NPCManager;
using static FortuneSimulation.Managers.DataViewManager;

namespace FortuneSimulation.Managers
{
    class SimulationManager
    {
        public static int SimulationRound = 0;

        private static BackgroundWorker SimulationRoutine = new BackgroundWorker();
        public static void MainRoutineSetup()
        {
            SimulationRoutine.DoWork += new DoWorkEventHandler(SimulationRoutine_doWork);
            SimulationRoutine.ProgressChanged += new ProgressChangedEventHandler(SimulationRoutine_ProgressChanged);
            SimulationRoutine.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SimulationRoutine_WorkerCompleted);
            SimulationRoutine.WorkerReportsProgress = true;
            SimulationRoutine.WorkerSupportsCancellation = true;
        }

        public static void NewSimulationSetup()
        {
            PauseSimulation();

            if (mMainWindow.Grid_map.Children.Count > 0)
                mMainWindow.Grid_map.Children.Clear();
            if(SimulationRound > 0)
                mMainWindow.ListViewHolder.Children.Clear();
            
            SimulationRound = 0;
            mListNPC = new List<Modules.NPC>();

            Money_stack_data_arr = new int[MAP_SIZE.width, MAP_SIZE.height];
            Money_abs_data_arr = new int[MAP_SIZE.width, MAP_SIZE.height];

            Random rand = new Random();
            GenerateBank(mMainWindow.Grid_map, rand.Next(2, 7), 
                new Point(rand.Next(0, MAP_SIZE.width), rand.Next(0, MAP_SIZE.width)));
            GenerateBank(mMainWindow.Grid_map, rand.Next(3, 8),
                new Point(rand.Next(0, MAP_SIZE.width), rand.Next(0, MAP_SIZE.width)));
            GenerateBank(mMainWindow.Grid_map, rand.Next(5, 10), 
                new Point(rand.Next(0, MAP_SIZE.width), rand.Next(0, MAP_SIZE.width)));

            GenerateNPCs(mMainWindow.Grid_map, Convert.ToInt32(mMainWindow.TB_npcs.Text));

            ListView view = new ListView();
            CreateGridView(view, mListNPC);
            mMainWindow.ListViewHolder.Children.Add(view);
        }

        public static void StartSimulation(int rounds)
        {
            if (!SimulationRoutine.IsBusy)
                SimulationRoutine.RunWorkerAsync(rounds);
        }

        public static void PauseSimulation()
        {
            if (SimulationRoutine.IsBusy)
                SimulationRoutine.CancelAsync();
        }

        private static void SimulationRoutine_doWork(object sender, DoWorkEventArgs e)
        {
            while (!SimulationRoutine.CancellationPending && SimulationRound < (int)e.Argument)
            {
                UpdateNPCAndBankData();
                SimulationRoutine.ReportProgress(0);
                Thread.Sleep(2000);
                SimulationRound++;
            }
        }

        private static void SimulationRoutine_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            mMainWindow.lbl_round.Content = "Current Day: " + SimulationRound;
            mMainWindow.Grid_map.Children.Clear();

            // Create new resource
            if (SimulationRound % 20 == 0)
            {
                Random rand = new Random();
                GenerateBank(mMainWindow.Grid_map, rand.Next(3, 8), 
                    new Point(rand.Next(0, MAP_SIZE.width), rand.Next(0, MAP_SIZE.width)));
            }

            UpdateBank(mMainWindow.Grid_map, Money_stack_data_arr);
            UpdateNPCUI(mMainWindow.Grid_map, mListNPC);

            mMainWindow.ListViewHolder.Children.Clear();
            ListView view = new ListView();
            CreateGridView(view, mListNPC);
            mMainWindow.ListViewHolder.Children.Add(view);
        }

        private static void SimulationRoutine_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}
