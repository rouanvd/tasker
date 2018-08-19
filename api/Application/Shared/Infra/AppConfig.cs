namespace Application.Shared.Infra
{
    public class AppConfig
    {
        public RunningEnvironment Environment {get;set;}
        public string[] RestrictedUsers {get;set;}
        public bool EnablePerformanceProfiling {get;set;}

        public string Path_TempWorkingDir {get;set;}
    }


    public enum RunningEnvironment
    {
        Dev,
        Qa,
        Prod
    }
}
