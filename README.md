    /// <summary>
    /// 최초 2024-10-22 작성 (명칭 등 변경 프로젝트 새로 파며 2024-12-03에 이 리포지토리 생성)
    /// .Net Standard에서는 로그 파일 생성용 라이브러리로 Serilog를 사용했는데, 구버전의 프레임워크(.Net Framework)를 사용해야만 하는 상황에서 마땅히 사용할 수 있는 로그 라이브러리를 몰라서 Serilog처럼 로그가 찍히도록 개발하여 사용하게 되었음.
    ///
    /// 아래와 같이 선언 후 Log.{logLevel}({message}); 형식으로 로그 사용
    /// 
    ///  Log.CreateConfiguration()
    ///        .LogPath(APP_NAME + "_Logs")                                // 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
    ///        .LogFileNameWithoutExtension(APP_NAME)                      // 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
    ///        .IsAdditionalErrorLogFileEnabled(true)                      // 로그레빌이 Error이상 로그만 기록하는 로그파일을 추가 생성
    ///        .ErrorLogPath(APP_NAME + "_Error_Logs")                     // 로그레빌이 Error이상 로그만 기록하는 로그파일을 위치시킬 폴더의 경로 (절대/상대 경로)
    ///        .ErrorLogFileNameWithoutExtension(APP_NAME + "_Error")      // 로그레빌이 Error이상 로그만 기록하는 로그파일의 이름(확장자는 자동으로 추가되므로 기입해도 .xxx.log파일이 생성됨)
    ///        .MinimumLogLevel(Log.LogLevel.Verbose)                      // 기록할 최소로그레벨
    ///        .WriteConsole(true);                                        // 콘솔에도 보이게 할지 여부
    ///        .LogFileSizeLimit()                                         // 최대 로그파일 크기
    ///        .LogFileCountLimitEnabled()                                 // 로그파일 갯수 제한 여부
    ///        .RollingCountLimit()                                        // 제한할 로그파일 갯수
    ///        
    ///         위와 같이 작성하지 않는 경우 기본값으로 적용, 기본값은 아래와 같음
    ///         LogPath                                    - Path.Combine(Environment.CurrentDirectory, "Logs")
    ///         LogFileNameWithoutExtension                -  "log"
    ///         IsAdditionalErrorLogFileEnabled            - true
    ///         ErrorLogPath                               - Path.Combine(Environment.CurrentDirectory, "ErrorLogs")
    ///         ErrorLogFileNameWithoutExtension           - "log_Error"
    ///         MinimumLogLevel                            - LogLevel.Infomation
    ///         WriteConsole                               - true
    ///         LogFileSizeLimit                           - 51_200_000
    ///         LogFileCountLimitEnabled                   - false
    ///         RollingCountLimit                          - 30
    ///         
    /// # 에러로그만 수집을 원한다면 IsAdditionalErrorLogFileEnabled 옵션을 false로 설정하고 MinimumLogLevel에서 최소로그레벨을 올리는 것을 권장
    /// </summary>

    /// unit test 결과
![image](https://github.com/user-attachments/assets/90c13997-c81f-4264-a5d2-495f60ab1fb6)
