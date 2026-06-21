using CRUDMahasiswaADO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PisangGoreng_DRCRUD
{

    public partial class Form3 : Form
    {
        static string connectionString = "Data Source=LAPTOP-24A5CGHI\\WILDHANFIGHT;Initial Catalog=DBAkademikADO;user ID=sa;password=wildhan1234";
        SqlConnection con = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DAL dbLogic = new DAL();
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            InisialisasiFormatForm();
        }

        private void InisialisasiFormatForm()
        {
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;
            btnCetak.Enabled = false;

            cmbProdi.DisplayMember = "Value";
            cmbProdi.ValueMember = "Key";
            cmbProdi.DataSource = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>
            {
                new System.Collections.Generic.KeyValuePair<string, string>("TI01", "Teknik Informatika"),
                new System.Collections.Generic.KeyValuePair<string, string>("SI01", "Sistem Informasi"),
                new System.Collections.Generic.KeyValuePair<string, string>("TI02", "Teknik Elektro"),
            };
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (cmbProdi.SelectedItem == null)
            {
                MessageBox.Show("Silakan pilih Program Studi terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand("sp_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@inProdi", SqlDbType.VarChar, 50).Value = cmbProdi.SelectedValue.ToString();
                cmd.Parameters.Add("@inTglMsuk", SqlDbType.VarChar, 4).Value = dtpTanggalMasuk.Value.Year.ToString();

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);

                dataGridView3.DataSource = dtMahasiswa;

                btnCetak.Enabled = (dtMahasiswa.Rows.Count > 0);

                if (dtMahasiswa.Rows.Count == 0)
                    MessageBox.Show("Data tidak ditemukan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            if (dtMahasiswa != null && dtMahasiswa.Rows.Count > 0)
            {
                string prodi = cmbProdi.SelectedValue.ToString();
                DateTime tglMasuk = dtpTanggalMasuk.Value;

                Form2 frm2 = new Form2(prodi, tglMasuk);
                frm2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Silakan klik tombol Load data terlebih dahulu sebelum mencetak!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Dibiarkan kosong
        }
    }
}