using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Platform
{
    class StackFrame
    {
        public string Name;
        Type _type;
        FieldInfo[] _fields;
        FieldInfo _fieldTaskAwait;
        IAsyncStateMachine _stateMachine;
        public IAsyncStateMachine StateMachine { get { return this._stateMachine; } }

        public StackFrame(IAsyncStateMachine stm)
        {
            var type = stm.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);

            this.Name = type.Name;
            this._stateMachine = stm;
            this._type = type;
            this._fields = ValidFieldStates(fields);
            this._fieldTaskAwait = TaskAwaitField(fields);
        }


        FieldInfo TaskAwaitField(FieldInfo[] fields)
        {
            return (from field in fields
                    where field.FieldType == typeof(System.Runtime.CompilerServices.TaskAwaiter)
                    select field
                    ).First();
        }

        FieldInfo[] ValidFieldStates(FieldInfo[] fields)
        {
            return (from field in fields
                    where !field.FieldType.FullName.StartsWith("System.Runtime.CompilerServices")
                    select field
                    ).ToArray();
        }

        public void Define(object o, object[] values)
        {
            //GetObjectValues(this._stateMachine);
            //Console.WriteLine($"Running: {this.Name}");
            SetObjectValues(this._stateMachine, values);
        }

        public void Redefine(object o)
        {
            //Console.WriteLine($"  Stepping into: {this.Name}");
            GetObjectValues(this._stateMachine);
        }

        object[] GetObjectValues(object o)
        {
            int count = _fields.Length;
            object[] values = new object[count];

            for(int i=0; i< count; i++)
            {
                var field = _fields[i];
                values[i] = field.GetValue(o);
            }
            
            return values;
        }

        public object[] GetFrameValues()
        {
            return GetObjectValues(this._stateMachine);
        }

        void SetObjectValues(object o, object[] values)
        {
            int count = values.Length;

            for (int i = 0; i < count; i++)
            {
                var field = _fields[i];
                var value = values[i];

                field.SetValue(o, value);
            }

            var task = System.Threading.Tasks.Task.CompletedTask.GetAwaiter();
            this._fieldTaskAwait.SetValue(o, task);
        }
    }
}
