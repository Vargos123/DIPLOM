using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shop
{
    public partial class MainForm : Form
    {
        private String idProduct;
        DataB db = new DataB();
        MySqlDataReader reader;
        private PictureBox PB;
        private Label Title;
        private Button button;
        static int notickcheckbox, notickcheckbox2;
        bool noAddImage = true;

        public MainForm()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files(*.png) |*.png|(*.jpg)|*.jpg|(*.gif)|*.gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                noAddImage = false;
                pictureBox2.BackgroundImage = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (noAddImage)
            {
                MessageBox.Show("Вы не загрузили картинку!");
                return;
            }
            else if(string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Вы не ввели название товара!");
                return;
            }
            else if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Вы не ввели цену товара!");
                return;
            }
            else if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Вы не выбрали категорию товара!");
                return;
            }
            else if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Вы не ввели описание товара!");
                return;
            }
            else if (checkBox1.Checked == true)
            {
                notickcheckbox = 1;
            }

                AddProduct();
        }

        private void AddProduct()
        {
            MemoryStream MS = new MemoryStream();
            pictureBox2.BackgroundImage.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arrImage = MS.GetBuffer();

            db.openConn();
            MySqlCommand command = new MySqlCommand("INSERT INTO `Household products` (Name, Description, Price, Image, ProductAvailability) VALUES (@Name, @Desription, @Price, @Image, @ProductAvailability)", db.getConn());
            command.Parameters.AddWithValue("@Name", textBox3.Text);
            command.Parameters.AddWithValue("@Desription", textBox5.Text);
            command.Parameters.AddWithValue("@Price", textBox4.Text);
            command.Parameters.AddWithValue("@Image", arrImage);
            command.Parameters.AddWithValue("@ProductAvailability", notickcheckbox);
            command.ExecuteNonQuery();
            db.closeConn();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            flowLayoutPanel2.Controls.Clear();
            db.openConn();
            MySqlCommand command = new MySqlCommand("SELECT `Image`, `id`, `Name`, `Description` FROM `Household products`", db.getConn());
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                long lend = reader.GetBytes(0, 0, null, 0, 0);
                byte[] array = new byte[Convert.ToInt32(lend) + 1];
                reader.GetBytes(0, 0, array, 0, Convert.ToInt32(lend));
                PB = new PictureBox();
                PB.Width = 200;
                PB.Height = 200;
                PB.BackgroundImageLayout = ImageLayout.Stretch;
                PB.BorderStyle = BorderStyle.FixedSingle;              



                Title = new Label();
                Title.Text = reader["Name"].ToString();
                Title.Width = 50;
                Title.BackColor = Color.FromArgb(255, 228, 255);
                Title.TextAlign = ContentAlignment.MiddleCenter;
                Title.Dock = DockStyle.Bottom;


                button = new Button();
                button.Width = 75;
                button.Height = 25;
                button.Text = "Открыть";
                button.BackgroundImageLayout = ImageLayout.Stretch;
                button.Tag = reader["id"].ToString();


                MemoryStream MS = new MemoryStream(array);
                Bitmap bitmap = new Bitmap(MS);
                PB.BackgroundImage = bitmap;


                PB.Controls.Add(button);
                PB.Controls.Add(Title);
                flowLayoutPanel2.Controls.Add(PB);

                button.Click += new EventHandler(OnClick);
            }
            reader.Close();
            db.closeConn();
        }

        public void OnClick(object sender, EventArgs e)
        {
            idProduct = ((Button)sender).Tag.ToString();


            db.openConn();
            MySqlCommand command = new MySqlCommand("SELECT `Image`, `Name`, `Price`, `ProductAvailability`, `Description` FROM `Household products` WHERE `id` = " + idProduct + "", db.getConn());
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                long lend = reader.GetBytes(0, 0, null, 0, 0);
                byte[] array = new byte[Convert.ToInt32(lend) + 1];
                reader.GetBytes(0, 0, array, 0, Convert.ToInt32(lend));
                PB = new PictureBox();
                MemoryStream MS = new MemoryStream(array);
                Bitmap bitmap = new Bitmap(MS);
                pictureBox1.BackgroundImage = bitmap;

                textBox6.Text = reader[1].ToString();
                textBox1.Text = reader[2].ToString();

                if (reader[3].ToString() == "1")
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }

                textBox2.Text = reader[4].ToString();
            }
            reader.Close();
            db.closeConn();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Проверяем наличие интернета
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                // Проверяем не пусты ли поля 'Название' и 'Сообщение' 
                if (string.IsNullOrWhiteSpace(textBox6.Text))
                {
                    MessageBox.Show("Название не может быть пустым!");
                    return;
                }
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Сообщение не может быть пустым!");
                    return;
                }

                if (checkBox2.Checked == true)
                {
                    notickcheckbox2 = 1;
                }
                else
                {
                    notickcheckbox2 = 0;
                }

                // Обновляем таблицу внося новые данные вместо старых
                MySqlCommand command = new MySqlCommand("UPDATE `Household products` SET Name = @Name, Description = @Desription, Price = @Price, ProductAvailability = @ProductAvailability WHERE id = @Id", db.getConn());
                command.Parameters.AddWithValue("Name", textBox6.Text);
                command.Parameters.AddWithValue("Desription", textBox2.Text);
                command.Parameters.AddWithValue("Price", textBox1.Text);
                command.Parameters.AddWithValue("@ProductAvailability", notickcheckbox2);
                command.Parameters.AddWithValue("@Id", idProduct);


                db.openConn();  // Открываем соединени
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Вы успешно обновлили данные!");
                }
                else
                    MessageBox.Show("Вы не смогли обновить данные!");
                db.closeConn(); // Закрываем соединени
            }
            else
            {
                MessageBox.Show("Не удалось обновить данные. Проверьте доступ к интернету!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Проверяем наличине интернета
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                if (MessageBox.Show("Вы действительно хотите удалить товар?", "Удаление", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    try
                    {

                        // Удаляем выделенную строку по индексу с базы данных
                        MySqlCommand command1 = new MySqlCommand("DELETE FROM `Household products` WHERE id = " + idProduct + "", db.getConn());

                        // Обновляем id в таблице базы данных
                        MySqlCommand command2 = new MySqlCommand("ALTER TABLE `Household products` DROP id;" +
                        "ALTER TABLE `Household products`" +
                        "ADD id INT UNSIGNED NOT NULL AUTO_INCREMENT FIRST," +
                        "ADD PRIMARY KEY(id)", db.getConn());

                        // Открываем соединение
                        db.openConn();
                        // Выполняем комманды
                        command1.ExecuteNonQuery();
                        command2.ExecuteNonQuery();
                        // Закрываем соединение
                        db.closeConn();
                        LoadData();

                        // Удаляем выеделнную строку из таблицы
                        MessageBox.Show("Вы успешно удалили товар!");
                    }
                    catch
                    {
                        MessageBox.Show("Произошла ошибка!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Не удалось удалить товар. Проверьте доступ к интернету!");
            }
        }
    }
}
