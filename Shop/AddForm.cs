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
    public partial class AddForm : Form
    {
        DataB db = new DataB();
        public AddForm()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files(*.png) |*.png|(*.jpg)|*.jpg|(*.gif)|*.gif";
            openFileDialog1.ShowDialog();
            pictureBox1.BackgroundImage = Image.FromFile(openFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.BackgroundImage == null)
            {
                MessageBox.Show("Вы не загрузили картинку!");
                return;
            }

            MemoryStream MS = new MemoryStream();
            pictureBox1.BackgroundImage.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arrImage = MS.GetBuffer();

            db.openConn();
            MySqlCommand command = new MySqlCommand("INSERT INTO textimageadd (Title, Desription, Image) VALUES (@Title, @Desription, @Image)", db.getConn());
            command.Parameters.AddWithValue("@Title", textBox1.Text);
            command.Parameters.AddWithValue("@Desription", textBox2.Text);
            command.Parameters.AddWithValue("@Image", arrImage);
            command.ExecuteNonQuery();
            db.closeConn();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
