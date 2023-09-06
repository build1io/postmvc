using System;

namespace Build1.PostMVC.Core.MVCS.Events.Impl
{
    public abstract class EventBase
    {
        private static int Id;

        private readonly int    _id;
        private readonly Type   _type;
        private readonly string _name;

        protected EventBase()
        {
            _id = ++Id;
        }

        protected EventBase(Type type)
        {
            _id = ++Id;
            _type = type;
        }

        protected EventBase(string name)
        {
            _id = ++Id;
            _name = name;
        }

        protected EventBase(Type type, string name)
        {
            _id = ++Id;
            _type = type;
            _name = name;
        }

        public override int  GetHashCode()         { return _id; }
        public override bool Equals(object obj)    { return Equals(obj as EventBase); }
        public          bool Equals(EventBase obj) { return obj != null && obj._id == _id; }

        internal string Format()
        {
            return _type != null
                       ? $"{_id} {_type.Name}.{_name}"
                       : $"{_id} {_name}";
        }

        public override string ToString()
        {
            if (_type != null)
                return _name != null ? $"{_type.Name}.{_name} [{_id}]" : $"{_type.Name} [{_id}]";
            return _name != null ? $"Event.{_name} [{_id}]" : $"Event [{_id}]";
        }
    }
}