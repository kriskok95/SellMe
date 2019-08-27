namespace SellMe.Web.Infrastructure.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DataPoint
    {
        public DataPoint(double y, string label)
        {
            Y = y;
            Label = label;
        }

        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }


        public DataPoint(double x, double y, string label)
        {
            X = x;
            Y = y;
            Label = label;
        }

        public DataPoint(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public DataPoint(double x, double y, double z, string label)
        {
            X = x;
            Y = y;
            Z = z;
            Label = label;
        }


        //Explicitly setting the name to be used while serializing to JSON. 
        [DataMember(Name = "label")]
        public string Label;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public double? Y;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public double? X;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "z")]
        public double? Z;
    }
}

