using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using e77.MeasureBase.Model;

namespace WinFormBlankTest
{
    public class Device 
    {
        #region Static bools to detect command sequence or DeviceMix

        public  int Original_MeasureID;
        public  long Original_SerialNumber;

        public  bool headerFirstIsOk;
        public bool headerSecondIsOk;
        public bool H62_error_count_valid;
        public bool Not_H62_error_count_valid;
        public bool HasError;
        public bool ClearAfterFirstReceive;

        public string BarcodeLOTValue;
        public string BarcodeSNAndRollValue;

        public string BarcodeSN;
        public string BarcodeRoll;

        public bool rtIsDisabled;
        public bool isNoteing;

       

        public bool lot_strip_count_ok;
        public bool lot_is_valid;
        public bool BarcodeLOTReadedAndSaved;
        public bool BarcodeLOTReaded;
        public bool SNAndRollReadedAndSaved;
        public bool SNAndRollReaded;
        public bool EarlyDribble { get; set; }

        public bool PacketIsReady;

        public bool PacketIsPreProcessed;

        public  int MixCheck_MeasureID;
        public  long MixCheck_SerialNumber;

        public bool BatteryFlagOn;
        public int batterIsLowCounter;

        public bool _IsBatteryShown;
        public bool IsBatteryShown;
        
        public  bool _IsDialogShown;
        public  bool IsDialogShown
        {
            get
            {
                return _IsDialogShown;
            }
            set
            {
                _IsDialogShown = value;
            }
        }

        public DateTime Start_Date;
        public DateTime End_Date;

        public bool startTimeStored;
        public bool endTimeStored;

        public bool IsLogging;
        public bool IsErrorShown;
        public  bool _strip_in_OK;
        public  bool strip_in_OK
        {
            get
            {
                return _strip_in_OK;
            }
            set
            {
                _strip_in_OK = value;
            }

        }

        public  bool _strip_wait_OK;
        public  bool strip_wait_OK
        {
            get
            {
                return _strip_wait_OK;
            }
            set
            {
                _strip_wait_OK = value;
            }
        }
        public  bool _drop_wait_OK;
        public  bool drop_wait_OK
        {
            get
            {
                return _drop_wait_OK;
            }
            set
            {
                _drop_wait_OK = value;
            }
        }
        public  bool _drop_detect_OK;
        public  bool drop_detect_OK
        {
            get
            {
                return _drop_detect_OK;
            }
            set
            {
                _drop_detect_OK = value;
            }
        }
        public string wrong_step;

        public int ThaCode { get; set; }
       public  bool _code_OK;
        public bool code_OK { get; set; }

        public string SubRoll_ID { get; set; }
        public string _lot_id;
        public string LOT_ID{get; set;}

        public bool AfterFirstLOt { get; set; }
        public string SN{ get; set; }
        public string Roll { get; set; }
        public string BarCode{get; set;}

        public bool AfterFirst { get; set; }
        public string Barcode1 { get; set; }
        public string Barcode2 { get; set; }
        public bool BarCodeOK;
        public bool BarCodeOK_lot;
        public string _sn;
        public string SerialNumber { get; set; }


        public bool IsLOTReady { get; set; }
        public string Panelnames { get; set; }
        public int lot_count_in_one_roll=Program.TubeCount;
        public int Averages_ID { get; set; }
        public bool Homogenity_Is_Valid { get; set; }
        public bool PostError { get; set; }
        public bool PostErrorAtDeviceReplace { get; set; }
        public int NotH62_Error { get; set; }
        public int H62_Error { get; set; }
        public bool IsResultValid { get; set; }
        public string Error_H_Text { get; set; }

        public bool Remeasured = false;

        public bool MasterLot { get; set; }
        public int stripCount { get; set; }
        public string BarcodeFirst { get; set; }
        public bool H62_error_happened { get; set; }
        public bool Not_H62_error_happened { get; set; }
        public bool PostDeviceReplace { get; set; }
        #endregion


        public Device()
        {

        }

        public static SerialPort actPort;
        public string VialID;
        public Device(SerialPort _port)
        {
            actPort = _port;
        }

    
    }
}
