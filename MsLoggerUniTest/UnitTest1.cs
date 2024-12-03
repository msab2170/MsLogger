using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MsLogger;
using System.IO;

namespace UniTestForMSLog
{
    [TestClass]
    public class UnitTest1
    {
        public readonly string APP_NAME = typeof(UnitTest1).Namespace;
        private readonly string verbose = "verbose log";
        private readonly string debug = "debug log";
        private readonly string information = "information log";
        private readonly string warning = "Warning log";
        private readonly string error = "error log";
        private readonly string fatal = "fatal log";

        public UnitTest1()
        {
            Log.CreateConfiguration()
                    .LogFileDirectory(APP_NAME + "_Logs")                                       // 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
                    .LogFileNameWithoutExtension(APP_NAME)                                  // 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
                    .IsAdditionalErrorLogFileEnabled(true)                                          // 로그레빌이 Error이상 로그만 기록하는 로그파일을 추가 생성
                    .ErrorLogFileDirectory(APP_NAME + "_Error_Logs")                         // 로그레빌이 Error이상 로그만 기록하는 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
                    .ErrorLogFileNameWithoutExtension(APP_NAME + "_Error")              // 로그레빌이 Error이상 로그만 기록하는 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
                    .MinimumLogLevel(Log.LogLevel.Verbose)                                    // 기록할 최소로그레벨
                    .WriteConsole(true)                                                                 // 콘솔에도 보이게 할지 여부
                    .LogFileSizeLimit(5_120_000)                                                     // 최대 로그파일 크기
                    .LogFileCountLimitEnabled(true)                                                 // 로그파일 갯수 제한 여부
                    .RollingCountLimit(30);                                                             // 제한할 로그파일 갯수
        }

        /// <summary>
        /// Log.Verbose 테스트
        /// </summary>
        [TestMethod]
        public void VerboseString()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Verbose(verbose);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(verbose) && result.ToLower().Contains("[verbose]"));

            }
        }

        /// <summary>
        /// /// Log.Verbose override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void VerboseFormat()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Verbose(verbose + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{verbose} 변수1, 변수2") && result.ToLower().Contains("[verbose]"));
            }
        }


        /// <summary>
        /// Log.Debug 테스트
        /// </summary>
        [TestMethod]
        public void DebugString()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Debug(debug);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(debug) && result.ToLower().Contains("[debug]"));

            }
        }

        /// <summary>
        /// /// Log.Debug override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void DebugFormat()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Debug(debug + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{debug} 변수1, 변수2") && result.ToLower().Contains("[debug]"));
            }
        }


        /// <summary>
        /// Log.Information 테스트
        /// </summary>
        [TestMethod]
        public void InformationString()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Information(information);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(information) && result.ToLower().Contains("[information]"));

            }
        }

        /// <summary>
        /// /// Log.Information override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void InformationFormat()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Information(information + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{information} 변수1, 변수2") && result.ToLower().Contains("[information]"));
            }
        }



        /// <summary>
        /// Log.Debug 테스트
        /// </summary>
        [TestMethod]
        public void WarningString()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Warning(warning);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(warning) && result.ToLower().Contains("[warning]"));

            }
        }


        /// <summary>
        /// /// Log.Debug override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void WarningFormat()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Warning(warning + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{warning} 변수1, 변수2") && result.ToLower().Contains("[warning]"));
            }
        }



        /// <summary>
        /// Log.Debug 테스트
        /// </summary>
        [TestMethod]
        public void ErrorString()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Error(error);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(error) && result.ToLower().Contains("[error]")); // 로그 메시지가 포함되어 있는지 확인

            }
        }

        /// <summary>
        /// /// Log.Debug override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void ErrorFormat()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Error(error + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{error} 변수1, 변수2") && result.ToLower().Contains("[error]"));
            }
        }




        /// <summary>
        /// Log.Debug 테스트
        /// </summary>
        [TestMethod]
        public void FatalString()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Fatal(fatal);

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains(fatal) && result.ToLower().Contains("[fatal]"));

            }
        }

        /// <summary>
        /// /// Log.Debug override (string.format) 테스트
        /// </summary>
        [TestMethod]
        public void FatalFormat()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Log.Fatal(fatal + " {0}, {1}", "변수1", "변수2");

                var result = sw.ToString().Trim(); // 콘솔에 출력된 내용 가져오기
                Assert.IsTrue(result.Contains($"{fatal} 변수1, 변수2") && result.ToLower().Contains("[fatal]"));
            }
        }
    }
}
