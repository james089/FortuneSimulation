using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FortuneSimulation.Managers;

namespace FortuneSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mMainWindow = null;
        public MainWindow()
        {
            mMainWindow = this;
            InitializeComponent();

            SimulationManager.MainRoutineSetup();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Init grid
            for (int i = 0; i < MapManager.MAP_SIZE.width; i++)
            {
                ColumnDefinition col1 = new ColumnDefinition();
                ColumnDefinition col2 = new ColumnDefinition();
                col1.Width = new GridLength(MapManager.GRID_W);
                col2.Width = new GridLength(MapManager.GRID_W);
                Grid_map.ColumnDefinitions.Add(col1);
                Grid_borders.ColumnDefinitions.Add(col2);

                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();
                row1.Height = new GridLength(MapManager.GRID_H);
                row2.Height = new GridLength(MapManager.GRID_H);
                Grid_map.RowDefinitions.Add(row1);
                Grid_borders.RowDefinitions.Add(row2);
            }

            for (int i = 0; i < MapManager.MAP_SIZE.width; i++)
            {
                for (int j = 0; j < MapManager.MAP_SIZE.height; j++)
                {
                    Border border = new Border();
                    border.Width = MapManager.GRID_W;
                    border.Height = MapManager.GRID_H;
                    border.BorderThickness = new Thickness(0.5);
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 220, 220));

                    Grid_borders.Children.Add(border);
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, j);
                }
            }

        }

        private void Btn_1step_Click(object sender, RoutedEventArgs e)
        {
            SimulationManager.StartSimulation(SimulationManager.SimulationRound + 1);
        }

        //private void UpdateUI()
        //{
        //    SimulationManager.SimulationRound++;
        //    lbl_round.Content = "Current Day: " + SimulationManager.SimulationRound;

        //    NPCManager.UpdateNPCAndBankData();

        //    Grid_map.Children.Clear();

        //    MapManager.UpdateBank(Grid_map, MapManager.Money_stack_data_arr);
        //    NPCManager.UpdateNPCUI(Grid_map, NPCManager.mListNPC);

        //    ListViewHolder.Children.Clear();
        //    ListView view = new ListView();
        //    DataViewManager.CreateGridView(view, NPCManager.mListNPC);
        //    ListViewHolder.Children.Add(view);
        //}

        private void Btn_run_Click(object sender, RoutedEventArgs e)
        {
            Btn_run.IsEnabled = !Btn_run.IsEnabled;
            Btn_pause.IsEnabled = !Btn_run.IsEnabled;
            SimulationManager.StartSimulation(Convert.ToInt32(TB_targetRounds.Text));
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            SimulationManager.NewSimulationSetup();
        }

        private void Btn_pause_Click(object sender, RoutedEventArgs e)
        {
            Btn_pause.IsEnabled = !Btn_pause.IsEnabled;
            Btn_run.IsEnabled = !Btn_pause.IsEnabled;
            SimulationManager.PauseSimulation();
        }
    }
}
