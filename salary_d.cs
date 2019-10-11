using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aps_finance
{
    public partial class salary_d : Form
    {
        private SqlConnection con = dbc.sqlc();
        public salary_d()
        {
            InitializeComponent();
            label1.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Transparent;
            label3.BackColor = System.Drawing.Color.Transparent;
            tableup();
        }

        public void tableup()
        {
            try
            {
                con.Open();
                //String query2 = "SELECT emp_id,emp_name,getDate() as salary_month,emp_designation,emp_salary as salary from employee";
                String query = "select e.emp_id,e.emp_name,s.salary_month,emp_designation,s.salary from salary s,employee e where s.emp_id=e.emp_id ORDER BY s.salary_month DESC";
                SqlCommand sc = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(sc);

                //SqlCommand sc2 = new SqlCommand(query2, con);
                //SqlDataAdapter sda2 = new SqlDataAdapter(sc2);
                DataTable sdt = new DataTable();
                //sda2.Fill(sdt);
                sda.Fill(sdt);
                //DataColumn newc=new DataColumn("edit",typeof(string));
                //newc.AllowDBNull = false;
                //sdt.Columns.Add(newc);
                foreach (DataRow row in sdt.Rows)
                {
                    dataGridView1.DataSource = sdt;
                    //dataGridView1.DataSource = sdt;
                    //row["edit"] = "edit";
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
            con.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            sal_con x = new sal_con();
            x.ShowDialog();
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRows = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRows];
            int id = (int)row.Cells[0].Value;
            String x = row.Cells[2].Value.ToString();
            sal_edit s = new sal_edit(id,x);
            s.ShowDialog();
            tableup();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            String n = textBox1.Text;
           
            try
            {
                String query2 = "select e.emp_id,e.emp_name,s.salary_month,emp_designation,s.salary from salary s,employee e WHERE s.emp_id=e.emp_id AND emp_name LIKE '%"+n+"%' ORDER BY s.salary_month DESC";
                SqlCommand sc2 = new SqlCommand(query2, con);
                SqlDataAdapter sda2 = new SqlDataAdapter(sc2);
                DataTable sdt2 = new DataTable();
                sda2.Fill(sdt2);
                foreach (DataRow row in sdt2.Rows)
                {
                    dataGridView1.DataSource = sdt2;

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 x = new Form1();
            x.ShowDialog();
            this.Close();
        }
    }
}
