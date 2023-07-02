using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace SiriusWFTest1
{
    public partial class Form1 : Form
    {
        // Timer Update (every 1 sec)
        private const int timerUpdate = 1000;
        // Interface Storage
        private NetworkInterface[] netArr;
        // Создаем переменную тип Timer
        private Timer timer;
        private int time = 0;

        public Form1()
        {
            InitializeComponent();
            InitializeNetworkInterface();
            InitializeTimer();
            ////Время надписи на экране
            //toolTip1.AutoPopDelay = 1500;
            ////Задержка перед появлением надписи
            //toolTip1.InitialDelay = 500;

        }
        // Initialize all network interfaces on this computer
        private void InitializeNetworkInterface()
        {
            //Заполнение массива всеми доступными интерфейсами сети
            netArr = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < netArr.Length; i++)
            {
                //вывод интерфейсов
                comboBox1.Items.Add(netArr[i].Name);
            }
            comboBox1.SelectedIndex = 0;
        }
        //объявление таймера
        private void InitializeTimer()
        {
            timer = new Timer();
            //назначение интервала срабатывания
            timer.Interval = timerUpdate;
            timer.Tick += new EventHandler(timer_Tick);
            //запуск таймера
            timer.Start();
        }
        //функция обновления интерфейсов.
        private void UpdateNetworkInterface()
        {
            //Выбор сети
            NetworkInterface net = netArr[comboBox1.SelectedIndex];
            //предоставляет статистику IPv4
            IPv4InterfaceStatistics InterStatic = net.GetIPv4Statistics();
            //Подсчет кол-ва килобайт получаемого по сети
            int bytesSendSpeed = (int)(InterStatic.BytesSent - double.Parse(lBlSend.Text)) / 1024;
            //Подсчет кол-ва килобайт отправляемого по сети
            int bytesReceivedSpeed = (int)(InterStatic.BytesReceived - double.Parse(lBlReceived.Text)) / 1024;
            //Получение и вывод скорости сети
            lBlSpeed.Text = (net.Speed / 8196).ToString() + " кБ/с";
            //Получение и вывод идентификатора сети
            lBlId.Text = net.Id.ToString();
            //Получение и вывод информации о сети
            lBlinfo.Text = net.Description.ToString();
            //Получение и вывод статуса работы: true/false
            lBlStat.Text = net.OperationalStatus.ToString();
            //Получение и вывод типа сети
            lBlInterfaceType.Text = net.NetworkInterfaceType.ToString();
            //Вывод кол-ва полученных килобайт
            lBlSend.Text = InterStatic.BytesSent.ToString();
            //Вывод кол-ва отправленных килобайт
            lBlReceived.Text = InterStatic.BytesReceived.ToString();
            //Разгрузка данных в сеть
            lBlUpload.Text = bytesSendSpeed.ToString() + " кБ/c";
            //Загрузка данных из сети
            lBlDownload.Text = bytesReceivedSpeed.ToString() + " кБ/c";
            //проверка на максимально допустимое значение разгрузки
            if (bytesSendSpeed >= Convert.ToInt32(comboBox2.Items[comboBox2.SelectedIndex]))
            {
                //счетчик времени передачи большого кол-во данных
                time++;
                //вывод времени
                lBlTime.Text = time.ToString() + "c";
            }
            //проверка на падение значения разгрузки
            if (bytesSendSpeed < Convert.ToInt32(comboBox2.Items[comboBox2.SelectedIndex]))
            {
                //обнуления счетчика времени
                time = 0;
                lBlTime.Text = "0";
                //скрытие изображения
                //this.pictureBox1.Visible = false;
            }


        }
        //функция выполняющаяся каждую секунду
        private void timer_Tick(object sender, EventArgs e)
        {
            //вызов функции обновления интерфейсов
            UpdateNetworkInterface();
            //превышение времени получения большого кол-ва данных 
            if (time == 5)
            {
                //изображение становиться видимым
                //this.pictureBox1.Visible = true;
                //вывод надписи при наведении на изображение
                //toolTip1.SetToolTip(this.pictureBox1, "Передача большого количества данных");
                MessageBox.Show("Передача данных больше максимума");
            }
        }
        //функция выполняется при запуске программы
        private void Form1_Load(object sender, EventArgs e)
        {



            lBlInterface.Location = new Point(5, 16);
            lBlInterface.Text = "Интерфейс";
            //
            lBlInterfaceTypeT.Location = new Point(5, 48);
            lBlInterfaceTypeT.Text = "Тип интерфейса:";
            //
            lBlSpeedT.Location = new Point(5, 144);
            lBlSpeedT.Text = "Скорость:";
            //
            lBlSendT.Location = new Point(5, 176);
            lBlSendT.Text = "Отправлено:";
            //
            lBlReceivedT.Location = new Point(5, 208);
            lBlReceivedT.Text = "Принято:";
            //
            lBlUploadT.Location = new Point(5, 240);
            lBlUploadT.Text = "Разгрузка:";
            //
            lBlDownloadT.Location = new Point(5, 272);
            lBlDownloadT.Text = "Загрузка:";
            //
            lBlIdT.Location = new Point(5, 112);
            lBlIdT.Text = "Идентификатор:";
            //
            lBlinfoT.Location = new Point(5, 80);
            lBlinfoT.Text = "Описание:";
            //
            lBlStatT.Location = new Point(5, 304);
            lBlStatT.Text = "Статус работы:";
            //
            lBlTimeT.AutoSize = true;
            lBlTimeT.Location = new Point(5, 336);
            lBlTimeT.Text = "Время передачи большого кол-ва данных";
            //
            lBlInterfaceType.AutoSize = true;
            lBlInterfaceType.Location = new System.Drawing.Point(110, 48);
            lBlInterfaceType.Text = "0";
            //
            lBlDownload.AutoSize = true;
            lBlDownload.Location = new System.Drawing.Point(110, 272);
            lBlDownload.Text = "0";
            //
            lBlReceived.AutoSize = true;
            lBlReceived.Location = new System.Drawing.Point(110, 208);
            lBlReceived.Text = "0";
            //
            lBlSend.AutoSize = true;
            lBlSend.Location = new System.Drawing.Point(110, 176);
            lBlSend.Text = "0";
            //
            lBlSpeed.AutoSize = true;
            lBlSpeed.Location = new System.Drawing.Point(110, 144);
            lBlSpeed.Text = "0";
            //
            lBlUpload.AutoSize = true;
            lBlUpload.Location = new System.Drawing.Point(110, 240);
            lBlUpload.Text = "0";
            //
            lBlId.AutoSize = true;
            lBlId.Location = new System.Drawing.Point(110, 112);
            lBlId.Text = "0";
            //
            lBlinfo.AutoSize = true;
            lBlinfo.Location = new System.Drawing.Point(110, 80);
            lBlinfo.Text = "0";
            //
            lBlStat.AutoSize = true;
            lBlStat.Location = new System.Drawing.Point(110, 304);
            lBlStat.Text = "0";
            //
            lBlTime.AutoSize = true;
            lBlTime.Location = new System.Drawing.Point(240, 336);
            lBlTime.Text = "0";
            //
            comboBox2.SelectedIndex = 0;
            //
            lBlMax.AutoSize = true;
            lBlMax.Location = new System.Drawing.Point(178, 240);
            lBlMax.Text = "Max разгрузки(кБ/c):";
            this.Controls.Add(lBlInterface);
            this.Controls.Add(lBlInterfaceTypeT);
            this.Controls.Add(lBlinfo);
            this.Controls.Add(lBlinfoT);
            this.Controls.Add(lBlId);
            this.Controls.Add(lBlIdT);
            this.Controls.Add(lBlSpeedT);
            this.Controls.Add(lBlSendT);
            this.Controls.Add(lBlReceivedT);
            this.Controls.Add(lBlUploadT);
            this.Controls.Add(lBlDownloadT);
            this.Controls.Add(lBlInterfaceType);
            this.Controls.Add(lBlSpeed);
            this.Controls.Add(lBlSend);
            this.Controls.Add(lBlReceived);
            this.Controls.Add(lBlUpload);
            this.Controls.Add(lBlDownload);
            this.Controls.Add(lBlStat);
            this.Controls.Add(lBlStatT);
            this.Controls.Add(lBlTime);
            this.Controls.Add(lBlTimeT);
            this.Controls.Add(lBlMax);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void обПрограмммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("программа выполняет такое такое то действие", "Об программе");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void прочиеФункцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void получитьАдресаСлужбыДоменныхИменDNSДляСетевыхИнтерфейсовНаЛокальномКомпьютереToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder list = new StringBuilder();
            foreach (NetworkInterface adapter in netArr)
            {

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0)
                {

                    foreach (var dns in dnsServers)
                    {
                        string dnsStr = dns.ToString();
                        if (dnsStr[0] != 'f')
                            list.Append(dnsStr + "\n");
                        //comboBox3.Items.Add(dns.ToString());
                    }


                }

            }
            MessageBox.Show(list.ToString());
        }
    }
}
