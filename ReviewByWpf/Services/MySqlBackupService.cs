using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReviewByWpf.Services
{
    public class MySqlBackupService
    {
        public string BackupDatabase(string user, string password, string database)
        {
            string backupFile = $"db_review_backup_{DateTime.Now:yyyyMMddHHmmss}.sql";

            var psi = new ProcessStartInfo
            {
                FileName = "mysqldump",
                Arguments = $"-u {user} -p{password} --default-character-set=utf8mb4 {database}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            using (var process = Process.Start(psi))
            {
                using (var reader = process.StandardOutput)
                {
                    File.WriteAllText(backupFile, reader.ReadToEnd(), Encoding.UTF8);
                }
            }

            return backupFile;
        }
    }
}
