using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using MsLogger;
using System.IO;

namespace UniTestForMSLog
{
    [TestClass]
    public class UnitTest1
    {
        public static readonly string APP_NAME = typeof(UnitTest1).Namespace;
        private readonly string _Verbose = "verbose log";
        private readonly string _Debug = "debug log";
        private readonly string _Information = "information log";
        private readonly string _Warning = "Warning log";
        private readonly string _Error = "error log";
        private readonly string _Fatal = "fatal log";

        private readonly int _LogFileSizeLimit = 5_120_000;
        private readonly string _LogDirectory = APP_NAME + "_Logs";
        private readonly string _LogFileName = $"{APP_NAME}-{DateTime.Now:yyyyMMdd}";
        private readonly string _LogFilePath = Path.Combine(APP_NAME + "_Logs", $"{APP_NAME}-{DateTime.Now:yyyyMMdd}.log");
        

        public UnitTest1()
        {
            Log.CreateConfiguration()
                    .LogFileDirectory(APP_NAME + "_Logs")                                       // 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
                    .LogFileNameWithoutExtension(APP_NAME)                                  // 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
                    .IsAdditionalErrorLogFileEnabled(true)                                          // 로그레빌이 Error이상 로그만 기록하는 로그파일을 추가 생성
                    .ErrorLogFileDirectory(APP_NAME + "_Error_Logs")                         // 로그레빌이 Error이상 로그만 기록하는 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
                    .ErrorLogFileNameWithoutExtension(APP_NAME + "_Error")              // 로그레빌이 Error이상 로그만 기록하는 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
                    .MinimumLogLevel(LogLevel.Verbose)                                    // 기록할 최소로그레벨
                    .WriteConsole(true)                                                                 // 콘솔에도 보이게 할지 여부
                    .LogFileSizeLimit(_LogFileSizeLimit)                                                     // 최대 로그파일 크기
                    .LogFileCountLimitEnabled(true)                                                 // 로그파일 갯수 제한 여부
                    .RollingCountLimit(30);                                                             // 제한할 로그파일 갯수
        }

