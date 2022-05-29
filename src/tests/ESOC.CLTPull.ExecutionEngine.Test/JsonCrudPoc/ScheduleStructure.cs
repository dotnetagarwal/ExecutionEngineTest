using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;


namespace ESOC.CLTPull.ExecutionEngine.Test.JsonCrudPoc
{
    public interface IScheduleStructure
    {
      //  public string SerializeConfigurationData();
     //   public JObject DeserializeConfigurationData(string data);
    }


    public enum Frequency
    {
        Daily,
        Weekly,
        Monthly
    }

    public class Daily : IScheduleStructure
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }     
        public int ReccurenceInterval { get; set; }
        public DateTime[] TimeSlots { get; set; }


        ////public JObject DeserializeConfigurationData(string data)
        ////{
        ////    return JsonConvert.DeserializeObject<JObject>(data);
        ////}

        ////public string SerializeConfigurationData()
        ////{
        ////    return JsonConvert.SerializeObject(this);
        ////}
    }

    public class ControlScheduleConfiguration
    {
        public IScheduleStructure _scheduleStructure;
        public ControlScheduleConfiguration(IScheduleStructure scheduleStructure)
        {
            _scheduleStructure = scheduleStructure;  //TODO we can get it from factory as well
        }

        public string ScheduleConfigurationId { get; set; }
        public Frequency ScheduleFrequency { get; set; }
        public string ScheduleData { get => _scheduleStructure.SerializeConfigurationData(); }

        public dynamic DeserializeConfigurationData(string data)
        {
            return _scheduleStructure.DeserializeConfigurationData(data, ScheduleFrequency.ToString());
        }
    }

    public static class IScheduleStructureExtensions
    {
        public static string SerializeConfigurationData(this IScheduleStructure scheduleStructure)
        {
            return JsonConvert.SerializeObject(scheduleStructure);
        }

        public static JObject DeserializeConfigurationData(this IScheduleStructure scheduleStructure, string data)
        {
            return JsonConvert.DeserializeObject<JObject>(data);
        }

        public static dynamic DeserializeConfigurationData(this IScheduleStructure scheduleStructure, string data, string typeName)
        {
            return JsonConvert.DeserializeObject(data, Type.GetType(typeName));
        }
    }
}
