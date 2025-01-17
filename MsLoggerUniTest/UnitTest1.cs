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
        private readonly int _LogFileSizeLimit = 51_200_000;
        private readonly string _LogDirectory = APP_NAME + "_Logs";
        private readonly string _LogFileName = $"{APP_NAME}-{DateTime.Now:yyyyMMdd}";
        private readonly string _LogFilePath = Path.Combine(APP_NAME + "_Logs", $"{APP_NAME}-{DateTime.Now:yyyyMMdd}.log");
        private readonly bool _WriteToConsole = true; 


        private readonly string _Verbose = "verbose log";
        private readonly string _Debug = "debug log";
        private readonly string _Information = "information log";
        private readonly string _Warning = "Warning log";
        private readonly string _Error = "error log";
        private readonly string _Fatal = "fatal log";
        private readonly string _LogPattern = @"\d{4}[-]\d{2}[-]\d{2} \d{2}[:]\d{2}[:]\d{2}[.]\d{3} \[(\w+)\] ";

        
        public UnitTest1()
        {
            Log.CreateConfiguration()
                    .LogFileDirectory(_LogDirectory)                                // 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
                    .LogFileNameWithoutExtension(APP_NAME)                          // 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
                    .IsAdditionalErrorLogFileEnabled(true)                          // 로그레빌이 Error이상 로그만 기록하는 로그파일을 추가 생성
                    .ErrorLogFileDirectory(APP_NAME + "_Error_Logs")                // 로그레빌이 Error이상 로그만 기록하는 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
                    .ErrorLogFileNameWithoutExtension(APP_NAME + "_Error")          // 로그레빌이 Error이상 로그만 기록하는 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
                    .MinimumLogLevel(LogLevel.Verbose)                              // 기록할 최소로그레벨
                    .WriteConsole(_WriteToConsole)                                  // 콘솔에도 보이게 할지 여부
                    .LogFileSizeLimit(_LogFileSizeLimit)                            // 최대 로그파일 크기
                    .LogFileCountLimitEnabled(true)                                 // 로그파일 갯수 제한 여부
                    .RollingCountLimit(30);                                         // 제한할 로그파일 갯수
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Verbose}");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Verbose");
            }
        }

        [TestMethod]
        public void VerboseStringFile()
        {
            Log.Verbose(_Verbose);


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Verbose}");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Verbose");
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


                var result = sw.ToString().Trim();
                Match match = Regex.Match(result, $"{_LogPattern}{_Verbose} 변수1, 변수2");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Verbose");
            }
        }


        [TestMethod]
        public void VerboseFormatFile()
        {
            Log.Verbose(_Verbose + " {0}, {1}", "변수1", "변수2");


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Verbose} 변수1, 변수2");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Verbose");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Debug}");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Debug");

            }
        }

        [TestMethod]
        public void DebugStringFile()
        {
            Log.Debug(_Debug);


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Debug}");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Debug");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Debug} 변수1, 변수2");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Debug");
            }
        }


        [TestMethod]
        public void DebugFormatFile()
        {
            Log.Debug(_Debug + " {0}, {1}", "변수1", "변수2");


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Debug} 변수1, 변수2");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Debug");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Information}");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Information");

            }
        }

        [TestMethod]
        public void InformationStringFile()
        {
            Log.Information(_Information);


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Information}");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Information");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Information} 변수1, 변수2");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Information");
            }
        }



        [TestMethod]
        public void InformationFormatFile()
        {
            Log.Information(_Information + " {0}, {1}", "변수1", "변수2");

            
            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Information} 변수1, 변수2");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Information");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Warning}");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Warning");

            }
        }


        [TestMethod]
        public void WarningStringFile()
        {
            Log.Warning(_Warning);


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Warning}");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Warning");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Warning} 변수1, 변수2");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Warning");
            }
        }

        [TestMethod]
        public void WarningFormatFile()
        {
            Log.Warning(_Warning + " {0}, {1}", "변수1", "변수2");


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Warning} 변수1, 변수2");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Warning");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Error}");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Error");

            }
        }

        [TestMethod]
        public void ErrorStringFile()
        {
            Log.Error(_Error);


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Error}");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Error");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Error} 변수1, 변수2");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Error");
            }
        }


        [TestMethod]
        public void ErrorFormatFile()
        {
            Log.Error(_Error + " {0}, {1}", "변수1", "변수2");


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Error} 변수1, 변수2");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Error");
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


                var result = sw.ToString().Trim(); 
                Match match = Regex.Match(result, $"{_LogPattern}{_Fatal}");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Fatal");

            }
        }


        [TestMethod]
        public void FatalStringFile()
        {
            Log.Fatal(_Fatal);


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Fatal}");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Fatal");
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


                var result = sw.ToString().Trim();          
                Match match = Regex.Match(result, $"{_LogPattern}{_Fatal} 변수1, 변수2");
                Assert.IsTrue(match.Success && match.Groups[1].Value == "Fatal");
            }
        }


        [TestMethod]
        public void FatalFormatFile()
        {
            Log.Fatal(_Fatal + " {0}, {1}", "변수1", "변수2");


            string[] lines = File.ReadAllLines(_LogFilePath);
            var result = lines[lines.Length - 1];
            Match match = Regex.Match(result, $"{_LogPattern}{_Fatal} 변수1, 변수2");
            Assert.IsTrue(match.Success && match.Groups[1].Value == "Fatal");
        }


    }
}