        /// <summary>
        /// Log.Verbose 테스트
        /// </summary>
        [TestMethod]
        public void VerboseStringConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Verbose(_Verbose);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(_Verbose) && result.ToLower().Contains("[verbose]"));
            }
        }
        [TestMethod]
        public void VerboseStringFile()
        {
            Log.Verbose(_Verbose);
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains(_Verbose) && lines[lines.Length - 1].ToLower().Contains("[verbose]"));
        }

        

        /// <summary>
        /// /// Log.Verbose override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void VerboseFormatConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Verbose(_Verbose + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{_Verbose} 변수1, 변수2") && result.ToLower().Contains("[verbose]"));
            }
        }


        [TestMethod]
        public void VerboseFormatFile()
        {
            Log.Verbose(_Verbose + " {0}, {1}", "변수1", "변수2");
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains($"{_Verbose} 변수1, 변수2") && lines[lines.Length - 1].ToLower().Contains("[verbose]"));
        }


        /// <summary>
        /// Log.Debug 테스트
        /// </summary>
        [TestMethod]
        public void DebugStringConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Debug(_Debug);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(_Debug) && result.ToLower().Contains("[debug]"));

            }
        }

        [TestMethod]
        public void DebugStringFile()
        {
            Log.Debug(_Debug);
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains(_Debug) && lines[lines.Length - 1].ToLower().Contains("[debug]"));
        }

        /// <summary>
        /// /// Log.Debug override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void DebugFormatConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Debug(_Debug + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{_Debug} 변수1, 변수2") && result.ToLower().Contains("[debug]"));
            }
        }


        [TestMethod]
        public void DebugFormatFile()
        {
            Log.Debug(_Debug + " {0}, {1}", "변수1", "변수2");
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains($"{_Debug} 변수1, 변수2") && lines[lines.Length - 1].ToLower().Contains("[debug]"));
        }


        /// <summary>
        /// Log.Information 테스트
        /// </summary>
        [TestMethod]
        public void InformationStringConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Information(_Information);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(_Information) && result.ToLower().Contains("[information]"));

            }
        }

        [TestMethod]
        public void InformationStringFile()
        {
            Log.Information(_Information);
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains(_Information) && lines[lines.Length - 1].ToLower().Contains("[information]"));
        }


        /// <summary>
        /// /// Log.Information override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void InformationFormatConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Information(_Information + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{_Information} 변수1, 변수2") && result.ToLower().Contains("[information]"));
            }
        }



        [TestMethod]
        public void InformationFormatFile()
        {
            Log.Information(_Information + " {0}, {1}", "변수1", "변수2");
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains($"{_Information} 변수1, 변수2") && lines[lines.Length - 1].ToLower().Contains("[information]"));
        }


        /// <summary>
        /// Log.Debug 테스트
        /// </summary>
        [TestMethod]
        public void WarningStringConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Warning(_Warning);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(_Warning) && result.ToLower().Contains("[warning]"));

            }
        }


        [TestMethod]
        public void WarningStringFile()
        {
            Log.Warning(_Warning);
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains(_Warning) && lines[lines.Length - 1].ToLower().Contains("[warning]"));
        }

        /// <summary>
        /// /// Log.Debug override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void WarningFormatConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Warning(_Warning + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{_Warning} 변수1, 변수2") && result.ToLower().Contains("[warning]"));
            }
        }

        [TestMethod]
        public void WarningFormatFile()
        {
            Log.Warning(_Warning + " {0}, {1}", "변수1", "변수2");
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains($"{_Warning} 변수1, 변수2") && lines[lines.Length - 1].ToLower().Contains("[warning]"));
        }

        /// <summary>
        /// Log.Debug 테스트
        /// </summary>
        [TestMethod]
        public void ErrorStringConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Error(_Error);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(_Error) && result.ToLower().Contains("[error]")); // 로그 메시지가 포함되어 있는지 확인

            }
        }

        [TestMethod]
        public void ErrorStringFile()
        {
            Log.Error(_Error);
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains(_Error) && lines[lines.Length - 1].ToLower().Contains("[error]"));
        }


        /// <summary>
        /// /// Log.Debug override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void ErrorFormatConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Error(_Error + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{_Error} 변수1, 변수2") && result.ToLower().Contains("[error]"));
            }
        }


        [TestMethod]
        public void ErrorFormatFile()
        {
            Log.Error(_Error + " {0}, {1}", "변수1", "변수2");
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains($"{_Error} 변수1, 변수2") && lines[lines.Length - 1].ToLower().Contains("[error]"));
        }


        /// <summary>
        /// Log.Debug 테스트
        /// </summary>
        [TestMethod]
        public void FatalStringConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Fatal(_Fatal);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(_Fatal) && result.ToLower().Contains("[fatal]"));

            }
        }


        [TestMethod]
        public void FatalStringFile()
        {
            Log.Fatal(_Fatal);
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains(_Fatal) && lines[lines.Length - 1].ToLower().Contains("[fatal]"));
        }



        /// <summary>
        /// /// Log.Debug override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void FatalFormatConsole()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Fatal(_Fatal + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{_Fatal} 변수1, 변수2") && result.ToLower().Contains("[fatal]"));
            }
        }


        [TestMethod]
        public void FatalFormatFile()
        {
            Log.Fatal(_Fatal + " {0}, {1}", "변수1", "변수2");
            string oldFile = ReadLastLineFromLogFile();
            string[] lines = File.ReadAllLines(oldFile);
            Assert.IsTrue(lines[lines.Length - 1].Contains($"{_Fatal} 변수1, 변수2") && lines[lines.Length - 1].ToLower().Contains("[fatal]"));
        }




        /// <summary>
        /// 원본 소스에 있는 함수로 용량한도 넘을시 괄호와 숫자가 붙으면 그 숫자 중 가장 큰값을 가져오는 함수
        /// </summary>
        /// <param name="logDirectory"></param>
        /// <param name="logFileName"></param>
        /// <returns></returns>
        private static int GetLastLogFileIndex(string logDirectory, string logFileName)
        {
            int maxIndex = 0;
            string pattern = $@"{logFileName} \(\d+\)\.log$"; // "(숫자).log" 패턴

            // 기존 파일 목록을 가져와서 인덱스를 찾기
            foreach (var file in Directory.GetFiles(logDirectory))
            {
                string fileName = Path.GetFileName(file);
                if (Regex.IsMatch(fileName, pattern))
                {
                    // 정규 표현식으로 숫자 부분 추출
                    var match = Regex.Match(fileName, @"\(\d+");
                    if (match.Success)
                    {
                        if (int.TryParse(match.Value.Substring(1), out int index))
                        {
                            maxIndex = Math.Max(maxIndex, index);
                        }
                    }
                }
            }

            return maxIndex; // 다음 인덱스 반환
        }


        private string ReadLastLineFromLogFile()
        {
            FileInfo file = new FileInfo(_LogFilePath);
            string oldFile = _LogFilePath;
            if (file.Exists && file.Length > _LogFileSizeLimit)
            {
                int fileIndex = GetLastLogFileIndex(_LogDirectory, _LogFileName);
                oldFile = Path.Combine(_LogDirectory, $"{_LogFileName} ({++fileIndex}).log");
                while (File.Exists(oldFile) && new FileInfo(oldFile).Length >= _LogFileSizeLimit)
                {
                    oldFile = Path.Combine(_LogDirectory, $"{_LogFileName} ({++fileIndex}).log");
                }
            }
            return oldFile;
        }
    }
}
