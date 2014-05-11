using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;

namespace TwitterAPI
{
    [DataContract]
    public class TwitterError
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "code")]
        public long ErrorCode { get; set; }
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class OptionContract : Attribute
    {
        public string Name { get; set; }

        private StackFrame stackFrame = new StackFrame();

        public OptionContract() { this.Name = stackFrame.GetMethod().Name; }
        public OptionContract(string Name) { this.Name = Name; }
    }


    [AttributeUsage(AttributeTargets.Field)]
    public class OptionProperty : Attribute
    {
        public string Name { get; set; }

        private StackFrame stackFrame = new StackFrame();

        public OptionProperty() { this.Name = stackFrame.GetMethod().Name; }
        public OptionProperty(string Name) { this.Name = Name; }
    }

    [AttributeUsage(AttributeTargets.Class |
         AttributeTargets.Field | AttributeTargets.Method |
          AttributeTargets.Property)]
    public class MyAttributes : Attribute
    {
        public string Value;

        public MyAttributes(string p) { Value = p; }
    }

    [OptionContract]
    public class TestOption
    {
        [OptionProperty]
        public string TestString = "a";

        public string Testing = "b";
    }

    public class TestOptionClass
    {
        public TestOptionClass()
        {
            TestOption option = new TestOption();

            Type t = option.GetType();

            foreach (FieldInfo field in t.GetFields())
            {
                FieldInfoShow(field);
            }

        }

        private void FieldInfoShow(FieldInfo field)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(field, typeof(OptionContract));
            foreach (Attribute att in attrs)
            {
                //OptionProperty name = att as OptionProperty;
                System.Windows.Forms.MessageBox.Show(att.ToString());
            }
        }
    }
}
