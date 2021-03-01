using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;


namespace chartTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadButton_Click(null, null);

            Random r = new Random();
            for (int i = 0; i < rnd.Length; i++)
            {
                rnd[i] = new int[1000];
                for (int j = 0; j < 1000; j++)
                {
                    rnd[i][j] = r.Next(10000);
                }
            }

            //test
            for (int i = 0; i < rnd.Length; i++)
            {
                textBox1.Text += rnd[i] + "\r\n";
                for (int j = 0; j < 1000; j++)
                {
                    textBox1.Text += Convert.ToString(rnd[i][j]) + " ";
                }
                textBox1.Text += "\r\n";
            }
        }

        int[][] rnd = new int[10][];
        

        private void loadButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                featureCheckedListBox.Items.Add($"A{i}");
            }
        }

        public static Chart CreatChart(string chartName, int xPoint, int yPoint, int xSize, int ySize)
        {
            Chart newChart = new Chart();
            ChartArea newChartArea = new ChartArea();
            Legend newLegend = new Legend();
            Series newSeries = new Series();

            ((System.ComponentModel.ISupportInitialize)(newChart)).BeginInit();

            newChartArea.Name = "ChartArea1";
            newChart.ChartAreas.Add(newChartArea);

            newLegend.Name = "Legend1";
            newChart.Legends.Add(newLegend);

            newChart.Name = chartName;
            newSeries.ChartArea = "ChartArea1";
            newSeries.ChartType = SeriesChartType.Line;
            newSeries.Legend = "Legend1";
            newSeries.Name = "Series1";
            newChart.Series.Add(newSeries);

            newChart.Location = new Point(xPoint, yPoint);
            newChart.Size = new Size(xSize, ySize);
            newChart.TabIndex = 0;

            return newChart;
        }


        Chart[] chartsList = new Chart[10];
        int[] atFlowChartIndex = new int[10];  //chart 在 flowLayoutPanel 中的位置
        private void featureCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!featureCheckedListBox.GetItemChecked(e.Index))
            {
                string checkStr = featureCheckedListBox.Items[e.Index].ToString();

                chartsList[e.Index] = CreatChart("chart1", 0, 0, flowLayoutPanel1.Width, 100);
                //add data to chart here
                chartsList[e.Index].Titles.Add(checkStr);
                chartsList[e.Index].Titles[0].Text = checkStr;

                flowLayoutPanel1.Controls.Add(chartsList[e.Index]);
                flowLayoutPanel1.ScrollControlIntoView(chartsList[e.Index]);

                //紀錄未來要移除的位置
                atFlowChartIndex[e.Index] = flowLayoutPanel1.Controls.Count - 1;
            }
            else
            {
                int removeIndex = atFlowChartIndex[e.Index];  //要移除的 chart 在 flowLayoutPanel 中的位置

                flowLayoutPanel1.Controls.RemoveAt(removeIndex);
                //清空
                chartsList[e.Index] = null;
                atFlowChartIndex[e.Index] = 0;

                //處理移除後 其他 chart 在 flowLayoutPanel 中順序變化
                for (int i = 0; i < atFlowChartIndex.Length; i++)
                {
                    if (atFlowChartIndex[i] >= removeIndex)  //在被移除 chart 的下面 index 全部往上
                    {
                        atFlowChartIndex[i] -= 1;
                    }
                }
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            //flowLayoutPanel1.Controls.Clear();  //會呼叫 featureCheckedListBox_ItemCheck 然後執行原本的清除條件
            foreach (var i in featureCheckedListBox.CheckedIndices)
            {
                textBox1.Text += Convert.ToString(i) + "\r\n";
                featureCheckedListBox.SetItemChecked((int)i, false);
            }
            
        }
    }
}
