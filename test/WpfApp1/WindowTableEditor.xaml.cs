using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
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
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для WindowTableEditor.xaml
    /// </summary>
    public partial class WindowTableEditor : Window
    {
        public MyDB myDB;
        public WindowTableEditor()
        {
            InitializeComponent();
        }

        private void WindowTableEditor_Cloasing(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => myDB.updateOrNewRow((DataRowView) e.Row.DataContext)), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                myDB.deleteRow((DataRowView)dataGrid.SelectedItem);
            }
        }
    }
}
