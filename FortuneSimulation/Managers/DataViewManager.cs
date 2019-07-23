using FortuneSimulation.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace FortuneSimulation.Managers
{
    class DataViewManager
    {
        public static void CreateGridView(ListView listView, List<NPC> listNpc)
        {
            listView.ItemsSource = listNpc;

            GridView grdView = new GridView();
            grdView.AllowsColumnReorder = true;
            GridViewColumn indexCol = new GridViewColumn();
            GridViewColumn nameCol = new GridViewColumn();
            GridViewColumn initMoneyCol = new GridViewColumn();
            GridViewColumn viewCol = new GridViewColumn();
            GridViewColumn consumeRateCol = new GridViewColumn();
            GridViewColumn earnRateCol = new GridViewColumn();
            GridViewColumn lustRateCol = new GridViewColumn();
            GridViewColumn skillImproveRateCol = new GridViewColumn();
            GridViewColumn currentMoneyCol = new GridViewColumn();
            GridViewColumn typeCol = new GridViewColumn();
            GridViewColumn dieCol = new GridViewColumn();

            indexCol.Width = 30;
            indexCol.DisplayMemberBinding = new Binding("index");
            indexCol.Header = "#";

            nameCol.Width = 80;
            nameCol.DisplayMemberBinding = new Binding("name");
            nameCol.Header = "姓名";

            typeCol.Width = 120;
            typeCol.DisplayMemberBinding = new Binding("type");
            typeCol.Header = "类别";

            initMoneyCol.Width = 80;
            initMoneyCol.DisplayMemberBinding = new Binding("initial_money");
            initMoneyCol.Header = "初始财富";

            viewCol.Width = 60;
            viewCol.DisplayMemberBinding = new Binding("view_size");
            viewCol.Header = "视野";

            currentMoneyCol.Width = 80;
            currentMoneyCol.DisplayMemberBinding = new Binding("current_money");
            currentMoneyCol.Header = "目前财富";

            consumeRateCol.Width = 80;
            consumeRateCol.DisplayMemberBinding = new Binding("consume_rate");
            consumeRateCol.Header = "花钱速度";

            earnRateCol.Width = 80;
            earnRateCol.DisplayMemberBinding = new Binding("earn_rate");
            earnRateCol.Header = "赚钱速度";

            lustRateCol.Width = 80;
            lustRateCol.DisplayMemberBinding = new Binding("lust_rate");
            lustRateCol.Header = "贪婪度";

            skillImproveRateCol.Width = 80;
            skillImproveRateCol.DisplayMemberBinding = new Binding("skill_improve_rate");
            skillImproveRateCol.Header = "能力提升";

            dieCol.Width = 50;
            dieCol.DisplayMemberBinding = new Binding("die_round");
            dieCol.Header = "死亡";


            grdView.Columns.Add(indexCol);
            grdView.Columns.Add(nameCol);
            grdView.Columns.Add(typeCol);
            grdView.Columns.Add(viewCol);
            grdView.Columns.Add(initMoneyCol);
            grdView.Columns.Add(earnRateCol);
            grdView.Columns.Add(skillImproveRateCol);
            grdView.Columns.Add(consumeRateCol);
            grdView.Columns.Add(lustRateCol);
            grdView.Columns.Add(currentMoneyCol);
            grdView.Columns.Add(dieCol);

            listView.View = grdView;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("current_money", ListSortDirection.Descending));
        }
    }
}
