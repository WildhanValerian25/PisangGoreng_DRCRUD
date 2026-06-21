using System;
using System.Collections.Generic;

namespace PisangGoreng_DRCRUD
{
    public class DataMahasiswa
    {
        public string Nama { get; set; }
        public string JenisKelamin { get; set; }
        public string Alamat { get; set; }
        public string NamaProdi { get; set; }
        public DateTime TanggalDaftar { get; set; }
    }

    public class ListMahasiswa : List<DataMahasiswa>
    {
        // kosong, hanya wrapper
    }
}