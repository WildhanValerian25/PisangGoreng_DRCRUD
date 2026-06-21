using PisangGoreng_DRCRUD;
using System;
using System.Data;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form2 : Form
    {
        DAL dbLogic = new DAL();
        string prodi { get; set; }
        DateTime tglMasuk { get; set; }

        public Form2(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();
            prodi = Prodi;
            tglMasuk = TglMasuk;

            try
            {
                // Konversi DataTable → ListMahasiswa
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglMasuk);

                ListMahasiswa listMahasiswa = new ListMahasiswa();
                foreach (DataRow row in dtMahasiswa.Rows)
                {
                    listMahasiswa.Add(new DataMahasiswa
                    {
                        Nama = row["Nama"].ToString(),
                        JenisKelamin = row["JenisKelamin"].ToString(),
                        Alamat = row["Alamat"].ToString(),
                        NamaProdi = row["NamaProdi"].ToString(),
                        TanggalDaftar = Convert.ToDateTime(row["TanggalDaftar"])
                    });
                }

                CrystalReport1 report = new CrystalReport1();
                report.SetDataSource(listMahasiswa);

                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}