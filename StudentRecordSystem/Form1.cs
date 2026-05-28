using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StudentRecordSystem
{
    public partial class frmStudentRecord : Form
    {
        MySqlConnection conn = new MySqlConnection(
        "server=127.0.0.1;port=3307;database=studentrecordsystem;uid=root;pwd=root;");

        public frmStudentRecord()
        {
            InitializeComponent();
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtStudentNumber.Clear();
            txtEmail.Clear();
            txtPhone.Clear();

            cmbGender.SelectedIndex = -1;
            cmbCourse.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;

            dtpBirthDate.Value = DateTime.Now;
        }



        public void LoadData()
        {
            try
            {
                conn.Open();

                MySqlDataAdapter da = new MySqlDataAdapter(
                "SELECT student_id, student_number, full_name, birth_date, gender, course, year_level, email, phone FROM tbl_students",
                conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Columns["student_id"].Visible = false;

                dataGridView1.Columns["student_number"].HeaderText = "Student ID";
                dataGridView1.Columns["full_name"].HeaderText = "Full Name";
                dataGridView1.Columns["birth_date"].HeaderText = "Birth Date";
                dataGridView1.Columns["gender"].HeaderText = "Gender";
                dataGridView1.Columns["course"].HeaderText = "Course";
                dataGridView1.Columns["year_level"].HeaderText = "Year Level";
                dataGridView1.Columns["email"].HeaderText = "Email";
                dataGridView1.Columns["phone"].HeaderText = "Phone Number";

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                MessageBox.Show("Connected Successfully!");
                conn.Close();

                LoadData(); //  IMPORTANT daw
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Clear()
        {
           
            {
                txtStudentNumber.Clear();
                txtName.Clear();
                dtpBirthDate.Value = DateTime.Now;
                cmbGender.SelectedIndex = -1;
                cmbCourse.SelectedIndex = -1;
                cmbYear.SelectedIndex = -1;
                txtEmail.Clear();
                txtPhone.Clear();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentNumber.Text) ||
            string.IsNullOrWhiteSpace(txtName.Text) ||
            string.IsNullOrWhiteSpace(cmbGender.Text) ||
            string.IsNullOrWhiteSpace(cmbCourse.Text) ||
            string.IsNullOrWhiteSpace(cmbYear.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please input all required fields!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn.Open();

                string query = @"INSERT INTO tbl_students
                (student_number, full_name, birth_date, gender, course, year_level, email, phone)
                VALUES
                (@studentnumber, @name, @birthdate, @gender, @course, @year, @email, @phone)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@birthdate", dtpBirthDate.Value);
                cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                cmd.Parameters.AddWithValue("@course", cmbCourse.Text);
                cmd.Parameters.AddWithValue("@year", Convert.ToInt32(cmbYear.Text));
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@studentnumber", txtStudentNumber.Text);

                cmd.ExecuteNonQuery();

                conn.Close();
                ClearFields();
                MessageBox.Show("Added Successfully!");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtStudentNumber.Tag == null)
            {
                MessageBox.Show("Please select a record first.");
                return;
            }

            try
            {
                conn.Open();

                string query = @"UPDATE tbl_students SET
                student_number=@studentnumber,
                full_name=@name,
                birth_date=@birthdate,
                gender=@gender,
                course=@course,
                year_level=@year,
                email=@email,
                phone=@phone
                WHERE student_id=@id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

               
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@birthdate", dtpBirthDate.Value);
                cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                cmd.Parameters.AddWithValue("@course", cmbCourse.Text);
                cmd.Parameters.AddWithValue("@year", Convert.ToInt32(cmbYear.Text));
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@studentnumber", txtStudentNumber.Text);
                cmd.Parameters.AddWithValue("@id", txtStudentNumber.Tag);

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Updated!");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                string query = "DELETE FROM tbl_students WHERE student_id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", txtStudentNumber.Tag);

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Deleted!");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtStudentNumber.Text = dataGridView1.Rows[e.RowIndex].Cells["student_number"].Value.ToString();
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells["full_name"].Value.ToString();
                dtpBirthDate.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["birth_date"].Value);
                cmbGender.Text = dataGridView1.Rows[e.RowIndex].Cells["gender"].Value.ToString();
                cmbCourse.Text = dataGridView1.Rows[e.RowIndex].Cells["course"].Value.ToString();
                cmbYear.Text = dataGridView1.Rows[e.RowIndex].Cells["year_level"].Value.ToString();
                txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells["email"].Value.ToString();
                txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells["phone"].Value.ToString();
                txtStudentNumber.Tag = dataGridView1.Rows[e.RowIndex].Cells["student_id"].Value.ToString();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)

        {
            if (txtSearchBar.Text == "Search...")
                return;

            try
            {
                conn.Open();

                string query = "SELECT * FROM tbl_students WHERE full_name LIKE @search";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearchBar.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void btsRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

