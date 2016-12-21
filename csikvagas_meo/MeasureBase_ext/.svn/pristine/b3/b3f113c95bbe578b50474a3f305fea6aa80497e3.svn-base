using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    /// <summary>
    /// Composite class for protocol elements
    /// It manages the sending and receiving of all of the contained protocol elements.
    /// </summary>
    public class ProtocolComposite : ProtocolElement, IProtocolElement, IProtocolComposite
    {
        #region Properties and Fields
        protected List<IProtocolElement> _protocolElements;

        /// <summary>
        /// Protocol elements contained by this composite class
        /// </summary>
        public IEnumerable<IProtocolElement> ProtocolElements
        {
            get
            {
                return _protocolElements;
            }
        }

        /// <summary>
        /// Indexer that allowes access to the protocol elements contained by this composite class
        /// </summary>
        /// <param name="index">Name of the element</param>
        /// <returns>The protocol element </returns>
        /// <remarks>
        /// It is advisory to use an enum for storing the names of the protocol elements.
        /// In a derived class there can be another indexer with the index value of that certain enum.
        /// </remarks>
        public IProtocolElement this[string index]
        {
            get
            {
                return _protocolElements.Single((protocolElement) => protocolElement.Name == index);
            }
        }

        public override IPortProxy DefaultPort
        {
            get
            {
                IPortProxy defaultPort = base.DefaultPort;
                if (_protocolElements.Any((pe) => pe.DefaultPort != defaultPort))
                    return null;
                else
                    return base.DefaultPort;
            }
            set
            {
                base.DefaultPort = value;
                _protocolElements.ForEach((pe) => { pe.DefaultPort = value; });
            }
        }

        #endregion Properties and Fields

        #region Constructors and Initiators

        public ProtocolComposite()
            : base()
        {
            _protocolElements = new List<IProtocolElement>();
        }

        public ProtocolComposite(IPortProxy port_in, IProtocolElement[] elements)
            : base(port_in)
        {
            _protocolElements.AddRange(elements);
        }

        public ProtocolComposite(IProtocolElement[] elements)
            : base()
        {
            _protocolElements.AddRange(elements);
        }

        public ProtocolComposite(IPortProxy port_in)
            : base(port_in)
        {
            _protocolElements = new List<IProtocolElement>();
        }

        #endregion Constructors and Initiators

        #region Methods

        /// <summary>
        /// Adds an element to the protocol element list
        /// </summary>
        /// <param name="protocolElement_in">the element that is to be added</param>
        public virtual void Add(IProtocolElement protocolElement_in)
        {
            if (_protocolElements.Any((protocolElement) => protocolElement.Name == protocolElement_in.Name))
                throw new ArgumentException("Multiple protocol element with the same name cannot be existed in a single protocol");
            _protocolElements.Add(protocolElement_in);
        }

        /// <summary>
        /// Adds an element to the protocol element list, with the corresponding name of the protocol element
        /// </summary>
        /// <param name="protocolElement_in">the element that is to be added</param>
        /// <param name="protocolElementName_in">Specified name of the protocol element;
        /// Later the certain element is accessible through this name through the indexer of this class</param>
        public virtual void Add(IProtocolElement protocolElement_in, String protocolElementName_in)
        {
            protocolElement_in.Name = protocolElementName_in;
            this.Add(protocolElement_in);
        }

        #endregion Methods

        #region IProtocolElement Members

        /// <summary>
        /// Timeout before the first data arrives
        /// </summary>
        public override int ReceiveBeginTimeout
        {
            get
            {
                if (_protocolElements == null || _protocolElements.Count == 0)
                    return base.ReceiveBeginTimeout;
                return (int)_protocolElements[0].ReceiveBeginTimeout;
            }
            set
            {
                if (_protocolElements != null && _protocolElements.Count != 0)
                    _protocolElements[0].ReceiveBeginTimeout = value;
                base.ReceiveBeginTimeout = value;
            }
        }

        /// <summary>
        /// Timeout between data segments
        /// </summary>
        public override int ReceiveTimeout
        {
            get
            {
                if (_protocolElements == null || _protocolElements.Count == 0)
                    return base.ReceiveTimeout;
                return (int)_protocolElements.Average((protElem) => protElem.ReceiveTimeout);
            }
            set
            {
                if (_protocolElements != null && _protocolElements.Count != 0)
                {
                    bool first = true;
                    foreach (ProtocolElement element in _protocolElements)
                    {
                        element.ReceiveTimeout = value;
                        if (first)
                            first = false;
                        else
                        {
                            element.ReceiveBeginTimeout = value;
                            element.ReceiveTimeout = value;
                        }
                    }
                }
                base.ReceiveTimeout = value;
            }
        }

        /// <summary>
        /// Timeout for sending
        /// </summary>
        public override int SendTimeout
        {
            get
            {
                if (_protocolElements == null || _protocolElements.Count == 0)
                    return base.SendTimeout;
                return (int)_protocolElements.Average((protElem) => protElem.SendTimeout);
            }
            set
            {
                if (_protocolElements != null && _protocolElements.Count != 0)
                    foreach (ProtocolElement element in _protocolElements)
                        element.SendTimeout = value;
                base.SendTimeout = value;
            }
        }

        public override void PrepareSendData(List<byte> sendDataList_in)
        {
            foreach (IProtocolElement element in _protocolElements)
                element.PrepareSendData(sendDataList_in);
        }

        public override void ProcessReceivedData(List<byte> receivedDataList_in)
        {
            foreach (IProtocolElement element in _protocolElements)
                element.ProcessReceivedData(receivedDataList_in);
        }

        public virtual void ProcessReceivedContentData()
        {
            foreach (IProtocolElement element in _protocolElements)
                element.ProcessReceivedData(_receivedDataList);
        }

        public override void Receive(List<byte> receivedDataList_in)
        
        {
            _receivedDataList.Clear();
            foreach (IProtocolElement element in _protocolElements)
                element.Receive(_receivedDataList);

            if (receivedDataList_in != _receivedDataList) // TODO_HL: 10 could use a better object equality check
                receivedDataList_in.AddRange(_receivedDataList);
        }

        #endregion IProtocolElement Members

        #region IProtocol Members

        public virtual void Send()
        {
            _sendDataList.Clear();
            PrepareSendData(_sendDataList);
            Send(_sendDataList);
        }

        public virtual void Receive()
        {
            _receivedDataList.Clear();
            WaitForReceiveBegin();
            Receive(_receivedDataList);
            ProcessReceivedData(_receivedDataList);
        }

        #endregion IProtocol Members
    }
}