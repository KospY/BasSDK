using System.Threading.Tasks;

namespace ThunderRoad
{
    public class AdbHelper
    {
        private readonly string _adbPath;

        public AdbHelper(string adbPath)
        {
            this._adbPath = adbPath;
        }
        
        private async void RunAdbCommandAsync(string arguments)
        {
            await Task.Run(() => {
                using var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = _adbPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                //split output and error by new line and return as array
                var logs = (output + "\n" + error).Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var log in logs)
                {
                    UnityEngine.Debug.Log(log);
                }
            });
        }

        public void ListDevicesAsync()
        {
            RunAdbCommandAsync("devices");
        }

        public void PushAsync(string localPath, string remotePath)
        {
            RunAdbCommandAsync($"push \"{localPath}\" \"{remotePath}\"");
        }

        public void ShellAsync(string command)
        {
            RunAdbCommandAsync($"shell {command}");
        }

        public void StopAppAsync(string packageName)
        {
            RunAdbCommandAsync($"shell am force-stop {packageName}");
        }

        public void StartAppAsync(string packageName, string activity, string args = "")
        {
            RunAdbCommandAsync($"shell am start -n {packageName}/{activity} --es args '{args}'");
        }
    }

}
