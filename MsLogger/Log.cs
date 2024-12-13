using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MsLogger
{
    /// <summary>
    /// 2024-10-22 작성
    /// Declare as shown below and use the log in the format Log.{logLevel}({message});
    /// 
    ///  Log.CreateConfiguration()
    ///        .LogPath(APP_NAME + "_Logs")                           // The path to the folder where the log files will be stored (Absolute /Relative path)
    ///        .LogFileNameWithoutExtension(APP_NAME)                 // The name of log file without extension (The extension is automatically added, so even if specified, a file with the name format .xxx.log will be created.)
    ///        .IsAdditionalErrorLogFileEnabled(true)                 // Whether to create an additional log file that records only logs of Error level or higher. 로그레벨이 Error이상 로그만 기록하는 로그파일을 추가 생성
    ///        .ErrorLogPath(APP_NAME + "_Error_Logs")                // The path to the folder where the log files that records only logs of Error level or higher will be stored (Absolute /Relative path) 로그레벨이 Error이상 로그만 기록하는 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
    ///        .ErrorLogFileNameWithoutExtension(APP_NAME + "_Error") // The name of log file that records only logs of Error level or higher name without extension (The extension is automatically added, so even if specified, a file with the name format .xxx.log will be created.) 로그레벨이 Error이상 로그만 기록하는 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
    ///        .MinimumLogLevel(LogLevel.Verbose)                     // Miminum log level 기록할 최소로그레벨
    ///        .WriteConsole(true);                                   // Whether write log text to console 콘솔에도 보이게 할지 여부
    ///        .LogFileSizeLimit()                                    // Maximum log file size 최대 로그파일 크기
    ///        .LogFileCountLimitEnabled()                            // Whether to limit the number of log files 로그파일 갯수 제한 여부
    ///        .RollingCountLimit()                                   // The number of log file to limit 제한할 로그파일 갯수
    ///        
    ///         If not specified as above, the default value will be applied, and the default value is as follows:
    ///         LogPath                                               - Path.Combine(Environment.CurrentDirectory, "Logs")
    ///         LogFileNameWithoutExtension                           -  "log"
    ///         IsAdditionalErrorLogFileEnabled                       - true
    ///         ErrorLogPath                                          - Path.Combine(Environment.CurrentDirectory, "ErrorLogs")
    ///         ErrorLogFileNameWithoutExtension                      - "log_Error"
    ///         MinimumLogLevel                                       - LogLevel.Infomation
    ///         WriteConsole                                          - true
    ///         LogFileSizeLimit                                      - 51_200_000
    ///         LogFileCountLimitEnabled                              - false
    ///         RollingCountLimit                                     - 30
    ///         
    ///         You can use this dll like:
    ///         string exception = "error message";
    ///         Log.Information("hi");
    ///         Log.Error($"error occurred {exception}");
    ///         Log.Fatal("fatal error occurred {0}", exception);
    /// # 에러로그만 수집을 원한다면 IsAdditionalErrorLogFileEnabled 옵션을 false로 설정하고 MinimumLogLevel에서 최소로그레벨을 올리는 것을 권장
    /// </summary>
    public class Log
    {
        private static readonly object lockObject = new object();
        public static LogConfiguration CreateConfiguration() => new LogConfiguration();
        private static LogLevel _minimumLogLevel = LogLevel.Information;
        
        private static string _logFileName = "log";
        private static string _errorLogFileName = $"log_Error";
        private static string _logFileDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
        private static string _errorlogFileDirectory = Path.Combine(Environment.CurrentDirectory, "ErrorLogs");        
        private static bool _isAdditionalErrorLogFileEnabled = true;
        private static bool _writeConsole = true;
        private static int _logFileSizeLimit = 51_200_000;
        private static bool _logFileCountLimitEnabled = false;
        private static int _rollingCountLimit = 30;
                
        
        private static void Logger(LogLevel logLevel, string message)
        {
            if (logLevel >= _minimumLogLevel)
            {
                DateTime dataTimeNow = DateTime.Now;
                Write(dataTimeNow, _logFileDirectory, $"{_logFileName}-{dataTimeNow:yyyyMMdd}", logLevel, message);

                if (_isAdditionalErrorLogFileEnabled && logLevel >= LogLevel.Error)
                {
                    Write(dataTimeNow, _errorlogFileDirectory, $"{_errorLogFileName}-{dataTimeNow:yyyyMMdd}", logLevel, message);
                }
            }
        }

        
        public static void Verbose(string message)
        {
            Logger(LogLevel.Verbose, message);
        }

        
        public static void Verbose(string message, params object[] args)
        {
            Verbose(String.Format(message, args));
        }

        
        public static void Debug(string message)
        {
            Logger(LogLevel.Debug, message);
        }

        
        public static void Debug(string message, params object[] args)
        {
            Debug(String.Format(message, args));
        }

        
        public static void Information(string message)
        {
            Logger(LogLevel.Information, message);
        }

        
        public static void Information(string message, params object[] args)
        {
            Information(String.Format(message, args));
        }

        
        public static void Warning(string message)
        {
            Logger(LogLevel.Warning, message);
        }

        
        public static void Warning(string message, params object[] args)
        {
            Warning(String.Format(message, args));
        }

        
        public static void Error(string message)
        {
            Logger(LogLevel.Error, message);
        }

        
        public static void Error(string message, params object[] args)
        {
            Error(String.Format(message, args));
        }

        
        public static void Fatal(string message)
        {
            Logger(LogLevel.Fatal, message);
        }

        
        public static void Fatal(string message, params object[] args)
        {
            Fatal(String.Format(message, args));
        }

        private static void Write(DateTime dataTimeNow, string logDirectory, string logFileName, LogLevel logLevel, string message)
        {
            lock (lockObject)
            {
                //Check Directory
                DirectoryInfo directory = new DirectoryInfo(logDirectory);
                if (!directory.Exists)
                {
                    directory.Create();
                }

                string logFilePath = Path.Combine(logDirectory, logFileName + ".log");
                FileInfo file = new FileInfo(logFilePath);

                
                //Check FileSize
                if (file.Exists && file.Length > _logFileSizeLimit)
                {
                    int fileIndex = GetLastLogFileIndex(logDirectory, logFileName);
                    string oldFile = Path.Combine(logDirectory, $"{logFileName} ({++fileIndex}).log");
                    while (File.Exists(oldFile) && new FileInfo(oldFile).Length >= _logFileSizeLimit)
                    {
                        oldFile = Path.Combine(logDirectory, $"{logFileName} ({++fileIndex}).log");
                    }
                    File.Move(logFilePath, oldFile);
                }

                
                //Write Log TExt
                // logStringBuilder.Append($"{dataTimeNow:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] {message}");
                var logStringBuilder = new StringBuilder();
                logStringBuilder.Append(dataTimeNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                logStringBuilder.Append(" [");
                logStringBuilder.Append(logLevel.ToString());
                logStringBuilder.Append("] ");
                logStringBuilder.Append(message);
                logStringBuilder.Append(Environment.NewLine);


                // Write To File
                File.AppendAllText(logFilePath, logStringBuilder.ToString());

                
                // Write To Console
                if (_writeConsole && logDirectory == _logFileDirectory)
                {
                    Console.WriteLine(logStringBuilder.ToString());
                }


                // delete lowest number log file for Log file count limitation
                if (_logFileCountLimitEnabled)
                {
                    var logFiles = Directory.GetFiles(logDirectory, $"{_logFileName} (*).log")
                                            .OrderBy(f => new FileInfo(f).LastWriteTimeUtc)
                                            .ToList();

                    while (logFiles.Count() > _rollingCountLimit)
                    {
                        File.Delete(logFiles[0]);
                        logFiles.RemoveAt(0); // delete the oldest file (lowest number file)
                    }
                }
            }
        }

        private static int GetLastLogFileIndex(string logDirectory, string logFileName)
        {
            int maxIndex = 0;
            string pattern = $@"{logFileName} \(\d+\)\.log$"; // "({numeric}).log" pattern

            // Get the list of existing files and find the index
            foreach (var file in Directory.GetFiles(logDirectory))
            {
                string fileName = Path.GetFileName(file);
                if (Regex.IsMatch(fileName, pattern))
                {
                    // Extract numbers from a string with regular expressions
                    var match = Regex.Match(fileName, @"\(\d+");
                    if (match.Success  && int.TryParse(match.Value.Substring(1), out int index))
                    {
                        maxIndex = Math.Max(maxIndex, index);
                    }              
                }
            }
            return maxIndex; 
        }


        /// <summary>
        /// you can change the field of Log by using LogConfiguration.
        /// It should be an inner class so that it can modify the private properties of Log class while allowing users to call the public members of LogConfiguration class.
        /// </summary>
        public class LogConfiguration
        {
            public LogConfiguration() { }

            public LogConfiguration MinimumLogLevel(LogLevel logLevel)
            {
                Log._minimumLogLevel = logLevel;
                return this;
            }

            public LogConfiguration LogFileNameWithoutExtension(string logFileName)
            {
                Log._logFileName = logFileName;
                return this;
            }

            public LogConfiguration ErrorLogFileNameWithoutExtension(string errorLogFileName)
            {
                Log._errorLogFileName = errorLogFileName;
                return this;
            }

            public LogConfiguration LogFileDirectory(string logFileDirectory)
            {
                if (Path.IsPathRooted(logFileDirectory))
                {
                    Log._logFileDirectory = logFileDirectory;
                }
                else
                {
                    Log._logFileDirectory = Path.Combine(Environment.CurrentDirectory, logFileDirectory);
                }
                return this;
            }

            public LogConfiguration ErrorLogFileDirectory(string errorlogFileDirectory)
            {
                if (Path.IsPathRooted(errorlogFileDirectory))
                {
                    Log._errorlogFileDirectory = errorlogFileDirectory;
                }
                else
                {
                    Log._errorlogFileDirectory = Path.Combine(Environment.CurrentDirectory, errorlogFileDirectory);
                }
                return this;
            }

            public LogConfiguration IsAdditionalErrorLogFileEnabled(bool isAdditionalLogFileEnabled)
            {
                Log._isAdditionalErrorLogFileEnabled = isAdditionalLogFileEnabled;
                return this;
            }

            public LogConfiguration WriteConsole(bool writeConsole)
            {
                Log._writeConsole = writeConsole;
                return this;
            }

            public LogConfiguration LogFileSizeLimit(int logFileSizeLimit)
            {
                Log._logFileSizeLimit = logFileSizeLimit;
                return this;
            }

            public LogConfiguration LogFileCountLimitEnabled(bool logFileCountLimitEnabled)
            {
                Log._logFileCountLimitEnabled = logFileCountLimitEnabled;
                return this;
            }

            public LogConfiguration RollingCountLimit(int rollingCountLimit)
            {
                Log._rollingCountLimit = rollingCountLimit;
                return this;
            }
        }
    }

    public enum LogLevel
    {
        Verbose, Debug, Information, Warning, Error, Fatal
    }
}

