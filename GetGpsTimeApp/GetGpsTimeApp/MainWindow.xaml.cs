using Microsoft.WindowsAPICodePack.Sensors;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GetGpsTimeApp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        private static Guid SENSOR_DATA_TYPE_LOCATION_GUID = new Guid("055C74D8-CA6F-47D6-95C6-1ED3637A0FF4");

        private Sensor GeolocationSensor;

        public MainWindow()
        {
            InitializeComponent();
            GeolocationSensor = SensorManager.GetSensorsByTypeId(SensorTypes.LocationGps)[0];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GeolocationSensor.DataReportChanged += DataReportChanged;
            button.IsEnabled = false;
        }

        private void DataReportChanged(Sensor sender, EventArgs e)
        {
            try
            {
                foreach (Guid formatId in sender.DataReport.Values.Keys)
                {
                    if (formatId == SENSOR_DATA_TYPE_LOCATION_GUID)
                    {
                        string[] gpsData = sender.DataReport.Values[formatId][25].ToString().Split(',');

                        DateTimeOffset dto = new DateTimeOffset(
                            DateTime.ParseExact(string.Format("{0}{1}", gpsData[9], gpsData[1].Substring(0, 6)), "yyMMddHHmmss", null), TimeSpan.Zero);
                        this.Dispatcher.Invoke(() =>
                        {
                            utcTimeText.Text = string.Format("UTC : {0}", dto.ToString());
                            jstTimeText.Text = string.Format("JST : {0}", dto.ToLocalTime().ToString());
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                this.Close();
            }
        }


    }
}
