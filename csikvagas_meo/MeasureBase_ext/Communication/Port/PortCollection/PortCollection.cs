using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace e77.MeasureBase.Communication
{
    public class PortCollection
    {
        #region Constructors

        static PortCollection()
        {
            if (PortCollection.Instance == null)
                new PortCollection();
        }

        public PortCollection()
        {
            if (Instance != null)
                throw new InvalidOperationException("PortCollection class is singleton. Do not create more.");
            Instance = this;
        }

        #endregion Constructors

        #region Properties

        public static PortCollection Instance { get; protected set; }

        //public static bool IsTraceOn = false; // comment //TODO_HL: 2 switch to false before release;

        private List<object> _portList = new List<object>();

        private Dictionary<string, object> _portDictionary = new Dictionary<string, object>();

        protected List<object> List { get { return _portList; } }

        protected Dictionary<string, object> Dictionary { get { return _portDictionary; } }

        #endregion Properties

        #region Methods

        #region Add

        public void Add(object port_in)
        {
            List.Add(port_in);
        }

        public void Add(string key, object port_in)
        {
            Dictionary.Add(key, port_in);
        }

        public void Add(IPortProxy port_in)
        {
            Dictionary.Add(port_in.PortName, port_in);
        }

        public void Add(SerialPort port_in)
        {
            Dictionary.Add(port_in.PortName, port_in);
        }

        #endregion Add

        #region Contains

        public bool ContainsPort(object value)
        {
            return Dictionary.Values.Any((port) => port == value)
                || List.Any((port) => port == value);
        }

        public bool ContainsPort(IPortProxy port_in)
        {
            //return Dictionary.Values.OfType<IPortProxy>().Any((port) => port_in.PortName == port.PortName)
            return (Dictionary.ContainsKey(port_in.PortName) && Dictionary[port_in.PortName] is IPortProxy)
                || List.OfType<IPortProxy>().Any((port) => port_in.PortName == port.PortName);
        }

        public bool ContainsPort(SerialPort port_in)
        {
            return ContainsSerialPortByName(port_in.PortName);
        }

        public bool ContainsPort<T>(T port_in)
            where T : class
        {
            return Dictionary.Values.OfType<T>().Any((port) => port == port_in)
                || List.OfType<T>().Any((port) => port == port_in);
        }

        public bool ContainsSerialPortByName(string portName_in)
        {
            return (Dictionary.ContainsKey(portName_in) && Dictionary[portName_in] is SerialPort)
                || List.OfType<SerialPort>().Any((port) => portName_in == port.PortName);
        }

        #endregion Contains

        #region IsPortAvaible

        public bool IsSerialPortAvailableByName(string portKey_in)
        {
            return SerialPort.GetPortNames().Contains(portKey_in);
        }

        public bool IsPortAvailable(SerialPort port_in)
        {
            return IsSerialPortAvailableByName(port_in.PortName);
        }

        #endregion IsPortAvaible

        public T GetPort<T>()
            where T : class
        {
            T listResult = List.OfType<T>().SingleOrDefault();
            T dictionaryResult = Dictionary.Values.OfType<T>().SingleOrDefault();
            if ((listResult == null && dictionaryResult == null)
                || (listResult != null && dictionaryResult != null))
                throw new InvalidOperationException("Multiple or no port instance found in the Port Collection of a specific type");

            if (listResult != null)
                return listResult;
            else
                return dictionaryResult;
        }

        public IEnumerable<T> GetAllPort<T>()
            where T : class
        {
            return List.OfType<T>().Concat(Dictionary.Values.OfType<T>());
        }

        public T GetPort<T>(string key)
        {
            return (T)Dictionary[key];
        }

        public object GetPort(string key)
        {
            return Dictionary[key];
        }

        #endregion Methods
    }
}