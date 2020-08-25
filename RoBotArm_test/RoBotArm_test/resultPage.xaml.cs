using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace RoBotArm_test
{
    /// <summary>
    /// resultPage.xaml 的互動邏輯
    /// </summary>
    public partial class resultPage : System.Windows.Window
    {
        Mysql mysql = new Mysql();
        string bicycle_id;
        System.Data.DataTable dt;
        public resultPage()
        {
            InitializeComponent();
            bicycle_id = mysql.get_Static_Bicycle_Id();
            MessageBox.Show(bicycle_id.ToString());
            resultDatagrid.DataContext = mysql.mysql_Select_SearchResult(bicycle_id);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true; //www.yazilimkodlama.com
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < resultDatagrid.Columns.Count; j++) //Başlıklar için
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true; //Başlığın Kalın olması için
                sheet1.Columns[j + 1].ColumnWidth = 15; //Sütun genişliği ayarı
                myRange.Value2 = resultDatagrid.Columns[j].Header;
            }
            for (int i = 0; i < resultDatagrid.Columns.Count; i++)
            { //www.yazilimkodlama.com
                for (int j = 0; j < resultDatagrid.Items.Count; j++)
                {
                    TextBlock b = resultDatagrid.Columns[i].GetCellContent(resultDatagrid.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }
    }
}
