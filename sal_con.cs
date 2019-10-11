using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aps_finance
{
    public partial class sal_con : Form 
    {
        private SqlConnection con= dbc.sqlc();
        DataTable sdt;


        public sal_con() 
        {
            con.Open();
            InitializeComponent();
            String query = "select emp_id,emp_name as Name,emp_designation as designation,emp_salary as salary from employee";
            SqlCommand sc = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(sc);
            sdt = new DataTable();
            sda.Fill(sdt);
            sdt.Columns.Add("increment",typeof(float));
            sdt.Columns.Add("Final salary", typeof(float));
            sdt.Columns.Add("hospital_fees", typeof(float));

            foreach (DataRow row in sdt.Rows)
            {
                dataGridView1.DataSource = sdt;
                row["increment"] = 0;
                row["Final salary"] =row[3];
                row["hospital_fees"]= 0;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value == null || dataGridView1.Rows[i].Cells[3].Value == DBNull.Value || String.IsNullOrWhiteSpace(dataGridView1.Rows[i].Cells[3].Value.ToString()))
                {
                    try
                    {
                        DateTime yd = DateTime.Today.AddDays(-1);
                        String datex = yd.ToString("yyyy-MM-dd");
                        String eid = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        int x = int.Parse(eid);
                        String query1 = "select MAX(salary_month) from salary where emp_id=" + eid;
                        SqlCommand sc1 = new SqlCommand(query1, con);
                        DateTime x1 = (DateTime) sc1.ExecuteScalar();

                        String x2 = x1.ToString("yyyy-MM-dd");
                        string query3 = "select COUNT(AppNo) from Appointment where Employee_emp_id=" + eid +
                                        " AND date BETWEEN '" + x2 + "' AND '" + datex + "'";
                        String query2 = "select SUM(AppPay) from Appointment where Employee_emp_id=" + eid +
                                        " AND date BETWEEN '" + x2 + "' AND '" + datex + "'";
                        SqlCommand sc2 = new SqlCommand(query2, con);
                        SqlCommand sc3 = new SqlCommand(query3, con);
                        int co = (int) sc3.ExecuteScalar();
                        double c = (double) sc2.ExecuteScalar();
                        double sal = c - (co * 300);           
                        dataGridView1.Rows[i].Cells["hospital_fees"].Value = co * 300;
                        dataGridView1.Rows[i].Cells[3].Value = sal;
                        dataGridView1.Rows[i].Cells[5].Value = sal;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    catch (Exception ex2)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = 0;
                        dataGridView1.Rows[i].Cells[5].Value = 0;
                    }
                }
            }
            con.Close();
        }

    




        private void button1_Click(object sender, EventArgs e)
        {           
            con.Open();
            DateTime yd = DateTime.Today.AddDays(-1);
            String date = yd.ToString("yyyy-MM-dd");
            float x=0,salary;
            int emp_id;
            String sql3 = "SELECT COUNT(*) FROM salaryh where salary_date='"+date+"'";
           
            SqlCommand sc3 = new SqlCommand(sql3, con);
            int UserExist = (int)sc3.ExecuteScalar();
            

            if (UserExist == 0)
            {
                try
                {
                    SqlCommand sc4 = new SqlCommand("INSERT INTO salaryh VALUES ('" + date + "'," + x + ")",con);
                    sc4.ExecuteNonQuery();
                }catch(Exception ex2)
                {
                   MessageBox.Show(ex2.ToString());
                    return;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                emp_id = (int)dataGridView1.Rows[i].Cells[0].Value;

                try
                {
                    salary = (float)dataGridView1.Rows[i].Cells[5].Value;               
                    x += (float)dataGridView1.Rows[i].Cells[6].Value;

                    String sql = "INSERT INTO salary VALUES(@emp_id,@date,@salary)";
                    
                    SqlCommand insert = new SqlCommand(sql, con);
                    insert.Parameters.AddWithValue("@emp_id", emp_id);
                    insert.Parameters.AddWithValue("@date", date);
                    insert.Parameters.AddWithValue("@salary", salary);
                    insert.ExecuteNonQuery();
                    
                }
                catch(SqlException n)
                {
                    if (n.Number == 2627)
                    {
                        MessageBox.Show("Already you create for this employee on this date. Emp id:"+emp_id);

                    }
                }
            }

            
            String sql2 = "UPDATE salaryh SET com+=@sc where salary_date=@date";
            SqlCommand insert2 = new SqlCommand(sql2,con);
            insert2.Parameters.AddWithValue("@date", date);
            insert2.Parameters.AddWithValue("@sc", x);
            insert2.ExecuteNonQuery();

            con.Close();
            MessageBox.Show("Successfully Added to DataBase");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRows = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRows];
            textBox1.Text = row.Cells[0].Value.ToString();
            textBox2.Text = row.Cells[1].Value.ToString();
            textBox3.Text = row.Cells[2].Value.ToString();
            textBox4.Text = row.Cells[3].Value.ToString();
            textBox5.Text = row.Cells[4].Value.ToString();
            textBox6.Text = row.Cells[5].Value.ToString();
        }






        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {         
            string x2 = textBox1.Text;
            if (x2 == "" || e.KeyCode==Keys.Back)
            {              
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                if (x2 == "") { return; }
            }
            try
            {
                int x3 = Convert.ToInt16(x2);

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((int)dataGridView1.Rows[i].Cells[0].Value == x3)
                    {
                        textBox2.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        textBox3.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        textBox4.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        textBox5.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        textBox6.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        return;
                    }                  
                }
                MessageBox.Show("wrong ID");
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
            }
            catch (Exception e2)
            {
                //MessageBox.Show(e2.ToString());
            }
        }






        private void button2_Click(object sender, EventArgs e)
        {
            try {
                string id = textBox1.Text;
                int id1 = Convert.ToInt16(id);
                string sal = textBox5.Text;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((int)dataGridView1.Rows[i].Cells[0].Value == id1)
                    {
                        float x = (Convert.ToSingle(textBox4.Text)) + (Convert.ToSingle(textBox5.Text));
                        dataGridView1.Rows[i].Cells[4].Value = Convert.ToSingle(textBox5.Text);
                        dataGridView1.Rows[i].Cells[5].Value = x;
                    }
                }
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
            }catch(Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }



        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            double x;
            if (String.IsNullOrWhiteSpace(textBox5.Text) || textBox5.Text == null )
            {
                x = (Convert.ToDouble(textBox4.Text));
                textBox5.Text = "0";
            }
            else { 
                x = (Convert.ToDouble(textBox4.Text)) + (Convert.ToDouble(textBox5.Text));
            }
            textBox6.Text = x.ToString();
        }




        private void sal_con_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }





        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textBox1.Text;
                int id1 = Convert.ToInt16(id);
                string sal = textBox5.Text;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((int)dataGridView1.Rows[i].Cells[0].Value == id1)
                    {
                        try
                        {
                            float x = (Convert.ToSingle(textBox4.Text)) + (Convert.ToSingle(textBox5.Text));
                            int emp_id = id1;
                            float salary = x;
                            DateTime yd = DateTime.Today.AddDays(-1);
                            String date = yd.ToString("yyyy-MM-dd");
                            con.Open();
                            String sql = "INSERT INTO salary VALUES(@emp_id,@date,@salary)";
                            SqlCommand insert = new SqlCommand(sql, con);
                            insert.Parameters.AddWithValue("@emp_id", emp_id);
                            insert.Parameters.AddWithValue("@date", date);
                            insert.Parameters.AddWithValue("@salary", salary);
                            insert.ExecuteNonQuery();
                            dataGridView1.Rows.RemoveAt(i);
                            con.Close();
                        }
                        catch (SqlException se)
                        {
                            int er_code =((SqlException)se.InnerException).Number ;
                            if (er_code==2627)
                            {
                                MessageBox.Show("Already you create for this employee on this date. Emp id:" + id1);
                            }
                        }
                    }
                }
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 x = new Form1();
            x.ShowDialog();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
