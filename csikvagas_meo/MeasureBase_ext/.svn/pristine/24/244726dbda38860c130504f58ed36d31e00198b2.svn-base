using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.e77Console;
using System.IO.Ports;

namespace e77.MeasureBase.Communication.Port
{
    public static class UsbAutoDetect
    {
        static public void WaitForUsbConnect(bool consoleMsg_in,  out string portName_out)
        {
            portName_out = null;
            string[] comPorts = SerialPort.GetPortNames();

            if(consoleMsg_in)
                ConsoleHelper.InfoMessage("Csatlakoztassa a készüléket USB kábellel.");

            while (true)
            {
                int currLenght = SerialPort.GetPortNames().Length;

                if (currLenght == comPorts.Count())
                    System.Threading.Thread.Sleep(150);
                else if (currLenght < comPorts.Count())
                {
                    //usb deatached now. update initial table:
                    comPorts = SerialPort.GetPortNames();
                }
                else //if currLenght > _initialComPorts.Count()
                    break;//new item arrives 
            }

            portName_out = SerialPort.GetPortNames().Except(comPorts).Single();

            if(consoleMsg_in)
                ConsoleHelper.InfoMessage("USB port: {0}", portName_out);
            //System.Threading.Thread.Sleep(150); //time for OS to attach the port
        }

        static public void WaitForUsbDisconnect(bool consoleMsg_in, string portName_in)
        {
            
            while (SerialPort.GetPortNames().Contains(portName_in))
            {
                if (consoleMsg_in)
                {
                    ConsoleHelper.InfoMessage("Húzza ki az USB kábelt a készülékből.");
                    consoleMsg_in = false;
                }

                System.Threading.Thread.Sleep(150);
            }
        }
    }
}
