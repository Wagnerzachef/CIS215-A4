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
using System.Windows.Forms.VisualStyles;
using System.Net;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing.Printing;
using System.Windows.Forms;




namespace A4._1
{
    public partial class Form1 : Form
    {
        SQLiteDataReader dr;
        float totalCost;
        public Form1()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string value1 = listBox1.SelectedItem.ToString();
            textBox9.Text = value1;

            SQLiteConnection con1 = new SQLiteConnection(@"data source=C:\sqlite\A4.1.db");
            con1.Open();
            string query1 = "SELECT POnum FROM POnum";
            SQLiteCommand cmd1 = new SQLiteCommand(query1, con1);

            dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                textBox10.Text = dr.GetValue(0).ToString();
            }
            con1.Close();

            int quantity = int.Parse(textBox4.Text);
            float cost = float.Parse(textBox7.Text);
            float unitCost = quantity * cost;
            totalCost = totalCost + unitCost;
            textBox11.Text = totalCost.ToString();

            SQLiteConnection con = new SQLiteConnection(@"data source=C:\sqlite\A4.1.db");
            con.Open();
            string query = "INSERT INTO currentPO (PO_num, Vendor_address, Vendor_name, Qty, Item_id, Desc, costPer, totalCost, warehouse_id, signature) VALUES (@pn, @va, @vn, @qt, @ii, @de, @cp, @tp, @wi, @s)";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            cmd.Parameters.AddWithValue("@pn", textBox10.Text.Trim());
            cmd.Parameters.AddWithValue("@va", textBox1.Text.Trim());
            cmd.Parameters.AddWithValue("@vn", textBox2.Text.Trim());
            cmd.Parameters.AddWithValue("@qt", textBox4.Text.Trim());
            cmd.Parameters.AddWithValue("@ii", textBox5.Text.Trim());
            cmd.Parameters.AddWithValue("@de", textBox6.Text.Trim());
            cmd.Parameters.AddWithValue("@cp", textBox7.Text.Trim());
            cmd.Parameters.AddWithValue("@tp", textBox11.Text.Trim());
            cmd.Parameters.AddWithValue("@wi", textBox9.Text.Trim());
            cmd.Parameters.AddWithValue("@s", textBox3.Text.Trim());
            cmd.ExecuteNonQuery();

            SQLiteConnection con2 = new SQLiteConnection(@"data source=C:\sqlite\A4.1.db");
            string query2 = "SELECT Qty, Item_id, Desc, costPer, totalCost FROM currentPO";
            SQLiteCommand cmd2 = new SQLiteCommand(query2, con2);

            DataTable dt = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd2);
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection(@"data source=C:\sqlite\A4.1.db");
            string query = "SELECT * FROM pastPO";
            SQLiteCommand cmd = new SQLiteCommand(query, con);

            DataTable dt = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection(@"data source=C:\sqlite\A4.1.db");
            con.Open();
            string query = "SELECT id FROM Warehouse";
            SQLiteCommand cmd = new SQLiteCommand(query, con);

            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listBox1.Items.Add(dr["id"]);
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            totalCost = 0;

            SQLiteConnection con = new SQLiteConnection(@"data source=C:\sqlite\A4.1.db");
            con.Open();
            string query = "DELETE FROM currentPO";
            SQLiteCommand cmd = new SQLiteCommand(query, con);

            cmd.ExecuteReader();

            con.Close();
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);

            PrintDialog printdlg = new PrintDialog();
            PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();

            // preview the assigned document or you can create a different previewButton for it
            printPrvDlg.Document = pd;
            printPrvDlg.ShowDialog(); 

            printdlg.Document = pd;

            if (printdlg.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);

            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            e.Graphics.DrawImage(bm, 0, 200);
            String venAd = textBox1.Text;
            String venAm = textBox2.Text;
            String sig = textBox3.Text;
            String ware = listBox1.SelectedItem.ToString();

            String string1 = "Vendor Address";
            String string2 = "Vendor Name";
            String string3 = "Warehouse Number";
            String string4 = "Signature";

            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            PointF drawPoint = new PointF(150.0F, 150.0F);
            PointF drawPoint2 = new PointF(500.0F, 150.0F);
            PointF drawPoint3 = new PointF(150.0F, 500.0F);
            PointF drawPoint4 = new PointF(500.0F, 500.0F);

            PointF drawPoint5 = new PointF(150.0F, 120.0F);
            PointF drawPoint6 = new PointF(500.0F, 120.0F);
            PointF drawPoint7 = new PointF(150.0F, 470.0F);
            PointF drawPoint8 = new PointF(500.0F, 470.0F);

            e.Graphics.DrawString(venAd, drawFont, drawBrush, drawPoint);
            e.Graphics.DrawString(venAm, drawFont, drawBrush, drawPoint2);
            e.Graphics.DrawString(ware, drawFont, drawBrush, drawPoint3);
            e.Graphics.DrawString(sig, drawFont, drawBrush, drawPoint4);

            e.Graphics.DrawString(string1, drawFont, drawBrush, drawPoint5);
            e.Graphics.DrawString(string2, drawFont, drawBrush, drawPoint6);
            e.Graphics.DrawString(string3, drawFont, drawBrush, drawPoint7);
            e.Graphics.DrawString(string4, drawFont, drawBrush, drawPoint8);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection(@"data source=C:\sqlite\A4.1.db");
            string query = "SELECT * FROM pastPO WHERE pastPO_num = @lu";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            cmd.Parameters.AddWithValue("@lu", textBox8.Text.Trim());

            DataTable dt = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }
        
    }
}
