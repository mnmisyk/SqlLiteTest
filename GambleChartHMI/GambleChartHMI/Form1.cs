using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Windows.Forms.DataVisualization.Charting;

namespace GambleChartHMI
{
    public partial class Form1 : Form
    {
        string DataName;
        SQLiteConnection MyConnection;
        List<string> timeList = new List<string>();
        List<int> valueList = new List<int>();
        List<int> VectorList = new List<int>();
        public Form1()
        {
            InitializeComponent();
        }

        private void getDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int vector=0;
            timeList.Clear();
            valueList.Clear();
            MyConnection= new SQLiteConnection(@"Data Source="+ DataName+"; Version=3;");
            MyConnection.Open();
            string sql = @"select * from MyDB";
            SQLiteCommand cmd = new SQLiteCommand(MyConnection);
            SQLiteDataReader reader = (SQLiteDataReader)SQLiteHelper.ExecuteReader(cmd, sql, null);
            while (reader.Read())
            {
                timeList.Add(reader[1].ToString());
                valueList.Add(Convert.ToInt16(reader[2]));
                vector = vector + Convert.ToInt16(reader[2]);
                VectorList.Add(vector);
            }

        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Zoom into the X axis
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(2, 3);

            // Enable range selection and zooming end user interface
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            //将滚动内嵌到坐标轴中
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 10;

            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;

            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 10;
            //----------------------------------------------------------------------------------------------------------

            chart1.ChartAreas[1].AxisX.ScaleView.Zoom(2, 3);

            // Enable range selection and zooming end user interface
            chart1.ChartAreas[1].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[1].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[1].AxisX.ScaleView.Zoomable = true;

            //将滚动内嵌到坐标轴中
            chart1.ChartAreas[1].AxisX.ScrollBar.IsPositionedInside = true;

            // 设置滚动条的大小
            chart1.ChartAreas[1].AxisX.ScrollBar.Size = 10;

            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart1.ChartAreas[1].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;

            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[1].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[1].AxisX.ScaleView.SmallScrollMinSize = 10;

            chart1.Series[0].Color = Color.Red;
            chart1.Series[1].ChartArea = chart1.ChartAreas[1].Name;
            chart1.ChartAreas[1].Visible = true;
            chart1.Series[0].Points.DataBindXY(timeList, valueList);
            chart1.Series[1].Points.DataBindXY(timeList, VectorList);
            int D = 0, T = 0, P = 0,A=0,B=0;
            foreach (var item in valueList)
            {
               
                if (item==-1)
                {
                    T++;
                }
                else if (item==0)
                {
                    P++;
                }
                else if (item==1)
                {
                    D++;
                }
            }

            MessageBox.Show("龙：" + D +" 虎:"+T+" 和:"+ P);
            for (int i = 0; i < valueList.Count-5; i++)
            {
                if (valueList[i]==1 && valueList[i+1] == 1 && valueList[i+2] == 1 && valueList[i + 3] == 1 && valueList[i + 4] == -1 && valueList[i + 5] == 1)
                {
                    B++;
                    Console.WriteLine("B" + timeList[i]);
                }
                else if (valueList[i] == 1 && valueList[i + 1] == 1 && valueList[i + 2] == 1 && valueList[i + 3] == 1 && valueList[i + 4] == -1 && valueList[i + 5] == -1)
                {
                    A++;
                    Console.WriteLine("A" + timeList[i]);
                }
            }
            textBoxA.Text = A.ToString();
            textBoxB.Text = B.ToString();
        }

        private void selectDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                DataName = ofd.FileName;
            }
                
        }
    }
}
