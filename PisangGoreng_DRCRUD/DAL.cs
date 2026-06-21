using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PisangGoreng_DRCRUD
{
    internal class DAL
    {
        public static string GetConnectionString()
        {
            return "Data Source=localhost\\WILDHANFIGHT;Initial Catalog=DBAkademikADO;User ID=sa;Password=wildhan1234";
        }

        SqlConnection conn = new SqlConnection(GetConnectionString());
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        public int CountMhs()
        {
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", newConn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();
                return Convert.ToInt32(outputParam.Value);
            }
        }

        public DataTable GetMhs()
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", newConn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtResult);
            }
            return dtResult;
        }

        public void InsertMhs(string nim, string nama, string alamat, string jenisKelamin, DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlTransaction trans = newConn.BeginTransaction();
                try
                {
                    SqlCommand command = new SqlCommand("sp_InsertMahasiswa", newConn, trans);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@pNIM", SqlDbType.Char, 11).Value = nim;
                    command.Parameters.Add("@pNama", SqlDbType.VarChar, 100).Value = nama;
                    command.Parameters.Add("@pAlamat", SqlDbType.VarChar, 200).Value = alamat;
                    command.Parameters.Add("@pJenisKelamin", SqlDbType.Char, 1).Value = jenisKelamin;
                    command.Parameters.Add("@pTanggalLahir", SqlDbType.DateTime).Value = tanggalLahir;
                    command.Parameters.Add("@pKodeProdi", SqlDbType.Char, 4).Value = kodeProdi;

                    SqlParameter fotoParam = new SqlParameter("@pFoto", SqlDbType.VarBinary, -1);
                    fotoParam.Value = (object)foto ?? DBNull.Value;
                    command.Parameters.Add(fotoParam);

                    command.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void UpdateMhs(string nim, string nama, string alamat, string jenisKelamin, DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand command = new SqlCommand("sp_UpdateMahasiswa", newConn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@pNIM", SqlDbType.Char, 11).Value = nim;
                command.Parameters.Add("@pNama", SqlDbType.VarChar, 100).Value = nama;
                command.Parameters.Add("@pAlamat", SqlDbType.VarChar, 200).Value = alamat;
                command.Parameters.Add("@pJenisKelamin", SqlDbType.Char, 1).Value = jenisKelamin;
                command.Parameters.Add("@pTanggalLahir", SqlDbType.DateTime).Value = tanggalLahir;
                command.Parameters.Add("@pKodeProdi", SqlDbType.Char, 4).Value = kodeProdi;

                SqlParameter fotoParam = new SqlParameter("@pFoto", SqlDbType.VarBinary, -1);
                fotoParam.Value = (object)foto ?? DBNull.Value;
                command.Parameters.Add(fotoParam);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteMhs(string nim)
        {
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", newConn);
                cmd.Parameters.AddWithValue("@NIM", nim);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }

        public void resetData()
        {
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                string deleteQuery = "DELETE FROM mahasiswa;";
                SqlCommand cmdDelete = new SqlCommand(deleteQuery, newConn);
                cmdDelete.ExecuteNonQuery();

                string insertQuery = "INSERT INTO mahasiswa SELECT * FROM mahasiswa_backup;";
                SqlCommand cmdInsert = new SqlCommand(insertQuery, newConn);
                cmdInsert.ExecuteNonQuery();
            }
        }

        public void testInject(string nim)
        {
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                string query = "Update mahasiswa set nama = 'HACKED' where NIM = " + nim;
                SqlCommand cmd = new SqlCommand(query, newConn);
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetMhsByNIM(string nim)
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetMahasiswaByNIM", newConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pNIM", nim);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtResult);
            }
            return dtResult;
        }

        public void InsertLog(string message)
        {
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("sp_LogMessage", newConn);
                cmd.Parameters.AddWithValue("psn", message);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable getProdi()
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("select namaprodi from prodi", newConn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtResult);
            }
            return dtResult;
        }

        public DataTable getDataRekap(string prodi, DateTime tanggalMasuk)
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("sp_Report", newConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tanggalMasuk.Year.ToString());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtResult);
            }
            return dtResult;
        }

        public DataTable getAllDataChart()
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("sp_DashBoard", newConn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtResult);
            }
            return dtResult;
        }

        public DataTable getDataChartByTahun(DateTime thMasuk)
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection newConn = new SqlConnection(GetConnectionString()))
            {
                newConn.Open();
                SqlCommand cmd = new SqlCommand("sp_DashBoardByTahun", newConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inTglMsuk", thMasuk.Year);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtResult);
            }
            return dtResult;
        }
    }
}