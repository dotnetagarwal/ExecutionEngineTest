
namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
    public class ExecutionEngineContract
    {
        public string ControlScheduleInstanceId { get; set; }
        public string ControlConfigurationDetail { get; set; }
    }
    //public class ExecutionEngineContract
    //{
    //    public ControlConfigurationDetail ControlConfigurationDetail { get; set; }
    //}

    public class ControlConfigurationDetail
    {
        public ControlInformation ControlInformation { get; set; }
        public LHS LHS { get; set; }
        public RHS RHS { get; set; }
        public Resultsetstructure Resultsetstructure { get; set; }
        public BusinessRules BusinessRules { get; set; }
    }

    public class ControlInformation
    {
        public string ControlId { get; set; }
        public string ControlType { get; set; }
        public string ControlDescription { get; set; }
        public string LookBackDate { get; set; }
        public Parent Parent { get; set; }
    }

    public class Parent
    {
        public string ControlId { get; set; }
        public string Status { get; set; }
    }

    public class LHS
    {
        public string ApplicationName { get; set; }
        public Adapterinformation AdapterInformation { get; set; }
    }

    public class Adapterinformation
    {
        public AdapterType AdapterType { get; set; }
    }

    public class Lhsresult
    {
        public dynamic Result { get; set; }
    }

    public class RHS
    {
        public string ApplicationName { get; set; }
        public Adapterinformation AdapterInformation { get; set; }
    }
    public class Rhsresult
    {
        public dynamic Result { get; set; }
    }

    public class Resultsetstructure
    {
        public Lhsresult Lhsresult { get; set; }
        public Rhsresult Rhsresult { get; set; }
    }
    public class BusinessRules
    {
        public ControlLevelRules RecordLevel { get; set; }
        public ControlLevelRules ThresholdLevel { get; set; }
    }
    public class ControlLevelRules
    {
        public ControlStatusLevelRule Green { get; set; }
        public ControlStatusLevelRule Red { get; set; }
        public ControlStatusLevelRule Yellow { get; set; }
    }
    public class ControlStatusLevelRule
    {
        public Rules[] Rules { get; set; }
    }
    public class Rules
    {
        public string Name { get; set; }
    }
}
